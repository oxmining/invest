using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.Wallets;
using OX.Ledger;
using OX.Wallets.Models;
using OX.Mining;
using OX.IO;
using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.IO.Data.LevelDB;
using OX.UI.Mining;
using OX.Mining.DEX;
using Nethereum.Model;

namespace OX.UI.Swap
{
    public partial class SideSwapRecharge : DarkDialog
    {
        INotecase Operater;
        UInt160 PoolAddress;
        UInt256 AssetId;
        AssetState assetState;
        public SideSwapRecharge()
        {
            InitializeComponent();
            btnOk.Text = UIHelper.LocalString("确定", "OK");
            btnOk.Enabled = false;
            btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_accounts.Text = UIHelper.LocalString("账户:", "Account:");
            this.lb_asset.Text = UIHelper.LocalString("资产:", "Asset:");
            this.lb_balance.Text = UIHelper.LocalString("可用余额:", "Available Balance:");
            this.lb_amount.Text = UIHelper.LocalString("金额:", "Amount:");
        }
        SideTransaction SideTransaction;
        public SideSwapRecharge(INotecase operater, SideTransaction sideTx) : this()
        {
            this.Operater = operater;
            this.SideTransaction = sideTx;
            this.PoolAddress = sideTx.GetContract().ScriptHash;
            this.AssetId = sideTx.Data.AsSerializable<UInt256>();
            assetState = Blockchain.Singleton.Store.GetAssets().TryGet(this.AssetId);
        }



        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (!Fixed8.TryParse(textBox2.Text, out Fixed8 amount))
            {
                btnOk.Enabled = false;
                return;
            }
            if (amount == Fixed8.Zero)
            {
                btnOk.Enabled = false;
                return;
            }
            if (!Fixed8.TryParse(textBox3.Text, out Fixed8 balance))
            {
                btnOk.Enabled = false;
                return;
            }

            if (amount == Fixed8.Zero || balance == Fixed8.Zero || amount > balance)
            {
                btnOk.Enabled = false;
                return;
            }

            if (rbTargetAsset.Checked)
            {
                if (amount.GetInternalValue() % Fixed8.D > 0)
                {
                    btnOk.Enabled = false;
                    return;
                }
            }
            btnOk.Enabled = true;
        }

        private void rbGTC_CheckedChanged(object sender, EventArgs e)
        {
            RefreshBalance();
        }
        private void RefreshBalance()
        {
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            var assetId = this.rbPriceAsset.Checked ? Blockchain.OXC : this.AssetId;
            var from = this.cb_accounts.SelectedItem as AccountListItem;
            textBox3.Text = "0";
            if (this.Operater.Wallet.TryGetWalletAccountBalance(from.Account.ScriptHash, out Dictionary<UInt256, WalletAccountBalance> balances) && balances.TryGetValue(assetId, out WalletAccountBalance balance))
            {
                textBox3.Text = balance.AvailableBalance.ToString();
            }
            textBox_TextChanged(this, EventArgs.Empty);
        }
        decimal forecast(UInt256 assetId, Fixed8 amount)
        {
            decimal outValue = 0;
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin.IsNotNull())
            {
                Fixed8 targetBalance = Fixed8.Zero;
                Fixed8 pricingBalance = Fixed8.Zero;

                var beforeExchanges = bizPlugin.GetAll<SwapBeforeExchangeKey, TransactionOutput>(InvestBizPersistencePrefixes.SwapPairBeforeExchange, this.PoolAddress);

                var lastExchange = bizPlugin.Get<SwapVolumeMerge>(InvestBizPersistencePrefixes.SwapPairLastExchange, this.PoolAddress);
                if (lastExchange.IsNotNull())
                {
                    targetBalance = lastExchange.Volume.TargetBalance;
                    pricingBalance = lastExchange.Volume.PricingBalance;
                    if (beforeExchanges.IsNotNullAndEmpty())
                    {
                        beforeExchanges = beforeExchanges.Where(m => (long)m.Key.BlockIndex * 10000 + (long)m.Key.TxN > (long)lastExchange.Volume.BlockIndex * 10000 + (long)lastExchange.Volume.TxN);
                    }
                    outValue = tryforecast(targetBalance, pricingBalance, beforeExchanges, assetId, amount);
                }
                else
                {
                    var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(this.PoolAddress, () => null);
                    if (acts.IsNotNull())
                    {
                        targetBalance = acts.GetBalance(this.AssetId);
                        pricingBalance = acts.GetBalance(Blockchain.OXC);
                        outValue = tryforecast(targetBalance, pricingBalance, beforeExchanges, assetId, amount);
                    }
                }
            }
            return outValue;
        }
        decimal tryforecast(Fixed8 targetBalance, Fixed8 pricingBalance, IEnumerable<KeyValuePair<SwapBeforeExchangeKey, TransactionOutput>> records, UInt256 assetId, Fixed8 amount)
        {
            var tb = (decimal)targetBalance;
            var pb = (decimal)pricingBalance;

            if (records.IsNotNullAndEmpty())
            {
                foreach (var r in records.OrderBy(m => (long)m.Key.BlockIndex * 10000 + (long)m.Key.TxN))
                {
                    var total = tb * pb;
                    var v = (decimal)r.Value.Value;
                    if (this.AssetId.Equals(r.Value.AssetId))//sale
                    {
                        tb += v;
                        pb = total / tb;
                    }
                    else if (r.Value.AssetId.Equals(Blockchain.OXC))//buy
                    {
                        pb += v;
                        tb = total / pb;
                    }
                }
            }
            var sum = tb * pb;
            var tbout = tb;
            var pbout = pb;
            if (this.AssetId.Equals(assetId))//sale
            {
                tb += (decimal)amount;
                pb = sum / tb;
                return pbout - pb;
            }
            else if (Blockchain.OXC.Equals(assetId))//buy
            {
                pb += (decimal)amount;
                tb = sum / pb;
                return tbout - tb;
            }
            else return 0;
        }

        private void PayToDialog_Load(object sender, EventArgs e)
        {
            this.rbTargetAsset.Text = assetState.GetName();
            this.rbPriceAsset.Text = "OXC";
            foreach (var act in this.Operater.Wallet.GetHeldAccounts())
            {
                this.cb_accounts.Items.Add(new AccountListItem(act));
            }

            this.Text = UIHelper.LocalString("交易", "Exchange");
            RefreshBalance();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            if (!Fixed8.TryParse(textBox2.Text, out Fixed8 amount)) return;

            var from = this.cb_accounts.SelectedItem as AccountListItem;
            var fromAddress = from.Account.ScriptHash;
            var assetId = this.rbPriceAsset.Checked ? Blockchain.OXC : this.AssetId;

            ContractTransaction ct = new ContractTransaction { Outputs = new TransactionOutput[] { new TransactionOutput { AssetId = assetId, ScriptHash = this.PoolAddress, Value = amount } } };
            this.Operater.Wallet.MixBuildAndRelaySingleOutputTransaction(ct, fromAddress, tx =>
            {
                string msg = $"{UIHelper.LocalString("交易已广播", "Relay transaction completed")}   {tx.Hash}";
                //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            });            
        }

        private void cb_accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshBalance();
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

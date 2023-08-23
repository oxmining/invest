using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets.UI;
using OX.Wallets.UI.Forms;
using OX.Wallets;
using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.Wallets.Models;
using OX.Mining;
using OX.Ledger;
using OX.IO;
using OX.SmartContract;
using OX.Cryptography.AES;
using OX.UI.Mining;
using OX.Mining.Trade;
using OX.Mining.OTC;
using OX.Wallets.UI.Controls;
using Nethereum.Model;
using Nethereum.Hex.HexConvertors.Extensions;

namespace OX.UI.OTC
{
    public partial class RegOTCDealerForm : DarkDialog, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        public RegOTCDealerForm(INotecase notecase)
        {
            this.Operator = notecase;
            InitializeComponent();
            this.Text = UIHelper.LocalString("注册场外交易商", "Register OTC Dealer");
            this.btnOk.Text = UIHelper.LocalString("确定", "OK");
            this.btnCancel.Text = UIHelper.LocalString("取消", "Cancel");
            this.lb_balance.Text = UIHelper.LocalString("USDX 余额:", "USDX Balance:");
            this.lb_amount.Text = UIHelper.LocalString("USDX 充值金额:", "USDX Recharge Amount:");
            this.lb_accounts.Text = UIHelper.LocalString("签名账户:", "签名账户:");
            this.lb_ethAddress.Text = UIHelper.LocalString("Eth兑换地址:", "Eth Exchange Address:");
            this.lb_infeerate.Text = UIHelper.LocalString("入金手续费率:", "Deposits Commission Ratio:");
            this.lb_State.Text = UIHelper.LocalString("状态设置:", "State Set:");
            this.lb_inpool_addr.Text = UIHelper.LocalString("交易池地址:", "OTC Pool Address:");
            this.lb_inpool_balance.Text = UIHelper.LocalString("交易池余额:", "OTC Pool Balance:");
        }


        private void RegMinerForm_Load(object sender, EventArgs e)
        {
            this.cb_accounts.Items.Clear();
            this.cb_ethAccounts.Items.Clear();
            this.cb_state.Items.Clear();
            if (Operator.IsNotNull() && Operator.Wallet.IsNotNull() && Operator.Wallet is OpenWallet openWallet)
            {
                foreach (var act in openWallet.GetHeldAccounts())
                {
                    this.cb_accounts.Items.Add(new AccountListItem(act));
                }
                foreach (var ethAct in openWallet.EthAccounts)
                {
                    this.cb_ethAccounts.Items.Add(new OpenAccountListItem(ethAct));
                }
            }
            foreach (var en in EnumHelper.All<OTCStatus>())
            {
                this.cb_state.Items.Add(new EnumItem<OTCStatus>(en));
            }
            this.cb_state.SelectedIndex = 0;
        }





        private void tb_infeerate_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as Wallets.UI.Controls.DarkTextBox;
            if (!byte.TryParse(tb.Text, out byte amt) || amt > 20)
            {
                var s = tb.Text;
                if (s.Length > 0)
                {
                    s = s.Substring(0, s.Length - 1);
                    tb.Clear();
                    tb.AppendText(s);
                }
            }
        }



        private void cb_ethAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lb_inpool_addr_v.Text = string.Empty;
            this.lb_inpool_balance_v.Text = string.Empty;
            if (this.cb_ethAccounts.Text.IsNullOrEmpty()) return;
            var from = this.cb_ethAccounts.SelectedItem as OpenAccountListItem;
            var st = from.Account.Address.BuildOTCDealerTransaction();
            var sh = st.GetContract().ScriptHash;
            this.lb_inpool_addr_v.Text = sh.ToAddress();
            var account = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(sh);
            if (account == null) return;
            if (account.Balances.TryGetValue(invest.USDX_Asset, out Fixed8 balance))
            {
                this.lb_inpool_balance_v.Text = balance.ToString();
            }
        }

        private void cb_accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tb_balance.Clear();
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            var from = this.cb_accounts.SelectedItem as AccountListItem;
            var account = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(from.Account.ScriptHash);
            if (account == null) return;
            if (account.Balances.TryGetValue(invest.USDX_Asset, out Fixed8 balance))
            {
                this.tb_balance.Text = balance.ToString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            if (this.cb_ethAccounts.Text.IsNullOrEmpty()) return;
            if (this.Operator.Wallet is OpenWallet openWallet)
            {
                var oxAccount = this.cb_accounts.SelectedItem as AccountListItem;
                var oxSh = oxAccount.Account.ScriptHash;
                var ethAccountItem = this.cb_ethAccounts.SelectedItem as OpenAccountListItem;
                var ethAccount = ethAccountItem.Account;
                if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amount)) return;
                if (!byte.TryParse(this.tb_infeerate.Text, out byte inrate)) return;
                EnumItem<OTCStatus> stateItem = this.cb_state.SelectedItem as EnumItem<OTCStatus>;
                OTCSetting setting = new OTCSetting
                {
                    InRate = inrate,
                    OutRate = 0,
                    EthAsset = 0,
                    State = stateItem.Target
                };
                var tx = ethAccount.Address.BuildOTCDealerTransaction(setting);
                var sh = tx.GetContract().ScriptHash;

                List<TransactionOutput> outputs = new List<TransactionOutput>();
                if (amount > Fixed8.Zero)
                {
                    outputs.Add(new TransactionOutput
                    {
                        ScriptHash = sh,
                        AssetId = invest.USDX_Asset,
                        Value = amount
                    });
                }
                tx.Outputs = outputs.ToArray();
                tx = this.Operator.Wallet.MakeTransaction(tx, oxSh, oxSh);
                var key = ethAccount.GetPrivateKey(openWallet.WalletPassword);
                var ecKey = new Nethereum.Signer.EthECKey(key, true);
                var signer = new Nethereum.Signer.EthereumMessageSigner();
                var signMessage = signer.EncodeUTF8AndSign(tx.InputOutputHash.ToArray().ToHexString(), ecKey);
                tx.Attributes = new TransactionAttribute[] { new TransactionAttribute { Usage = TransactionAttributeUsage.EthSignature, Data = signMessage.HexToByteArray() } };

                //var sg = tx.EthSignatures.First().CreateStringSignature();

                if (tx.IsNotNull())
                {
                    this.Operator.SignAndSendTx(tx);
                    if (this.Operator != default)
                    {
                        string msg = $"{UIHelper.LocalString("广播发布场外交易商交易", "Relay issue otc dealer transaction")}   {tx.Hash}";
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

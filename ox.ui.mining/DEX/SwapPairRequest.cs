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
using OX.Cryptography.ECC;
using OX.Mining.DEX;

namespace OX.UI.Swap
{
    public partial class SwapPairRequest : DarkDialog
    {
        INotecase Operater;
        UInt256 AssetId;
        //ECPoint SwapPoolPubKey;
        UInt160 BizSH;
        public SwapPairRequest(INotecase notecase, UInt160 bizSH)
        {
            this.Operater = notecase;
            this.BizSH = bizSH;
            InitializeComponent();
            this.DialogButtons = DarkDialogButton.OkCancel;
            this.Text = UIHelper.LocalString("申请交易对", "Apply Swap Pair");
            this.btnOk.Text = UIHelper.LocalString("申请", "Apply");
            this.btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_targetAssetId.Text = UIHelper.LocalString("交易资产Id:", "Swap Asset Id:");
            this.lb_targetBalance.Text = UIHelper.LocalString("交易资产初始量:", "Swap Asset Init:");
            this.lb_oxcBalance.Text = UIHelper.LocalString("OXC初始量:", "OXC Init:");
            this.lb_lockexpire.Text = UIHelper.LocalString("锁定区块数:", "Lock Blocks:");
            this.lb_swapstamp.Text = UIHelper.LocalString("首次交易区块:", "First Swap Block:");
            this.lb_idoPrice.Text = UIHelper.LocalString("IDO价格:", "IDO Price:");
            this.lb_MinLiquidity.Text = UIHelper.LocalString("最少底池量:", "Min Base Pool:");
            this.lb_DividentSlope.Text = UIHelper.LocalString("手续费坡度:", "Bonus Slope:");
            this.rd_DividentSlopeBig.Text = UIHelper.LocalString("大", "Big");
            this.rd_DividentSlopeMedium.Text = UIHelper.LocalString("中", "Medium");
            this.rd_DividentSlopeSmall.Text = UIHelper.LocalString("小", "Small");
            this.lb_idolockexpire.Text = UIHelper.LocalString("IDO锁仓区块:", "IDO Lock Blocks:");
            this.btnOk.Enabled = false;
        }


        private void RegMinerForm_Load(object sender, EventArgs e)
        {
            foreach (var act in this.Operater.Wallet.GetHeldAccounts())
            {
                this.cb_accounts.Items.Add(new AccountListItem(act));
            }
        }
        private void RefreshBalance()
        {
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            var from = this.cb_accounts.SelectedItem as AccountListItem;
            if (UInt256.TryParse(this.tb_targetAssetId.Text, out AssetId))
            {
                var balance = this.Operater.Wallet.GeAccountAvailable(from.Account.ScriptHash, this.AssetId).ToString();
                this.tb_target_balance.Text = balance;
            }
            else
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.tb_oxc_balance.Text = this.Operater.Wallet.GeAccountAvailable(from.Account.ScriptHash, Blockchain.OXC).ToString();
            this.btnOk.Enabled = Check();
        }
        private void tb_targetAssetId_TextChanged(object sender, EventArgs e)
        {
            RefreshBalance();
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            var from = this.cb_accounts.SelectedItem as AccountListItem;
            if (!Fixed8.TryParse(this.tb_target_amount.Text, out Fixed8 targetamount)) return;
            if (!uint.TryParse(this.tb_lockexpire.Text, out uint lockexpire)) return;
            if (!uint.TryParse(this.tb_swapstamp.Text, out uint blocknumber)) return;
            if (blocknumber != 0 && blocknumber < Blockchain.Singleton.Height + 1000) return;
            if (!Fixed8.TryParse(this.tb_idoPrice.Text, out Fixed8 idoprice)) return;
            if (!uint.TryParse(this.tb_MinLiquidity.Text, out uint lq)) return;
            if (!uint.TryParse(this.tb_idolockexpire.Text, out uint idolock)) return;

            if (AssetId.IsNotNull())
            {
                SwapPairAsk reply = new SwapPairAsk
                {
                    Applicant = from.Account.GetKey().PublicKey,
                    TargetAssetId = this.AssetId,
                    PricingAssetId = Blockchain.OXC,
                    LockExpire = lockexpire,
                    LockPercent = 100,
                    Flag = 0,
                    Kind = 0,
                    Stamp = blocknumber,
                };
                var minlq = Fixed8.One * lq;
                if (blocknumber > 0 && targetamount > minlq)
                {
                    DividentSlope ds = DividentSlope.Big_5;
                    if (rd_DividentSlopeBig.Checked) ds = DividentSlope.Big_5;
                    else if (rd_DividentSlopeMedium.Checked) ds = DividentSlope.Medium_6;
                    else if (rd_DividentSlopeSmall.Checked) ds = DividentSlope.Small_8;
                    SwapPairIDO ido = new SwapPairIDO
                    {
                        Price = idoprice,
                        MinLiquidity = minlq,
                        IDOLockExpire = idolock,
                        CommissionPercent = 100,
                        DividentSlope = ds
                    };
                    reply.Mark = ido.ToArray();
                }
                List<TransactionOutput> outputs = new List<TransactionOutput>();
                outputs.Add(new TransactionOutput
                {
                    ScriptHash = this.BizSH,
                    AssetId = this.AssetId,
                    Value = targetamount
                });
                Fixed8.TryParse(this.tb_oxc_amount.Text, out Fixed8 oxcamount);
                outputs.Add(new TransactionOutput
                {
                    ScriptHash = this.BizSH,
                    AssetId = Blockchain.OXC,
                    Value = oxcamount
                });
                SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(from.Account, outputs.ToArray());
                var tx = this.Operater.Wallet.MakeSingleAskTransaction(stw, this.BizSH, (byte)InvestType.SwapPairRequest, reply);
                if (tx.IsNotNull())
                {
                    this.Operater.SignAndSendTx(tx);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("广播申请交易对交易", "Relay Apply Swap Pair transaction")}   {tx.Hash}";
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                    this.Close();
                }
            }
        }

        private void tb_lockexpire_TextChanged(object sender, EventArgs e)
        {
            if (!uint.TryParse(this.tb_lockexpire.Text, out uint lockexpire))
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }

        private void tb_target_amount_TextChanged(object sender, EventArgs e)
        {
            Check();
        }
        bool Check()
        {
            if (!Fixed8.TryParse(this.tb_target_amount.Text, out Fixed8 amount))
            {
                this.btnOk.Enabled = false;
                return false;
            }
            if (amount < Fixed8.One * 1000)
            {
                this.btnOk.Enabled = false;
                return false;
            }
            if (!Fixed8.TryParse(this.tb_target_balance.Text, out Fixed8 balance))
            {
                this.btnOk.Enabled = false;
                return false;
            }
            if (balance < amount)
            {
                this.btnOk.Enabled = false;
                return false;
            }

            if (!Fixed8.TryParse(this.tb_oxc_amount.Text, out Fixed8 amountoxc))
            {
                this.btnOk.Enabled = false;
                return false;
            }
            if (amountoxc < Fixed8.One * 5000)
            {
                this.btnOk.Enabled = false;
                return false;
            }
            if (!Fixed8.TryParse(this.tb_oxc_balance.Text, out Fixed8 oxcbalance))
            {
                this.btnOk.Enabled = false;
                return false;
            }
            if (oxcbalance < amountoxc)
            {
                this.btnOk.Enabled = false;
                return false;
            }
            this.btnOk.Enabled = true;
            return true;
        }

        private void tb_oxc_amount_TextChanged(object sender, EventArgs e)
        {
            Check();
        }

        private void tb_swapstamp_TextChanged(object sender, EventArgs e)
        {
            if (!uint.TryParse(this.tb_swapstamp.Text, out uint blocknumber))
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }

        private void tb_idoPrice_TextChanged(object sender, EventArgs e)
        {
            if (!Fixed8.TryParse(this.tb_idoPrice.Text, out Fixed8 amount))
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }

        private void tb_MinLiquidity_TextChanged(object sender, EventArgs e)
        {
            if (!uint.TryParse(this.tb_MinLiquidity.Text, out uint lq))
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }

        private void cb_accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshBalance();
        }
    }
}

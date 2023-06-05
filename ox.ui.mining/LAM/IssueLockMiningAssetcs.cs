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
using OX.Mining.StakingMining;

namespace OX.UI.LAM
{
    public partial class IssueLockMiningAssetcs : DarkDialog
    {
        INotecase Operater;
        WalletAccount Account;
        UInt256 AssetId;
        UInt160 BizSH;
        public IssueLockMiningAssetcs(INotecase notecase, WalletAccount account, UInt160 bizSH)
        {
            this.Operater = notecase;
            this.Account = account;
            this.BizSH = bizSH;
            InitializeComponent();
            this.DialogButtons = DarkDialogButton.OkCancel;
            this.Text = UIHelper.LocalString("发布自锁挖矿资产", "Issue Self Lock Mining Asset");
            this.btnOk.Text = UIHelper.LocalString("发布", "Issue");
            this.btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_AssetId.Text = UIHelper.LocalString("资产Id:", "Asset Id:");
            this.lb_AssetName.Text = UIHelper.LocalString("资产名称:", "Asset Name:");
            this.lb_cap.Text = UIHelper.LocalString("最大发行量:", "Issue Cap:");
            this.lb_issued.Text = UIHelper.LocalString("已发行量:", "Issued:");
            this.groupBox1.Text = UIHelper.LocalString("设置", "Setting");
            this.lb_minAmount.Text = UIHelper.LocalString("最少锁仓量:", "Min Lock:");
            this.lb_maxAmount.Text = UIHelper.LocalString("最大锁仓量:", "Max Lock:");
            this.lb_100000.Text = UIHelper.LocalString("100000 利率:", "100000 Ratio:");
            this.lb_500000.Text = UIHelper.LocalString("500000 利率:", "500000 Ratio:");
            this.lb_1000000.Text = UIHelper.LocalString("1000000 利率:", "1000000 Ratio:");
            this.lb_2000000.Text = UIHelper.LocalString("2000000 利率:", "2000000 Ratio:");
            this.lb_4000000.Text = UIHelper.LocalString("4000000 利率:", "4000000 Ratio:");
            this.lb_6000000.Text = UIHelper.LocalString("6000000 利率:", "6000000 Ratio:");
            this.btnOk.Enabled = false;
        }


        private void RegMinerForm_Load(object sender, EventArgs e)
        {

        }

        private void tb_targetAssetId_TextChanged(object sender, EventArgs e)
        {
            this.tb_AssetName.Text = String.Empty;
            AssetId = default;
            if (UInt256.TryParse(this.tb_AssetId.Text, out AssetId))
            {
                var assetState = Blockchain.Singleton.Store.GetAssets().TryGet(AssetId);
                this.tb_AssetName.Text = assetState.GetName();
                this.tb_cap.Text = assetState.Amount == -Fixed8.Satoshi ? "+\u221e" : assetState.Amount.ToString();
                this.tb_issued.Text = assetState.Available.ToString();
            }
            else
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            if (AssetId.IsNull()) return;
            if (!Fixed8.TryParse(this.tb_minAmount.Text, out Fixed8 minAmount)) return;
            if (!Fixed8.TryParse(this.tb_maxAmount.Text, out Fixed8 maxAmount)) return;
            if (!uint.TryParse(this.tb_100000.Text, out uint r_100000)) return;
            if (!uint.TryParse(this.tb_500000.Text, out uint r_500000)) return;
            if (!uint.TryParse(this.tb_1000000.Text, out uint r_1000000)) return;
            if (!uint.TryParse(this.tb_2000000.Text, out uint r_2000000)) return;
            if (!uint.TryParse(this.tb_4000000.Text, out uint r_4000000)) return;
            if (!uint.TryParse(this.tb_6000000.Text, out uint r_6000000)) return;

            LockMiningAssetReply reply = new LockMiningAssetReply
            {
                AssetId = AssetId,
                MinAmount = minAmount,
                MaxAmount = maxAmount,
                Interest_100000 = r_100000,
                Interest_500000 = r_500000,
                Interest_1000000 = r_1000000,
                Interest_2000000 = r_2000000,
                Interest_4000000 = r_4000000,
                Interest_6000000 = r_6000000,
            };

            var tx = new ReplyTransaction()
            {
                EdgeVersion = 0x00,
                BizScriptHash = this.BizSH,
                DataType = (byte)InvestType.LockMiningAssetReply,
                Data = reply.ToArray(),
                BizNo = 0,
                To = UInt160.Zero,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            };
            tx = this.Operater.Wallet.MakeTransaction(tx, this.Account.ScriptHash, this.Account.ScriptHash);
            if (tx.IsNotNull())
            {
                this.Operater.SignAndSendTx(tx);
                if (this.Operater != default)
                {
                    string msg = $"{UIHelper.LocalString("广播发行自锁挖矿资产交易", "Relay issue self lock mining asset transaction")}   {tx.Hash}";
                    DarkMessageBox.ShowInformation(msg, "");
                }
                this.Close();
            }
        }




    }
}

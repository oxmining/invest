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
    public partial class IssueMutualLockMiningAssetcs : DarkDialog
    {
        INotecase Operater;
        WalletAccount Account;
        UInt256 AssetId;
        UInt160 BizSH;
        public IssueMutualLockMiningAssetcs(INotecase notecase, WalletAccount account, UInt160 bizSH)
        {
            this.Operater = notecase;
            this.Account = account;
            this.BizSH = bizSH;
            InitializeComponent();
            this.DialogButtons = DarkDialogButton.OkCancel;
            this.Text = UIHelper.LocalString("发布互锁挖矿资产", "Issue Mutual Lock Mining Asset");
            this.btnOk.Text = UIHelper.LocalString("发布", "Issue");
            this.btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_AssetId.Text = UIHelper.LocalString("资产Id:", "Asset Id:");
            this.lb_AssetName.Text = UIHelper.LocalString("资产名称:", "Asset Name:");
            this.lb_cap.Text = UIHelper.LocalString("最大发行量:", "Issue Cap:");
            this.lb_issued.Text = UIHelper.LocalString("已发行量:", "Issued:");
            this.lb_minAmount.Text = UIHelper.LocalString("最少锁仓量:", "Min Lock:");
            this.lb_maxAmount.Text = UIHelper.LocalString("最大锁仓量:", "Max Lock:");
            this.lb_airdropratio.Text = UIHelper.LocalString("出矿率:", "Airdrop Ratio:");
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
            if (!uint.TryParse(this.tb_airdropratio.Text, out uint airdropratio)) return;

            MutualLockMiningAssetReply reply = new MutualLockMiningAssetReply
            {
                AssetId = AssetId,
                MinAmount = minAmount,
                MaxAmount = maxAmount,
                AirdropAmount = airdropratio
            };

            var tx = new ReplyTransaction()
            {
                EdgeVersion = 0x00,
                BizScriptHash = this.BizSH,
                DataType = (byte)InvestType.MutualLockMiningAssetReply,
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
                    string msg = $"{UIHelper.LocalString("广播发行互锁挖矿资产交易", "Relay issue mutual lock mining asset transaction")}   {tx.Hash}";
                    DarkMessageBox.ShowInformation(msg, "");
                }
                this.Close();
            }
        }




    }
}

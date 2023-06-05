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

namespace OX.UI.Swap
{
    public partial class SideSwapPairDetail : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        SideTransaction SideTransaction;
        AssetState assetState;
        string poolAddr = string.Empty;
        public SideSwapPairDetail(INotecase operater, SideTransaction sideTx)
        {
            this.Operator = operater;
            this.SideTransaction = sideTx;
            InitializeComponent();
        }
        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be)
        {

        }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void BeforeOnBlock(Block block) { }
        public void OnBlock(Block block) { }
        public void AfterOnBlock(Block block)
        {

        }
        public void ChangeWallet(INotecase operater)
        {

            this.Operator = operater;

        }
        public void OnRebuild()
        {

        }
        #endregion

        private void RegMinerForm_Load(object sender, EventArgs e)
        {
            var assetId = this.SideTransaction.Data.AsSerializable<UInt256>();
            assetState = Blockchain.Singleton.Store.GetAssets().TryGet(assetId);
            var poolSH = this.SideTransaction.GetContract().ScriptHash;
            poolAddr = poolSH.ToAddress();
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(poolSH, () => null);
            if (acts.IsNotNull())
            {
                var targetBalance = acts.GetBalance(assetId);
                var pricingBalance = acts.GetBalance(Blockchain.OXC);
                this.tb_TargetAssetBalance.Text = targetBalance.ToString();
                this.tb_PricingAssetBalance.Text = pricingBalance.ToString();
            }
            this.Text = $"{assetState.GetName()}  <=>  OXC";
            this.lb_poolAddress.Text = UIHelper.LocalString("交易地址:", "Swap Addr:");
            this.bt_copy.Text = UIHelper.LocalString("复制", "Copy");
            this.btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.btnOk.Text = UIHelper.LocalString("信托交易", "Trust Swap");
            this.tb_poolAddress.Text = poolAddr;
            this.groupBox2.Text = UIHelper.LocalString($"{assetState.GetName()} 资产详情", $"{assetState.GetName()}  Asset Details");
            this.lb_assetId.Text = UIHelper.LocalString("资产Id:", "Asset Id:");
            this.label2.Text = UIHelper.LocalString("发行者:", "Owner:");
            this.label3.Text = UIHelper.LocalString("管理者:", "Admin:");
            this.label4.Text = UIHelper.LocalString("发行上限:", "Cap:");
            this.label5.Text = UIHelper.LocalString("已发行:", "Issued:");
            this.lb_TargetAsset.Text = $"{assetState.GetName()}:";
            this.lb_PricingAsset.Text = $"OXC:";
            this.tb_assetId.Text = assetId.ToString();
            textBox1.Text = Contract.CreateSignatureRedeemScript(assetState.Owner).ToScriptHash().ToAddress();
            textBox2.Text = assetState.Admin.ToAddress();
            textBox3.Text = assetState.Amount == -Fixed8.Satoshi ? "+\u221e" : assetState.Amount.ToString();
            textBox4.Text = assetState.Available.ToString();
            if (this.assetState.AssetId == Blockchain.OXS)
                this.btnOk.Enabled = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //this.Hide();
            new SideSwapTrustRecharge(this.Operator, this.SideTransaction).ShowDialog();
            this.Close();
        }

        private void bt_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(poolAddr);
            string msg = poolAddr + UIHelper.LocalString("  已复制", "  copied");
            DarkMessageBox.ShowError(msg, "");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

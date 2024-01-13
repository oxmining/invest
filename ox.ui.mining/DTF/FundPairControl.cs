using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Mining;
using OX.Wallets;
using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.IO;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.UI.Mining;
using OX.Mining.DEX;
using OX.UI.Mining.DEX;
using OX.Mining.DTF;
using OX.UI.Mining.DTF;

namespace OX.UI.DTF
{
    public partial class FundPairControl : UserControl, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        protected INotecase Operater;
        TrustFundModel TFModel;
        UInt160 TrusteeAddress;
        Fixed8 TotalSubscribeAmount = default;
        public FundPairControl(Module module, INotecase notecase, UInt160 trusteeAddress, TrustFundModel tfModel, Fixed8 totalSubscribeAmount = default)
        {
            this.Module = module;
            this.Operater = notecase;
            this.TrusteeAddress = trusteeAddress;
            this.TFModel = tfModel;
            this.TotalSubscribeAmount = totalSubscribeAmount;
            InitializeComponent();

        }

        private void SwapPairControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
        }

        private void bt_showKLine_Click(object sender, EventArgs e)
        {
            var addr = this.TFModel.SubscribeAddress.ToAddress();
            Clipboard.SetText(addr);
            string msg = addr + UIHelper.LocalString("  已复制", "  copied");
            DarkMessageBox.ShowError(msg, "");
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
        public void BeforeOnBlock(Block block)
        {

        }
        public void OnBlock(Block block)
        {
        }
        public void AfterOnBlock(Block block)
        {

        }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            RefreshTrustBalance();
        }
        public void OnRebuild() { }
        #endregion

        void RefreshTrustBalance()
        {
            this.DoInvoke(() =>
            {
                var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(this.TFModel.TrustAddress, () => null);
                if (acts.IsNotNull())
                {
                    var targetBalance = acts.GetBalance(Blockchain.OXC);
                    this.lb_trustOXC.Text = UIHelper.LocalString($"信托 OXC 余额:{targetBalance}", $"Trust OXC Balance:{targetBalance}");
                }
            });
        }
        void RefreshTotal()
        {
            RefreshTrustBalance();
            this.lb_totalDivident.Text = UIHelper.LocalString($"分红 OXC 总额  :    {this.TFModel.TotalDividendAmount}", $"Total dividend OXC:{this.TFModel.TotalDividendAmount}");
            this.lb_totalSubscribe.Text = UIHelper.LocalString($"认筹 OXC 总额  :    {this.TFModel.TotalSubscribeAmount}", $"Total subscribed OXC:{this.TFModel.TotalSubscribeAmount}");
        }

        private void SideSwapPairControl_Load(object sender, EventArgs e)
        {
            this.lb_trusteeAddress.Text = UIHelper.LocalString($"受托人地址  :  {this.TrusteeAddress.ToAddress()}", $"Trustee Address:{this.TrusteeAddress.ToAddress()}");
            this.lb_trustAddress.Text = UIHelper.LocalString($"信托地址  :  {this.TFModel.TrustAddress.ToAddress()}", $"Trust Address:{this.TFModel.TrustAddress.ToAddress()}");
            this.lb_subscribeAddress.Text = UIHelper.LocalString($"认筹地址  :  {this.TFModel.SubscribeAddress.ToAddress()}", $"Subscribe Address:{this.TFModel.SubscribeAddress.ToAddress()}");
            this.bt_copySubscibeAddress.Text = UIHelper.LocalString("复制认筹地址", "Copy Subscribe Address");
            this.bt_copyTrustAddress.Text = UIHelper.LocalString("复制信托地址", "Copy Trust Address");
            this.bt_trustAssetDetail.Text = UIHelper.LocalString("信托资产明细", "Trust Asset Details");
            this.bt_refresh.Text = UIHelper.LocalString("刷新", "Refresh");
            this.lb_myTotalSubscribe.Text = string.Empty;
            if (this.TotalSubscribeAmount != default)
            {
                this.lb_myTotalSubscribe.Text = UIHelper.LocalString($"我累计认筹 OXC  :  {this.TotalSubscribeAmount}", $"Myself Total Subscribe OXC :{this.TotalSubscribeAmount}");
            }
            this.MouseDown += SwapPairControl_MouseDown;

            RefreshTotal();
        }

        private void lb_totalSubscribe_Click(object sender, EventArgs e)
        {

        }

        private void bt_trustAssetDetail_Click(object sender, EventArgs e)
        {
            new FundAssetDetail(this.Operater, this.TrusteeAddress, this.TFModel).ShowDialog();
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            RefreshTotal();
        }

        private void bt_copyTrustAddress_Click(object sender, EventArgs e)
        {
            var addr = this.TFModel.TrustAddress.ToAddress();
            Clipboard.SetText(addr);
            string msg = addr + UIHelper.LocalString("  已复制", "  copied");
            DarkMessageBox.ShowError(msg, "");
        }
    }
}

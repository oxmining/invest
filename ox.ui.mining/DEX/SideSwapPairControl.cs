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

namespace OX.UI.Swap
{
    public partial class SideSwapPairControl : UserControl, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        protected INotecase Operater;
        SideTransaction SideTx;
        UInt160 PoolAddress;
        SideSwapVolumeMerge LastSwapVolume;
        UInt256 assetId;

        public SideSwapPairControl(Module module, INotecase notecase, SideTransaction st)
        {
            this.Module = module;
            this.Operater = notecase;
            this.SideTx = st;
            PoolAddress = st.GetContract().ScriptHash;
            InitializeComponent();

        }

        private void SwapPairControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
        }





        public void RefreshPrice()
        {
            this.DoInvoke(() =>
            {
                if (this.LastSwapVolume.IsNull())
                {
                    var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                    if (bizPlugin.IsNotNull())
                    {
                        var vom = bizPlugin.Get<SideSwapVolumeMerge>(InvestBizPersistencePrefixes.SideSwapPairLastExchange, this.PoolAddress);
                        if (vom.IsNotNull())
                            this.LastSwapVolume = vom;
                    }
                }
                if (this.LastSwapVolume.IsNotNull())
                {
                    this.lb_price.Text = this.LastSwapVolume.Price.ToString("f6");
                }
            });
        }
        public void RefreshState()
        {
            this.DoInvoke(() =>
            {
                this.lb_priceBalance.Text = String.Empty;
                //var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(this.PoolAddress, () => null);
                //if (acts.IsNotNull() && this.assetId.IsNotNull())
                //{
                //    var targetBalance = acts.GetBalance(this.assetId);
                //    var pricingBalance = acts.GetBalance(Blockchain.OXC);
                //    this.lb_priceBalance.Text = $"{targetBalance}  <=> {pricingBalance}";
                //}
                if (this.LastSwapVolume.IsNotNull())
                {
                    this.lb_priceBalance.Text = $"{this.LastSwapVolume.Volume.TargetBalance}  <=> {this.LastSwapVolume.Volume.PricingBalance}";
                }
            });
        }

        private void bt_showKLine_Click(object sender, EventArgs e)
        {
            if (this.Module is DEXModule sm)
            {
                sm.OpenSideKLine(this.PoolAddress);
            }
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
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin.IsNotNull())
            {
                foreach (var tx in block.Transactions)
                {
                    foreach (var output in tx.Outputs)
                    {
                        if (output.ScriptHash.Equals(this.PoolAddress) && (output.AssetId.Equals(this.assetId) || output.AssetId.Equals(Blockchain.OXC)))
                        {
                            RefreshState();
                        }
                    }
                    if (tx.References.IsNotNullAndEmpty())
                    {
                        foreach (var reference in tx.References)
                        {
                            if (bizPlugin.Side_SwapPairs.TryGetValue(reference.Value.ScriptHash, out SideSwapPairKeyMerge merge) && reference.Value.ScriptHash.Equals(this.PoolAddress))
                            {
                                if (merge.Key.AssetId.Equals(reference.Value.AssetId) || reference.Value.AssetId.Equals(Blockchain.OXC))
                                {
                                    var attr = tx.Attributes.FirstOrDefault(mbox => mbox.Usage == TransactionAttributeUsage.Remark4);
                                    if (attr.IsNotNull())
                                    {
                                        try
                                        {
                                            var sop = attr.Data.AsSerializable<SideSwapVolume>();
                                            if (sop.IsNotNull())
                                            {
                                                this.LastSwapVolume = new SideSwapVolumeMerge { Volume = sop, Price = (decimal)sop.PricingAssetVolume.GetInternalValue() / (decimal)sop.TargetAssetVolume.GetInternalValue() }; ;
                                                RefreshPrice();
                                            }
                                        }
                                        catch { }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            RefreshState();
            RefreshPrice();
        }
        public void OnRebuild() { }
        #endregion

        private void bt_goSwap_Click(object sender, EventArgs e)
        {
            new SideSwapRecharge(this.Operater, this.SideTx).ShowDialog();

        }

        private void bt_detail_Click(object sender, EventArgs e)
        {
            new SideSwapPairDetail(this.Operater, this.SideTx).ShowDialog();
        }

        private void SideSwapPairControl_Load(object sender, EventArgs e)
        {
            assetId = this.SideTx.Data.AsSerializable<UInt256>();
            var assetState = Blockchain.Singleton.Store.GetAssets().TryGet(assetId);
            this.lb_pairname.Text = $"{assetState.GetName()}  <=>  OXC";
            this.bt_showKLine.Text = UIHelper.LocalString("查看K线", "View K-line");
            this.bt_goSwap.Text = UIHelper.LocalString("去交易", "Go Swap");
            this.bt_detail.Text = UIHelper.LocalString("资产详情", "Asset Details");
            this.lb_price.Text = String.Empty;
            this.MouseDown += SwapPairControl_MouseDown;
            RefreshState();
            RefreshPrice();
        }
    }
}

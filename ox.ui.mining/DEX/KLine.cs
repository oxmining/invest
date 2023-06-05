using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX;
using OX.IO;
using OX.Mining;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OX.SmartContract;
using OX.UI.Mining;
using OX.Mining.DEX;

namespace OX.UI.Swap
{
    public partial class KLine : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        SwapPairMerge SwapPairMerge;
        UInt160 HostSH;
        public Module Module { get; set; }
        protected INotecase Operater;
        StockControl SC;
        uint showPairIndex = 0;
        #region Constructor Region

        public KLine(INotecase notecase, SwapPairMerge swapPairMerge)
        {
            this.Operater = notecase;
            this.SwapPairMerge = swapPairMerge;
            this.HostSH = swapPairMerge.PoolAddress;
            InitializeComponent();
            this.DockText = $"{swapPairMerge.TargetAssetState.GetName()} /  OXC";
            this.RoundPanel.SizeChanged += RoundPanel_SizeChanged;
            this.SizeChanged += GameRoom_SizeChanged;

        }

        private void GameRoom_SizeChanged(object sender, EventArgs e)
        {
        }

        protected virtual void RoundPanel_SizeChanged(object sender, System.EventArgs e)
        {
            //foreach (Control ctrl in this.RoundPanel.Controls)
            //{
            //    if (ctrl is DarkTitle dt)
            //        dt.Width = this.RoundPanel.Width - 10;
            //    if (ctrl is Panel pl)
            //        pl.Width = this.RoundPanel.Width - 10;
            //}
            //int w = this.RoundPanel.Size.Width - 30;
            //IEnumerator itr = this.RoundPanel.Controls.GetEnumerator();
            //List<Control> cs = new List<Control>();
            //while (itr.MoveNext())
            //{
            //    cs.Add(itr.Current as Control);
            //}
            //this.RoundPanel.Controls.Clear();
            //foreach (var c in cs)
            //{
            //    c.Width = this.RoundPanel.Width / 4 - 10;
            //    this.RoundPanel.Controls.Add(c);
            //}
        }

        #endregion
        #region Event Handler Region

        public override void Close()
        {
            base.Close();
        }

        #endregion
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
        public void OnBlock(Block block)
        {
            if (block.Index == showPairIndex)
            {
                ShowPairs();
            }
        }
        public void AfterOnBlock(Block block)
        {
            foreach (var tx in block.Transactions)
            {
                foreach (var output in tx.Outputs)
                {
                    if (output.ScriptHash.Equals(HostSH) && (output.AssetId.Equals(this.SwapPairMerge.SwapPairReply.TargetAssetId) || output.AssetId.Equals(Blockchain.OXC)))
                    {
                        showPairIndex = block.Index + 1;
                        //ShowPairs();
                    }
                }
            }
            //ShowPairs();
        }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            ShowPairs();
        }
        public void OnRebuild() { }
        #endregion
        void ShowPairs()
        {
            this.DoInvoke(() =>
            {
                //this.RoundPanel.Controls.Clear();
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin != default)
                {
                    var data = bizPlugin.GetAll<SwapPairExchangeKey, SwapVolumeMerge>(InvestBizPersistencePrefixes.SwapPairExchange, this.HostSH);
                    if (data.IsNotNullAndEmpty())
                    {
                        if (SC.IsNull())
                        {
                            SC = new StockControl(this.Operater, this.SwapPairMerge);
                            SC.Dock = System.Windows.Forms.DockStyle.Fill;
                            SC.Margin = new Padding(2);
                            SC.ResetNullGraph();
                            SC.ShowLeftScale = true;
                            SC.ShowRightScale = true;
                            SC.RightPixSpace = 85;
                            SC.RightOrderSpace = 400;
                            this.RoundPanel.Controls.Add(SC);
                        }
                        SC.Init(data);
                    }
                }
            });
        }

        private void KLine_Load(object sender, EventArgs e)
        {

        }
    }
}

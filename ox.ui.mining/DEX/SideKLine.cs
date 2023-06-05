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
    public partial class SideKLine : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        SideSwapPairKeyMerge SwapPairMerge;
        UInt160 PoolAddress;
        public Module Module { get; set; }
        protected INotecase Operater;
        SideStockControl SC;
        uint showPairIndex = 0;
        UInt256 AssetId;
        AssetState AssetState;
        #region Constructor Region

        public SideKLine(INotecase notecase, SideSwapPairKeyMerge swapPairMerge)
        {
            this.Operater = notecase;
            this.SwapPairMerge = swapPairMerge;
            this.PoolAddress = swapPairMerge.Value.GetContract().ScriptHash;
            this.AssetId = swapPairMerge.Value.Data.AsSerializable<UInt256>();
            this.AssetState = Blockchain.Singleton.Store.GetAssets().TryGet(this.AssetId);
            InitializeComponent();
            this.DockText = $"{AssetState.GetName()} / OXC";
            this.RoundPanel.SizeChanged += RoundPanel_SizeChanged;
            this.SizeChanged += GameRoom_SizeChanged;

        }

        private void GameRoom_SizeChanged(object sender, EventArgs e)
        {
        }

        protected virtual void RoundPanel_SizeChanged(object sender, System.EventArgs e)
        {
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
                    if (output.ScriptHash.Equals(PoolAddress) && (output.AssetId.Equals(this.AssetId) || output.AssetId.Equals(Blockchain.OXC)))
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
                    var data = bizPlugin.GetAll<SwapPairExchangeKey, SideSwapVolumeMerge>(InvestBizPersistencePrefixes.SideSwapPairExchange, this.PoolAddress);
                    if (data.IsNotNullAndEmpty())
                    {
                        if (SC.IsNull())
                        {
                            SC = new SideStockControl(this.Operater, this.SwapPairMerge, this.AssetState);
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

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
using OX.UI.Mining;
using OX.Mining.DEX;

namespace OX.UI.Swap
{
    public partial class SidePairView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        //static Dictionary<uint, RoomLine> Lines = new Dictionary<uint, RoomLine>();
        public Module Module { get; set; }
        protected INotecase Operater;
        Dictionary<UInt160, SideSwapPairControl> Pairs = new Dictionary<UInt160, SideSwapPairControl>();
        uint showPairIndex = 0;
        #region Constructor Region

        public SidePairView()
        {
            InitializeComponent();

            this.DockText = UIHelper.LocalString("边池交易对", "Side Swap Pair");
            this.RoundPanel.SizeChanged += RoundPanel_SizeChanged;
            this.SizeChanged += GameRoom_SizeChanged;
            this.RoundPanel.MouseDown += PairView_MouseDown;
        }

        private void PairView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
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
            foreach (var p in this.Pairs.Values)
                p.OnBappEvent(be);
        }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
            foreach (var p in this.Pairs.Values)
                p.OnCrossBappMessage(message);
        }
        public void HeartBeat(HeartBeatContext context)
        {
            foreach (var p in this.Pairs.Values)
                p.HeartBeat(context);
        }
        public void BeforeOnBlock(Block block)
        {
            foreach (var p in this.Pairs.Values)
                p.BeforeOnBlock(block);
        }
        public void OnBlock(Block block)
        {
            foreach (var p in this.Pairs.Values)
                p.OnBlock(block);
            if (block.Index == showPairIndex)
            {
                ShowPairs();
            }
        }
        public void AfterOnBlock(Block block)
        {
            foreach (var tx in block.Transactions)
            {
                if (tx is SideTransaction st)
                {
                    if (st.VerifyRegSideSwap(out UInt256 assetId))
                    {
                        var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                        if (bizPlugin != default)
                        {
                            var settings = bizPlugin.GetAllInvestSettings();
                            var feesetting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { InvestSettingTypes.SidePairRegFee }));
                            if (feesetting.Equals(new KeyValuePair<byte[], InvestSettingRecord>())) return;
                            var fee = Fixed8.FromDecimal(decimal.Parse(feesetting.Value.Value));
                            if (st.VerifyRegSideSwapFee(fee))
                            {
                                showPairIndex = block.Index + 1;
                            }
                        }
                    }
                }
            }
            foreach (var p in this.Pairs.Values)
                p.AfterOnBlock(block);
        }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            ShowPairs();
            foreach (var p in this.Pairs.Values)
                p.ChangeWallet(operater);
        }
        public void OnRebuild()
        {
            foreach (var p in this.Pairs.Values)
                p.OnRebuild();
        }
        #endregion
        void ShowPairs()
        {
            this.DoInvoke(() =>
            {
                this.RoundPanel.Controls.Clear();
                this.Pairs.Clear();
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin != default)
                {
                    foreach (var p in bizPlugin.GetAll<SideSwapPairKey, SideTransaction>(InvestBizPersistencePrefixes.SideSwapPair))
                    {
                        var c = new SideSwapPairControl(this.Module, this.Operater, p.Value);
                        this.RoundPanel.Controls.Add(c);
                        this.Pairs[p.Key.PoolAddress] = c;
                    }
                }
            });
        }
    }
}

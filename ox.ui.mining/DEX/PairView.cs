using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
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
    public partial class PairView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        //static Dictionary<uint, RoomLine> Lines = new Dictionary<uint, RoomLine>();
        public Module Module { get; set; }
        protected INotecase Operater;
        Dictionary<UInt160, SwapPairControl> Pairs = new Dictionary<UInt160, SwapPairControl>();
        uint showPairIndex = 0;
        #region Constructor Region

        public PairView()
        {
            InitializeComponent();

            this.DockText = UIHelper.LocalString("主池交易对", "Main Swap Pair");
            this.RoundPanel.SizeChanged += RoundPanel_SizeChanged;
            this.SizeChanged += GameRoom_SizeChanged;
            this.RoundPanel.MouseDown += PairView_MouseDown;
        }

        private void PairView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DarkContextMenu menu = new DarkContextMenu();
                //var sm = new ToolStripMenuItem(UIHelper.LocalString("申请交易对", "Apply Exchange Pair"));
                //sm.Click += Sm_Click1;
                //menu.Items.Add(sm);
                if (invest.BizAddresses.Keys.FirstOrDefault(m => this.Operater.Wallet.ContainsAndHeld(m)).IsNotNull())
                {
                    //var sm = new ToolStripMenuItem(UIHelper.LocalString("查看交易对申请", "View Exchange Pair Application"));
                    //sm.Click += Sm_Click2;
                    //menu.Items.Add(sm);
                    var sm = new ToolStripMenuItem(UIHelper.LocalString("发布交易对", "Issue Exchange Pair"));
                    sm.Click += Sm_Click;
                    menu.Items.Add(sm);
                }
                if (menu.Items.Count > 0)
                    menu.Show(this, e.Location);
            }
        }

        //private void Sm_Click2(object sender, EventArgs e)
        //{
        //    new ViewSwapPair(this.Operater).ShowDialog();
        //}

        //private void Sm_Click1(object sender, EventArgs e)
        //{
        //    var sh = invest.BizAddresses.Keys.FirstOrDefault();
        //    new SwapPairRequest(this.Operater, sh).ShowDialog();
        //}

        private void Sm_Click(object sender, EventArgs e)
        {
            var sh = invest.MasterAccountAddress;
            var account = this.Operater.Wallet.GetAccount(sh);
            new IssueSwapPair(this.Operater, account, sh).ShowDialog();
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
                    if (st.VerifyRegMainSwap(out UInt256 _, out SwapPairReply swapPairReply))
                    {
                        showPairIndex = block.Index + 1;
                    }
                }
                else if (tx is ReplyTransaction rt)
                {
                    if (rt.DataType == (byte)InvestType.SwapPairStateReply)
                        showPairIndex = block.Index + 1;
                    //ShowPairs();
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
                    foreach (var p in bizPlugin.GetAll<UInt160, SwapPairMerge>(InvestBizPersistencePrefixes.SwapPair).OrderByDescending(m => m.Value.SwapPairReply.TargetAssetId.Equals(Blockchain.OXS)).ThenByDescending(m => m.Value.SwapPairReply.TargetAssetId==invest.USDX_Asset).ThenByDescending(m => m.Value.Index))
                    {
                        if (bizPlugin.SwapPairStates.TryGetValue(p.Key, out SwapPairStateReply stateReply) && stateReply.Flag != 1)
                            continue;
                        var c = new SwapPairControl(this.Module, this.Operater, p.Key, p.Value);
                        this.RoundPanel.Controls.Add(c);
                        this.Pairs[p.Key] = c;
                    }
                }
            });
        }
    }
}

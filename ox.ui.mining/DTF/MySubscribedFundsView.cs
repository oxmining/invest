using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Cryptography.ECC;
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
using OX.Mining.DTF;
using System.Windows.Forms.VisualStyles;
using OX.IO.Data.LevelDB;
using System.Runtime.CompilerServices;

namespace OX.UI.DTF
{
    public partial class MySubscribedFundsView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        //static Dictionary<uint, RoomLine> Lines = new Dictionary<uint, RoomLine>();
        public Module Module { get; set; }
        protected INotecase Operater;
        Dictionary<UInt160, FundPairControl> Funds = new Dictionary<UInt160, FundPairControl>();
        uint showFundIndex = 0;
        #region Constructor Region

        public MySubscribedFundsView()
        {
            InitializeComponent();

            this.DockText = UIHelper.LocalString("我认筹的的基金", "My Subscribed Funds");
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
            foreach (var p in this.Funds.Values)
                p.OnBappEvent(be);
        }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
            foreach (var p in this.Funds.Values)
                p.OnCrossBappMessage(message);
        }
        public void HeartBeat(HeartBeatContext context)
        {
            foreach (var p in this.Funds.Values)
                p.HeartBeat(context);
        }
        public void BeforeOnBlock(Block block)
        {
            foreach (var p in this.Funds.Values)
                p.BeforeOnBlock(block);
        }
        public void OnBlock(Block block)
        {
            foreach (var p in this.Funds.Values)
                p.OnBlock(block);
            if (block.Index == showFundIndex)
            {
                ShowFunds();
            }
        }
        public void AfterOnBlock(Block block)
        {
            foreach (var tx in block.Transactions)
            {
                if (WatchIDOSubscribe(tx))
                {
                    showFundIndex = block.Index + 1;
                }
            }
            foreach (var p in this.Funds.Values)
                p.AfterOnBlock(block);
        }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            ShowFunds();
            foreach (var p in this.Funds.Values)
                p.ChangeWallet(operater);
        }
        public void OnRebuild()
        {
            foreach (var p in this.Funds.Values)
                p.OnRebuild();
        }
        #endregion
        public bool WatchIDOSubscribe(Transaction tx)
        {
            if (tx.IsNull()) return false;
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin.IsNull()) return false;
            if (this.Operater.IsNull()) return false;
            if (this.Operater.Wallet.IsNull()) return false;
            foreach (var output in tx.Outputs)
            {
                if (output.AssetId == Blockchain.OXC && output.Value >= Fixed8.OXT)
                {
                    var tf = bizPlugin.TrustFunds.Where(m => m.Value.SubscribeAddress.Equals(output.ScriptHash)).FirstOrDefault();
                    if (!tf.Equals(new KeyValuePair<UInt160, TrustFundModel>()))
                    {
                        var rfsOutput = tx.References.Values.Where(m => m.AssetId == Blockchain.OXC).OrderByDescending(m => m.Value).FirstOrDefault();
                        if (rfsOutput.IsNotNull())
                        {
                            var fromsh = rfsOutput.ScriptHash;
                            if (!fromsh.Equals(tf.Value.TrustAddress) && !fromsh.Equals(tf.Value.SubscribeAddress))
                            {
                                if (this.Operater.Wallet.ContainsAndHeld(rfsOutput.ScriptHash)) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        void ShowFunds()
        {
            this.DoInvoke(() =>
            {
                this.RoundPanel.Controls.Clear();
                this.Funds.Clear();
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin != default && this.Operater.IsNotNull() && this.Operater.Wallet.IsNotNull())
                {
                    Dictionary<UInt160, Fixed8> funds = new Dictionary<UInt160, Fixed8>();
                    foreach (var act in this.Operater.Wallet.GetHeldAccounts())
                    {
                        foreach (var g in bizPlugin.GetAllDTFIDOSummary(act.ScriptHash))
                        {
                            var amount = g.Value;
                            if (funds.TryGetValue(g.Key.TrusteeAddress, out var balance))
                            {
                                amount += balance;
                            }
                            funds[g.Key.TrusteeAddress] = amount;
                        }
                    }
                    foreach (var f in funds)
                    {
                        if (bizPlugin.TrustFunds.TryGetValue(f.Key, out TrustFundModel model))
                        {
                            var c = new FundPairControl(this.Module, this.Operater, f.Key, model, f.Value);
                            this.RoundPanel.Controls.Add(c);
                            this.Funds[f.Key] = c;
                        }
                    }

                }
            });
        }
    }
}

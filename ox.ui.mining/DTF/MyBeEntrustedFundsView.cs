using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Forms;
using OX.Wallets.UI;
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

namespace OX.UI.DTF
{
    public partial class MyBeEntrustedFundsView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        //static Dictionary<uint, RoomLine> Lines = new Dictionary<uint, RoomLine>();
        public Module Module { get; set; }
        protected INotecase Operater;
        Dictionary<UInt160, FundPairControl> Funds = new Dictionary<UInt160, FundPairControl>();
        uint showFundIndex = 0;
        #region Constructor Region

        public MyBeEntrustedFundsView()
        {
            InitializeComponent();

            this.DockText = UIHelper.LocalString("我受托的基金", "My Be Entrusted Funds");
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
                if (tx is AssetTrustTransaction att)
                {
                    if (att.TryVerifyRegTrustFund(out ECPoint trustee))
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
        void ShowFunds()
        {
            this.DoInvoke(() =>
            {
                this.RoundPanel.Controls.Clear();
                this.Funds.Clear();
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin != default && this.Operater.IsNotNull() && this.Operater.Wallet.IsNotNull())
                {
                    foreach (var p in bizPlugin.TrustFunds.OrderByDescending(m => m.Value.TotalSubscribeAmount))
                    {
                        if (this.Operater.Wallet.ContainsAndHeld(p.Key))
                        {
                            var c = new FundPairControl(this.Module, this.Operater, p.Key, p.Value);
                            this.RoundPanel.Controls.Add(c);
                            this.Funds[p.Key] = c;
                        }
                    }
                }
            });
        }
    }
}

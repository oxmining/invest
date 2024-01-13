using OX.Bapps;
using OX.IO;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.SmartContract;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OX.Ledger;
using OX.IO.Json;
using System.Net.WebSockets;
using OX.Mining;
using OX.UI.Mining;
using System.Security.Principal;
using OX.Mining.DEX;
using OX.UI.OTC;
using OX.UI.Swap;
using OX.UI.DTF;

namespace OX.UI.Mining.DTF
{
    public class DTFModule : Module
    {
        public override string ModuleName { get { return "dtfmodule"; } }
        public override uint Index { get { return 15; } }

        public INotecase Operater;
        protected FundsView FundsView;
        protected MyBeEntrustedFundsView EntrustedFundsView;
        protected MySubscribedFundsView SubscribedFundsView;
        public DTFModule(Bapp bapp) : base(bapp)
        {
        }
        public override void InitEvents() { }
        public override void InitWindows()
        {
            ToolStripMenuItem swapMenu = new ToolStripMenuItem();
            swapMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);

            swapMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            swapMenu.Name = "dtfMenu";
            swapMenu.Size = new System.Drawing.Size(39, 21);
            swapMenu.Text = UIHelper.LocalString("信托基金", "Trust Fund");

            //所有基金
            ToolStripMenuItem viewAllFundsMenu = new ToolStripMenuItem();
            viewAllFundsMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            viewAllFundsMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            viewAllFundsMenu.Name = "viewAllFundsMenu";
            viewAllFundsMenu.ShortcutKeys = Keys.Control | Keys.A;
            viewAllFundsMenu.Size = new System.Drawing.Size(170, 22);
            viewAllFundsMenu.Text = UIHelper.LocalString("所有基金", "All Funds");
            viewAllFundsMenu.Click += ViewFundsMenu_Click;

            //我受托的基金
            ToolStripMenuItem myEntrustedFundsMenu = new ToolStripMenuItem();
            myEntrustedFundsMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            myEntrustedFundsMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            myEntrustedFundsMenu.Name = "myEntrustedFundsMenu";
            myEntrustedFundsMenu.ShortcutKeys = Keys.Control | Keys.M;
            myEntrustedFundsMenu.Size = new System.Drawing.Size(170, 22);
            myEntrustedFundsMenu.Text = UIHelper.LocalString("我受托的基金", "My Be Entrusted Funds");
            myEntrustedFundsMenu.Click += MyEntrustedFundsMenu_Click;

            //我认筹的的基金
            ToolStripMenuItem mySubscribedFundsMenu = new ToolStripMenuItem();
            mySubscribedFundsMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            mySubscribedFundsMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            mySubscribedFundsMenu.Name = "mySubscribedFundsMenu";
            mySubscribedFundsMenu.ShortcutKeys = Keys.Control | Keys.S;
            mySubscribedFundsMenu.Size = new System.Drawing.Size(170, 22);
            mySubscribedFundsMenu.Text = UIHelper.LocalString("我认筹的的基金", "My Subscribed Funds");
            mySubscribedFundsMenu.Click += MySubscribedFundsMenu_Click;

            //注册信托基金
            ToolStripMenuItem regTrustFundMenu = new ToolStripMenuItem();
            regTrustFundMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            regTrustFundMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            regTrustFundMenu.Name = "regTrustFundMenu";
            regTrustFundMenu.ShortcutKeys = Keys.Control | Keys.R;
            regTrustFundMenu.Size = new System.Drawing.Size(170, 22);
            regTrustFundMenu.Text = UIHelper.LocalString("注册信托基金", "Register Trust Fund");
            regTrustFundMenu.Click += RegTrustFundMenu_Click;


            swapMenu.DropDownItems.AddRange(new ToolStripItem[] {
                viewAllFundsMenu,
                myEntrustedFundsMenu,
                mySubscribedFundsMenu,
                regTrustFundMenu
            });
            Container.TopMenus.Items.AddRange(new ToolStripItem[] {
            swapMenu});
        }

        private void RegTrustFundMenu_Click(object sender, EventArgs e)
        {
            new NewTrustFund(Operater).ShowDialog();
        }

        private void MySubscribedFundsMenu_Click(object sender, EventArgs e)
        {
            if (SubscribedFundsView == default)
            {
                SubscribedFundsView = new MySubscribedFundsView();
                SubscribedFundsView.Module = this;
                if (Operater != default && Operater.Wallet != default)
                    SubscribedFundsView.ChangeWallet(Operater);
            }
            Container.DockPanel.AddContent(SubscribedFundsView);
        }

        private void MyEntrustedFundsMenu_Click(object sender, EventArgs e)
        {
            if (EntrustedFundsView == default)
            {
                EntrustedFundsView = new MyBeEntrustedFundsView();
                EntrustedFundsView.Module = this;
                if (Operater != default && Operater.Wallet != default)
                    EntrustedFundsView.ChangeWallet(Operater);
            }
            Container.DockPanel.AddContent(EntrustedFundsView);
        }

        private void ViewFundsMenu_Click(object sender, EventArgs e)
        {
            if (FundsView == default)
            {
                FundsView = new FundsView();
                FundsView.Module = this;
                if (Operater != default && Operater.Wallet != default)
                    FundsView.ChangeWallet(Operater);
            }
            Container.DockPanel.AddContent(FundsView);
        }




        public override void OnCrossBappMessage(CrossBappMessage message)
        {
            if (this.FundsView.IsNotNull())
                this.FundsView.OnCrossBappMessage(message);
            if (this.EntrustedFundsView.IsNotNull())
                this.EntrustedFundsView.OnCrossBappMessage(message);
            if (this.SubscribedFundsView.IsNotNull())
                this.SubscribedFundsView.OnCrossBappMessage(message);
        }


        public override void OnBappEvent(BappEvent be)
        {
            if (this.FundsView.IsNotNull())
                this.FundsView.OnBappEvent(be);
            if (this.EntrustedFundsView.IsNotNull())
                this.EntrustedFundsView.OnBappEvent(be);
            if (this.SubscribedFundsView.IsNotNull())
                this.SubscribedFundsView.OnBappEvent(be);
        }

        public override void HeartBeat(HeartBeatContext context)
        {
            if (this.FundsView.IsNotNull())
                this.FundsView.HeartBeat(context);
            if (this.EntrustedFundsView.IsNotNull())
                this.EntrustedFundsView.HeartBeat(context);
            if (this.SubscribedFundsView.IsNotNull())
                this.SubscribedFundsView.HeartBeat(context);
        }
        public override void OnBlock(Block block)
        {
            if (this.FundsView.IsNotNull())
                this.FundsView.OnBlock(block);
            if (this.EntrustedFundsView.IsNotNull())
                this.EntrustedFundsView.OnBlock(block);
            if (this.SubscribedFundsView.IsNotNull())
                this.SubscribedFundsView.OnBlock(block);

        }
        public override void BeforeOnBlock(Block block)
        {
            if (this.FundsView.IsNotNull())
                this.FundsView.BeforeOnBlock(block);
            if (this.EntrustedFundsView.IsNotNull())
                this.EntrustedFundsView.BeforeOnBlock(block);
            if (this.SubscribedFundsView.IsNotNull())
                this.SubscribedFundsView.BeforeOnBlock(block);
        }
        public override void AfterOnBlock(Block block)
        {
            if (this.FundsView.IsNotNull())
                this.FundsView.AfterOnBlock(block);
            if (this.EntrustedFundsView.IsNotNull())
                this.EntrustedFundsView.AfterOnBlock(block);
            if (this.SubscribedFundsView.IsNotNull())
                this.SubscribedFundsView.AfterOnBlock(block);
        }
        public override void ChangeWallet(INotecase operater)
        {
            Operater = operater;
            if (this.FundsView.IsNotNull())
                this.FundsView.ChangeWallet(operater);
            if (this.EntrustedFundsView.IsNotNull())
                this.EntrustedFundsView.ChangeWallet(operater);
            if (this.SubscribedFundsView.IsNotNull())
                this.SubscribedFundsView.ChangeWallet(operater);
        }
        public override void OnRebuild()
        {

        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {

        }
    }
}

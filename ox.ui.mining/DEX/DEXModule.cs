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

namespace OX.UI.Mining.DEX
{
    public class DEXModule : Module
    {
        public override string ModuleName { get { return "dexmodule"; } }
        public override uint Index { get { return 13; } }

        public INotecase Operater;
        protected PairView PairView;
        protected SidePairView SidePairView;
        protected SwapRule SwapRule;
        protected RegSideSwapPair RegSideSwapPair;
        Dictionary<UInt160, KLine> KLines = new Dictionary<UInt160, KLine>();
        Dictionary<UInt160, SideKLine> SideKLines = new Dictionary<UInt160, SideKLine>();
        public DEXModule(Bapp bapp) : base(bapp)
        {
        }
        public override void InitEvents() { }
        public override void InitWindows()
        {
            ToolStripMenuItem swapMenu = new ToolStripMenuItem();
            swapMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);

            swapMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            swapMenu.Name = "swapMenu";
            swapMenu.Size = new System.Drawing.Size(39, 21);
            swapMenu.Text = UIHelper.LocalString("兑换", "Swap");

            //ToolStripMenuItem viewPairMenu = new ToolStripMenuItem();
            //viewPairMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            //viewPairMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //viewPairMenu.Name = "viewPairMenu";
            //viewPairMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            //viewPairMenu.Size = new System.Drawing.Size(170, 22);
            //viewPairMenu.Text = UIHelper.LocalString("链上交易", "On-Chain Swap");

            //查看主池交易对
            ToolStripMenuItem viewMainPairMenu = new ToolStripMenuItem();
            viewMainPairMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            viewMainPairMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            viewMainPairMenu.Name = "viewMainPairMenu";
            viewMainPairMenu.ShortcutKeys = Keys.Control | Keys.M;
            viewMainPairMenu.Size = new System.Drawing.Size(170, 22);
            viewMainPairMenu.Text = UIHelper.LocalString("主池交易对", "Main Swap Pair");
            viewMainPairMenu.Click += ViewPairMenu_Click;
            //viewPairMenu.DropDownItems.Add(viewMainPairMenu);
            //查看边池交易对
            ToolStripMenuItem viewSidePairMenu = new ToolStripMenuItem();
            viewSidePairMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            viewSidePairMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            viewSidePairMenu.Name = "viewSidePairMenu";
            viewSidePairMenu.ShortcutKeys = Keys.Control | Keys.S;
            viewSidePairMenu.Size = new System.Drawing.Size(170, 22);
            viewSidePairMenu.Text = UIHelper.LocalString("边池交易对", "Side Swap Pair");
            viewSidePairMenu.Click += ViewSidePairMenu_Click;
            //viewPairMenu.DropDownItems.Add(viewSidePairMenu);

            //查看注册边池交易对
            ToolStripMenuItem regSidePairMenu = new ToolStripMenuItem();
            regSidePairMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            regSidePairMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            regSidePairMenu.Name = "regSidePairMenu";
            regSidePairMenu.ShortcutKeys = Keys.Control | Keys.G;
            regSidePairMenu.Size = new System.Drawing.Size(170, 22);
            regSidePairMenu.Text = UIHelper.LocalString("注册边池交易对", "Register Side Exchange Pair");
            regSidePairMenu.Click += RegSidePairMenu_Click;
            //viewPairMenu.DropDownItems.Add(regSidePairMenu);
            //挖矿规则
            ToolStripMenuItem ruleSettingMenu = new ToolStripMenuItem();
            ruleSettingMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            ruleSettingMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            ruleSettingMenu.Name = "custodyAccountsMenu";
            ruleSettingMenu.ShortcutKeys = Keys.Control | Keys.R;
            ruleSettingMenu.Size = new System.Drawing.Size(170, 22);
            ruleSettingMenu.Text = UIHelper.LocalString("兑换规则", "Swap Rule");
            ruleSettingMenu.Click += RuleSettingMenu_Click;

            //挖矿社区
            ToolStripMenuItem swapCommunityMenu = new ToolStripMenuItem();
            swapCommunityMenu.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            swapCommunityMenu.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            //swapCommunityMenu.Image = global::Example.Icons.NewFile_6276;
            swapCommunityMenu.Name = "swapCommunityMenu";
            swapCommunityMenu.ShortcutKeys = Keys.Control | Keys.C;
            swapCommunityMenu.Size = new System.Drawing.Size(170, 22);
            swapCommunityMenu.Text = UIHelper.LocalString("兑换社区", "Swap Community");
            swapCommunityMenu.Click += SwapCommunityMenu_Click;

            swapMenu.DropDownItems.AddRange(new ToolStripItem[] {
                //viewPairMenu,
                viewMainPairMenu,
                viewSidePairMenu,
                regSidePairMenu,
                  new ToolStripSeparator(),
                ruleSettingMenu,
                swapCommunityMenu
            });
            Container.TopMenus.Items.AddRange(new ToolStripItem[] {
            swapMenu});
        }

        private void RegSidePairMenu_Click(object sender, EventArgs e)
        {
            new RegSideSwapPair(Operater).ShowDialog();
        }

        private void ViewSidePairMenu_Click(object sender, EventArgs e)
        {
            if (SidePairView == default)
            {
                SidePairView = new SidePairView();
                SidePairView.Module = this;
                if (Operater != default && Operater.Wallet != default)
                    SidePairView.ChangeWallet(Operater);
            }
            Container.DockPanel.AddContent(SidePairView);
        }

        private void SwapCommunityMenu_Click(object sender, EventArgs e)
        {
            Bapp.PushCrossBappMessage(new CrossBappMessage() { MessageType = 1, Attachment = invest.DEXOfficalEventBoardId });

        }

        private void RuleSettingMenu_Click(object sender, EventArgs e)
        {
            if (SwapRule == default)
            {
                SwapRule = new SwapRule();
                SwapRule.Module = this;
                if (Operater != default && Operater.Wallet != default)
                    SwapRule.ChangeWallet(Operater);
                Container.ToolWindows.Add(SwapRule);
            }
            Container.DockPanel.AddContent(SwapRule);
        }

        private void ViewPairMenu_Click(object sender, EventArgs e)
        {
            if (PairView == default)
            {
                PairView = new PairView();
                PairView.Module = this;
                if (Operater != default && Operater.Wallet != default)
                    PairView.ChangeWallet(Operater);
            }
            Container.DockPanel.AddContent(PairView);
        }

        private void AsBizMenu_Click(object sender, EventArgs e)
        {

        }

      
       
        public override void OnCrossBappMessage(CrossBappMessage message)
        {

        }


        public override void OnBappEvent(BappEvent be)
        {
            if (PairView != default)
                PairView.OnBappEvent(be);
            if (SidePairView != default)
                SidePairView.OnBappEvent(be);
            if (SwapRule != default)
                SwapRule.OnBappEvent(be);
            foreach (var k in KLines.Values)
                k.OnBappEvent(be);
            foreach (var k in SideKLines.Values)
                k.OnBappEvent(be);
        }

        public override void HeartBeat(HeartBeatContext context)
        {
            if (PairView != default)
                PairView.HeartBeat(context);
            if (SidePairView != default)
                SidePairView.HeartBeat(context);
            if (SwapRule != default)
                SwapRule.HeartBeat(context);
            foreach (var k in KLines.Values)
                k.HeartBeat(context);
            foreach (var k in SideKLines.Values)
                k.HeartBeat(context);
        }
        public override void OnBlock(Block block)
        {
            if (PairView != default)
                PairView.OnBlock(block);
            if (SidePairView != default)
                SidePairView.OnBlock(block);
            if (SwapRule != default)
                SwapRule.OnBlock(block);
            foreach (var k in KLines.Values)
                k.OnBlock(block);
            foreach (var k in SideKLines.Values)
                k.OnBlock(block);
        }
        public override void BeforeOnBlock(Block block)
        {
            if (PairView != default)
                PairView.BeforeOnBlock(block);
            if (SidePairView != default)
                SidePairView.BeforeOnBlock(block);
            if (SwapRule != default)
                SwapRule.BeforeOnBlock(block);
            foreach (var k in KLines.Values)
                k.BeforeOnBlock(block);
            foreach (var k in SideKLines.Values)
                k.BeforeOnBlock(block);
        }
        public override void AfterOnBlock(Block block)
        {
            if (PairView != default)
                PairView.AfterOnBlock(block);
            if (SidePairView != default)
                SidePairView.AfterOnBlock(block);
            if (SwapRule != default)
                SwapRule.AfterOnBlock(block);
            foreach (var k in KLines.Values)
                k.AfterOnBlock(block);
            foreach (var k in SideKLines.Values)
                k.AfterOnBlock(block);
        }
        public override void ChangeWallet(INotecase operater)
        {
            Operater = operater;
            if (PairView != default)
                PairView.ChangeWallet(operater);
            if (SidePairView != default)
                SidePairView.ChangeWallet(operater);
            if (SwapRule != default)
                SwapRule.ChangeWallet(operater);
            foreach (var k in KLines.Values)
                k.ChangeWallet(operater);
            foreach (var k in SideKLines.Values)
                k.ChangeWallet(operater);
        }
        public override void OnRebuild()
        {
            if (PairView != default)
                PairView.OnRebuild();
            if (SidePairView != default)
                SidePairView.OnRebuild();
            if (SwapRule != default)
                SwapRule.OnRebuild();
            foreach (var k in KLines.Values)
                k.OnRebuild();
            foreach (var k in SideKLines.Values)
                k.OnRebuild();
        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {

        }
        public void OpenKLine(UInt160 host)
        {
            if (!KLines.TryGetValue(host, out KLine kl))
            {
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin.IsNull()) return;
                if (bizPlugin.SwapPairs.TryGetValue(host, out SwapPairMerge merge))
                {
                    kl = new KLine(Operater, merge);
                    kl.Module = this;
                    if (Operater != default && Operater.Wallet != default)
                        kl.ChangeWallet(Operater);
                    KLines[host] = kl;
                }
            }
            if (kl.IsNotNull())
            {
                Container.DockPanel.AddContent(kl);
            }
        }
        public void OpenSideKLine(UInt160 poolAddress)
        {
            if (!SideKLines.TryGetValue(poolAddress, out SideKLine kl))
            {
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin.IsNull()) return;
                if (bizPlugin.Side_SwapPairs.TryGetValue(poolAddress, out SideSwapPairKeyMerge merge))
                {
                    kl = new SideKLine(Operater, merge);
                    kl.Module = this;
                    if (Operater != default && Operater.Wallet != default)
                        kl.ChangeWallet(Operater);
                    SideKLines[poolAddress] = kl;
                }
            }
            if (kl.IsNotNull())
            {
                Container.DockPanel.AddContent(kl);
            }
        }
    }
}

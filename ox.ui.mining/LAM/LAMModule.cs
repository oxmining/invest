using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets.UI;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using System.ComponentModel.Design.Serialization;
using System.Security.Claims;
using OX.Bapps;
using OX.Mining;
using OX.IO.Json;
using OX.UI.LAM;

namespace OX.UI.LAM
{
    public class LAMModule : Module
    {
        public override string ModuleName { get { return "lammodule"; } }
        public override uint Index { get { return 12; } }

        protected INotecase Operater;
        protected LockMingingAssets LockMingingAssets;
        protected MyLockMingingRecords MyLockMingingRecords;
        protected MyLockInterestRecords MyLockInterestRecords;
        protected MyMutualLockNodes MyMutualLockNodes;
        protected MutualLockMingingAssets MutualLockMingingAssets;
        protected MyMutualLockMingingRecords MyMutualLockMingingRecords;
        protected MyMutualLockInterestRecords MyMutualLockInterestRecords;
        protected LevelLockMingingAssets LevelLockMingingAssets;
        protected MyLevelLockInterestRecords MyLevelLockInterestRecords;
        protected MyLevelLockInMingingRecords MyLevelLockInMingingRecords;
        protected MyLevelLockOutMingingRecords MyLevelLockOutMingingRecords;
        protected LAMRule RuleSetting;
        public LAMModule(Bapp bapp) : base(bapp)
        {
        }

        public override void InitEvents() { }
        public override void InitWindows()
        {
            //if (!State)
            //    return;
            ToolStripMenuItem investMenu = new ToolStripMenuItem();
            investMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));

            investMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            investMenu.Name = "lamMenu";
            investMenu.Size = new System.Drawing.Size(39, 21);
            investMenu.Text = UIHelper.LocalString("质押挖矿", "Staking Mining");
            //矿机
            ToolStripMenuItem MinerMenu = new ToolStripMenuItem();
            MinerMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            MinerMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            MinerMenu.Name = "MinerMenu";
            MinerMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            MinerMenu.Size = new System.Drawing.Size(170, 22);
            MinerMenu.Text = UIHelper.LocalString("矿机", "Miner");

            //计算矿机种子
            ToolStripMenuItem viewMutualNodeSeedMenu = new ToolStripMenuItem();
            viewMutualNodeSeedMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            viewMutualNodeSeedMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            viewMutualNodeSeedMenu.Name = "viewMutualNodeSeedMenu";
            viewMutualNodeSeedMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            viewMutualNodeSeedMenu.Size = new System.Drawing.Size(170, 22);
            viewMutualNodeSeedMenu.Text = UIHelper.LocalString("计算矿机种子", "Calculation Miner  Seed");
            viewMutualNodeSeedMenu.Click += ViewMutualNodeSeedMenu_Click;
            MinerMenu.DropDownItems.Add(viewMutualNodeSeedMenu);

            //注册互锁节点
            ToolStripMenuItem viewTotalLockVolumeMenu = new ToolStripMenuItem();
            viewTotalLockVolumeMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            viewTotalLockVolumeMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            viewTotalLockVolumeMenu.Name = "viewTotalLockVolumeMenu";
            viewTotalLockVolumeMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            viewTotalLockVolumeMenu.Size = new System.Drawing.Size(170, 22);
            viewTotalLockVolumeMenu.Text = UIHelper.LocalString("查询总锁仓量", "View Total Lock Volume");
            viewTotalLockVolumeMenu.Click += ViewTotalLockVolumeMenu_Click;
            MinerMenu.DropDownItems.Add(viewTotalLockVolumeMenu);

            //我的矿机
            ToolStripMenuItem myMutualLockNodeMenu = new ToolStripMenuItem();
            myMutualLockNodeMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myMutualLockNodeMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myMutualLockNodeMenu.Name = "myMutualLockNodeMenu";
            myMutualLockNodeMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            myMutualLockNodeMenu.Size = new System.Drawing.Size(170, 22);
            myMutualLockNodeMenu.Text = UIHelper.LocalString("我的矿机", "My Miner");
            myMutualLockNodeMenu.Click += MyMutualLockNodeMenu_Click;
            MinerMenu.DropDownItems.Add(myMutualLockNodeMenu);
            //自锁挖矿
            ToolStripMenuItem selfLockMenu = new ToolStripMenuItem();
            selfLockMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            selfLockMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            selfLockMenu.Name = "selfLockMenu";
            selfLockMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            selfLockMenu.Size = new System.Drawing.Size(170, 22);
            selfLockMenu.Text = UIHelper.LocalString("自锁挖矿", "Self Lock Mining");
            //互锁挖矿
            ToolStripMenuItem mutalLockMenu = new ToolStripMenuItem();
            mutalLockMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            mutalLockMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            mutalLockMenu.Name = "mutualLockMenu";
            mutalLockMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            mutalLockMenu.Size = new System.Drawing.Size(170, 22);
            mutalLockMenu.Text = UIHelper.LocalString("互锁挖矿", "Mutual Lock Mining");
            //级锁挖矿
            ToolStripMenuItem LevelLockMenu = new ToolStripMenuItem();
            LevelLockMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            LevelLockMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            LevelLockMenu.Name = "LevelLockMenu";
            LevelLockMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            LevelLockMenu.Size = new System.Drawing.Size(170, 22);
            LevelLockMenu.Text = UIHelper.LocalString("级锁挖矿", "Level Lock Mining");


            //锁仓挖矿资产列表
            ToolStripMenuItem miningAssetListMenu = new ToolStripMenuItem();
            miningAssetListMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            miningAssetListMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            miningAssetListMenu.Name = "miningAssetListMenu";
            miningAssetListMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            miningAssetListMenu.Size = new System.Drawing.Size(170, 22);
            miningAssetListMenu.Text = UIHelper.LocalString("自锁挖矿资产列表", "Self Lock Mining Assets");
            miningAssetListMenu.Click += MiningAssetListMenu_Click;
            selfLockMenu.DropDownItems.Add(miningAssetListMenu);

            //我的锁仓挖矿记录
            ToolStripMenuItem myLockMiningRecordsMenu = new ToolStripMenuItem();
            myLockMiningRecordsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myLockMiningRecordsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myLockMiningRecordsMenu.Name = "myLockMiningRecordsMenu";
            myLockMiningRecordsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            myLockMiningRecordsMenu.Size = new System.Drawing.Size(170, 22);
            myLockMiningRecordsMenu.Text = UIHelper.LocalString("我的自锁挖矿记录", " My Self Lock Mining Records");
            myLockMiningRecordsMenu.Click += MyLockMiningRecordsMenu_Click;
            selfLockMenu.DropDownItems.Add(myLockMiningRecordsMenu);
            //我的锁仓利息记录
            ToolStripMenuItem myLockInterestRecordsMenu = new ToolStripMenuItem();
            myLockInterestRecordsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myLockInterestRecordsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myLockInterestRecordsMenu.Name = "myLockInterestRecordsMenu";
            myLockInterestRecordsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            myLockInterestRecordsMenu.Size = new System.Drawing.Size(170, 22);
            myLockInterestRecordsMenu.Text = UIHelper.LocalString("我的自锁出矿记录", " My Self Lock Interest Records");
            myLockInterestRecordsMenu.Click += MyLockInterestRecordsMenu_Click;
            selfLockMenu.DropDownItems.Add(myLockInterestRecordsMenu);

            //互锁挖矿资产列表
            ToolStripMenuItem mutualLockMiningAssetListMenu = new ToolStripMenuItem();
            mutualLockMiningAssetListMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            mutualLockMiningAssetListMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            mutualLockMiningAssetListMenu.Name = "mutualLockMiningAssetListMenu";
            mutualLockMiningAssetListMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            mutualLockMiningAssetListMenu.Size = new System.Drawing.Size(170, 22);
            mutualLockMiningAssetListMenu.Text = UIHelper.LocalString("互锁挖矿资产列表", "Mutual Lock Mining Asset List");
            mutualLockMiningAssetListMenu.Click += MutualLockMiningAssetListMenu_Click;
            mutalLockMenu.DropDownItems.Add(mutualLockMiningAssetListMenu);


            //我的互锁挖矿记录
            ToolStripMenuItem myMutualLockMininRecordsMenu = new ToolStripMenuItem();
            myMutualLockMininRecordsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myMutualLockMininRecordsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myMutualLockMininRecordsMenu.Name = "myMutualLockMininRecordsMenu";
            myMutualLockMininRecordsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            myMutualLockMininRecordsMenu.Size = new System.Drawing.Size(170, 22);
            myMutualLockMininRecordsMenu.Text = UIHelper.LocalString("我的互锁挖矿记录", " My Mutual Lock Mining Records");
            myMutualLockMininRecordsMenu.Click += MyMutualLockMininRecordsMenu_Click;
            mutalLockMenu.DropDownItems.Add(myMutualLockMininRecordsMenu);
            //我的互锁出矿记录
            ToolStripMenuItem myMutualLockInterestRecordsMenu = new ToolStripMenuItem();
            myMutualLockInterestRecordsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myMutualLockInterestRecordsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myMutualLockInterestRecordsMenu.Name = "myMutualLockInterestRecordsMenu";
            myMutualLockInterestRecordsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            myMutualLockInterestRecordsMenu.Size = new System.Drawing.Size(170, 22);
            myMutualLockInterestRecordsMenu.Text = UIHelper.LocalString("我的互锁出矿记录", " My Mutual Lock Interest Records");
            myMutualLockInterestRecordsMenu.Click += MyMutualLockInterestRecordsMenu_Click;
            mutalLockMenu.DropDownItems.Add(myMutualLockInterestRecordsMenu);

            //级锁挖矿资产列表
            ToolStripMenuItem levelLockMiningAssetListMenu = new ToolStripMenuItem();
            levelLockMiningAssetListMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            levelLockMiningAssetListMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            levelLockMiningAssetListMenu.Name = "levelLockMiningAssetListMenu";
            levelLockMiningAssetListMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            levelLockMiningAssetListMenu.Size = new System.Drawing.Size(170, 22);
            levelLockMiningAssetListMenu.Text = UIHelper.LocalString("级锁挖矿资产列表", "Level Lock Mining Asset List");
            levelLockMiningAssetListMenu.Click += LevelLockMiningAssetListMenu_Click;
            LevelLockMenu.DropDownItems.Add(levelLockMiningAssetListMenu);
            //我的级锁出矿记录
            ToolStripMenuItem myLevelLockInterestRecordsMenu = new ToolStripMenuItem();
            myLevelLockInterestRecordsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myLevelLockInterestRecordsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myLevelLockInterestRecordsMenu.Name = "myLevelLockInterestRecordsMenu";
            myLevelLockInterestRecordsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            myLevelLockInterestRecordsMenu.Size = new System.Drawing.Size(170, 22);
            myLevelLockInterestRecordsMenu.Text = UIHelper.LocalString("我的级锁出矿记录", " My Level Lock Interest Records");
            myLevelLockInterestRecordsMenu.Click += MyLevelLockInterestRecordsMenu_Click;
            LevelLockMenu.DropDownItems.Add(myLevelLockInterestRecordsMenu);
            //我的级锁买入记录
            ToolStripMenuItem myLevelLockInRecordsMenu = new ToolStripMenuItem();
            myLevelLockInRecordsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myLevelLockInRecordsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myLevelLockInRecordsMenu.Name = "myLevelLockInRecordsMenu";
            myLevelLockInRecordsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            myLevelLockInRecordsMenu.Size = new System.Drawing.Size(170, 22);
            myLevelLockInRecordsMenu.Text = UIHelper.LocalString("我的质押买入记录", "My Level Lock Buy Records");
            myLevelLockInRecordsMenu.Click += MyLevelLockInRecordsMenu_Click;
            LevelLockMenu.DropDownItems.Add(myLevelLockInRecordsMenu);
            //我的级锁卖出记录
            ToolStripMenuItem myLevelLockOutRecordsMenu = new ToolStripMenuItem();
            myLevelLockOutRecordsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myLevelLockOutRecordsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            myLevelLockOutRecordsMenu.Name = "myLevelLockOutRecordsMenu";
            myLevelLockOutRecordsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            myLevelLockOutRecordsMenu.Size = new System.Drawing.Size(170, 22);
            myLevelLockOutRecordsMenu.Text = UIHelper.LocalString("我的质押卖出记录", "My Level Lock Sell Records");
            myLevelLockOutRecordsMenu.Click += MyLevelLockOutRecordsMenu_Click;
            LevelLockMenu.DropDownItems.Add(myLevelLockOutRecordsMenu);



            //挖矿规则
            ToolStripMenuItem ruleSettingMenu = new ToolStripMenuItem();
            ruleSettingMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            ruleSettingMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            ruleSettingMenu.Name = "custodyAccountsMenu";
            ruleSettingMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            ruleSettingMenu.Size = new System.Drawing.Size(170, 22);
            ruleSettingMenu.Text = UIHelper.LocalString("锁仓挖矿规则", "Lock Mining Rule");
            ruleSettingMenu.Click += RuleSettingMenu_Click;
            //挖矿社区
            ToolStripMenuItem miningCommunityMenu = new ToolStripMenuItem();
            miningCommunityMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            miningCommunityMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //miningCommunityMenu.Image = global::Example.Icons.NewFile_6276;
            miningCommunityMenu.Name = "miningCommunityMenu";
            miningCommunityMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            miningCommunityMenu.Size = new System.Drawing.Size(170, 22);
            miningCommunityMenu.Text = UIHelper.LocalString("质押挖矿社区", "Staking Mining Community"); ;
            miningCommunityMenu.Click += MiningCommunityMenu_Click;

            investMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                MinerMenu,
             selfLockMenu,
             mutalLockMenu,
             LevelLockMenu,
               new ToolStripSeparator(),
             ruleSettingMenu,
             miningCommunityMenu
            });
            this.Container.TopMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            investMenu});
        }

        private void ViewTotalLockVolumeMenu_Click(object sender, EventArgs e)
        {
            new ViewTotalLockVolume().ShowDialog();
        }

        private void MyLevelLockOutRecordsMenu_Click(object sender, EventArgs e)
        {
            if (this.MyLevelLockOutMingingRecords == default)
            {
                this.MyLevelLockOutMingingRecords = new MyLevelLockOutMingingRecords();
                this.MyLevelLockOutMingingRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyLevelLockOutMingingRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyLevelLockOutMingingRecords);
            }
            this.Container.DockPanel.AddContent(this.MyLevelLockOutMingingRecords);
        }

        private void MyLevelLockInRecordsMenu_Click(object sender, EventArgs e)
        {
            if (this.MyLevelLockInMingingRecords == default)
            {
                this.MyLevelLockInMingingRecords = new MyLevelLockInMingingRecords();
                this.MyLevelLockInMingingRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyLevelLockInMingingRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyLevelLockInMingingRecords);
            }
            this.Container.DockPanel.AddContent(this.MyLevelLockInMingingRecords);
        }

        private void MyLevelLockInterestRecordsMenu_Click(object sender, EventArgs e)
        {
            if (this.MyLevelLockInterestRecords == default)
            {
                this.MyLevelLockInterestRecords = new MyLevelLockInterestRecords();
                this.MyLevelLockInterestRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyLevelLockInterestRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyLevelLockInterestRecords);
            }

            this.Container.DockPanel.AddContent(this.MyLevelLockInterestRecords);
        }

        private void LevelLockMiningAssetListMenu_Click(object sender, EventArgs e)
        {
            if (this.LevelLockMingingAssets == default)
            {
                this.LevelLockMingingAssets = new LevelLockMingingAssets();
                this.LevelLockMingingAssets.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.LevelLockMingingAssets.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.LevelLockMingingAssets);
            }
            this.Container.DockPanel.AddContent(this.LevelLockMingingAssets);
        }

        private void MyMutualLockInterestRecordsMenu_Click(object sender, EventArgs e)
        {
            if (this.MyMutualLockInterestRecords == default)
            {
                this.MyMutualLockInterestRecords = new MyMutualLockInterestRecords();
                this.MyMutualLockInterestRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyMutualLockInterestRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyMutualLockInterestRecords);
            }

            this.Container.DockPanel.AddContent(this.MyMutualLockInterestRecords);
        }

        private void MyMutualLockMininRecordsMenu_Click(object sender, EventArgs e)
        {
            if (this.MyMutualLockMingingRecords == default)
            {
                this.MyMutualLockMingingRecords = new MyMutualLockMingingRecords();
                this.MyMutualLockMingingRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyMutualLockMingingRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyMutualLockMingingRecords);
            }

            this.Container.DockPanel.AddContent(this.MyMutualLockMingingRecords);
        }

        private void MiningCommunityMenu_Click(object sender, EventArgs e)
        {
            Bapp.PushCrossBappMessage(new CrossBappMessage() { MessageType = 1, Attachment = invest.LockMiningOfficalEventBoardId });

        }

        private void RuleSettingMenu_Click(object sender, EventArgs e)
        {
            if (this.RuleSetting == default)
            {
                this.RuleSetting = new LAMRule();
                this.RuleSetting.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.RuleSetting.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.RuleSetting);
            }
            this.Container.DockPanel.AddContent(this.RuleSetting);
        }

        private void MyMutualLockNodeMenu_Click(object sender, EventArgs e)
        {
            if (this.MyMutualLockNodes == default)
            {
                this.MyMutualLockNodes = new MyMutualLockNodes();
                this.MyMutualLockNodes.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyMutualLockNodes.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyMutualLockNodes);
            }
            this.Container.DockPanel.AddContent(this.MyMutualLockNodes);
        }

        private void ViewMutualNodeSeedMenu_Click(object sender, EventArgs e)
        {
            new ViewMutualLockSeed().ShowDialog();
        }

        private void MutualLockMiningAssetListMenu_Click(object sender, EventArgs e)
        {
            if (this.MutualLockMingingAssets == default)
            {
                this.MutualLockMingingAssets = new MutualLockMingingAssets();
                this.MutualLockMingingAssets.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MutualLockMingingAssets.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MutualLockMingingAssets);
            }

            this.Container.DockPanel.AddContent(this.MutualLockMingingAssets);
        }

        private void RegMutualLockAssetMenu_Click(object sender, EventArgs e)
        {
        }





        private void MyLockInterestRecordsMenu_Click(object sender, EventArgs e)
        {
            if (this.MyLockInterestRecords == default)
            {
                this.MyLockInterestRecords = new MyLockInterestRecords();
                this.MyLockInterestRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyLockInterestRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyLockInterestRecords);
            }

            this.Container.DockPanel.AddContent(this.MyLockInterestRecords);
        }

        private void MyLockMiningRecordsMenu_Click(object sender, EventArgs e)
        {
            if (this.MyLockMingingRecords == default)
            {
                this.MyLockMingingRecords = new MyLockMingingRecords();
                this.MyLockMingingRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyLockMingingRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyLockMingingRecords);
            }

            this.Container.DockPanel.AddContent(this.MyLockMingingRecords);
        }

        private void MiningAssetListMenu_Click(object sender, EventArgs e)
        {
            if (this.LockMingingAssets == default)
            {
                this.LockMingingAssets = new LockMingingAssets();
                this.LockMingingAssets.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.LockMingingAssets.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.LockMingingAssets);
            }

            this.Container.DockPanel.AddContent(this.LockMingingAssets);
        }



        public override void OnBlock(Block blockArg)
        {
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.OnBlock(blockArg);
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.OnBlock(blockArg);
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.OnBlock(blockArg);
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.OnBlock(blockArg);
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.OnBlock(blockArg);
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.OnBlock(blockArg);
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.OnBlock(blockArg);
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.OnBlock(blockArg);
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.OnBlock(blockArg);
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.OnBlock(blockArg);
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.OnBlock(blockArg);
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.OnBlock(blockArg);
        }
        public override void BeforeOnBlock(Block blockArg)
        {
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.BeforeOnBlock(blockArg);
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.BeforeOnBlock(blockArg);
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.BeforeOnBlock(blockArg);
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.BeforeOnBlock(blockArg);
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.BeforeOnBlock(blockArg);
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.BeforeOnBlock(blockArg);
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.BeforeOnBlock(blockArg);
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.BeforeOnBlock(blockArg);
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.BeforeOnBlock(blockArg);
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.BeforeOnBlock(blockArg);
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.BeforeOnBlock(blockArg);
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.BeforeOnBlock(blockArg);
        }
        public override void AfterOnBlock(Block blockArg)
        {
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.AfterOnBlock(blockArg);
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.AfterOnBlock(blockArg);
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.AfterOnBlock(blockArg);
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.AfterOnBlock(blockArg);
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.AfterOnBlock(blockArg);
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.AfterOnBlock(blockArg);
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.AfterOnBlock(blockArg);
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.AfterOnBlock(blockArg);
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.AfterOnBlock(blockArg);
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.AfterOnBlock(blockArg);
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.AfterOnBlock(blockArg);
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.AfterOnBlock(blockArg);
        }
        public override void OnBappEvent(BappEvent be)
        {
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.OnBappEvent(be);
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.OnBappEvent(be);
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.OnBappEvent(be);
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.OnBappEvent(be);
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.OnBappEvent(be);
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.OnBappEvent(be);
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.OnBappEvent(be);
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.OnBappEvent(be);
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.OnBappEvent(be);
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.OnBappEvent(be);
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.OnBappEvent(be);
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.OnBappEvent(be);
        }

        public override void OnCrossBappMessage(CrossBappMessage message)
        {
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.OnCrossBappMessage(message);
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.OnCrossBappMessage(message);
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.OnCrossBappMessage(message);
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.OnCrossBappMessage(message);
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.OnCrossBappMessage(message);
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.OnCrossBappMessage(message);
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.OnCrossBappMessage(message);
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.OnCrossBappMessage(message);
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.OnCrossBappMessage(message);
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.OnCrossBappMessage(message);
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.OnCrossBappMessage(message);
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.OnCrossBappMessage(message);
        }


        public override void HeartBeat(HeartBeatContext context)
        {
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.HeartBeat(context);
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.HeartBeat(context);
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.HeartBeat(context);
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.HeartBeat(context);
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.HeartBeat(context);
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.HeartBeat(context);
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.HeartBeat(context);
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.HeartBeat(context);
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.HeartBeat(context);
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.HeartBeat(context);
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.HeartBeat(context);
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.HeartBeat(context);
        }

        public override void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.ChangeWallet(operater);
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.ChangeWallet(operater);
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.ChangeWallet(operater);
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.ChangeWallet(operater);
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.ChangeWallet(operater);
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.ChangeWallet(operater);
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.ChangeWallet(operater);
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.ChangeWallet(operater);
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.ChangeWallet(operater);
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.ChangeWallet(operater);
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.ChangeWallet(operater);
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.ChangeWallet(operater);
        }
        public override void OnRebuild()
        {
            if (this.LockMingingAssets.IsNotNull()) this.LockMingingAssets.OnRebuild();
            if (this.MyLockMingingRecords.IsNotNull()) this.MyLockMingingRecords.OnRebuild();
            if (this.MyLockInterestRecords.IsNotNull()) this.MyLockInterestRecords.OnRebuild();
            if (this.MyMutualLockNodes.IsNotNull()) this.MyMutualLockNodes.OnRebuild();
            if (this.MutualLockMingingAssets.IsNotNull()) this.MutualLockMingingAssets.OnRebuild();
            if (this.RuleSetting.IsNotNull()) this.RuleSetting.OnRebuild();
            if (this.MyMutualLockMingingRecords.IsNotNull()) this.MyMutualLockMingingRecords.OnRebuild();
            if (this.MyMutualLockInterestRecords.IsNotNull()) this.MyMutualLockInterestRecords.OnRebuild();
            if (this.LevelLockMingingAssets.IsNotNull()) this.LevelLockMingingAssets.OnRebuild();
            if (this.MyLevelLockInterestRecords.IsNotNull()) this.MyLevelLockInterestRecords.OnRebuild();
            if (this.MyLevelLockInMingingRecords.IsNotNull()) this.MyLevelLockInMingingRecords.OnRebuild();
            if (this.MyLevelLockOutMingingRecords.IsNotNull()) this.MyLevelLockOutMingingRecords.OnRebuild();
        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {
        }
    }
}

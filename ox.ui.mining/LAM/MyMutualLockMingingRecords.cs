using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using OX.Network.P2P;
using OX.Cryptography;
using OX.Cryptography.ECC;
using OX.Mining;
using OX.UI.Mining;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OX.UI.Swap;
using OX.IO.Data.LevelDB;
using OX.Mining.StakingMining;
//using static NBitcoin.Scripting.OutputDescriptor;

namespace OX.UI.LAM
{
    public partial class MyMutualLockMingingRecords : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        bool needReload;
        #region Constructor Region

        public MyMutualLockMingingRecords()
        {
            InitializeComponent();
            this.DockArea = DarkDockArea.Left;
            this.treePools.MouseDown += TreeAsset_MouseDown;
        }

        private void TreeAsset_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DarkContextMenu menu = new DarkContextMenu();

                DarkTreeNode[] nodes = treePools.SelectedNodes.ToArray();
                if (nodes != null && nodes.Length == 1)
                {
                    var node = nodes.FirstOrDefault();
                    var nodetype = (int)node.NodeType;
                    if (nodetype == 2)
                    {
                        var sm = new ToolStripMenuItem(UIHelper.LocalString("时空量快照", "Space-Time volume snapshot"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click;
                        menu.Items.Add(sm);
                    }
                }
                if (menu.Items.Count > 0)
                    menu.Show(this.treePools, e.Location);
            }
        }

        private void Sm_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            MutualLockSnapshotData snapshotData = ToolStripMenuItem.Tag as MutualLockSnapshotData;
            new SpaceTimeSnapshot(snapshotData).ShowDialog();
        }

        public void Clear()
        {
            this.treePools.Nodes.Clear();
        }

        #endregion
        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be) { }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void BeforeOnBlock(Block block)
        {
            if (needReload)
            {
                DoReload();
                needReload = false;
            }
        }
        public void OnBlock(Block block) { }
        public void AfterOnBlock(Block block)
        {
            foreach (var tx in block.Transactions)
            {
                if (tx is LockAssetTransaction lat)
                {
                    if (!lat.IsTimeLock && lat.IsSelfLock(out UInt160 sh))
                    {
                        if (this.Operater.Wallet.ContainsAndHeld(sh))
                        {
                            needReload = true;
                        }
                        else
                        {
                            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                            if (bizPlugin != default)
                            {
                                if (bizPlugin.MutualLockNodes.TryGetValue(sh.GetMutualLockSeed(), out MutualNode miner) && this.Operater.Wallet.ContainsAndHeld(miner.ParentHolder))
                                    needReload = true;
                            }
                        }
                    }
                }
                else if (tx is EthereumMapTransaction emt &&emt.IsSelfLock())
                {
                    var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                    if (bizPlugin != default)
                    {
                        if (bizPlugin.MutualLockNodes.TryGetValue(emt.EthereumAddress.BuildMapAddress().GetMutualLockSeed(), out MutualNode miner) && this.Operater.Wallet.ContainsAndHeld(miner.ParentHolder))
                            needReload = true;
                    }
                }
            }
        }


        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            if (this.Operater.IsNotNull() && this.Operater.Wallet.IsNotNull())
                DoReload();
        }
        public void OnRebuild()
        {
            this.DoInvoke(() =>
            {
                this.treePools.Nodes.Clear();
            });
        }
        void DoReload()
        {
            this.DoInvoke(() =>
            {
                this.treePools.Nodes.Clear();
            });
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                var ps = bizPlugin.GetAll<MutualLockKey, MutualLockValue>(InvestBizPersistencePrefixes.MutualLockRecords);
                if (ps.IsNotNullAndEmpty())
                {
                    this.DoInvoke(() =>
                    {
                        foreach (var g in ps.GroupBy(m => m.Key.AssetId))
                        {
                            if (bizPlugin.MutualLockAssets.TryGetValue(g.Key, out MutualLockMiningAssetReply mar))
                            {
                                var assetState = Blockchain.Singleton.Store.GetAssets().TryGet(g.Key);
                                var cap = assetState.Amount == -Fixed8.Satoshi ? "+\u221e" : assetState.Amount.ToString();
                                var havecap = assetState.Available.ToString();
                                var node = new DarkTreeNode(UIHelper.LocalString($"资产Id:{g.Key.ToString()}", $"Asset Id:{g.Key.ToString()}"));
                                node.NodeType = 1;
                                node.Tag = g.Key;
                                var subNode = new DarkTreeNode(UIHelper.LocalString($"资产名:{assetState.GetName()}", $"Asset Name:{assetState.GetName()}"));
                                subNode.NodeType = 1;
                                subNode.Tag = g.Key;
                                node.Nodes.Add(subNode);
                                subNode = new DarkTreeNode(UIHelper.LocalString($"最大发行量:{cap}", $"Issue Cap:{cap}"));
                                subNode.NodeType = 1;
                                subNode.Tag = g.Key;
                                node.Nodes.Add(subNode);
                                subNode = new DarkTreeNode(UIHelper.LocalString($"已发行量:{havecap}", $"Issued:{havecap}"));
                                subNode.NodeType = 1;
                                subNode.Tag = g.Key;
                                node.Nodes.Add(subNode);
                                subNode = new DarkTreeNode(UIHelper.LocalString($"挖矿记录", $"Mining Records"));
                                subNode.NodeType = 1;
                                subNode.Tag = g.Key;
                                node.Nodes.Add(subNode);
                                foreach (var act in this.Operater.Wallet.GetHeldAccounts())
                                {
                                    if (bizPlugin.MutualLockNodes.TryGetValue(act.ScriptHash.GetMutualLockSeed(), out MutualNode mn))
                                    {
                                        var subNode2 = new DarkTreeNode(act.Address);
                                        subNode2.NodeType = 2;
                                        subNode.Nodes.Add(subNode2);
                                        var gs = g.Where(m => m.Key.Owner.Equals(act.ScriptHash));
                                        var gs2 = g.Where(m => m.Key.ParentOwner.Equals(act.ScriptHash));
                                        IEnumerable<MutualLockKey> self = default;
                                        IEnumerable<MutualLockKey> leaf = default;
                                        DarkTreeNode n3 = default;
                                        if (gs.IsNotNullAndEmpty())
                                        {
                                            self = gs.Select(m => m.Key);
                                            n3 = new DarkTreeNode(UIHelper.LocalString($"本节点挖矿记录", $"Current Node Mining Records"));
                                            n3.NodeType = 3;
                                            n3.Tag = act.ScriptHash;
                                            subNode2.Nodes.Add(n3);
                                            foreach (var r in gs.OrderByDescending(m => m.Key.Timestamp))
                                            {
                                                var n4 = new DarkTreeNode(UIHelper.LocalString($"锁仓数:{r.Key.Amount.ToString()}", $"Lock Volume:{r.Key.Amount.ToString()}"));
                                                n4.NodeType = 4;
                                                n4.Tag = act.ScriptHash;
                                                var n5 = new DarkTreeNode(UIHelper.LocalString($"锁仓时间:{r.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}", $"Lock Time:{r.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n5 = new DarkTreeNode(UIHelper.LocalString($"锁仓高度:{r.Key.StartIndex}", $"Lock Height:{r.Key.StartIndex}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n5 = new DarkTreeNode(UIHelper.LocalString($"锁仓地址:{r.Key.LockAddress.ToAddress()}", $"Lock Address:{r.Key.LockAddress.ToAddress()}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n5 = new DarkTreeNode(UIHelper.LocalString($"解锁高度:{r.Key.EndIndex}", $"Unlock Height:{r.Key.EndIndex}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n3.Nodes.Add(n4);
                                            }
                                        }
                                        if (gs2.IsNotNullAndEmpty())
                                        {
                                            leaf = gs2.Select(m => m.Key);
                                            n3 = new DarkTreeNode(UIHelper.LocalString($"叶子节点挖矿记录", $"Leaf Node Mining Records"));
                                            n3.NodeType = 3;
                                            n3.Tag = act.ScriptHash;
                                            subNode2.Nodes.Add(n3);
                                            foreach (var r in gs2.OrderByDescending(m => m.Key.Timestamp))
                                            {
                                                var n4 = new DarkTreeNode(UIHelper.LocalString($"锁仓数:{r.Key.Amount.ToString()}", $"Lock Volume:{r.Key.Amount.ToString()}"));
                                                n4.NodeType = 4;
                                                n4.Tag = act.ScriptHash;
                                                var n5 = new DarkTreeNode(UIHelper.LocalString($"锁仓时间:{r.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}", $"Lock Time:{r.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n5 = new DarkTreeNode(UIHelper.LocalString($"锁仓高度:{r.Key.StartIndex}", $"Lock Height:{r.Key.StartIndex}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n5 = new DarkTreeNode(UIHelper.LocalString($"锁仓地址:{r.Key.LockAddress.ToAddress()}", $"Lock Address:{r.Key.LockAddress.ToAddress()}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n5 = new DarkTreeNode(UIHelper.LocalString($"解锁地址:{r.Key.Owner.ToAddress()}", $"Unlock Address:{r.Key.Owner.ToAddress()}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n5 = new DarkTreeNode(UIHelper.LocalString($"解锁高度:{r.Key.EndIndex}", $"Unlock Height:{r.Key.EndIndex}"));
                                                n5.NodeType = 5;
                                                n5.Tag = act.ScriptHash;
                                                n4.Nodes.Add(n5);
                                                n3.Nodes.Add(n4);
                                            }
                                        }

                                        subNode2.Tag = new MutualLockSnapshotData { MutualLockMiningAssetReply = mar, Node = mn, Self = self, Leaf = leaf };
                                    }
                                }

                                this.treePools.Nodes.Add(node);
                            }
                        }
                    });


                }
            }
        }

        #endregion
    }
}

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

namespace OX.UI.LAM
{
    public partial class MyLevelLockOutMingingRecords : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        bool needReload;
        #region Constructor Region

        public MyLevelLockOutMingingRecords()
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

                }
                if (menu.Items.Count > 0)
                    menu.Show(this.treePools, e.Location);
            }
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
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                foreach (var tx in block.Transactions)
                {
                    foreach (var output in tx.Outputs)
                    {
                        if (bizPlugin.MutualLockNodes.TryGetValue(output.ScriptHash, out MutualNode leafNode))
                        {
                            if (leafNode.ParentHolder.IsNotNull() && this.Operater.Wallet.ContainsAndHeld(leafNode.ParentHolder))
                            {
                                foreach (var refer in tx.References)
                                {
                                    if (refer.Value.ScriptHash == leafNode.ParentHolder)
                                        needReload = true;
                                }
                            }
                        }
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
                var ps = bizPlugin.GetAll<LevelLockKey, LevelLockTx>(InvestBizPersistencePrefixes.LevelLockOutRecords);
                if (ps.IsNotNullAndEmpty())
                {
                    this.DoInvoke(() =>
                    {

                        foreach (var act in this.Operater.Wallet.GetHeldAccounts())
                        {
                            var gs = ps.Where(m => m.Key.Owner.Equals(act.ScriptHash));
                            Fixed8 total = gs.IsNotNullAndEmpty() ? gs.Sum(m => m.Key.Amount) : Fixed8.Zero;
                            var node = new DarkTreeNode($"{act.Address}  --  {total}");
                            node.NodeType = 1;
                            node.Tag = act.ScriptHash;

                            if (gs.IsNotNullAndEmpty())
                            {
                                foreach (var r in gs.OrderByDescending(m => m.Key.Timestamp))
                                {
                                    var n2 = new DarkTreeNode(UIHelper.LocalString($"质押卖出数:{r.Key.Amount.ToString()}", $"Level Lock Sell Volume:{r.Key.Amount.ToString()}"));
                                    n2.NodeType = 2;
                                    n2.Tag = act.ScriptHash;
                                    var n3 = new DarkTreeNode(UIHelper.LocalString($"质押卖出时间:{r.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}", $"Level Lock Sell Time:{r.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}"));
                                    n3.NodeType = 3;
                                    n3.Tag = act.ScriptHash;
                                    n2.Nodes.Add(n3);
                                    n3 = new DarkTreeNode(UIHelper.LocalString($"质押目标地址:{r.Key.To.ToAddress()}", $"Level Lock To:{r.Key.To.ToAddress()}"));
                                    n3.NodeType = 3;
                                    n3.Tag = act.ScriptHash;
                                    n2.Nodes.Add(n3);
                                    n3 = new DarkTreeNode(UIHelper.LocalString($"质押卖出高度:{r.Key.PledgeIndex}", $"Level Lock Sell Height:{r.Key.PledgeIndex}"));
                                    n3.NodeType = 3;
                                    n3.Tag = act.ScriptHash;
                                    n2.Nodes.Add(n3);
                                    node.Nodes.Add(n2);
                                }
                            }
                            this.treePools.Nodes.Add(node);
                        }

                    });


                }
            }
        }

        #endregion
    }
}

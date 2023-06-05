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
using OX.Mining.StakingMining;

namespace OX.UI.LAM
{
    public partial class MyLockMingingRecords : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        bool needReload;
        #region Constructor Region

        public MyLockMingingRecords()
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
            foreach (var tx in block.Transactions)
            {
                if (tx is LockAssetTransaction lat)
                {
                    if (!lat.IsTimeLock)
                    {
                        var sh = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
                        if (this.Operater.IsNotNull() && this.Operater.Wallet.ContainsAndHeld(sh))
                        {
                            var contractSH = lat.GetContract().ScriptHash;
                            if (lat.Witnesses.Select(m => m.ScriptHash).Contains(sh))
                            {
                                for (ushort k = 0; k < lat.Outputs.Length; k++)
                                {
                                    TransactionOutput output = lat.Outputs[k];
                                    if (output.ScriptHash.Equals(contractSH))
                                    {
                                        needReload = true;
                                    }
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
                var ps = bizPlugin.GetAll<SelfLockKey, LockMiningRecordMerge>(InvestBizPersistencePrefixes.LockMiningRecords);
                if (ps.IsNotNullAndEmpty())
                {
                    foreach (var g in ps.Where(m=>this.Operater.Wallet.ContainsAndHeld(m.Key.Holder)).GroupBy(m => m.Value.AssetId))
                    {
                        this.DoInvoke(() =>
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
                            foreach (var p in g.OrderByDescending(m => m.Value.Timestamp))
                            {
                                TransactionOutput output = p.Value.Output;
                                var subNode2 = new DarkTreeNode(UIHelper.LocalString($"锁仓量:{output.Value}", $"Lock Volume:{output.Value}"));
                                subNode2.NodeType = 2;
                                subNode2.Tag = p.Value;
                                subNode.Nodes.Add(subNode2);
                                var subsubNode = new DarkTreeNode(UIHelper.LocalString($"锁仓时间:{p.Value.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}", $"Lock Time:{p.Value.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")}"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Value;
                                subNode2.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"锁仓高度:{p.Value.Index}", $"Lock Height:{p.Value.Index}"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Value;
                                subNode2.Nodes.Add(subsubNode);

                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"锁仓地址:{output.ScriptHash.ToAddress()}", $"Lock Address:{output.ScriptHash.ToAddress()}"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Value;
                                subNode2.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"解锁高度:{p.Value.LockExpiration}", $"Unlock Height:{p.Value.LockExpiration}"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Value;
                                subNode2.Nodes.Add(subsubNode);
                            }
                            this.treePools.Nodes.Add(node);
                        });

                    }
                }
            }
        }

        #endregion
    }
}

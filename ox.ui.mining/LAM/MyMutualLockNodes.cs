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
    public partial class MyMutualLockNodes : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        bool needReload;
        List<DarkTreeNode> CountDownNodes = new List<DarkTreeNode>();
        #region Constructor Region

        public MyMutualLockNodes()
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
                if (nodes.IsNotNullAndEmpty() && nodes.Length == 1)
                {
                    DarkTreeNode node = nodes[0];
                    var nodetype = (int)node.NodeType;
                    if (nodetype == 1)
                    {
                        var sm = new ToolStripMenuItem(UIHelper.LocalString("复制种子地址", "Copy Seed Address"));
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
            UInt160 holder = ToolStripMenuItem.Tag as UInt160;
            var address = holder.GetMutualLockSeed().ToAddress();
            try
            {
                Clipboard.SetText(address);
                string msg = address + UIHelper.LocalString("  已复制", "  copied");
                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            }
            catch (Exception) { }
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
            this.DoInvoke(() =>
            {
                foreach (var node in this.CountDownNodes)
                {
                    var tag = (uint)node.Tag;
                    var rem = tag % 100000;
                    var rem2 = Blockchain.Singleton.Height % 100000;
                    if (rem2 > rem) rem += 100000;
                    var sub = rem - rem2;
                    node.Text = UIHelper.LocalString($"出矿倒计时:{sub}", $"Intrest Countdown:{sub}");
                }
            });
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
                    if (tx.Outputs.IsNotNullAndEmpty())
                    {
                        for (ushort k = 0; k < tx.Outputs.Length; k++)
                        {
                            TransactionOutput output = tx.Outputs[k];
                            if (output.ScriptHash.Equals(MutualLockHelper.GenesisSeed()) || bizPlugin.MutualLockNodes.ContainsKey(output.ScriptHash))
                            {
                                if (output.VerifyMutualLockNodeRegister())
                                {
                                    needReload = true;
                                }
                            }
                        }
                    }
                }
            }
            foreach (var tx in block.Transactions)
            {
                if (tx is LockAssetTransaction lat && !lat.IsTimeLock)
                {
                    var sh = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
                    if (this.Operater.Wallet.ContainsAndHeld(sh))
                    {
                        var contractSH = lat.GetContract().ScriptHash;
                        if (lat.Witnesses.Select(m => m.ScriptHash).Contains(sh))
                        {
                            for (ushort k = 0; k < lat.Outputs.Length; k++)
                            {
                                TransactionOutput output = lat.Outputs[k];
                                if (output.ScriptHash.Equals(contractSH))
                                {

                                    if (output.AssetId.Equals(Blockchain.OXS))
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
            this.CountDownNodes.Clear();
            this.DoInvoke(() =>
            {
                this.treePools.Nodes.Clear();

                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin != default)
                {
                    var oxsLocks = bizPlugin.GetAll<UInt160, Fixed8>(InvestBizPersistencePrefixes.LockMiningOXSTotal);
                    foreach (var act in this.Operater.Wallet.GetHeldAccounts())
                    {
                        if (bizPlugin.MutualLockNodes.TryGetValue(act.ScriptHash.GetMutualLockSeed(), out var miner))
                        {
                            var rm = miner.RegIndex % 100000;
                            var rem = rm;
                            var rem2 = Blockchain.Singleton.Height % 100000;
                            if (rem2 > rem) rem += 100000;
                            var sub = rem - rem2;
                            var s = invest.MasterAccountAddress.Equals(miner.ParentHolder) ? UIHelper.LocalString("根", "Root ") : string.Empty;
                            var node = new DarkTreeNode(UIHelper.LocalString($"{s}矿机({rm}):{miner.HolderAddress.ToAddress()}", $"{s}Miner({rm}):{miner.HolderAddress.ToAddress()}"));
                            node.NodeType = 1;
                            node.Tag = act.ScriptHash;
                            var subnode = new DarkTreeNode(UIHelper.LocalString($"出矿倒计时:{sub}", $"Intrest Countdown:{sub}"));
                            subnode.NodeType = 2;
                            subnode.Tag = miner.RegIndex;
                            node.Nodes.Add(subnode);
                            this.CountDownNodes.Add(subnode);
                            subnode = new DarkTreeNode(UIHelper.LocalString($"种子矿机:{miner.ParentHolder.ToAddress()}", $"Seed Miner:{miner.ParentHolder.ToAddress()}"));
                            subnode.NodeType = 3;
                            subnode.Tag = miner;
                            node.Nodes.Add(subnode);

                            if (oxsLocks.IsNotNullAndEmpty())
                            {
                                var oxsMsg = oxsLocks.FirstOrDefault(m => m.Key == act.ScriptHash);
                                if (!oxsMsg.Equals(new KeyValuePair<UInt160, Fixed8>()))
                                {
                                    subnode = new DarkTreeNode(UIHelper.LocalString($"OXS锁仓时空量:{oxsMsg.Value.GetInternalValue()}", $"OXS Lock Space-Time Volume:{oxsMsg.Value.GetInternalValue()}"));
                                    subnode.NodeType = 3;
                                    subnode.Tag = miner;
                                    node.Nodes.Add(subnode);
                                }
                            }
                            var lfs = bizPlugin.MutualLockNodes.Values.Where(m => m.ParentHolder == miner.HolderAddress);
                            foreach (var LeafHolder in lfs)
                            {
                                var leafrem = LeafHolder.RegIndex % 100000;
                                subnode = new DarkTreeNode(UIHelper.LocalString($"叶子矿机({leafrem}):{LeafHolder.HolderAddress.ToAddress()}", $"Leaf Miner({leafrem}):{LeafHolder.HolderAddress.ToAddress()}"));
                                subnode.NodeType = 3;
                                subnode.Tag = LeafHolder;
                                node.Nodes.Add(subnode);
                            }
                            this.treePools.Nodes.Add(node);
                        }
                    }

                }
            });
        }

        #endregion
    }
}

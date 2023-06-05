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
    public partial class LockMingingAssets : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        bool needReload;
        LockMiningAssetReply Last = default;
        #region Constructor Region

        public LockMingingAssets()
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
                if (invest.BizAddresses.Keys.FirstOrDefault(m => this.Operater.Wallet.ContainsAndHeld(m)).IsNotNull())
                {
                    var lammenu = new ToolStripMenuItem(UIHelper.LocalString($"发布自锁挖矿资产", $"Issue Self Lock Mining Asset"));
                    //lammenu.Tag = node.Tag;
                    lammenu.Click += Lammenu_Click;
                    menu.Items.Add(lammenu);
                }
                DarkTreeNode[] nodes = treePools.SelectedNodes.ToArray();
                if (nodes != null && nodes.Length == 1)
                {
                    var node = nodes.FirstOrDefault();
                    var sm = new ToolStripMenuItem(UIHelper.LocalString($"自主锁定该资产", $"Self Lock Asset"));
                    sm.Tag = node.Tag;
                    sm.Click += Sm_Click1;
                    menu.Items.Add(sm);
                    sm = new ToolStripMenuItem(UIHelper.LocalString($"复制资产Id", $"Copy Asset Id"));
                    sm.Tag = node.Tag;
                    sm.Click += Sm_Click;
                    menu.Items.Add(sm);
                }
                if (menu.Items.Count > 0)
                    menu.Show(this.treePools, e.Location);
            }
        }

        private void Sm_Click1(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            var tag = ToolStripMenuItem.Tag as UInt256;
            using (SelfLockForm dialog = new SelfLockForm(this.Operater, tag, this.Last.MinAmount, this.Last.MaxAmount,101000))
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                var output = dialog.GetOutput(out ECPoint ecp, out uint expiration);
                if (expiration - Blockchain.Singleton.Height < 101000)
                {
                    string msg = $"{UIHelper.LocalString("锁定的区块高度太低", "Locked block height is too low")}";
                    Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                    DarkMessageBox.ShowInformation(msg, "");
                    return;
                }
                LockAssetTransaction lat = new LockAssetTransaction
                {
                    LockContract = Blockchain.LockAssetContractScriptHash,
                    IsTimeLock = false,
                    LockExpiration = expiration,
                    Recipient = ecp
                };
                var from = Contract.CreateSignatureRedeemScript(ecp).ToScriptHash();
                output.ScriptHash = lat.GetContract().ScriptHash;
                lat.Outputs = new TransactionOutput[] { output };
                lat = this.Operater.Wallet.MakeTransaction(lat, from, from);
                if (lat != null)
                {
                    if (lat.Inputs.Count() > 20)
                    {
                        string msg = $"{UIHelper.LocalString("交易输入项太多,请分为多次转账", "There are too many transaction input. Please transfer multiple times")}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                        return;
                    }
                    this.Operater.SignAndSendTx(lat);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("交易已广播", "Relay transaction completed")}   {lat.Hash}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                }
            }
        }

        private void Sm_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            var tag = ToolStripMenuItem.Tag as UInt256;
            var assetId = tag.ToString();
            Clipboard.SetText(assetId);
            string msg = assetId + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
            DarkMessageBox.ShowError(msg, "");
        }

        private void Lammenu_Click(object sender, EventArgs e)
        {
            var sh = invest.BizAddresses.Keys.FirstOrDefault();
            var account = this.Operater.Wallet.GetAccount(sh);
            new IssueLockMiningAssetcs(this.Operater, account, sh).ShowDialog();
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
                if (tx is ReplyTransaction rt)
                {
                    if (rt.DataType == (byte)InvestType.LockMiningAssetReply)
                        needReload = true;
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
            this.Last = default;
            this.DoInvoke(() =>
            {
                this.treePools.Nodes.Clear();
            });
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                var lockMiningAssets = bizPlugin.GetAll<LockMiningAssetKey, LockMiningAssetReply>(InvestBizPersistencePrefixes.LockMiningAssetReply);
                if (lockMiningAssets.IsNotNullAndEmpty())
                {

                    this.DoInvoke(() =>
                    {
                        foreach (var p in lockMiningAssets.GroupBy(m => m.Key.AssetId))
                        {

                            var assetState = Blockchain.Singleton.Store.GetAssets().TryGet(p.Key);
                            var cap = assetState.Amount == -Fixed8.Satoshi ? "+\u221e" : assetState.Amount.ToString();
                            var havecap = assetState.Available.ToString();

                            var node = new DarkTreeNode(UIHelper.LocalString($"资产Id:{p.Key.ToString()}", $"Asset Id:{p.Key.ToString()}"));
                            node.NodeType = 1;
                            node.Tag = p.Key;
                            var subNode = new DarkTreeNode(UIHelper.LocalString($"资产名:{assetState.GetName()}", $"Asset Name:{assetState.GetName()}"));
                            subNode.NodeType = 2;
                            subNode.Tag = p.Key;
                            node.Nodes.Add(subNode);
                            subNode = new DarkTreeNode(UIHelper.LocalString($"最大发行量:{cap}", $"Issue Cap:{cap}"));
                            subNode.NodeType = 2;
                            subNode.Tag = p.Key;
                            node.Nodes.Add(subNode);
                            subNode = new DarkTreeNode(UIHelper.LocalString($"已发行量:{havecap}", $"Issued:{havecap}"));
                            subNode.NodeType = 2;
                            subNode.Tag = p.Key;
                            node.Nodes.Add(subNode);

                            foreach (var l in p.OrderByDescending(m => m.Key.IssueIndex))
                            {
                                if (Last.IsNull()) Last = l.Value;
                                subNode = new DarkTreeNode(UIHelper.LocalString($"利率发布区块:{l.Key.IssueIndex}", $"Ratio issue height:{l.Key.IssueIndex}"));
                                subNode.NodeType = 2;
                                subNode.Tag = p.Key;
                                node.Nodes.Add(subNode);

                                var subsubNode = new DarkTreeNode(UIHelper.LocalString($"最少锁仓量:{l.Value.MinAmount}", $"Min Lock:{l.Value.MinAmount}"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"最大锁仓量:{l.Value.MaxAmount}", $"Max Lock:{l.Value.MaxAmount}"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"100000区块利率:{l.Value.Interest_100000}/1000", $"100000 blocks ratio:{l.Value.Interest_100000}/1000"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"500000区块利率:{l.Value.Interest_500000}/1000", $"500000 blocks ratio:{l.Value.Interest_500000}/1000"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"1000000区块利率:{l.Value.Interest_1000000}/1000", $"1000000 blocks ratio:{l.Value.Interest_1000000}/1000"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"2000000区块利率:{l.Value.Interest_2000000}/1000", $"2000000 blocks ratio:{l.Value.Interest_2000000}/1000"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"4000000区块利率:{l.Value.Interest_4000000}/1000", $"4000000 blocks ratio:{l.Value.Interest_4000000}/1000"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                                subsubNode = new DarkTreeNode(UIHelper.LocalString($"6000000区块利率:{l.Value.Interest_6000000}/1000", $"6000000 blocks ratio:{l.Value.Interest_6000000}/1000"));
                                subsubNode.NodeType = 2;
                                subsubNode.Tag = p.Key;
                                subNode.Nodes.Add(subsubNode);
                            }
                            Fixed8 total = Fixed8.Zero;
                            foreach (var account in this.Operater.Wallet.GetHeldAccounts())
                            {
                                var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(account.ScriptHash, () => null);
                                if (acts.IsNotNull())
                                {
                                    total += acts.GetBalance(p.Key);
                                }
                            }
                            subNode = new DarkTreeNode(UIHelper.LocalString($"私钥账户余额:{total}", $"Private Account Balance:{total}"));
                            subNode.NodeType = 2;
                            subNode.Tag = p.Key;
                            node.Nodes.Add(subNode);
                            this.treePools.Nodes.Add(node);
                        }
                    });

                }
            }
        }

        #endregion
    }
}

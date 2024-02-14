using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Mining;
using OX.Wallets;
using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.IO;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.UI.Mining;
using OX.Mining.DEX;
using OX.UI.Mining.DEX;

namespace OX.UI.Swap
{
    public partial class SwapPairControl : UserControl, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        protected INotecase Operater;
        UInt160 HostSH;
        SwapPairMerge SwapPairMerge;
        IDORecord LastIDORecord;
        SwapVolumeMerge LastSwapVolume;
        bool IsIDOTime = false;

        public SwapPairControl(Module module, INotecase notecase, UInt160 hostSH, SwapPairMerge swapPairMerge)
        {
            this.Module = module;
            this.Operater = notecase;
            this.HostSH = hostSH;
            this.SwapPairMerge = swapPairMerge;
            InitializeComponent();
            this.lb_pairname.Text = $"{swapPairMerge.TargetAssetState.GetName()}  <=>  OXC";
            this.lb_lockinfo.Text = UIHelper.LocalString($"锁仓 {SwapPairMerge.SwapPairReply.LockPercent}%, {SwapPairMerge.SwapPairReply.LockExpire} 区块", $"Lock {SwapPairMerge.SwapPairReply.LockPercent}%, {SwapPairMerge.SwapPairReply.LockExpire} blocks");
            this.bt_showKLine.Text = UIHelper.LocalString("查看K线", "View K-line");
            this.bt_goSwap.Text = UIHelper.LocalString("去交易", "Go Swap");
            this.bt_detail.Text = UIHelper.LocalString("详情", "Details");
            this.lb_price.Text = String.Empty;
            this.MouseDown += SwapPairControl_MouseDown;
            RefreshBuy();
            RefreshState();
            RefreshPrice();
        }

        private void SwapPairControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DarkContextMenu menu = new DarkContextMenu();
                //var sm = new ToolStripMenuItem(UIHelper.LocalString("申请交易对", "Apply Exchange Pair"));
                //sm.Click += Sm_Click2;
                //menu.Items.Add(sm);
                if (invest.BizAddresses.Keys.FirstOrDefault(m => this.Operater.Wallet.ContainsAndHeld(m)).IsNotNull())
                {
                    //var sm = new ToolStripMenuItem(UIHelper.LocalString("查看交易对申请", "View Exchange Pair Application"));
                    //sm.Click += Sm_Click3;
                    //menu.Items.Add(sm);
                    var sm = new ToolStripMenuItem(UIHelper.LocalString("发布交易对", "Issue Exchange Pair"));
                    sm.Click += Sm_Click;
                    menu.Items.Add(sm);
                    sm = new ToolStripMenuItem(UIHelper.LocalString("关闭交易对", "Close Exchange Pair"));
                    sm.Click += Sm_Click1;
                    menu.Items.Add(sm);
                }
                if (menu.Items.Count > 0)
                    menu.Show(this, e.Location);
            }
        }

        //private void Sm_Click3(object sender, EventArgs e)
        //{
        //    new ViewSwapPair(this.Operater).ShowDialog();
        //}

        //private void Sm_Click2(object sender, EventArgs e)
        //{
        //    var sh = invest.BizAddresses.Keys.FirstOrDefault();
        //    new SwapPairRequest(this.Operater, sh).ShowDialog();
        //}

        private void Sm_Click1(object sender, EventArgs e)
        {
            var sh = invest.BizAddresses.Keys.FirstOrDefault();
            var account = this.Operater.Wallet.GetAccount(sh);
            if (account.IsNotNull())
            {
                SwapPairStateReply reply = new SwapPairStateReply
                {
                    PoolAddress = this.SwapPairMerge.PoolAddress,
                    Flag = 0x00
                };
                var tx = new ReplyTransaction()
                {
                    EdgeVersion = 0x00,
                    BizScriptHash = account.ScriptHash,
                    DataType = (byte)InvestType.SwapPairStateReply,
                    Data = reply.ToArray(),
                    BizNo = 0,
                    To = UInt160.Zero,
                    Attributes = new TransactionAttribute[0],
                    Outputs = new TransactionOutput[0],
                    Inputs = new CoinReference[0]
                };
                List<TransactionOutput> outputs = new List<TransactionOutput>();
                tx.Outputs = outputs.ToArray();
                tx = this.Operater.Wallet.MakeTransaction(tx, account.ScriptHash, account.ScriptHash);
                if (tx.IsNotNull())
                {
                    this.Operater.SignAndSendTx(tx);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("广播关闭交易对交易", "Relay closse swap pair transaction")}   {tx.Hash}";
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                }
            }
        }

        private void Sm_Click(object sender, EventArgs e)
        {
            var sh = invest.MasterAccountAddress;
            var account = this.Operater.Wallet.GetAccount(sh);
            new IssueSwapPair(this.Operater, account, sh).ShowDialog();
        }

        public void RefreshBuy()
        {
            this.DoInvoke(() =>
            {
                if (this.SwapPairMerge.SwapPairReply.Stamp > Blockchain.Singleton.Height)
                {
                    IsIDOTime = true;
                    this.bt_goSwap.Text = UIHelper.LocalString("IDO预购", "IDO Buy");
                }
                else
                {
                    IsIDOTime = false;
                }
            });
        }
        public void RefreshPrice()
        {
            this.DoInvoke(() =>
            {
                if (this.LastSwapVolume.IsNull())
                {
                    var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                    if (bizPlugin.IsNotNull())
                    {
                        var vom = bizPlugin.Get<SwapVolumeMerge>(InvestBizPersistencePrefixes.SwapPairLastExchange, this.HostSH);
                        if (vom.IsNotNull())
                            this.LastSwapVolume = vom;
                    }
                }
                if (this.LastSwapVolume.IsNotNull())
                {
                    this.lb_price.Text = this.LastSwapVolume.Price.ToString("f6");
                }
            });
        }
        public void RefreshState()
        {
            this.DoInvoke(() =>
            {
                this.lb_priceBalance.Text = String.Empty;
                //var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(HostSH, () => null);
                //if (acts.IsNotNull())
                //{
                //    var targetBalance = acts.GetBalance(SwapPairMerge.TargetAssetState.AssetId);
                //    var pricingBalance = acts.GetBalance(SwapPairMerge.PricingAssetState.AssetId);
                //    this.lb_priceBalance.Text = $"{targetBalance}  <=> {pricingBalance}";
                //}
                if (this.LastSwapVolume.IsNotNull())
                {
                    this.lb_priceBalance.Text = $"{this.LastSwapVolume.Volume.TargetBalance}  <=> {this.LastSwapVolume.Volume.PricingBalance}";
                }
            });
        }

        private void bt_showKLine_Click(object sender, EventArgs e)
        {
            if (this.Module is DEXModule sm)
            {
                sm.OpenKLine(this.HostSH);
            }
        }

        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be)
        {

        }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void BeforeOnBlock(Block block)
        {

        }
        public void OnBlock(Block block)
        {
            RefreshBuy();
        }
        public void AfterOnBlock(Block block)
        {
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin.IsNotNull())
            {
                foreach (var tx in block.Transactions)
                {
                    foreach (var output in tx.Outputs)
                    {
                        if (output.ScriptHash.Equals(HostSH) && (output.AssetId.Equals(this.SwapPairMerge.SwapPairReply.TargetAssetId) || output.AssetId.Equals(Blockchain.OXC)))
                        {
                            RefreshState();
                        }
                    }
                    if (tx.References.IsNotNullAndEmpty())
                    {
                        foreach (var reference in tx.References)
                        {
                            if (bizPlugin.SwapPairs.TryGetValue(reference.Value.ScriptHash, out SwapPairMerge spm) && reference.Value.ScriptHash.Equals(this.HostSH))
                            {
                                if (spm.TargetAssetState.AssetId.Equals(reference.Value.AssetId) || Blockchain.OXC.Equals(reference.Value.AssetId))
                                {
                                    var attr = tx.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Remark2);
                                    if (attr.IsNotNull())
                                    {
                                        try
                                        {
                                            var sop = attr.Data.AsSerializable<SwapVolume>();
                                            if (sop.IsNotNull())
                                            {
                                                this.LastSwapVolume = new SwapVolumeMerge { Volume = sop, Price = (decimal)sop.PricingAssetVolume.GetInternalValue() / (decimal)sop.TargetAssetVolume.GetInternalValue() }; ;
                                                RefreshPrice();
                                            }
                                        }
                                        catch { }
                                    }
                                    var attr3 = tx.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Remark3);
                                    if (attr3.IsNotNull())
                                    {
                                        try
                                        {
                                            var idor = attr3.Data.AsSerializable<IDORecord>();
                                            if (idor.IsNotNull())
                                            {
                                                this.LastIDORecord = idor;
                                            }
                                        }
                                        catch { }
                                    }
                                    break;
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
            RefreshState();
            RefreshPrice();
        }
        public void OnRebuild() { }
        #endregion

        private void bt_goSwap_Click(object sender, EventArgs e)
        {
            new SwapRecharge(this.Operater, this.HostSH, this.SwapPairMerge, IsIDOTime).ShowDialog();

        }

        private void bt_detail_Click(object sender, EventArgs e)
        {
            new SwapPairDetail(this.Operater, this.HostSH, this.SwapPairMerge, this.LastSwapVolume).ShowDialog();
        }
    }
}

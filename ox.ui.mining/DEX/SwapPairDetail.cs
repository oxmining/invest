using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets.UI;
using OX.Wallets.UI.Forms;
using OX.Wallets;
using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.Wallets.Models;
using OX.Mining;
using OX.Ledger;
using OX.IO;
using OX.SmartContract;
using OX.Cryptography.AES;
using OX.UI.Mining;
using OX.Mining.DEX;

namespace OX.UI.Swap
{
    public partial class SwapPairDetail : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        public SwapPairMerge SwapPairMerge;
        public SwapPairIDO IDO;
        public bool IsIDOTime;
        public UInt160 HostSH;
        public SwapVolumeMerge SwapVolumeMerge;
        public SwapPairDetail(INotecase operater, UInt160 hostSH, SwapPairMerge swapPairMerge, SwapVolumeMerge swapVolumeMerge)
        {
            this.Operator = operater;
            this.SwapPairMerge = swapPairMerge;
            this.HostSH = hostSH;
            this.SwapVolumeMerge = swapVolumeMerge;
            try
            {
                IDO = this.SwapPairMerge.SwapPairReply.Mark.AsSerializable<SwapPairIDO>();
            }
            catch { }
            InitializeComponent();

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
        public void BeforeOnBlock(Block block) { }
        public void OnBlock(Block block) { }
        public void AfterOnBlock(Block block)
        {

        }
        public void ChangeWallet(INotecase operater)
        {

            this.Operator = operater;

        }
        public void OnRebuild()
        {

        }
        #endregion

        private void RegMinerForm_Load(object sender, EventArgs e)
        {
            this.Text = $"{this.SwapPairMerge.TargetAssetState.GetName()}  <=>  OXC";
            this.lb_poolAddress.Text = UIHelper.LocalString("交易地址:", "Swap Addr:");
            this.bt_copy.Text = UIHelper.LocalString("复制", "Copy");
            this.btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.btnOk.Text = UIHelper.LocalString("信托交易", "Trust Swap");
            this.tb_pooladdress.Text = this.HostSH.ToAddress();
            var state = this.SwapPairMerge.TargetAssetState;
            this.groupBox2.Text = UIHelper.LocalString($"{state.GetName()} 资产详情", $"{state.GetName()}  Asset Details");
            this.lb_assetId.Text = UIHelper.LocalString("资产Id:", "Asset Id:");
            this.label2.Text = UIHelper.LocalString("发行者:", "Owner:");
            this.label3.Text = UIHelper.LocalString("管理者:", "Admin:");
            this.label4.Text = UIHelper.LocalString("发行上限:", "Cap:");
            this.label5.Text = UIHelper.LocalString("已发行:", "Issued:");
            this.tb_assetId.Text = this.SwapPairMerge.TargetAssetState.AssetId.ToString();
            textBox1.Text = Contract.CreateSignatureRedeemScript(state.Owner).ToScriptHash().ToAddress();
            textBox2.Text = state.Admin.ToAddress();
            textBox3.Text = state.Amount == -Fixed8.Satoshi ? "+\u221e" : state.Amount.ToString();
            textBox4.Text = state.Available.ToString();
            this.groupBox3.Text = UIHelper.LocalString("交易池", "Exchange Pool");
            this.lb_TargetAsset.Text = $"{this.SwapPairMerge.TargetAssetState.GetName()}:";
            this.lb_PricingAsset.Text = $"OXC:";
            this.lb_price.Text = UIHelper.LocalString("现价:", "Price:");
            this.lb_stamp.Text = UIHelper.LocalString("首次开盘:", "First Open:");
            this.lb_lockpercent.Text = UIHelper.LocalString("锁仓比例:", "Lock Percent:");
            this.lb_lockexpire.Text = UIHelper.LocalString("锁仓区块:", "Lock Blocks:");
            this.tb_stamp.Text = this.SwapPairMerge.SwapPairReply.Stamp.ToString();
            this.tb_lockpercent.Text = $"{this.SwapPairMerge.SwapPairReply.LockPercent}%";
            this.tb_lockexpire.Text = this.SwapPairMerge.SwapPairReply.LockExpire.ToString();
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(HostSH, () => null);
            if (acts.IsNotNull())
            {
                var targetBalance = acts.GetBalance(SwapPairMerge.TargetAssetState.AssetId);
                var pricingBalance = acts.GetBalance(Blockchain.OXC);
                this.tb_TargetAssetBalance.Text = targetBalance.ToString();
                this.tb_PricingAssetBalance.Text = pricingBalance.ToString();
            }
            if (this.SwapVolumeMerge.IsNotNull())
            {
                this.tb_price.Text = this.SwapVolumeMerge.Price.ToString("f6");
            }
            else
                this.tb_price.Text = String.Empty;
            this.groupBox4.Text = UIHelper.LocalString("IDO预售", "IDO Sale");
            this.lb_ido_price.Text = UIHelper.LocalString("IDO价格:", "IDO Price:");
            this.lb_ido_MinLiquidity.Text = UIHelper.LocalString("底池下限:", "Min Pool:");
            this.lb_ido_IDOLockExpire.Text = UIHelper.LocalString("预售锁仓:", "IDO Lock:");
            this.lb_ido_DividentSlope.Text = UIHelper.LocalString("分红坡度:", "Bonus Slope:");
            if (this.IDO.IsNotNull())
            {
                this.tb_ido_price.Text = this.IDO.Price.ToString();
                this.tb_ido_MinLiquidity.Text = this.IDO.MinLiquidity.ToString();
                this.tb_ido_IDOLockExpire.Text = UIHelper.LocalString($"{this.IDO.IDOLockExpire} 区块", $"{this.IDO.IDOLockExpire} blocks");
                var s = string.Empty;
                switch (this.IDO.DividentSlope)
                {
                    case DividentSlope.Big_5:
                        s = "大";
                        break;
                    case DividentSlope.Medium_6:
                        s = "中";
                        break;
                    case DividentSlope.Small_8:
                        s = "小";
                        break;
                }
                this.tb_ido_DividentSlope.Text = s;
            }
            this.lb_bonusratio.Text = String.Empty;
            this.lst_ido.Items.Clear();
            if (this.IDO.IsNotNull())
            {
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                if (bizPlugin.IsNotNull())
                {
                    var idos = bizPlugin.GetAll<SwapIDOKey, IDORecord>(InvestBizPersistencePrefixes.SwapPairIDORecord, this.HostSH);
                    if (idos.IsNotNullAndEmpty())
                    {
                        decimal totalRatio = 0;
                        var total = idos.Sum(m => m.Value.IdoAmount.GetInternalValue());
                        long t = 0;
                        foreach (var ido in idos.OrderBy(m => (long)m.Value.BlockIndex * 10000 + (long)m.Value.TxN))
                        {
                            var f = ido.Value.IdoAmount.GetInternalValue();
                            var ratio = this.IDO.DividentSlope.ComputeBonusRatio(total, t, f);
                            t += f;
                            string s = string.Empty;
                            if (this.Operator.Wallet.ContainsAndHeld(ido.Value.IdoOwner))
                            {
                                totalRatio += ratio;
                                s = "*";
                            }
                            this.lst_ido.Items.Add(new Wallets.UI.Controls.DarkListItem { Tag = ido.Value, Text = $"{s}  {ido.Value.IdoAmount} / {ido.Value.IdoOwner.ToAddress()}" });
                        }
                        this.lb_bonusratio.Text = UIHelper.LocalString($"合计手续费分成率: {totalRatio.ToString("f6")}", $"Total  fee sharing rate: {totalRatio.ToString("f6")}");
                    }
                }
            }
            if (this.SwapPairMerge.TargetAssetState.AssetId == Blockchain.OXS)
                this.btnOk.Enabled = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            new SwapTrustRecharge(this.Operator, this.HostSH, this.SwapPairMerge, IsIDOTime).ShowDialog();
            this.Close();
        }

        private void bt_copy_Click(object sender, EventArgs e)
        {
            var addr = this.HostSH.ToAddress();
            Clipboard.SetText(addr);
            string msg = addr + UIHelper.LocalString("  已复制", "  copied");
            DarkMessageBox.ShowError(msg, "");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

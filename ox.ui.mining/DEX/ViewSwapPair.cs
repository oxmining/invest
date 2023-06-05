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
    public partial class ViewSwapPair : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        public SwapPairAsk SwapPairAsk;
        public SwapPairIDO IDO;
        public Transaction Tx;
        public ViewSwapPair(INotecase operater)
        {
            this.Operator = operater;


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
            this.Text = UIHelper.LocalString($"查看交易对 申请", $"View Swap Pair Application for");
            this.groupBox2.Text = UIHelper.LocalString($"资产详情", $"Asset Details");
            this.lb_assetId.Text = UIHelper.LocalString("资产Id:", "Asset Id:");
            this.label2.Text = UIHelper.LocalString("发行者:", "Owner:");
            this.label3.Text = UIHelper.LocalString("管理者:", "Admin:");
            this.label4.Text = UIHelper.LocalString("发行上限:", "Cap:");
            this.label5.Text = UIHelper.LocalString("已发行:", "Issued:");
            this.groupBox3.Text = UIHelper.LocalString("交易池", "Exchange Pool");
            this.lb_stamp.Text = UIHelper.LocalString("首次开盘:", "First Open:");
            this.lb_lockpercent.Text = UIHelper.LocalString("锁定比例:", "Lock Percent:");
            this.lb_lockexpire.Text = UIHelper.LocalString("锁定区块:", "Lock Blocks:");
            this.groupBox4.Text = UIHelper.LocalString("IDO预售", "IDO Sale");
            this.lb_ido_price.Text = UIHelper.LocalString("IDO价格:", "IDO Price:");
            this.lb_ido_MinLiquidity.Text = UIHelper.LocalString("底池下限:", "Min Pool:");
            this.lb_ido_IDOLockExpire.Text = UIHelper.LocalString("预售锁定:", "IDO Lock:");
            this.lb_ido_DividentSlope.Text = UIHelper.LocalString("分红坡度:", "Bonus Slope:");
            this.lb_TargetAsset.Text = String.Empty;
            this.lb_PricingAsset.Text = "OXC";
            this.lst_ido.Items.Clear();
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                foreach (var r in bizPlugin.GetAll<UInt256, SwapPairAsk>(InvestBizPersistencePrefixes.SwapPairRequest))
                {
                    this.lst_ido.Items.Add(new Wallets.UI.Controls.DarkListItem { Tag = r, Text = $"{r.Value.TargetAssetId.ToString()}" });
                }

            }
        }
        void Reload()
        {
            var state = Blockchain.Singleton.Store.GetAssets().TryGet(this.SwapPairAsk.TargetAssetId);
            if (state.IsNotNull())
            {
                this.tb_assetId.Text = this.SwapPairAsk.TargetAssetId.ToString();
                textBox1.Text = Contract.CreateSignatureRedeemScript(state.Owner).ToScriptHash().ToAddress();
                textBox2.Text = state.Admin.ToAddress();
                textBox3.Text = state.Amount == -Fixed8.Satoshi ? "+\u221e" : state.Amount.ToString();
                textBox4.Text = state.Available.ToString();
                this.tb_stamp.Text = this.SwapPairAsk.Stamp.ToString();
                this.tb_lockpercent.Text = $"{this.SwapPairAsk.LockPercent}%";
                this.tb_lockexpire.Text = this.SwapPairAsk.LockExpire.ToString();
                this.lb_TargetAsset.Text = $"{state.GetName()}:";
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
                if (this.Tx.IsNotNull())
                {
                    var sh = invest.BizAddresses.Keys.FirstOrDefault();
                    Fixed8 targetBalance = Fixed8.Zero;
                    var output1 = this.Tx.Outputs.Where(m => m.ScriptHash.Equals(sh) && m.AssetId.Equals(state.AssetId));
                    if (output1.IsNotNullAndEmpty())
                        targetBalance = output1.Sum(m => m.Value);
                    Fixed8 oxcBalance = Fixed8.Zero;
                    var output2 = this.Tx.Outputs.Where(m => m.ScriptHash.Equals(sh) && m.AssetId.Equals(Blockchain.OXC));
                    if (output2.IsNotNullAndEmpty())
                        oxcBalance = output1.Sum(m => m.Value);
                    this.tb_TargetAssetBalance.Text = targetBalance.ToString();
                    this.tb_PricingAssetBalance.Text = oxcBalance.ToString();
                }
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lst_ido_SelectedIndicesChanged(object sender, EventArgs e)
        {
            if (this.lst_ido.SelectedIndices.IsNotNullAndEmpty())
            {
                var index = this.lst_ido.SelectedIndices.FirstOrDefault();
                var item = this.lst_ido.Items[index];
                KeyValuePair<UInt256, SwapPairAsk> p = (KeyValuePair<UInt256, SwapPairAsk>)item.Tag;
                this.IDO = default;
                this.SwapPairAsk = default;
                this.Tx = default;
                this.SwapPairAsk = p.Value;
                try
                {
                    IDO = this.SwapPairAsk.Mark.AsSerializable<SwapPairIDO>();
                }
                catch { }
                this.Tx = Blockchain.Singleton.GetTransaction(p.Key);
                Reload();
            }
        }

        private void tb_ido_price_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

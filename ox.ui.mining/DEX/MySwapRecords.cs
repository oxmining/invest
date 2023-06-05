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
using OX.Ledger;
using OX.IO;
using OX.SmartContract;
using OX.Cryptography.AES;
using OX.Mining.DEX;

namespace OX.UI.Swap
{
    public partial class MySwapRecords : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        public SwapPairMerge SwapPairMerge;
        public UInt160 HostSH;
        IEnumerable<KeyValuePair<SwapPairExchangeKey, SwapVolumeMerge>> LoadData;
        public MySwapRecords(INotecase operater, SwapPairMerge swapPairMerge, IEnumerable<KeyValuePair<SwapPairExchangeKey, SwapVolumeMerge>> loadData)
        {
            this.Operator = operater;
            this.SwapPairMerge = swapPairMerge;
            this.HostSH = swapPairMerge.PoolAddress;
            this.LoadData = loadData;
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
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
            foreach (var data in this.LoadData.Where(m => this.Operator.Wallet.ContainsAndHeld(m.Value.Volume.Payee)).OrderByDescending(m => (long)m.Value.Volume.BlockIndex * 10000 + (long)m.Value.Volume.TxN))
            {
                string s = UIHelper.LocalString(data.Value.Volume.IsBuy ? "买入" : "卖出", data.Value.Volume.IsBuy ? "buy" : "sale");
                this.lst_ido.Items.Add(new Wallets.UI.Controls.DarkListItem { Tag = data.Value, Text = $"{data.Key.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm")}   {s}   {data.Value.Volume.Payee.ToAddress()}   /   {data.Value.Price.ToString("f6")}  /  {data.Value.Volume.TargetAssetVolume} {this.SwapPairMerge.TargetAssetState.GetName()}  /  {data.Value.Volume.PricingAssetVolume} OXC" });
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

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
using Nethereum.Util;

namespace OX.UI.LAM
{
    public partial class ViewMutualLockSeed : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        public ViewMutualLockSeed()
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString("计算矿机种子", "Calculation Miner Seed");
            this.bt_copy.Text = UIHelper.LocalString("复制", "Copy");
            this.lb_accounts.Text = UIHelper.LocalString("账户地址:", "Account Address:");
            this.lb_seedAddress.Text = UIHelper.LocalString("种子地址:", "Seed Address:");
            this.lb_genesisSeed.Text = UIHelper.LocalString("根种子:", "Root Seed:");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
            this.bt_copyGenesisSeed.Text = UIHelper.LocalString("复制", "Copy");
            this.tb_genesisSeed.Text = MutualLockHelper.GenesisSeed().ToAddress();
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

        }

        private void bt_copy_Click(object sender, EventArgs e)
        {
            try
            {
                var s = this.tb_seedAddress.Text;
                Clipboard.SetText(s);
                string msg = s + UIHelper.LocalString("  已复制", "  copied");
                //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            }
            catch (Exception) { }
        }

        private void tb_address_TextChanged(object sender, EventArgs e)
        {
            var s = this.tb_address.Text;
            try
            {
                var sh =s.ToScriptHash();
                this.tb_seedAddress.Text = sh.GetMutualLockSeed().ToAddress();
            }
            catch
            {
                if (s.IsValidEthereumAddressHexFormat())
                {
                    this.tb_seedAddress.Text = s.BuildMapAddress().GetMutualLockSeed().ToAddress();
                }
                else
                    this.tb_seedAddress.Text = string.Empty;
            }
        }

        private void bt_copyGenesisSeed_Click(object sender, EventArgs e)
        {
            try
            {
                var s = this.tb_genesisSeed.Text;
                Clipboard.SetText(s);
                string msg = s + UIHelper.LocalString("  已复制", "  copied");
                //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            }
            catch (Exception) { }
        }
    }
}

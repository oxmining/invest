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

namespace OX.UI.LAM
{
    public partial class ViewTotalLockVolume : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        public ViewTotalLockVolume()
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString("查询有效时空锁仓总量", "View Total Valid Space-Time Lock Volume");
            this.bt_query.Text = UIHelper.LocalString("查询", "Query");
            this.lb_holder.Text = UIHelper.LocalString("账户地址:", "Account Address:");
            this.lb_volume.Text = UIHelper.LocalString("锁仓量:", "Lock Volume:");
            this.lb_assetId.Text = UIHelper.LocalString("资产ID:", "Asset ID:");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
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
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                if (UInt256.TryParse(this.tb_assetId.Text, out UInt256 assetID))
                {
                    try
                    {
                        var sh = this.tb_holder.Text.ToScriptHash();
                        var lw = bizPlugin.GetTotalValidLockVolume(assetID, sh);
                        if (lw.IsNotNull())
                        {
                            this.tb_volume.Text = lw.Value.ToString();
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }



    }
}

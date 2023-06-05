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
using OX.Cryptography.ECC;
using OX.Mining.StakingMining;

namespace OX.UI.LAM
{
    public partial class SpaceTimeSnapshot : DarkDialog
    {
        MutualLockSnapshotData Data;
        uint settleHeight;
        public SpaceTimeSnapshot(MutualLockSnapshotData data)
        {
            this.Data = data;
            InitializeComponent();
            this.Text = UIHelper.LocalString("时空量快照", "Space-Time volume snapshot");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_BlockIndex.Text = UIHelper.LocalString("结算高度:", "Settle Height:");
            this.lb_SelfSTVol.Text = UIHelper.LocalString("本矿机出矿:", "Current Miner Interest:");
            this.lb_LeafSTVol.Text = UIHelper.LocalString("叶子矿机出矿:", "Leaf Miner Interest:");
            this.bt_pre.Text = UIHelper.LocalString("上一结算点:", "Pre Settle Point:");
            this.bt_next.Text = UIHelper.LocalString("下一结算点:", "Next Settle Point:");

        }


        private void RegMinerForm_Load(object sender, EventArgs e)
        {
            var rem = Blockchain.Singleton.HeaderHeight % 100000;
            var rem2 = Data.Node.RegIndex % 100000;
            settleHeight = Blockchain.Singleton.HeaderHeight - rem + rem2;
            show();
        }

        private void tb_targetAssetId_TextChanged(object sender, EventArgs e)
        {

        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        void show()
        {
            var selfNum = this.Data.Self.Calculate(this.settleHeight) * this.Data.MutualLockMiningAssetReply.AirdropAmount;
            var leafNum = this.Data.Leaf.Calculate(this.settleHeight) * this.Data.MutualLockMiningAssetReply.AirdropAmount;
            this.tb_SelfSTVol.Text = selfNum.ToString();
            this.tb_LeafSTVol.Text = leafNum.ToString();
            this.tb_BlockIndex.Text = this.settleHeight.ToString();
        }

        private void bt_pre_Click(object sender, EventArgs e)
        {
            if (settleHeight > 100000)
                settleHeight -= 100000;
            show();
        }

        private void bt_next_Click(object sender, EventArgs e)
        {
            settleHeight += 100000;
            show();
        }
    }
}

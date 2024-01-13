using Akka.Actor;
using OX.IO.Actors;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Persistence;
using OX.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.Wallets.UI;
using OX.SmartContract;
using OX.IO;
using System.Xml;
using OX.Bapps;
using OX.Cryptography;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OX.Cryptography.ECC;
using Akka.IO;
using OX.Mining;
using OX.Mining.DTF;

namespace OX.UI.Mining.DTF
{
    public partial class NewTrustFund : DarkForm, INotecaseTrigger, IModuleComponent
    {
        public class AccountDescriptor
        {
            public WalletAccount Account;
            public override string ToString()
            {
                return Account.Address;
            }
        }
        INotecase Operater;
        public Module Module { get; set; }
        public NewTrustFund(INotecase operater)
        {
            InitializeComponent();
            this.Operater = operater;
        }


        private void NewEvent_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString("注册信托基金", "Register Trust Fund");
            this.lb_truster.Text = UIHelper.LocalString("受托账户:", "Trustee:");
            this.bt_OK.Text = UIHelper.LocalString("立即创建", "Create Now");
            this.bt_Close.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_balance.Text = UIHelper.LocalString("OXC余额:", "OXC Balance:");
            this.lb_trustAddr.Text = UIHelper.LocalString("信托地址:", "Trust Address:");
            this.bt_copy.Text = UIHelper.LocalString("复制", "Copy");
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                foreach (var act in this.Operater.Wallet.GetHeldAccounts())
                {
                    if (!bizPlugin.TrustFunds.ContainsKey(act.ScriptHash))
                        this.cbAccounts.Items.Add(new AccountDescriptor { Account = act });
                }
                this.cbAccounts.SelectedIndex = 0;
            }


        }

        private void Dcb_CheckedChanged(object sender, EventArgs e)
        {
            getTrustAddress();
        }

        bool refreshBlance(out AccountDescriptor ad)
        {
            ad = default;
            var item = this.cbAccounts.SelectedItem;
            if (item.IsNotNull())
            {
                ad = item as AccountDescriptor;
                this.tb_balance.Text = this.Operater.Wallet.GeAccountAvailable(ad.Account.ScriptHash, Blockchain.OXC).ToString();
                return true;
            }
            return false;
        }
        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        public void OnBappEvent(BappEvent be) { }

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
        }
        public void AfterOnBlock(Block block)
        {
        }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
        }
        public void OnRebuild()
        {
        }







        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (refreshBlance(out AccountDescriptor ad))
            {
                getTrustAddress();
            }
        }


        void getTrustAddress()
        {
            this.tb_trustAddr.Text = string.Empty;
            var obj = this.cbAccounts.SelectedItem;
            if (obj.IsNull()) return;
            AccountDescriptor ad = obj as AccountDescriptor;
            var att = ad.Account.GetKey().PublicKey.BuildFund();
            this.tb_trustAddr.Text = att.GetContract().ScriptHash.ToAddress();
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            var obj = this.cbAccounts.SelectedItem;
            if (obj.IsNull()) return;
            AccountDescriptor ad = obj as AccountDescriptor;
            if (!Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balance)) return;
            if (balance >= Fixed8.One * 10000)
            {
                var att = ad.Account.GetKey().PublicKey.BuildFund();
                TransactionOutput output = new TransactionOutput { AssetId = Blockchain.OXC, ScriptHash = att.GetContract().ScriptHash, Value = Fixed8.One * 10000 };
                att.Outputs = new TransactionOutput[] { output };
                att = this.Operater.Wallet.MakeTransaction(att, ad.Account.ScriptHash, ad.Account.ScriptHash);
                if (att != null)
                {
                    if (att.Inputs.Count() > 20)
                    {
                        string msg = $"{UIHelper.LocalString("交易输入项太多,请分为多次转账", "There are too many transaction input. Please transfer multiple times")}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                        return;
                    }
                    this.Operater.SignAndSendTx(att);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("交易已广播", "Relay transaction completed")}   {att.Hash}";
                        //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                        this.Close();
                    }
                }
            }
        }

        private void bt_copy_Click(object sender, EventArgs e)
        {
            var s = this.tb_trustAddr.Text;
            if (s.IsNotNullAndEmpty())
            {
                Clipboard.SetText(s);
                string msg = s + UIHelper.LocalString("  已复制", "  copied");
                //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            }
        }
    }
}

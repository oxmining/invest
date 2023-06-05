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
using OX.UI.Mining;

namespace OX.UI.Swap
{
    public partial class RegSideSwapPair : DarkDialog
    {
        INotecase Operater;
        UInt256 AssetId;
        UInt160 owner;
        public RegSideSwapPair(INotecase notecase)
        {
            this.Operater = notecase;
            InitializeComponent();
            this.DialogButtons = DarkDialogButton.OkCancel;
            this.Text = UIHelper.LocalString("注册边池交易对", "Reg Side Swap Pair");
            this.btnOk.Text = UIHelper.LocalString("注册", "Register");
            this.btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_targetAssetId.Text = UIHelper.LocalString("交易资产Id:", "Swap Asset Id:");
            this.lb_targetBalance.Text = UIHelper.LocalString("交易资产初始量:", "Swap Asset Init:");
            this.lb_oxcBalance.Text = UIHelper.LocalString("OXC初始量:", "OXC Init:");
            this.lb_poolAddress.Text = UIHelper.LocalString("边池地址:", "Side Pool Address:");
            this.btnOk.Enabled = false;
        }


        private void RegMinerForm_Load(object sender, EventArgs e)
        {

        }
        void ClearText()
        {
            this.lb_assetMsg.Text = string.Empty;
            this.tb_target_amount.Text = string.Empty;
            this.tb_target_balance.Text = string.Empty;
            this.tb_oxc_amount.Text = string.Empty;
            this.tb_oxc_balance.Text = string.Empty;
            this.owner = default;
            this.AssetId = default;
            this.tb_poolAddress.Text = string.Empty;
        }
        private void tb_targetAssetId_TextChanged(object sender, EventArgs e)
        {
            this.tb_target_balance.Text = String.Empty;
            AssetId = default;
            if (UInt256.TryParse(this.tb_targetAssetId.Text, out AssetId))
            {
                var assetState = Blockchain.Singleton.Store.GetAssets().TryGet(AssetId);
                if (assetState.IsNull())
                {
                    ClearText();
                    this.btnOk.Enabled = false;
                    return;
                }
                owner = Contract.CreateSignatureRedeemScript(assetState.Owner).ToScriptHash();
                if (!this.Operater.Wallet.ContainsAndHeld(owner))
                {
                    ClearText();
                    this.btnOk.Enabled = false;
                    return;
                }
                var balance = this.Operater.Wallet.GeAccountAvailable(owner, this.AssetId).ToString();
                this.tb_target_balance.Text = balance;
                var oxcbalance = this.Operater.Wallet.GeAccountAvailable(owner, Blockchain.OXC).ToString();
                this.tb_oxc_balance.Text = oxcbalance;
                this.lb_assetMsg.Text = $"{assetState.GetName()}   /   {owner.ToAddress()}";
                var tx = new SideTransaction()
                {
                    Recipient = invest.SidePoolAccountPubKey,
                    SideType = SideType.AssetID,
                    Data = this.AssetId.ToArray(),
                    Flag = 1,
                    AuthContract = Blockchain.SideAssetContractScriptHash,
                    Attributes = new TransactionAttribute[0],
                    Outputs = new TransactionOutput[0],
                    Inputs = new CoinReference[0]
                };
                this.tb_poolAddress.Text = tx.GetContract().ScriptHash.ToAddress();
            }
            else
            {
                ClearText();
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                var settings = bizPlugin.GetAllInvestSettings();
                var feesetting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { InvestSettingTypes.SidePairRegFee }));
                if (feesetting.Equals(new KeyValuePair<byte[], InvestSettingRecord>())) return;
                var fee = Fixed8.FromDecimal(decimal.Parse(feesetting.Value.Value));

                if (!Fixed8.TryParse(this.tb_target_amount.Text, out Fixed8 targetamount) || targetamount < SideTransactionHelper.MinSidePoolOXC) return;
                if (!Fixed8.TryParse(this.tb_oxc_amount.Text, out Fixed8 oxcamount) || oxcamount < SideTransactionHelper.MinSidePoolOXC) return;
                if (this.AssetId.IsNull()) return;
                var tx = new SideTransaction()
                {
                    Recipient = invest.SidePoolAccountPubKey,
                    SideType = SideType.AssetID,
                    Data = this.AssetId.ToArray(),
                    Flag = 1,
                    AuthContract = Blockchain.SideAssetContractScriptHash,
                    Attributes = new TransactionAttribute[0],
                    Outputs = new TransactionOutput[0],
                    Inputs = new CoinReference[0]
                };
                var sh = tx.GetContract().ScriptHash;

                List<TransactionOutput> outputs = new List<TransactionOutput>();

                outputs.Add(new TransactionOutput
                {
                    ScriptHash = sh,
                    AssetId = this.AssetId,
                    Value = targetamount
                });

                outputs.Add(new TransactionOutput
                {
                    ScriptHash = sh,
                    AssetId = Blockchain.OXC,
                    Value = oxcamount
                });
                outputs.Add(new TransactionOutput
                {
                    ScriptHash = invest.SidePoolAccountAddress,
                    AssetId = Blockchain.OXC,
                    Value = fee
                });
                tx.Outputs = outputs.ToArray();
                tx = this.Operater.Wallet.MakeTransaction(tx, this.owner, this.owner);
                if (tx.IsNotNull())
                {
                    this.Operater.SignAndSendTx(tx);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("广播注册边池交易对交易", "Relay reg side swap pair transaction")}   {tx.Hash}";
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                    this.Close();
                }
            }
        }



        private void tb_target_amount_TextChanged(object sender, EventArgs e)
        {
            if (!Fixed8.TryParse(this.tb_target_amount.Text, out Fixed8 amount))
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }

        private void tb_oxc_amount_TextChanged(object sender, EventArgs e)
        {
            if (!Fixed8.TryParse(this.tb_oxc_amount.Text, out Fixed8 amount))
            {
                this.btnOk.Enabled = false;
                return;
            }
            this.btnOk.Enabled = true;
        }
    }
}

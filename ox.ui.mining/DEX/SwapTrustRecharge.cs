﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.Wallets;
using OX.Ledger;
using OX.Wallets.Models;
using OX.Mining;
using OX.IO;
using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.IO.Data.LevelDB;
using OX.UI.Mining;
using OX.SmartContract;
using OX.VM;
using OX.Mining.DEX;
using OX.Persistence;

namespace OX.UI.Swap
{
    public partial class SwapTrustRecharge : DarkDialog
    {
        public class TrustAccountDescription
        {
            public UInt160 TrustAddress;
            public AssetTrustContract AssetTrustContract;
            public override string ToString()
            {
                return $"{TrustAddress.ToAddress()}  /   {Contract.CreateSignatureRedeemScript(AssetTrustContract.Truster).ToScriptHash().ToAddress()}";
            }
        }
        INotecase Operater;
        public SwapTrustRecharge()
        {
            InitializeComponent();
            btnOk.Text = UIHelper.LocalString("确定", "OK");
            btnOk.Enabled = false;
            this.lb_accounts.Text = UIHelper.LocalString("账户:", "Account:");
            this.lb_asset.Text = UIHelper.LocalString("资产:", "Asset:");
            this.lb_balance.Text = UIHelper.LocalString("信托余额:", "Trust Balance:");
            this.lb_amount.Text = UIHelper.LocalString("金额:", "Amount:");
        }
        public SwapPairMerge SwapPairMerge;
        public SwapPairIDO IDO;
        public bool IsIDOTime;
        public UInt160 HostSH;
        public SwapTrustRecharge(INotecase operater, UInt160 hostSH, SwapPairMerge swapPairMerge, bool isIDOTime) : this()
        {
            this.Operater = operater;
            this.SwapPairMerge = swapPairMerge;

            this.HostSH = hostSH;
            this.IsIDOTime = isIDOTime;
            try
            {
                IDO = this.SwapPairMerge.SwapPairReply.Mark.AsSerializable<SwapPairIDO>();
            }
            catch { }
        }



        private void textBox_TextChanged(object sender, EventArgs e)
        {
            this.lb_msg.Text = String.Empty;
            if (!Fixed8.TryParse(textBox2.Text, out Fixed8 amount))
            {
                btnOk.Enabled = false;
                return;
            }
            if (amount == Fixed8.Zero)
            {
                btnOk.Enabled = false;
                return;
            }
            if (!Fixed8.TryParse(textBox3.Text, out Fixed8 balance))
            {
                btnOk.Enabled = false;
                return;
            }

            if (amount == Fixed8.Zero || balance == Fixed8.Zero || amount > balance)
            {
                btnOk.Enabled = false;
                return;
            }
            if (this.IsIDOTime)
            {
                if (this.IDO.IsNull())
                {
                    btnOk.Enabled = false;
                    return;
                }

                if (amount.GetInternalValue() % this.IDO.Price.GetInternalValue() > 0)
                {
                    btnOk.Enabled = false;
                    return;
                }

            }
            if (rbTargetAsset.Checked)
            {
                if (amount.GetInternalValue() % Fixed8.D > 0)
                {
                    btnOk.Enabled = false;
                    return;
                }
            }
            UInt256 assetId = default;
            string outAssetName = string.Empty;

            if (this.rbPriceAsset.Checked)
            {
                assetId = Blockchain.OXC;
                outAssetName = this.SwapPairMerge.TargetAssetState.GetName();
            }
            else
            {
                assetId = this.SwapPairMerge.TargetAssetState.AssetId;
                outAssetName = "OXC";
            }
            if (!IsIDOTime)
            {
                var v = forecast(assetId, amount);
                if (v == 0)
                {
                    btnOk.Enabled = false;
                    return;
                }
                var returnValue = Fixed8.FromDecimal(v);
                this.lb_msg.Text = UIHelper.LocalString($"预计将兑回 {returnValue}  {outAssetName}", $"Expected swap back {returnValue}  {outAssetName}");
            }
            btnOk.Enabled = true;
        }

        private void rbGTC_CheckedChanged(object sender, EventArgs e)
        {
            RefreshBalance();
        }
        private void RefreshBalance()
        {
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            var assetId = this.rbPriceAsset.Checked ? Blockchain.OXC : this.SwapPairMerge.TargetAssetState.AssetId;
            var from = this.cb_accounts.SelectedItem as TrustAccountDescription;
            textBox3.Text = "0";
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(from.TrustAddress, () => null);
            if (acts.IsNotNull())
            {
                textBox3.Text = acts.GetBalance(assetId).ToString();
            }
            textBox_TextChanged(this, EventArgs.Empty);
        }
        decimal forecast(UInt256 assetId, Fixed8 amount)
        {
            decimal outValue = 0;
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin.IsNotNull())
            {
                Fixed8 targetBalance = Fixed8.Zero;
                Fixed8 pricingBalance = Fixed8.Zero;

                var beforeExchanges = bizPlugin.GetAll<SwapBeforeExchangeKey, TransactionOutput>(InvestBizPersistencePrefixes.SwapPairBeforeExchange, this.HostSH);

                var lastExchange = bizPlugin.Get<SwapVolumeMerge>(InvestBizPersistencePrefixes.SwapPairLastExchange, this.HostSH);
                if (lastExchange.IsNotNull())
                {
                    targetBalance = lastExchange.Volume.TargetBalance;
                    pricingBalance = lastExchange.Volume.PricingBalance;
                    if (beforeExchanges.IsNotNullAndEmpty())
                    {
                        beforeExchanges = beforeExchanges.Where(m => (long)m.Key.BlockIndex * 10000 + (long)m.Key.TxN > (long)lastExchange.Volume.BlockIndex * 10000 + (long)lastExchange.Volume.TxN);
                    }
                    outValue = tryforecast(targetBalance, pricingBalance, beforeExchanges, assetId, amount);
                }
                else
                {
                    var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(HostSH, () => null);
                    if (acts.IsNotNull())
                    {
                        targetBalance = acts.GetBalance(SwapPairMerge.TargetAssetState.AssetId);
                        pricingBalance = acts.GetBalance(Blockchain.OXC);
                        outValue = tryforecast(targetBalance, pricingBalance, beforeExchanges, assetId, amount);
                    }
                }
            }
            return outValue;
        }
        decimal tryforecast(Fixed8 targetBalance, Fixed8 pricingBalance, IEnumerable<KeyValuePair<SwapBeforeExchangeKey, TransactionOutput>> records, UInt256 assetId, Fixed8 amount)
        {
            var tb = (decimal)targetBalance;
            var pb = (decimal)pricingBalance;

            if (records.IsNotNullAndEmpty())
            {
                foreach (var r in records.OrderBy(m => (long)m.Key.BlockIndex * 10000 + (long)m.Key.TxN))
                {
                    var total = tb * pb;
                    var v = (decimal)r.Value.Value;
                    if (this.SwapPairMerge.SwapPairReply.TargetAssetId.Equals(r.Value.AssetId))//sale
                    {
                        tb += v;
                        pb = total / tb;
                    }
                    else if (Blockchain.OXC.Equals(r.Value.AssetId))//buy
                    {
                        pb += v;
                        tb = total / pb;
                    }
                }
            }
            var sum = tb * pb;
            var tbout = tb;
            var pbout = pb;
            if (this.SwapPairMerge.SwapPairReply.TargetAssetId.Equals(assetId))//sale
            {
                tb += (decimal)amount;
                pb = sum / tb;
                return pbout - pb;
            }
            else if (Blockchain.OXC.Equals(assetId))//buy
            {
                pb += (decimal)amount;
                tb = sum / pb;
                return tbout - tb;
            }
            else return 0;
        }

        private void PayToDialog_Load(object sender, EventArgs e)
        {
            this.lb_msg.Text = String.Empty;
            this.rbTargetAsset.Text = this.SwapPairMerge.TargetAssetState.GetName();
            this.rbPriceAsset.Text = "OXC";
            Dictionary<UInt160, AssetTrustContract> atcts = new Dictionary<UInt160, AssetTrustContract>();
            if (this.Operater.Wallet is OpenWallet openWallet)
            {
                foreach (var act in openWallet.GetAssetTrustContacts())
                {
                    if (act.Value.Targets.Contains(this.HostSH))
                    {
                        atcts[act.Key] = act.Value;
                    }
                    else if (act.Value.SideScopes.IsNotNullAndEmpty())
                    {
                        foreach (var t in act.Value.SideScopes)
                        {
                            var ssl = Blockchain.Singleton.CurrentSnapshot.GetSides(t);
                            if (ssl.IsNotNull() && ssl.SideStateList.IsNotNullAndEmpty())
                            {
                                if (ssl.SideStateList.Select(m => m.SideScriptHash).Contains(this.HostSH))
                                {
                                    atcts[act.Key] = act.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (atcts.IsNotNullAndEmpty())
            {
                foreach (var act in atcts)
                {
                    this.cb_accounts.Items.Add(new TrustAccountDescription { TrustAddress = act.Key, AssetTrustContract = act.Value });
                }
                this.cb_accounts.SelectedIndex = 0;
            }
            if (this.IsIDOTime)
            {
                this.rbPriceAsset.Checked = true;
                this.rbTargetAsset.Checked = false;
                this.rbTargetAsset.Enabled = false;
                if (this.IDO.IsNotNull())
                    this.Text = UIHelper.LocalString($"IDO预购         {this.IDO.Price.ToString()} oxc/{this.SwapPairMerge.TargetAssetState.GetName()}", $"IDO Buy          {this.IDO.Price.ToString()} oxc/{this.SwapPairMerge.TargetAssetState.GetName()}");
                else
                    this.Text = UIHelper.LocalString($"IDO预购", $"IDO Buy");
            }
            else
            {
                this.Text = UIHelper.LocalString("信托交易", "Trust Exchange");
            }
            RefreshBalance();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.cb_accounts.Text.IsNullOrEmpty()) return;
            if (!Fixed8.TryParse(textBox2.Text, out Fixed8 amount)) return;

            var from = this.cb_accounts.SelectedItem as TrustAccountDescription;
            var fromAddress = from?.TrustAddress.ToAddress();
            var assetId = this.rbPriceAsset.Checked ? Blockchain.OXC : this.SwapPairMerge.TargetAssetState.AssetId;

            var trusteeAddress = Contract.CreateSignatureRedeemScript(from.AssetTrustContract.Trustee).ToScriptHash();
            var trustee = this.Operater.Wallet.GetAccount(trusteeAddress);
            if (trustee.IsNotNull() && !trustee.WatchOnly && this.Operater.Wallet is OpenWallet openWallet)
            {
                using (ScriptBuilder sb = new ScriptBuilder())
                {
                    var account = LockAssetHelper.CreateAccount(openWallet, from.AssetTrustContract.GetContract(), trustee.GetKey());//lock asset account have a some private key with master account
                    if (account != null)
                    {
                        List<AssetTrustUTXO> utxos = new List<AssetTrustUTXO>();
                        foreach (var r in openWallet.GetAssetTrustUTXO().Where(m =>m.Value.SpentIndex == 0 && m.Value.OutPut.AssetId.Equals(assetId) && m.Value.OutPut.ScriptHash.Equals(from.TrustAddress)))
                        {
                            utxos.Add(new AssetTrustUTXO
                            {
                                 AssetTrustOutput=r.Value,
                                Address = r.Value.OutPut.ScriptHash,
                                Value = r.Value.OutPut.Value.GetInternalValue(),
                                TxId = r.Key.TxId,
                                N = r.Key.N
                            });
                        }
                        List<string> excludedUtxoKeys = new List<string>();
                        if (utxos.SortSearch(amount.GetInternalValue(), excludedUtxoKeys, out AssetTrustUTXO[] selectedUtxos, out long remainder))
                        {
                            List<TransactionOutput> outputs = new List<TransactionOutput>();
                            outputs.Add(new TransactionOutput { AssetId = assetId, Value = amount, ScriptHash = this.HostSH });
                            if (remainder > 0)
                            {
                                outputs.Add(new TransactionOutput { AssetId = assetId, Value = new Fixed8(remainder), ScriptHash = from.TrustAddress });
                            }
                            List<CoinReference> inputs = new List<CoinReference>();
                            foreach (var utxo in selectedUtxos)
                            {
                                inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                            }
                            ContractTransaction tx = new ContractTransaction
                            {
                                Attributes = new TransactionAttribute[] {
                                            new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedPublicKey, Data = from.AssetTrustContract.Truster.EncodePoint(true) },
                                            new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedScriptHash, Data = from.TrustAddress.ToArray() }
                                        },
                                Outputs = outputs.ToArray(),
                                Inputs = inputs.ToArray(),
                                Witnesses = new Witness[0]
                            };
                            tx = LockAssetHelper.Build(tx, new AvatarAccount[] { account });
                            if (tx.IsNotNull())
                            {
                                this.Operater.Wallet.ApplyTransaction(tx);
                                this.Operater.Relay(tx);
                                foreach (var u in selectedUtxos)
                                {
                                    u.AssetTrustOutput.SpentIndex = Blockchain.Singleton.HeaderHeight;
                                }
                                if (this.Operater != default)
                                {
                                    string msg = UIHelper.LocalString($"广播信托下注交易成功  {tx.Hash}", $"Relay  trust bet transaction completed  {tx.Hash}");
                                    //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                                    DarkMessageBox.ShowInformation(msg, "");
                                    this.Close();
                                }
                            }
                        }

                    }
                }
            }
        }

        private void cb_accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshBalance();
        }
    }
}

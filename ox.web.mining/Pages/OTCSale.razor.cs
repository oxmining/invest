
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using OX.Wallets;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using OX.Network.P2P.Payloads;
using OX;
using OX.IO;
using OX.Cryptography.ECC;
using OX.Mining;
using OX.Ledger;
using OX.SmartContract;
using OX.Cryptography.AES;
using OX.Web.Models;
using OX.Wallets.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using OX.Wallets.Authentication;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using OX.Wallets.States;
using OX.Mining.OTC;
using OX.Bapps;
using OX.UI.Mining;
using AntDesign;
using OX.Wallets.Eths;
using OX.MetaMask;
//using static NBitcoin.Scripting.OutputDescriptor;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Model;
using OX.Wallets.UI.Forms;
using System.Security.Principal;
using Microsoft.AspNetCore.SignalR.Protocol;
//using NBitcoin.OpenAsset;

namespace OX.Web.Pages
{



    public partial class OTCSale
    {
        public override string PageTitle => UIHelper.LocalString("场外出金", "OTC Sale");
        string msg;
        OTCSaleViewModel model { get; set; } = new OTCSaleViewModel { State = 1 };
        Fixed8 OTCDealerOXPoolBalance = Fixed8.Zero;
        public EthAssetBalanceState BalanceState = new EthAssetBalanceState();
        private readonly FormItemLayout _formItemLayout = new FormItemLayout
        {
            LabelCol = new ColLayoutParam
            {
                Xs = new EmbeddedProperty { Span = 24 },
                Sm = new EmbeddedProperty { Span = 7 },
            },

            WrapperCol = new ColLayoutParam
            {
                Xs = new EmbeddedProperty { Span = 24 },
                Sm = new EmbeddedProperty { Span = 12 },
                Md = new EmbeddedProperty { Span = 10 },
            }
        };

        private readonly FormItemLayout _submitFormLayout = new FormItemLayout
        {
            WrapperCol = new ColLayoutParam
            {
                Xs = new EmbeddedProperty { Span = 24, Offset = 0 },
                Sm = new EmbeddedProperty { Span = 10, Offset = 7 },
            }
        };

        protected override async void OnMiningInit()
        {

            if (this.EthID.IsNotNull())
            {
                this.model.PoolEthAddress = this.EthID.EthAddress;
                var st = this.EthID.EthAddress.BuildOTCDealerTransaction();
                var sh = st.GetContract().ScriptHash;
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                if (bizPlugin.IsNotNull())
                {
                    if (bizPlugin.OTCDealers.TryGetValue(sh, out OTCDealerMerge otcDealerMerge) && otcDealerMerge.IsNotNull() && otcDealerMerge.Setting.IsNotNull())
                    {
                        this.model.FeeRatio = otcDealerMerge.Setting.InRate;
                        this.model.State = (byte)otcDealerMerge.Setting.State;
                    }
                }
                this.model.InPoolAddress = sh;
                this.model.Amount = 0;
                var account = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(sh);
                if (account.IsNotNull() && account.Balances.TryGetValue(invest.USDX_Asset, out OTCDealerOXPoolBalance))
                {

                }

                if (this.EthID.IsNotNull() && this.Box.Notecase.Wallet is OpenWallet openWallet)
                {
                    var balanceState = openWallet.QueryBalanceState(this.EthID);
                    if (balanceState.IsNotNull())
                    {
                        this.BalanceState = balanceState.TryGetBalance(invest.USDX_Asset);
                    }
                }
            }
            await Task.CompletedTask;
        }

        private async void HandleSubmit()
        {
            if (this.Valid)
            {
                OTCSetting setting = new OTCSetting
                {
                    InRate = (byte)this.model.FeeRatio,
                    OutRate = 0,
                    EthAsset = 0,
                    State = (OTCStatus)this.model.State
                };
                try
                {
                    if (this.Box.Notecase.Wallet is OpenWallet openWallet)
                    {
                        var allutxos = openWallet.GetAllEthereumMapUTXOs();
                        if (allutxos.IsNotNullAndEmpty())
                        {
                            var us = allutxos.Where(m => m.Value.Output.AssetId == invest.USDX_Asset && m.Value.EthAddress == this.EthID.EthAddress && m.Value.LockExpirationIndex < Blockchain.Singleton.Height);
                            if (us.IsNotNullAndEmpty())
                            {
                                List<EthMapUTXO> utxos = new List<EthMapUTXO>();
                                foreach (var r in us)
                                {
                                    utxos.Add(new EthMapUTXO
                                    {
                                        Address = r.Value.Output.ScriptHash,
                                        Value = r.Value.Output.Value.GetInternalValue(),
                                        TxId = r.Key.PrevHash,
                                        N = r.Key.PrevIndex,
                                        EthAddress = r.Value.EthAddress,
                                        LockExpirationIndex = r.Value.LockExpirationIndex
                                    });
                                }
                                var tx = this.EthID.EthAddress.BuildOTCDealerTransaction(setting);
                                var shPool = tx.GetContract().ScriptHash;
                                List<string> excludedUtxoKeys = new List<string>();
                                var amt = Fixed8.FromDecimal(this.model.Amount);
                                if (utxos.SortSearch(amt.GetInternalValue(), excludedUtxoKeys, out EthMapUTXO[] selectedUtxos, out long remainder))
                                {
                                    List<TransactionOutput> outputs = new List<TransactionOutput>();
                                    outputs.Add(new TransactionOutput { AssetId = invest.USDX_Asset, Value = amt, ScriptHash = shPool });
                                    if (remainder > 0)
                                    {
                                        outputs.Add(new TransactionOutput { AssetId = invest.USDX_Asset, Value = new Fixed8(remainder), ScriptHash = this.EthID.MapAddress });
                                    }
                                    List<CoinReference> inputs = new List<CoinReference>();
                                    Dictionary<UInt160, Contract> contracts = new Dictionary<UInt160, Contract>();
                                    foreach (var utxo in selectedUtxos)
                                    {
                                        inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                                        EthereumMapTransaction emt = new EthereumMapTransaction { EthereumAddress = utxo.EthAddress, LockExpirationIndex = utxo.LockExpirationIndex };
                                        var c = emt.GetContract();
                                        var esh = c.ScriptHash;
                                        if (!contracts.ContainsKey(esh))
                                            contracts[esh] = c;
                                    }
                                    tx.Outputs = outputs.ToArray();
                                    tx.Inputs = inputs.ToArray();
                                    var stringToSign = tx.InputOutputHash.ToArray().ToHexString();
                                    var signatureData = await this.MetaMaskService.PersonalSign(stringToSign);
                                    var signer = new Nethereum.Signer.EthereumMessageSigner();
                                    var ethaddress = signer.EncodeUTF8AndEcRecover(stringToSign, signatureData);
                                    if (ethaddress.ToLower() == this.EthID.EthAddress.ToLower())
                                    {
                                        tx.Attributes = new TransactionAttribute[] { new TransactionAttribute { Usage = TransactionAttributeUsage.EthSignature, Data = System.Text.Encoding.UTF8.GetBytes(signatureData) },
                                                new TransactionAttribute { Usage = TransactionAttributeUsage.EthSignature, Data = signatureData.HexToByteArray() } };
                                        var oxKey = openWallet.GetHeldAccounts().First().GetKey();
                                        List<AvatarAccount> avatars = new List<AvatarAccount>();
                                        foreach (var c in contracts)
                                        {
                                            avatars.Add(LockAssetHelper.CreateAccount(openWallet, c.Value, oxKey));
                                        }
                                        tx = LockAssetHelper.Build(tx, avatars.ToArray());
                                        if (tx.IsNotNull())
                                        {
                                            this.Box.Notecase.Wallet.ApplyTransaction(tx);
                                            this.Box.Notecase.Relay(tx);
                                            this.EthID.SetLastTransactionIndex(Blockchain.Singleton.Height);
                                            msg = UIHelper.LocalString($"广播以太坊映射转帐交易成功  {tx.Hash}", $"Relay transfer ethereum map asset transaction completed  {tx.Hash}");
                                            StateHasChanged();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    msg = UIHelper.LocalString($"内部错误", $"internal error");
                }
            }

        }
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            if (this.EthID.IsNotNull())
            {
                this.model.PoolEthAddress = this.EthID.EthAddress;
                var st = this.EthID.EthAddress.BuildOTCDealerTransaction();
                var sh = st.GetContract().ScriptHash;
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                if (bizPlugin.IsNotNull())
                {
                    if (bizPlugin.OTCDealers.TryGetValue(sh, out OTCDealerMerge otcDealerMerge) && otcDealerMerge.IsNotNull() && otcDealerMerge.Setting.IsNotNull())
                    {
                        this.model.FeeRatio = otcDealerMerge.Setting.InRate;
                        this.model.State = (byte)otcDealerMerge.Setting.State;
                    }
                }
                this.model.InPoolAddress = sh;
                this.model.Amount = 0;
                this.model.FeeRatio = (byte)OTCStatus.Open;
                var account = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(sh);
                if (account.IsNotNull() && account.Balances.TryGetValue(invest.USDX_Asset, out OTCDealerOXPoolBalance))
                {

                }
                this.BalanceState = new EthAssetBalanceState();
                if (this.EthID.IsNotNull() && this.Box.Notecase.Wallet is OpenWallet openWallet)
                {
                    var balanceState = openWallet.QueryBalanceState(this.EthID);
                    if (balanceState.IsNotNull())
                    {
                        this.BalanceState = balanceState.TryGetBalance(invest.USDX_Asset);
                    }
                }
            }
        }
    }
}

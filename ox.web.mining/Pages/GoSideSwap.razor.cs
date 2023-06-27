
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
using Akka.Routing;
using OX.Mining.DEX;
using NuGet.Protocol.Plugins;
using Nethereum.Hex.HexConvertors.Extensions;
using AntDesign.Charts;
using OX.UI.Mining.DEX.Chart;

namespace OX.Web.Pages
{



    public partial class GoSideSwap
    {
        public override string PageTitle => UIHelper.LocalString("边池交易", "Side Pool Exchange");
        [Parameter]
        public string kind { get; set; }
        [Parameter]
        public string pooladdress { get; set; }
        protected IMiningProvider Provider { get; set; }

        public UInt160 PoolSH { get; set; } = default;
        public SideSwapPairKey SideSwapPairKey { get; set; } = default;
        decimal Price { get; set; }
        public UInt256 AssetId { get; set; }
        public string AssetName { get; set; }
        public EthAssetBalanceState BalanceState = new EthAssetBalanceState();
        string msg;
        bool loading2 = false;
        SwapViewModel Model { get; set; } = new SwapViewModel();

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
        IChartComponent chart1;
        string KLKind = "day";
        void changeKind(string e)
        {
            KLKind = e;
            reloadChart();
        }
        private void OnChartRender(IChartComponent stockChart)
        {
            chart1 = stockChart;
            reloadChart();
        }
        async void reloadChart()
        {
            try
            {
                var pool = pooladdress.ToScriptHash();
                IEnumerable<StockItem> lds;
                List<StockItem> datas = new List<StockItem>();
                var pv = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
                var data = pv.GetAll<SwapPairExchangeKey, SideSwapVolumeMerge>(InvestBizPersistencePrefixes.SideSwapPairExchange, pool);
                foreach (var g in data.GroupBy(m => KLKind == "hour" ? m.Key.Timestamp.ToHourLong() : m.Key.Timestamp.ToDayLong()).OrderBy(m => m.Key))
                {

                    var ePrice = g.OrderByDescending(m => m.Key.Timestamp).FirstOrDefault().Value.Price;
                    var bPrice = g.OrderBy(m => m.Key.Timestamp).FirstOrDefault().Value.Price;
                    var hPrice = g.OrderByDescending(m => m.Value.Price).FirstOrDefault().Value.Price;
                    var lPrice = g.OrderBy(m => m.Value.Price).FirstOrDefault().Value.Price;
                    datas.Add(new StockItem
                    {
                        trade_date = KLKind == "hour" ? g.Key.ToHourString() : g.Key.ToDayString(),
                        open = Decimal.ToDouble(bPrice),
                        high = Decimal.ToDouble(hPrice),
                        low = Decimal.ToDouble(lPrice),
                        close = Decimal.ToDouble(ePrice),
                        amount = Decimal.ToDouble(((decimal)g.Sum(m => m.Value.Volume.TargetAssetVolume))),
                        ts_code = this.pooladdress,
                        vol = Decimal.ToDouble(((decimal)g.Sum(m => m.Value.Volume.PricingAssetVolume))),
                    }); ;
                }
                if (datas.Count > 100)
                    lds = datas.TakeLast(100);
                else
                    lds = datas;

                if (lds.IsNotNullAndEmpty())
                    await chart1.ChangeData(lds);
            }
            catch
            {

            }
        }
        StockConfig config1 = new StockConfig()
        {
            Padding = "auto",
            XField = "trade_date",
            YField = new string[4] { "open", "close", "high", "low" }
        };
        protected override async void OnMiningInit()
        {
            if (kind != "0" && kind != "1")
                NavigationManager.NavigateTo("/");
            try
            {
                PoolSH = pooladdress.ToScriptHash();
            }
            catch
            {
                NavigationManager.NavigateTo("/");
            }

            this.Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (Provider.IsNotNull())
            {
                foreach (var p in Provider.GetAll<SideSwapPairKey, SideTransaction>(InvestBizPersistencePrefixes.SideSwapPair))
                {
                    if (p.Key.PoolAddress == this.PoolSH)
                    {
                        SideSwapPairKey = p.Key;
                    }
                }
                var vom = Provider.Get<SideSwapVolumeMerge>(InvestBizPersistencePrefixes.SideSwapPairLastExchange, this.PoolSH);
                if (vom.IsNotNull())
                    Price = vom.Price;
            }
            if (kind == "0")
            {
                AssetId = Blockchain.OXC;
                AssetName = "OXC";
            }
            else
            {
                AssetId = SideSwapPairKey.AssetId;
                var assetState = Blockchain.Singleton.CurrentSnapshot.Assets.TryGet(AssetId);
                if (assetState.IsNotNull())
                    AssetName = assetState.GetName();
            }
            if (this.EthID.IsNotNull() && this.Box.Notecase.Wallet is OpenWallet openWallet)
            {
                var balanceState = openWallet.QueryBalanceState(this.EthID);
                if (balanceState.IsNotNull())
                {
                    this.BalanceState = balanceState.TryGetBalance(AssetId);
                }
            }
            await Task.CompletedTask;
        }

        private async void HandleSubmit()
        {
            if (this.Valid)
            {
                loading2 = true;
                try
                {
                    if (this.Box.Notecase.Wallet is OpenWallet openWallet)
                    {

                        var allutxos = openWallet.GetAllEthereumMapUTXOs();
                        if (allutxos.IsNotNullAndEmpty())
                        {
                            var us = allutxos.Where(m => m.Value.Output.AssetId == this.AssetId && m.Value.EthAddress == this.EthID.EthAddress && m.Value.LockExpirationIndex < Blockchain.Singleton.Height);
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
                                List<string> excludedUtxoKeys = new List<string>();
                                var amt = Fixed8.One * this.Model.Amount;
                                if (utxos.SortSearch(amt.GetInternalValue(), excludedUtxoKeys, out EthMapUTXO[] selectedUtxos, out long remainder))
                                {
                                    List<TransactionOutput> outputs = new List<TransactionOutput>();
                                    outputs.Add(new TransactionOutput { AssetId = this.AssetId, Value = amt, ScriptHash = this.PoolSH });
                                    if (remainder > 0)
                                    {
                                        outputs.Add(new TransactionOutput { AssetId = this.AssetId, Value = new Fixed8(remainder), ScriptHash = this.EthID.MapAddress });
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
                                    ContractTransaction tx = new ContractTransaction
                                    {
                                        Attributes = new TransactionAttribute[0],
                                        Outputs = outputs.ToArray(),
                                        Inputs = inputs.ToArray(),
                                        Witnesses = new Witness[0]
                                    };
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
                                            loading2 = false;
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
                loading2 = false;
            }
        }
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            this.BalanceState = new EthAssetBalanceState();
            if (this.EthID.IsNotNull() && this.Box.Notecase.Wallet is OpenWallet openWallet)
            {
                var balanceState = openWallet.QueryBalanceState(this.EthID);
                if (balanceState.IsNotNull())
                {
                    this.BalanceState = balanceState.TryGetBalance(AssetId);
                }
            }
            //StateHasChanged();
        }
    }
}

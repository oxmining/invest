
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
using OX.Mining.StakingMining;
using Akka.Util;
using NBitcoin.OpenAsset;
using Nethereum.Model;
using NuGet.Protocol.Plugins;
using OX.IO.Data.LevelDB;
using OX.Wallets.Eths;
using System.Diagnostics.Contracts;
using OX.Mining.CheckinMining;

namespace OX.Web.Pages
{
    public partial class CheckinMining
    {
        public override string PageTitle => this.WebLocalString("签到挖矿", "Checkin Mining");
        public string msg;
        ulong Count = 0;
        Fixed8 SwapFeeBalance = Fixed8.Zero;
        protected override void OnMiningInit()
        {
            base.OnMiningInit();
            ReloadData();
        }

        protected override void AccountChanged()
        {
            ReloadData();
        }
        void ReloadData()
        {
            var act = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(invest.SwapFeeAccountAddress);
            act.Balances.TryGetValue(Blockchain.OXC, out SwapFeeBalance);
            if (this.HaveEthID && this.Provider.IsNotNull())
            {
                var lw = this.Provider.Get<LongWrapper>(InvestBizPersistencePrefixes.MarkMiningCount, new StringWrapper(this.EthID.EthAddress));
                if (lw.IsNotNull()) Count = lw.Value;
            }
        }
        async void Checkin()
        {
            if (this.HaveEthID)
            {
                var remainder = Blockchain.Singleton.HeaderHeight % 10000;
                var minIndex = Blockchain.Singleton.HeaderHeight - remainder;
                var blockHash = Blockchain.Singleton.GetBlockHash(minIndex);
                if (blockHash.IsNotNull())
                {
                    var block = Blockchain.Singleton.GetBlock(blockHash);
                    if (block.IsNotNull())
                    {
                        var stringToSign = $"{block.Index}-{block.ConsensusData}";
                        var signatureData = await this.MetaMaskService.PersonalSign(stringToSign);
                        var signer = new Nethereum.Signer.EthereumMessageSigner();
                        var ethaddress = signer.EncodeUTF8AndEcRecover(stringToSign, signatureData);
                        if (ethaddress.ToLower() == this.EthID.EthAddress.ToLower())
                        {
                            var balance = await this.MetaMaskService.GetBalance(this.EthID.EthAddress);
                            var b = balance * 10 / 1000000000000000000;
                            var amt = (uint)b;
                            if (amt > 0)
                            {
                                RangeTransaction tx = new RangeTransaction
                                {
                                    Attributes = new TransactionAttribute[0],
                                    Inputs = new CoinReference[0],
                                    Outputs = new TransactionOutput[0],
                                    Witnesses = new Witness[0],
                                    MinIndex = minIndex,
                                    MaxIndex = minIndex + 10000
                                };
                                CheckinMark mark = new CheckinMark { BlockIndex = minIndex, Kind = 0 };
                                tx.Attributes = new TransactionAttribute[] { new TransactionAttribute { Usage = TransactionAttributeUsage.Tip4, Data = mark.ToArray() }, new TransactionAttribute { Usage = TransactionAttributeUsage.EthSignature, Data = System.Text.Encoding.UTF8.GetBytes(signatureData) } };
                                if (tx.IsNotNull())
                                {
                                    if (Blockchain.Singleton.ContainsTransaction(tx.Hash))
                                    {
                                        msg = this.WebLocalString($"无需重复签到", $"No need for duplicate check-in");
                                    }
                                    else
                                    {
                                        this.Box.Notecase.Wallet.ApplyTransaction(tx);
                                        this.Box.Notecase.Relay(tx);
                                        this.EthID.SetLastTransactionIndex(Blockchain.Singleton.Height);
                                        msg = this.WebLocalString($"广播以太坊签到挖矿交易成功  {tx.Hash}", $"Relay transfer ethereum check-in mine transaction completed  {tx.Hash}");
                                    }
                                    StateHasChanged();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}


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
using OX.Cryptography;
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
using Akka.Actor.Dsl;

namespace OX.Web.Pages
{
    public class FormItemLayout
    {
        public ColLayoutParam LabelCol { get; set; }
        public ColLayoutParam WrapperCol { get; set; }
    }


    public partial class GoDeposit
    {
        public override string PageTitle => this.WebLocalString("入金", "Buy");
        [Parameter]
        public string dealerethaddressHex { get; set; }

        string msg;
        DepositModel model { get; set; } = new DepositModel();
        UInt160 OTCDealerOXPoolScriptHash;
        string OTCDealerOXPoolAddress;
        Fixed8 OTCDealerOXPoolBalance = Fixed8.Zero;
        bool success = false;
        string ethtxid = string.Empty;
        string revertEthHash = string.Empty;
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
        void Revert()
        {
            if (this.Valid && this.ValidChain)
            {
                var act = Box.Notecase.Wallet.GetHeldAccounts().First();
                if (UInt256.TryParse(revertEthHash, out UInt256 ethId))
                {
                    var Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                    if (Provider.IsNotNull())
                    {
                        if (!Provider.ContainEthExchangeRequest(ethId))
                        {
                            ethtxid = revertEthHash;
                            if (Box.Notecase.DoSimpleDeposit(act, ethtxid))
                            {
                                this.success = true;
                            }
                        }
                        else
                        {
                            this.msg = this.WebLocalString("之前提交已经生效，无需再提交", "The previous submission has already taken effect, there is no need to submit again");
                        }
                    }
                }
                this.revertEthHash = string.Empty;
                StateHasChanged();
            }
        }
        protected override async void OnMiningInit()
        {
            if (dealerethaddressHex.IsNotNullAndEmpty())
            {
                try
                {
                    var dealerethaddress = System.Text.Encoding.UTF8.GetString(dealerethaddressHex.HexToBytes());
                    this.model.PoolEthAddress = dealerethaddress;
                    var st = dealerethaddress.BuildOTCDealerTransaction();
                    OTCDealerOXPoolScriptHash = st.GetContract().ScriptHash;
                    OTCDealerOXPoolAddress = OTCDealerOXPoolScriptHash.ToAddress();
                    var account = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(OTCDealerOXPoolScriptHash);
                    if (account.IsNotNull() && account.Balances.TryGetValue(invest.USDT_Asset, out OTCDealerOXPoolBalance))
                    {

                    }

                    if (this.EthID.IsNotNull())
                    {
                        this.model.FromEthAddress = this.EthID.EthAddress;
                        this.model.OxAddress = this.EthID.MapAddress.ToAddress();
                        if (this.Valid && this.ValidChain)
                        {
                            var n = string.Join(',', this.ValidChains.Select(m => m.ToString()));
                            this.msg = this.WebLocalString($"仅支持  {n}", $"Only supported {n}");
                        }
                    }
                    await Task.CompletedTask;
                }
                catch
                {

                }
            }
        }

        private async void HandleSubmit()
        {
            this.success = false;
            this.ethtxid = string.Empty;
            if (this.Valid && this.ValidChain)
            {
                var act = Box.Notecase.Wallet.GetHeldAccounts().First();
                var sh = this.model.OxAddress.ToScriptHash();
                try
                {
                    var r = await this.MetaMaskService.TrySimpleDeposit(this.model.PoolEthAddress, OTCDealerOXPoolScriptHash, this.model.Amount);
                    if (r.IsNotNullAndEmpty())
                    {
                        this.ethtxid = r;
                        if (Box.Notecase.DoSimpleDeposit(act, r))
                        {
                            this.success = true;
                        }

                        StateHasChanged();
                    }
                }
                catch (UserDeniedException e)
                {
                    this.msg = this.WebLocalString($"已经拒绝交易", $"Transaction has been rejected");
                    StateHasChanged();
                }
            }
        }
        //private async void HandleSubmit2()
        //{
        //    this.success = false;
        //    this.ethtxid = string.Empty;
        //    if (this.Valid && this.ValidChain)
        //    {
        //        var act = Box.Notecase.Wallet.GetHeldAccounts().First();
        //        var sh = this.model.OxAddress.ToScriptHash();
        //        try
        //        {
        //            var r = await this.MetaMaskService.TryDeposit(this.model.FromEthAddress, this.model.PoolEthAddress, sh, this.model.Amount);
        //            if (r.OK)
        //            {

        //                if (Box.Notecase.DoDeposit(act, this.model.FromEthAddress, this.model.PoolEthAddress, sh, r.EthTxId, r.stringToSign, r.signatureData, true))
        //                {
        //                    this.success = true;
        //                }

        //                StateHasChanged();
        //            }
        //        }
        //        catch (UserDeniedException e)
        //        {
        //            this.msg = this.WebLocalString($"已经拒绝交易", $"Transaction has been rejected");
        //            StateHasChanged();
        //        }
        //    }
        //}
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            if (this.EthID.IsNotNull())
            {
                this.model.FromEthAddress = this.EthID.EthAddress;
                this.model.OxAddress = this.EthID.MapAddress.ToAddress();
            }
            //StateHasChanged();
        }
    }
}

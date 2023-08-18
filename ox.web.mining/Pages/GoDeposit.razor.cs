
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
        public string dealerethaddress { get; set; }

        string msg;
        DepositModel model { get; set; } = new DepositModel();
        string OTCDealerOXPoolAddress;
        Fixed8 OTCDealerOXPoolBalance = Fixed8.Zero;
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

            this.model.PoolEthAddress = dealerethaddress;
            var st = dealerethaddress.BuildOTCDealerTransaction();
            var sh = st.GetContract().ScriptHash;
            OTCDealerOXPoolAddress = sh.ToAddress();
            var account = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(sh);
            if (account.IsNotNull() && account.Balances.TryGetValue(invest.USDX_Asset, out OTCDealerOXPoolBalance))
            {

            }

            if (this.EthID.IsNotNull())
            {
                this.model.FromEthAddress = this.EthID.EthAddress;
                this.model.OxAddress = this.EthID.MapAddress.ToAddress();
            }
            await Task.CompletedTask;
        }

        private async void HandleSubmit()
        {
            if (this.Valid)
            {
                var act = Box.Notecase.Wallet.GetHeldAccounts().First();
                var sh = this.model.OxAddress.ToScriptHash();
                try
                {
                    var r = await this.MetaMaskService.TryDeposit(this.model.FromEthAddress, this.model.PoolEthAddress, sh, this.model.Amount);
                    if (r.OK)
                    {

                        if (Box.Notecase.DoDeposit(act, this.model.FromEthAddress, this.model.PoolEthAddress, sh, r.EthTxId, r.stringToSign, r.signatureData, true))
                        {
                            this.msg = this.WebLocalString($"以太坊交易 {r.EthTxId}已经尝试", $"Ethereum transaction {r.EthTxId} has been attempted");
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

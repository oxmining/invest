
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
using OX.Ledger;
using OX.SmartContract;
using OX.Cryptography.AES;
using OX.Web.Models;
using OX.Wallets.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using OX.Wallets.Authentication;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using OX.Wallets.States;
using OX.Bapps;
using OX.Wallets.Eths;
using OX.Mining;
using OX.UI.Mining;
using OX.Mining.DTF;
using OX.UI.DTF;

namespace OX.Web.Pages
{
    public partial class SubscribedFunds
    {
        public override string PageTitle => this.WebLocalString("我认筹的的基金", "My Subscribed Funds");
        Dictionary<UInt160, SubscribedFundViewModel> TrustFunds = new Dictionary<UInt160, SubscribedFundViewModel>();
        protected override void OnMiningInit()
        {
            Init();
        }
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            Init();
        }
        void Init()
        {
            if (this.Valid)
            {
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                if (bizPlugin.IsNotNull())
                {
                    //TrustFunds = bizPlugin.TrustFunds;
                    Dictionary<UInt160, Fixed8> funds = new Dictionary<UInt160, Fixed8>();

                    foreach (var g in bizPlugin.GetAllDTFIDOSummary(this.EthID.MapAddress))
                    {
                        var amount = g.Value;
                        if (funds.TryGetValue(g.Key.TrusteeAddress, out var balance))
                        {
                            amount += balance;
                        }
                        funds[g.Key.TrusteeAddress] = amount;
                    }
                    foreach (var f in funds)
                    {
                        if (bizPlugin.TrustFunds.TryGetValue(f.Key, out TrustFundModel model))
                        {
                            this.TrustFunds[f.Key] = new SubscribedFundViewModel { TrusteeAddress = f.Key, Model = model, Amount = f.Value };
                        }
                    }
                }
            }
        }
    }
}


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
using static NBitcoin.Scripting.OutputDescriptor;
using OX.Wallets.Eths;
using OX.Mining;
using OX.UI.Mining;
using OX.Mining.DTF;
using System.Collections.Specialized;

namespace OX.Web.Pages
{
    public partial class AllFunds
    {
        public override string PageTitle => this.WebLocalString("所有信托基金", "All Trust Funds");
        IOrderedEnumerable<KeyValuePair<UInt160, TrustFundModel>> TrustFunds =default;
        protected override void OnMiningInit()
        {
            Init();
        }
      
        void Init()
        {
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
            if (bizPlugin.IsNotNull())
            {
                TrustFunds = bizPlugin.TrustFunds.OrderByDescending(m => m.Value.TotalSubscribeAmount);
            }
        }
    }
}

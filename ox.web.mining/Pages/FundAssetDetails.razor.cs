
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
using OX.UI.Mining.DTF;
using OX.Mining.StakingMining;
using Akka.Util;
using Nethereum.Model;
using NuGet.Protocol.Plugins;
using OX.IO.Data.LevelDB;
using OX.Mining.DTF;
using OX.Wallets.UI.Controls;

namespace OX.Web.Pages
{
    public partial class FundAssetDetails
    {
        public override string PageTitle => this.WebLocalString("信托基金资产明细", "Trust Fund Asset Details");
        [Parameter]
        public string trusteeaddress { get; set; }
        public TrustFundModel TrustFundModel;
        public UInt160 TrusteeScriptHash;
        protected override void OnMiningInit()
        {
            Init();
        }


        void Init()
        {
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
            if (bizPlugin.IsNotNull() && trusteeaddress.IsNotNullAndEmpty())
            {
                try
                {
                    var sh = trusteeaddress.ToScriptHash();
                    if (bizPlugin.TrustFunds.TryGetValue(sh, out TrustFundModel))
                    {
                        TrusteeScriptHash = sh;
                       
                    }
                }
                catch
                {

                }
            }
        }
    }
}

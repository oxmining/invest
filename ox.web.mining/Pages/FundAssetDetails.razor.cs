
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
using OX.Mining.DEX;

namespace OX.Web.Pages
{
    public partial class FundAssetDetails
    {
        public override string PageTitle => this.WebLocalString("信托基金资产明细", "Trust Fund Asset Details");
        [Parameter]
        public string trusteeaddress { get; set; }
        public TrustFundModel TrustFundModel;
        public UInt160 TrusteeScriptHash;
        Dictionary<UInt160, decimal> Rates = new Dictionary<UInt160, decimal>();
        string RateTitle = string.Empty;
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
                        var idos = bizPlugin.GetAll<DTFIDOKey, DTFIDORecord>(InvestBizPersistencePrefixes.TrustFundIDORecord, this.TrusteeScriptHash);
                        if (idos.IsNotNullAndEmpty())
                        {
                            decimal totalRatio = 0;
                            var total = idos.Sum(m => m.Value.IdoAmount.GetInternalValue());
                            long t = 0;
                            foreach (var ido in idos.OrderBy(m => (long)m.Value.BlockIndex * 10000 + (long)m.Value.TxN))
                            {
                                var f = ido.Value.IdoAmount.GetInternalValue();
                                var ratio = DividentSlope.Big_5.ComputeBonusRatio(total, t, f);
                                t += f;
                                if (this.Valid && this.EthID.MapAddress.Equals(ido.Value.IdoOwner))
                                {
                                    totalRatio += ratio;
                                }
                                decimal r = ratio;
                                if (Rates.TryGetValue(ido.Value.IdoOwner, out decimal d))
                                    r += d;
                                Rates[ido.Value.IdoOwner] = r;
                            }
                            if (this.Valid)
                                RateTitle= UIHelper.LocalString($"我的合计分红率  :   {totalRatio.ToString("f6")}", $"My total dividend rate  :   {totalRatio.ToString("f6")}");
                        }
                    }
                }
                catch
                {

                }
            }
        }
    }
}

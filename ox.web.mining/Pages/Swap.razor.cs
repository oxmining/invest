
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
//using static NBitcoin.Scripting.OutputDescriptor;
using OX.Mining.DEX;
using OX.UI.Swap;

namespace OX.Web.Pages
{
    public partial class Swap
    {
        public override string PageTitle => UIHelper.LocalString("兑换", "Swap");
        Dictionary<UInt160, SwapPairMerge> ExchangePairs = new Dictionary<UInt160, SwapPairMerge>();
        SideTransaction[] SideExchangePaires = new SideTransaction[0];
        protected override void OnMiningInit()
        {
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                foreach (var p in bizPlugin.GetAll<UInt160, SwapPairMerge>(InvestBizPersistencePrefixes.SwapPair).OrderByDescending(m => m.Value.SwapPairReply.TargetAssetId.Equals(Blockchain.OXS)).ThenByDescending(m => m.Value.SwapPairReply.TargetAssetId==invest.USDX_Asset).ThenByDescending(m => m.Value.Index))
                {
                    if (bizPlugin.SwapPairStates.TryGetValue(p.Key, out SwapPairStateReply stateReply) && stateReply.Flag != 1)
                        continue;
                    if (p.Value.TargetAssetState.AssetId == Blockchain.OXS)
                        continue;
                    ExchangePairs[p.Key] = p.Value;
                }
                List<SideTransaction> list = new List<SideTransaction>();
                foreach (var p in bizPlugin.GetAll<SideSwapPairKey, SideTransaction>(InvestBizPersistencePrefixes.SideSwapPair))
                {
                    list.Add(p.Value);
                }
                this.SideExchangePaires = list.ToArray();
            }
        }
    }
}

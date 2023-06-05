
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

namespace OX.Web.Pages
{
    public partial class Deposit
    {
        public override string PageTitle => UIHelper.LocalString("场外入金", "OTC Buy");
        OTCDealerViewModel[] OTCDealers { get; set; } = new OTCDealerViewModel[0];

        protected override void OnMiningInit()
        {
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
            if (bizPlugin.IsNotNull())
            {
                List<OTCDealerViewModel> list = new List<OTCDealerViewModel>();
                foreach (var r in bizPlugin.OTCDealers.Select(m => m.Value).Where(m => m.Setting.State == OTCStatus.Open))
                {
                    var account = Blockchain.Singleton.CurrentSnapshot.Accounts.TryGet(r.InPoolAddress);
                    if (account.IsNotNull() && account.Balances.TryGetValue(invest.USDX_Asset, out Fixed8 OTCDealerOXPoolBalance))
                    {
                        list.Add(new OTCDealerViewModel
                        {
                            OTCDealerMerge = r,
                            Balance = OTCDealerOXPoolBalance
                        });
                    }
                }
                OTCDealers = list.OrderByDescending(m => m.Balance).ToArray();
            }
        }
 
    }
}

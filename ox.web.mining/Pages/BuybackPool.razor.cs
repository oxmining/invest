
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

namespace OX.Web.Pages
{
    public partial class BuybackPool
    {
        public override string PageTitle => this.WebLocalString("回购信托池", "Buy-Back Trust Pool");
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
            
        }
       
    }
}

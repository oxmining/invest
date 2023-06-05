
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using OX.Wallets;
using OX.Wallets.Authentication;
using OX.Wallets.States;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using OX.MetaMask;
using OX.Mining.StakingMining;
using OX.Mining;
using Akka.Util;
using OX.Bapps;
using OX.UI.Mining;
using System.Linq;

namespace OX.Web.Models
{
    public abstract class LockMiningComponentBase : MiningComponentBase
    {
        protected MiningProvider Provider { get; set; }
        protected UInt160 SeedAddress { get; set; }
        protected MutualNode Miner;
        protected override void OnMiningInit()
        {
            ReloadMiner();
        }
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            ReloadMiner();
            AccountChanged();
        }
        protected abstract void AccountChanged();
        void ReloadMiner()
        {
            if (this.Valid)
            {
                Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                if (Provider.IsNotNull())
                {
                    SeedAddress = this.EthID.MapAddress.GetMutualLockSeed();
                    Provider.MutualLockNodes.TryGetValue(SeedAddress, out Miner);
                }
            }
        }
    }
}

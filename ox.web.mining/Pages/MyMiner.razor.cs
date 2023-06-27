
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
using OX.Mining.StakingMining;
using Akka.Util;
using Nethereum.Model;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace OX.Web.Pages
{
    public partial class MyMiner
    {
        public override string PageTitle => UIHelper.LocalString("我的矿机", "My Miner");
        MiningProvider Provider { get; set; }
        UInt160 SeedAddress { get; set; }
        MutualNode Miner;
        UInt160[] Leafs;
        string RegSeed;
        string HA = string.Empty;
        string HS = string.Empty;
        protected override void OnMiningInit()
        {
            if (this.Valid)
            {
                Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                if (Provider.IsNotNull())
                {
                    SeedAddress = this.EthID.MapAddress.GetMutualLockSeed();
                    Provider.MutualLockNodes.TryGetValue(SeedAddress, out Miner);
                    var lfs = Provider.MutualLockNodes.Values.Where(m => m.ParentHolder == this.EthID.MapAddress);
                    Leafs = new UInt160[0];
                    if (lfs.IsNotNullAndEmpty())
                    {
                        Leafs = lfs.Select(m => m.HolderAddress).ToArray();
                    }
                }
            }
        }
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            if (this.Valid)
            {
                Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                if (Provider.IsNotNull())
                {
                    SeedAddress = this.EthID.MapAddress.GetMutualLockSeed();
                    Provider.MutualLockNodes.TryGetValue(SeedAddress, out Miner);
                    var lfs = Provider.MutualLockNodes.Values.Where(m => m.ParentHolder == this.EthID.MapAddress);
                    Leafs = new UInt160[0];
                    if (lfs.IsNotNullAndEmpty())
                    {
                        Leafs = lfs.Select(m => m.HolderAddress).ToArray();
                    }
                }
            }
        }
        void OnRegister()
        {
            try
            {
                var regSh = this.RegSeed.ToScriptHash();
                NavigationManager.NavigateTo($"/wallet/transferasset/{Blockchain.OXC.ToString()}/1/{this.RegSeed}/100");
            }
            catch
            {
                this.RegSeed = string.Empty;
            }
        }
        void Cal()
        {
            if (HA.IsNotNullAndEmpty())
            {
                try
                {
                    var sh = HA.ToScriptHash();
                    HS = sh.GetMutualLockSeed().ToAddress();
                }
                catch
                {
                    HS = string.Empty;
                }
            }
        }
    }
}

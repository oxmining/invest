
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
using NuGet.Protocol.Plugins;
using OX.IO.Data.LevelDB;

namespace OX.Web.Pages
{
    public partial class MutualLockMining
    {
        public override string PageTitle => this.WebLocalString("互锁挖矿", "Mutual Lock Mining");
        Dictionary<UInt256, MutualMiningAssetData> Data = new Dictionary<UInt256, MutualMiningAssetData>();
        protected override void OnMiningInit()
        {
            base.OnMiningInit();
            ReloadData();
        }
    
        protected override void AccountChanged()
        {
            ReloadData();
        }
        void ReloadData()
        {
            this.Data.Clear();

            Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
            foreach (var r in Provider.GetAll<MutualLockMiningAssetKey, MutualLockMiningAssetReply>(InvestBizPersistencePrefixes.MutualLockMiningAssetReply).GroupBy(m => m.Key.AssetId))
            {
                MutualMiningAssetData d = new MutualMiningAssetData
                {
                    AssetId = r.Key,
                    AssetInfos = r.Select(m => new Tuple<MutualLockMiningAssetKey, MutualLockMiningAssetReply>(m.Key, m.Value)).ToArray()
                };
                if (this.Valid)
                {
                    var sb = SliceBuilder.Begin().Add(r.Key).Add(this.Miner.ParentHolder).Add(this.Miner.HolderAddress);
                    var k = Provider.GetAll<MutualLockKey, MutualLockValue>(InvestBizPersistencePrefixes.MutualLockRecords, sb.ToArray());
                    if (k.IsNotNullAndEmpty())
                    {
                        d.SelfMiningRecords = k.Select(m => m.Key).ToArray();
                    }
                    var j = Provider.GetAll<MutualLockInterestKey, MutualLockValue>(InvestBizPersistencePrefixes.MutualLockInterestRecords, sb.ToArray());
                    if (j.IsNotNullAndEmpty())
                    {
                        d.SelfInterestRecords = j.Select(m => m.Key).ToArray();
                    }
                    sb = SliceBuilder.Begin().Add(r.Key).Add(this.Miner.HolderAddress);
                    k = Provider.GetAll<MutualLockKey, MutualLockValue>(InvestBizPersistencePrefixes.MutualLockRecords, sb.ToArray());
                    if (k.IsNotNullAndEmpty())
                    {
                        d.LeafMiningRecords = k.Select(m => m.Key).ToArray();
                    }
                    j = Provider.GetAll<MutualLockInterestKey, MutualLockValue>(InvestBizPersistencePrefixes.MutualLockInterestRecords, sb.ToArray());
                    if (j.IsNotNullAndEmpty())
                    {
                        d.LeafInterestRecords = j.Select(m => m.Key).ToArray();
                    }
                }
                this.Data[r.Key] = d;
            }
        }
    }
}


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
    public partial class LevelLockMining
    {
        public override string PageTitle => this.WebLocalString("级锁挖矿", "Level Lock Mining");
        Dictionary<UInt256, LevelMiningAssetData> Data = new Dictionary<UInt256, LevelMiningAssetData>();
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
            foreach (var r in Provider.GetAll<LevelLockMiningAssetKey, LevelLockMiningAssetReply>(InvestBizPersistencePrefixes.LevelLockMiningAssetReply).GroupBy(m => m.Key.AssetId))
            {
                LevelMiningAssetData d = new LevelMiningAssetData
                {
                    AssetId = r.Key,
                    AssetInfos = r.Select(m => new Tuple<LevelLockMiningAssetKey, LevelLockMiningAssetReply>(m.Key, m.Value)).ToArray()
                };
                if (this.Valid&&this.Miner.IsNotNull())
                {
                    var sb = SliceBuilder.Begin().Add(this.EthID.MapAddress);
                    d.LevelLockInRecords = Provider.GetAll<LevelLockKey, LevelLockTx>(InvestBizPersistencePrefixes.LevelLockInRecords, sb.ToArray());
                    d.LevelLockOutRecords = Provider.GetAll<LevelLockKey, LevelLockTx>(InvestBizPersistencePrefixes.LevelLockOutRecords, sb.ToArray());
                    sb = SliceBuilder.Begin().Add(r.Key).Add(this.Miner.ParentHolder).Add(this.Miner.HolderAddress);
                    var ps = Provider.GetAll<LevelLockInterestKey, UInt256>(InvestBizPersistencePrefixes.LevelLockInterestRecords,sb.ToArray());
                    if (ps.IsNotNullAndEmpty())
                    {
                        d.SelfInterestRecords = ps.Select(m => m.Key).ToArray();
                    }
                    sb = SliceBuilder.Begin().Add(r.Key).Add(this.Miner.HolderAddress);
                    ps = Provider.GetAll<LevelLockInterestKey, UInt256>(InvestBizPersistencePrefixes.LevelLockInterestRecords,sb.ToArray());
                    if (ps.IsNotNullAndEmpty())
                    {
                        d.LeafInterestRecords = ps.Select(m => m.Key).ToArray();
                    }
                }
                this.Data[r.Key] = d;
            }
        }
    }
}

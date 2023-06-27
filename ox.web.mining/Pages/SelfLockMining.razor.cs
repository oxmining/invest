
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
    public partial class SelfLockMining
    {
        public override string PageTitle => UIHelper.LocalString("自锁挖矿", "Self Lock Mining");
        Dictionary<UInt256, SelfMiningAssetData> Data = new Dictionary<UInt256, SelfMiningAssetData>();
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
            foreach (var r in Provider.GetAll<LockMiningAssetKey,LockMiningAssetReply>(InvestBizPersistencePrefixes.LockMiningAssetReply).GroupBy(m => m.Key.AssetId))
            {
                SelfMiningAssetData d = new SelfMiningAssetData
                {
                    AssetId = r.Key,
                    AssetInfos = r.Select(m => new Tuple<LockMiningAssetKey, LockMiningAssetReply>(m.Key, m.Value)).ToArray()
                };
                if (this.Valid)
                {
                    var sb = SliceBuilder.Begin().Add(r.Key).Add(this.EthID.MapAddress);
                    var k = Provider.GetAll<SelfLockKey, LockMiningRecordMerge>(InvestBizPersistencePrefixes.LockMiningRecords, sb.ToArray());
                    if (k.IsNotNullAndEmpty())
                    {
                        d.MiningRecords = k.Select(m => m.Value).ToArray();
                    }
                    var j = Provider.GetAll<SelfLockKey, LockMiningRecordMerge>(InvestBizPersistencePrefixes.LockMiningInterestRecords, sb.ToArray());
                    if (j.IsNotNullAndEmpty())
                    {
                        d.InterestRecords = j.Select(m => m.Value).ToArray();
                    }
                    
                }
                this.Data[r.Key] = d;
            }
        }
    }
}

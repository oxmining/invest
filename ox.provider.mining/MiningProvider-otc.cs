using OX.Bapps;
using OX.Cryptography.ECC;
using OX.IO;
using OX.IO.Data.LevelDB;
using OX.Ledger;
using OX.Mining.DEX;
using OX.Mining.OTC;
using OX.Mining.Trade;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OX.Mining
{
    public partial class MiningProvider
    {
        public Dictionary<UInt160, OTCDealerMerge> OTCDealers { get; set; } = new Dictionary<UInt160, OTCDealerMerge>();
    }
    public static partial class MiningPersistenceHelper
    {
        public static void Save_OTCDealer(this WriteBatch batch, MiningProvider miningProvider, Block block, SideTransaction st, string ethAddress, OTCSetting setting)
        {
            var sh = st.GetContract().ScriptHash;
            if (miningProvider.OTCDealers.TryGetValue(sh, out OTCDealerMerge dealerMerge))
            {
                dealerMerge.Setting = setting;
            }
            else
            {
                dealerMerge = new OTCDealerMerge { EthAddress = ethAddress, InPoolAddress = sh, Setting = setting };
                miningProvider.OTCDealers[sh] = dealerMerge;
            }
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.OTC_Dealer).Add(sh), SliceBuilder.Begin().Add(dealerMerge));
        }
    }
}

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
using System.Runtime.CompilerServices;

namespace OX.Mining
{
    public partial class MiningProvider
    {
        public Dictionary<UInt160, OTCDealerMerge> OTCDealers { get; set; } = new Dictionary<UInt160, OTCDealerMerge>();
        public Dictionary<UInt256, Uint32Wrapper> ExchangeRequestRecords { get; set; } = new Dictionary<UInt256, Uint32Wrapper>();
        public bool ContainEthExchangeRequest(UInt256 ethTxId)
        {
            return ExchangeRequestRecords.ContainsKey(ethTxId);
        }
    }
    public static partial class MiningPersistenceHelper
    {
        public static void Save_OTCDealer(this WriteBatch batch, MiningProvider miningProvider, Block block, SlotSideTransaction st, string ethAddress, OTCSetting setting)
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
        public static void Save_OTCExchangeRequest(this WriteBatch batch, MiningProvider miningProvider, Block block, AskTransaction at, OTCExchangeRequest request)
        {
            var rs = miningProvider.ExchangeRequestRecords.Where(m => m.Value.Value + 10000 < block.Index);
            if (rs.IsNotNullAndEmpty())
            {
                var ks = rs.Select(m => m.Key).ToArray();
                foreach (var k in ks)
                {
                    if (miningProvider.ExchangeRequestRecords.Remove(k))
                    {
                        batch.Delete(SliceBuilder.Begin(InvestBizPersistencePrefixes.OTC_ExchangeRequest).Add(k));
                    }
                }
            }
            Uint32Wrapper index = new Uint32Wrapper(block.Index);
            miningProvider.ExchangeRequestRecords[request.EthTxHash] = index;
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.OTC_ExchangeRequest).Add(request.EthTxHash), SliceBuilder.Begin().Add(index));
        }
    }
}

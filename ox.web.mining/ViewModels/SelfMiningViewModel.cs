using OX.Mining.StakingMining;
using System;

namespace OX.Web
{

    public class SelfMiningAssetData
    {
        public UInt256 AssetId;
        public Tuple<LockMiningAssetKey,LockMiningAssetReply>[] AssetInfos = new Tuple<LockMiningAssetKey,LockMiningAssetReply>[0]; 
        public LockMiningRecordMerge[] MiningRecords=new LockMiningRecordMerge[0];
        public LockMiningRecordMerge[] InterestRecords = new LockMiningRecordMerge[0];
    }
}

using OX.Mining.StakingMining;
using System;

namespace OX.Web
{

    public class MutualMiningAssetData
    {
        public UInt256 AssetId;
        public Tuple<MutualLockMiningAssetKey, MutualLockMiningAssetReply>[] AssetInfos = new Tuple<MutualLockMiningAssetKey, MutualLockMiningAssetReply>[0]; 
        public MutualLockKey[] SelfMiningRecords=new  MutualLockKey[0];
        public MutualLockKey[] LeafMiningRecords = new MutualLockKey[0];
        public MutualLockInterestKey[] SelfInterestRecords = new MutualLockInterestKey[0];
        public MutualLockInterestKey[] LeafInterestRecords = new MutualLockInterestKey[0];
    }
}

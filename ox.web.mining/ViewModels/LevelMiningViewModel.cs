using OX.Mining.StakingMining;
using System;
using System.Collections.Generic;

namespace OX.Web
{

    public class LevelMiningAssetData
    {
        public UInt256 AssetId;
        public Tuple<LevelLockMiningAssetKey, LevelLockMiningAssetReply>[] AssetInfos = new Tuple<LevelLockMiningAssetKey, LevelLockMiningAssetReply>[0];
        public IEnumerable<KeyValuePair<LevelLockKey, LevelLockTx>> LevelLockInRecords = default;
        public IEnumerable<KeyValuePair<LevelLockKey, LevelLockTx>> LevelLockOutRecords = default;
        public LevelLockInterestKey[] SelfInterestRecords = new LevelLockInterestKey[0];
        public LevelLockInterestKey[] LeafInterestRecords = new LevelLockInterestKey[0];
    }
}

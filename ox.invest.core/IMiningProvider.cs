using OX.Bapps;
using OX.Cryptography.ECC;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.IO;
using System.Collections.Generic;
using OX.IO.Data.LevelDB;
using OX.Mining.StakingMining;
using OX.Mining.DEX;
using OX.Mining.Trade;
using OX.Mining.OTC;

namespace OX.Mining
{
    public interface IMiningProvider : IBappProvider
    {
        DB Db { get; }
        //Dictionary<UInt160, MinerData> MyMiners { get; }
        //Dictionary<UInt160, MinerData> MyPledgeMiners { get; }
        //Dictionary<UInt160, LeafMinerData> MyLeafPledgeMiners { get; }
        //Dictionary<UInt160, SwapTraderRecord> Deposit_SwapTraders { get; }
        Dictionary<UInt160, SwapPairMerge> SwapPairs { get; set; }
        Dictionary<UInt160, SideSwapPairKeyMerge> Side_SwapPairs { get; set; }
        Dictionary<UInt160, SwapPairStateReply> SwapPairStates { get; set; }
        Dictionary<UInt160, MutualNode> MutualLockNodes { get; set; }
        //Dictionary<UInt160, MutualNode> MyLeafMutualLockNodes { get; set; }
        //Dictionary<UInt160, MutualNode> MyParentMutualLockNodes { get; set; }
        Dictionary<UInt256, MutualLockMiningAssetReply> MutualLockAssets { get; set; }
        LevelLockMiningAssetReply LevelLockAsset { get; set; }
        Dictionary<string, LevelLockValue> LevelLockInTx { get; set; }
         Dictionary<UInt160, OTCDealerMerge> OTCDealers { get; set; }
        bool TryGetHashAccount(UInt256 PubkeyHash, out AccountPack pack);
        IEnumerable<KeyValuePair<byte[], InvestSettingRecord>> GetAllInvestSettings();
        LongWrapper GetTotalValidLockVolume(UInt256 assetId, UInt160 holder);
        //IEnumerable<KeyValuePair<MiningHashKey, MinePoolPublishRecord>> GetMinePools(UInt160 holder = default);
        //LevelThresholdPublishRecord GetMinePoolLevelThreshold(MiningKey minePoolKey);
        //IEnumerable<KeyValuePair<UInt160, MinerData>> GetMyMiners();
        //MinerData GetMinerData(UInt160 MinerHolder);
        //IEnumerable<KeyValuePair<MiningHolderKey, MinerParentPrivatePublish>> GetMyLeafMiners();
    }
}

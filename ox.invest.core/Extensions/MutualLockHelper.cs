using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using System.Runtime.CompilerServices;
using System.Security;
using OX.Mining.StakingMining;
using OX.Cryptography.ECC;

namespace OX.Mining
{

    public static class MutualLockHelper
    {
        static UInt160 genesisSeedAddress;
        public static UInt160 GenesisSeed()
        {
            if (genesisSeedAddress.IsNull())
            {
                genesisSeedAddress = invest.MasterAccountAddress.GetMutualLockSeed();
            }
            return genesisSeedAddress;
        }
        public static UInt160 GetMutualLockSeed(this UInt160 sh)
        {
            return new SideTransaction()
            {
                Recipient = invest.LockMiningAccountPubKey,
                SideType = SideType.ScriptHash,
                Data = sh.ToArray(),
                Flag = 1,//1标记代表种子
                AuthContract = Blockchain.SideAssetContractScriptHash,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            }.GetContract().ScriptHash;
        }
        public static bool VerifyMutualLockNodeRegister(this TransactionOutput output)
        {
            if (output.Value >= Fixed8.One * 100 && output.AssetId.Equals(Blockchain.OXC))
            {
                return true;
            }
            return false;
        }
        
        public static uint Calculate(this IEnumerable<MutualLockKey> mutualLockRecords, uint currentBlockHeight)
        {
            uint v = 0;
            foreach (var r in mutualLockRecords.Where(m => m.EndIndex >= currentBlockHeight))
            {
                if (currentBlockHeight > r.StartIndex)
                {
                    //The time gradient value of 1 coin is generated for every 100 coins
                    var multiple = (currentBlockHeight - r.StartIndex) / 100000;
                    var num = r.Amount.GetInternalValue() / (Fixed8.D * 100);
                    v += (uint)num * multiple;
                }
            }
            return v;
        }
        public static bool VerifyLevelLockRecord(this TransactionOutput output, IMiningProvider miningProvider)
        {
            if (miningProvider.LevelLockAsset.IsNotNull() && output.AssetId.Equals(Blockchain.OXC) && output.Value >= miningProvider.LevelLockAsset.MinAmount)
            {
                return true;
            }
            return false;
        }
    }
}

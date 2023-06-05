using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Mining
{
    public enum InvestType : byte
    {
        //MinePoolRequest = 0x12,
        //MinerRequest = 0x17,
        //NewSwapTraderRequest = 0x18,
        //SwapTraderSettingRequest = 0x19,
        OTCDealerRedeemRequest = 0x1A,
        //NewSwapTraderRequest = 0x1D,
        //SwapOutGoldConfirmRequest = 0x1E,
        //SwapOutGoldRequest = 0x1F,
        //SwapInGoldRequest = 0x20,
        //SwapInGoldReply = 0x21,
        //SwapPairReply = 0x22,
        SwapPairStateReply = 0x23,
        //SwapPairRequest = 0x24,
        //LockMiningAssetReply = 0x25,obsolete
        MutualLockMiningAssetReply = 0x26,
        LockMiningAssetReply = 0x27,
        LevelLockMiningAssetReply = 0x28,
        OTCDepositRequest = 0x29,
        OTCWithdrawalRequest = 0x2A,
        OTCRegisterRequest = 0x40,
    }
}

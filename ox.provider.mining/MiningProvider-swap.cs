using OX.Bapps;
using OX.Cryptography.ECC;
using OX.IO;
using OX.IO.Data.LevelDB;
using OX.Ledger;
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
        bool VerifyNewSwapTraderRequest(SwapTraderSettingRequest SwapTraderSetting)
        {
            if (SwapTraderSetting.InRate == 0) return false;
            if (SwapTraderSetting.OutRate == 0) return false;
            if (SwapTraderSetting.InFeeRate >= 50) return false;
            if (SwapTraderSetting.OutFeeRate >= 50 || SwapTraderSetting.OutFeeRate == 0) return false;
            if (SwapTraderSetting.OXSInMin >= 0 && SwapTraderSetting.OXSInMin < 100) return false;
            if (SwapTraderSetting.OXSOutMin >= 0 && SwapTraderSetting.OXSOutMin < 100) return false;
            if (SwapTraderSetting.OXCInMin >= 0 && SwapTraderSetting.OXCInMin < 100) return false;
            if (SwapTraderSetting.OXCOutMin >= 0 && SwapTraderSetting.OXCOutMin < 100) return false;
            return true;
        }

    }
}

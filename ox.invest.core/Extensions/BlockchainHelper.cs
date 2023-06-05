using OX.Ledger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Mining
{
    public static class BlockchainHelper
    {
        public static string GetAssetName(this UInt256 assetId)
        {
            if (assetId == Blockchain.OXS)
                return "OXS";
            else if (assetId == Blockchain.OXC)
                return "OXC";
            return string.Empty;
        }
    }
}

using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Mining.Trade;
using System.Linq;
using Nethereum.Model;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.AMI
{
    public static class AnchorMortgageIssueHelper
    {
        public static bool VerifyAnchorMortgageIssue(this EthereumMapTransaction emt, out TransactionOutput output)
        {
            output = default;
            if (!emt.IsIssue) return false;
            var contract = emt.GetContract();
            var op = emt.Outputs.FirstOrDefault(m => m.ScriptHash.Equals(contract.ScriptHash));
            if (op.IsNull()) return false;
            if (!op.AssetId.Equals(invest.USDT_Asset)) return false;
            output = op;
            return true;
        }
       
    }
}

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
using OX.SmartContract;
using OX.VM;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.OTC
{
    public static class OTCWithdrawalHelper
    {
        public static UInt160 BuildOutPoolAddress(this OTCWithdrawalRequest request)
        {
            return request.BuildOutPoolContract().ScriptHash;
        }
        public static Contract BuildOutPoolContract(this OTCWithdrawalRequest request)
        {
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                EthereumMapTransaction emt = new EthereumMapTransaction
                {
                    EthereumAddress = request.ToEthAddress
                };
                sb.EmitPush(request.OTCMasterPubKey);
                sb.EmitPush(request.From);
                sb.EmitPush(emt.GetContract().ScriptHash);
                sb.EmitPush(request.ExpirationIndex);
                sb.EmitAppCall(invest.OTCWithdrawalContractScriptHash);
                return Contract.Create(new[] { ContractParameterType.Signature }, sb.ToArray());
            }
        }
        public static bool Verify(this OTCWithdrawalRequest request, Transaction tx, out TransactionOutput output)
        {
            var sh = request.BuildOutPoolAddress();
            output = tx.Outputs.FirstOrDefault(m => m.AssetId == request.AssetId && m.ScriptHash == sh);
            return output.IsNotNull();
        }
    }
}

using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P.Payloads;
using System.IO;
using OX.VM;
using System.Security.Policy;
using OX.SmartContract;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.StakingMining
{
    public static class MiningHelper
    {
        public static bool IsSelfLock(this LockAssetTransaction lat, out UInt160 sh)
        {
            sh = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
            return lat.Witnesses.Select(m => m.ScriptHash).Contains(sh);
        }
        public static bool IsSelfLock(this EthereumMapTransaction emt)
        {
            if (emt.Attributes.IsNullOrEmpty()) return false;

            var attrs = emt.Attributes.Where(p => p.Usage == TransactionAttributeUsage.EthSignature);
            if (attrs.IsNullOrEmpty()) return false;

            var message = emt.InputOutputHash.ToArray().ToHexString();
            var signer = new Nethereum.Signer.EthereumMessageSigner();
            foreach (var attr in attrs)
            {
                try
                {
                    var signature = Encoding.UTF8.GetString(attr.Data);
                    var address = signer.EncodeUTF8AndEcRecover(message, signature);
                    if (address.IsNotNullAndEmpty())
                    {
                        if (emt.EthereumAddress.ToLower() == address.ToLower()) return true;

                    }
                }
                catch
                {
                    continue;
                }
            }
            return false;
        }
    }
}

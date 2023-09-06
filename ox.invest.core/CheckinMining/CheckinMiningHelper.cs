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
using System.Runtime.CompilerServices;
using System.Text;
using System;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.CheckinMining
{
    public static class CheckinMiningHelper
    {
        public static bool VerifyCheckinMining(this RangeTransaction rt, uint blockIndex, Func<uint, ulong> funGetConsensusData, out string ethAddress, out uint markIndex)
        {
            markIndex = 0;
            ethAddress = string.Empty;
            if (blockIndex <= rt.MaxIndex && blockIndex > rt.MinIndex && rt.MinIndex % 10000 == 0 && rt.MaxIndex % 10000 == 0 && rt.MaxIndex == rt.MinIndex + 10000 && rt.Attributes.IsNotNullAndEmpty() && rt.Attributes.Count() == 2 && rt.Witnesses.IsNullOrEmpty() && rt.Inputs.IsNullOrEmpty() && rt.Outputs.IsNullOrEmpty())
            {
                var attrs = rt.Attributes.Where(p => p.Usage == TransactionAttributeUsage.Tip4);
                if (attrs.IsNotNullAndEmpty() && attrs.Count() == 1)
                {
                    try
                    {
                        var chainPosition = attrs.First().Data.AsSerializable<CheckinMark>();
                        var signs = rt.Attributes.Where(p => p.Usage == TransactionAttributeUsage.EthSignature);
                        if (signs.IsNotNullAndEmpty() && signs.Count() == 1 && chainPosition.BlockIndex == rt.MinIndex && chainPosition.Kind == 0)
                        {
                            var sign = signs.First();
                            var signature = Encoding.UTF8.GetString(sign.Data);
                            var signer = new Nethereum.Signer.EthereumMessageSigner();
                            if (funGetConsensusData.IsNull()) return false;
                            var nonce = funGetConsensusData(rt.MinIndex);
                            if (nonce == 0) return false;
                            var message = $"{rt.MinIndex}-{nonce}";
                            ethAddress = signer.EncodeUTF8AndEcRecover(message, signature);
                            if (ethAddress.IsNotNullAndEmpty())
                            {
                                markIndex = rt.MinIndex;
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}

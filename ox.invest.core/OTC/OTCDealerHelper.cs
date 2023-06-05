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

namespace OX.Mining.OTC
{
    public static class OTCDealerHelper
    {
        public static SideTransaction BuildOTCDealerTransaction(this string ethAddress, OTCSetting setting = default)
        {
            byte[] attch = setting.IsNotNull() ? setting.ToArray() : new byte[0];
            SideTransaction st = new SideTransaction
            {
                Recipient = invest.OTCAccountPubKey,
                SideType = SideType.EthereumAddress,
                Data = ethAddress.HexToByteArray(),
                Flag = 0,
                AuthContract = Blockchain.SideAssetContractScriptHash,
                Attach = attch
            };
            return st;
        }
        public static bool VerifyOTCDealerTx(this SideTransaction st, out string ethAddress, out OTCSetting setting)
        {
            ethAddress = string.Empty;
            setting = default;
            if (
                st.Flag == 0
                && st.SideType == SideType.EthereumAddress
                && st.Recipient.Equals(invest.OTCAccountPubKey)
                && st.AuthContract == Blockchain.SideAssetContractScriptHash
                && st.EthSignatures.IsNotNullAndEmpty())
            {
                try
                {
                    if (st.Attach.IsNotNullAndEmpty())
                    {
                        setting = st.Attach.AsSerializable<OTCSetting>();
                        if (setting.InRate > 20 || setting.OutRate > 20) return false;

                    }
                    var addr = new AddressUtil().ConvertToChecksumAddress(st.Data.ToHex());
                    var message = st.InputOutputHash.ToArray().ToHexString();
                    var signer = new Nethereum.Signer.EthereumMessageSigner();
                    foreach (var sig in st.EthSignatures)
                    {
                        try
                        {
                            var address = signer.EncodeUTF8AndEcRecover(message, sig.CreateStringSignature());
                            bool ok = addr.ToLower() == address.ToLower();
                            if (ok)
                            {
                                ethAddress = address;
                                return true;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}

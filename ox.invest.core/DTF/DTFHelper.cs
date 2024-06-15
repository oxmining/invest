using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Cryptography.ECC;
using OX.SmartContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OX.Mining.DTF
{
    public static class DTFHelper
    {
        public static ECPoint CasinoSettleAccountPubKey = ECPoint.DecodePoint("02e673665c8a62067bc2764fa4fa75328494bf74546dbf60faf04dd8d21f25e324".HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 CasinoSettleAccountAddress = Contract.CreateSignatureRedeemScript(CasinoSettleAccountPubKey).ToScriptHash();
        public static ECPoint CasinoWitnessAccountPubKey = ECPoint.DecodePoint("02a3147c018322228c648a9d5f95f8f73eafb932f790d307388c0918921fca73f0".HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 CasinoWitnessAccountAddress = Contract.CreateSignatureRedeemScript(CasinoWitnessAccountPubKey).ToScriptHash();

        public static AssetTrustTransaction BuildFund(this ECPoint Trustee)
        {
            return new AssetTrustTransaction
            {
                TrustContract = Blockchain.TrustAssetContractScriptHash,
                IsMustRelateTruster = true,
                Truster = invest.DTFLockAssetCustodyAccountPubKey,
                Trustee = Trustee,
                Targets = new UInt160[] { Trustee.BuildWitnessAddress() },
                SideScopes = new UInt160[] { invest.SidePoolAccountAddress, CasinoSettleAccountAddress, CasinoWitnessAccountAddress }
            };
        }
        public static UInt160 BuildWitnessAddress(this ECPoint Trustee, byte flag = 0)
        {
            var tx = new SlotSideTransaction()
            {
                Slot = invest.LockMiningAccountPubKey,
                Channel = 0x02,
                SideType = SideType.PublicKey,
                Data = Trustee.ToArray(),
                Flag = flag,
                AuthContract = Blockchain.SideAssetContractScriptHash,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            };
            return tx.GetContract().ScriptHash;
        }
        public static bool TryVerifyRegTrustFund(this AssetTrustTransaction att, out ECPoint trustee)
        {
            trustee = default;
            if (att.IsNull()) return false;
            var fundContract = att.Trustee.BuildFund();
            var sh = att.GetContract().ScriptHash;
            if (sh == fundContract.GetContract().ScriptHash)
            {
                if (att.Outputs.Any(m => m.ScriptHash.Equals(sh) && m.AssetId == Blockchain.OXC && m.Value >= Fixed8.One * 10000))
                {
                    if (att.GetPublicKeys().Contains(att.Trustee))
                    {
                        trustee = att.Trustee;
                        return true;
                    }
                }
            }
            return false;
        }

    }
}

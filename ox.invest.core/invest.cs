using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.Cryptography.ECC;
using OX.SmartContract;
using OX.Wallets;
using OX.Ledger;

namespace OX.Mining
{
    public class invest
    {
        public const string Name = "invest";
        public const string LockMiningOfficalEventBoardId = "105753-1";
        public const string DEXOfficalEventBoardId = "105764-1";
        public static string[] PubKeys = new string[] {
        "02a829c4171ff79c581097abd1f6de90223a7f053f955e16d8f0749d4fe4440aa4",
        "03f58b8373b85e9fc54ab09bfff5741423b7aaf277d6c70cebf7d04db18ed55ad4",
        "03d03c73dce48f5a8c93e25a5172aebbadc511768eab807cfebaf47353ea0bb6b0",
        "021c4d1fe831fded3e57b7bc954bacc3e1a66060557aca378e02638bcf6cdfe0df",
        "02aade56ee0d4e40a18fb939ab6c3d1eff3f84ea2681134536c3d2126c6029ac83",
        "02ea34b278a3812ee869ba7e06a815ab4a6e2b9c33c1bad6b91ef914f0f22a920e",
        "032bc82546381ed634c7940cf7a63b7c8dc972fae142d82d4d84d871524b75b1a4"
        };
        public static ECPoint[] BizPublicKeys { get; private set; }

        public static Dictionary<UInt160, ECPoint> BizAddresses { get; private set; }
        public static string[] SettingAccounts = new string[] {
        "AH1a5t7N7csjunnRxAAbtVoh35Q1aLG6UR",
        "ANg8pku2SoBRY7v62YEyFYV1jkhHs2qeqq",
        "AY64o5DbkHuY5oeUefuqk3cRNwDAsqRDdh",
        "AHyxwA5Wxwh5Dxu3Xo3xmMS64hhLLAcBpb",
        "ANdiYeezEZ3TA9q3ps7KSo6ovE9vizJrHG",
        "AFzmGz1iqaf1PyR9TSkFJm77wg7xhhXvvX",
        "AFqfGhapKPGqx34Pk2gqHwh6mZLzQrwDfN"
        };
        public static ECPoint MasterAccountPubKey = ECPoint.DecodePoint(PubKeys[0].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 MasterAccountAddress = Contract.CreateSignatureRedeemScript(MasterAccountPubKey).ToScriptHash();
       
        public static ECPoint SidePoolAccountPubKey = ECPoint.DecodePoint(PubKeys[1].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 SidePoolAccountAddress = Contract.CreateSignatureRedeemScript(SidePoolAccountPubKey).ToScriptHash();

        public static ECPoint SwapFeeAccountPubKey = ECPoint.DecodePoint(PubKeys[2].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 SwapFeeAccountAddress = Contract.CreateSignatureRedeemScript(SwapFeeAccountPubKey).ToScriptHash();

        public static ECPoint LockMiningAccountPubKey = ECPoint.DecodePoint(PubKeys[5].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 LockMiningAccountAddress = Contract.CreateSignatureRedeemScript(LockMiningAccountPubKey).ToScriptHash();

        public static ECPoint OTCAccountPubKey = ECPoint.DecodePoint(PubKeys[6].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 OTCAccountAddress = Contract.CreateSignatureRedeemScript(OTCAccountPubKey).ToScriptHash();

        
         public static UInt256 USDX_Asset
        { get; private set; } = UInt256.Parse("0x7035ea8f10c4209340aafc6f826c969be9ce2ab3e168feb067303c1dc05a1573");

        public static UInt256 DEXBonusAsset
        { get; private set; } = UInt256.Parse("0xa24f75b23a4965cb132c9a38651bee994a2083bd6a716017f509b22c7a7f2e5c");

        public static UInt160 OTCWithdrawalContractScriptHash 
        { get; private set; } = UInt160.Parse("0x046f69a88fde323a8ca157dbd4d82a98f7d13723");
        static invest()
        {
            BizPublicKeys = PubKeys.Select(p => ECPoint.DecodePoint(p.HexToBytes(), ECCurve.Secp256r1)).ToArray();
            BizAddresses = BizPublicKeys.ToDictionary(n => Contract.CreateSignatureRedeemScript(n).ToScriptHash());
        }
        public static bool AllowSetting(uint SettingIndex)
        {
            if (SettingIndex >= SettingAccounts.Length) return false;
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(SettingAccounts[SettingIndex].ToScriptHash(), () => null);
            if (acts.IsNull()) return false;
            return acts.GetBalance(Blockchain.OXC) > Fixed8.Zero;
        }
    }
}

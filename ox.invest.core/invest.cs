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
        "032bc82546381ed634c7940cf7a63b7c8dc972fae142d82d4d84d871524b75b1a4",
        "02f3a117fd8a2ba5483ba3599edd53ddf58e075bf495ea4c9e78e57f0d092c2fa1",
        "03529db24e1006e8153d1ae50cfd59de5b8daed7ae87bef8402c1d467957243316",
        "02d957c68656b3b9c1316551cc9851fee327ba8a92ee127c2ca1585151df932015"
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

        public static ECPoint MiningBuyBackFundAccountPubKey = ECPoint.DecodePoint(PubKeys[3].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 MiningBuyBackFundAccountAddress = Contract.CreateSignatureRedeemScript(MiningBuyBackFundAccountPubKey).ToScriptHash();

        public static ECPoint LockMiningAccountPubKey = ECPoint.DecodePoint(PubKeys[5].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 LockMiningAccountAddress = Contract.CreateSignatureRedeemScript(LockMiningAccountPubKey).ToScriptHash();

        public static ECPoint OTCAccountPubKey = ECPoint.DecodePoint(PubKeys[6].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 OTCAccountAddress = Contract.CreateSignatureRedeemScript(OTCAccountPubKey).ToScriptHash();

        public static ECPoint SlaveSidePoolAccountPubKey = ECPoint.DecodePoint(PubKeys[7].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 SlaveSidePoolAccountAddress = Contract.CreateSignatureRedeemScript(SlaveSidePoolAccountPubKey).ToScriptHash();

        public static ECPoint TrustFundWitnessPubKey = ECPoint.DecodePoint(PubKeys[8].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 TrustFundWitnessAddress = Contract.CreateSignatureRedeemScript(TrustFundWitnessPubKey).ToScriptHash();

        public static UInt256 USDX_Asset
        { get; private set; } = UInt256.Parse("0x7035ea8f10c4209340aafc6f826c969be9ce2ab3e168feb067303c1dc05a1573");
        public static UInt256 MLM_Asset
        { get; private set; } = UInt256.Parse("0x80c531d84f1f1fd04c3be5fe8a2ce8b50831b60e145035ac1ed18281e2133608");

        public static UInt256 SLM_Asset
        { get; private set; } = UInt256.Parse("0x1e953288acd127a066110ca9e6bbfc2ce8821cd0e10a94fd0a6b3138b452434d");

        public static UInt256 LLM_Asset
        { get; private set; } = UInt256.Parse("0xb6be9c0e8e8360eceb44f1fa503332f9f63418204757f21e34cf10769d6fd5e4");

        public static UInt256 BNS_Asset
        { get; private set; } = UInt256.Parse("0xb11e03edb58288218f5e9e12da3ab77cfadaf046cae5b547bd402c6f5b452725");
        public static UInt256 ML2_Asset
        { get; private set; } = UInt256.Parse("0x70955c804f4263513b671cf18b5ff10a80b5088c08c5aceaa55cdfae44acb2ea");
        public static UInt256 SL2_Asset
        { get; private set; } = UInt256.Parse("0xcbb321f3db6dddb1b7311f57a8219d3bd822a4a1ea61657d9e5ccdfd7e5696be");
        public static UInt256 LL2_Asset
        { get; private set; } = UInt256.Parse("0x3a1788400daaa7b489dd498154cc2bf1a2c456c91a5d01f086e1712a072ddffc");



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

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
        public const string Name = "mining";
        public const string LockMiningOfficalEventBoardId = "171735-1";
        public const string DEXOfficalEventBoardId = "171746-1";
        public static string[] PubKeys = new string[] {
        "0281f5df26e72bcf48e7378d33bf280b88fb51dd7e73f196d6f07a8d125a2f7c98",
        "022e5d9efad9b521d8cf4e5ce1b53c3520542d73a57f1bcc8d50a17fb9a9213a5e",
        "02a2915a9ef5ac069d7a6912a0b6a7a6bc6fa9cfd10994bbb80bbb027cb0c463a3",
        "031f59c5b11d0a7dc865efd31ddb6cb13f946cc32c714b5d8baa1c8d2cadf644c6",
        "03bdcd89b6e24a2f7e2beff79aefa460b22716d1d83846ae0df2e1c2f6cc8a7eed",
        "03f2e255acd0b2641fec0bf4f348544c94fca18526681814b16ec62b8cae776d28",
        "031210e335f143c2a6300741210efaea87c3632cd265cf9dbdace1fb9411709f23",
        "03dda536fa3c26e749bec80b47588a60523db2d778c24dd65bf67170c0b2f693dc",
        "024e84200ee49934bb0d63ce8a0f9862bb81bd609ef7fcd2e3328e16b6e1a94512",
        "0215dad3056b04b6d43da274af560a68fee82b67312ac260416f114ad9ecd81597"
        };
        public static ECPoint[] BizPublicKeys { get; private set; }

        public static Dictionary<UInt160, ECPoint> BizAddresses { get; private set; }
        
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
 
        public static ECPoint SlaveSidePoolAccountPubKey = ECPoint.DecodePoint(PubKeys[7].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 SlaveSidePoolAccountAddress = Contract.CreateSignatureRedeemScript(SlaveSidePoolAccountPubKey).ToScriptHash();

      
        public static UInt256 USDT_Asset
        { get; private set; } = UInt256.Parse("0xb2858d485532ec422d7e82371945cf384563adbf8bc934982ecd3ca557fb6f74");
        public static UInt256 MLM_Asset
        { get; private set; } = UInt256.Parse("0x41a5e60f3a44d562cc544ef6fcfbc217bbd67475a63783d19676147af696f89e");

        public static UInt256 SLM_Asset
        { get; private set; } = UInt256.Parse("0x24d60cdd3624f006d7237e3bc1eae2604453a23a77d66c0013298e3ac357cd7b");

        public static UInt256 LLM_Asset
        { get; private set; } = UInt256.Parse("0xfbf06dcdf304bbfbf7b0f2303ac5e9b2ffd06afa0d09246ca5b23fd1a0d678a5");

        public static UInt256 BNS_Asset
        { get; private set; } = UInt256.Parse("0x1714b263a281e8931a24c4cf9bbd575f75a73214b2fedfc7f208c9d01307d751");
        public static UInt256 ML2_Asset
        { get; private set; } = UInt256.Parse("0xc0e7a5416f620ca317ce1ecb89ec090abbf8722bee2807d72e26995f97850480");
        public static UInt256 SL2_Asset
        { get; private set; } = UInt256.Parse("0x15fcfd80c8da7db8b5f8028af2f4072fb815431319a89681be3bd664528918ca");
        public static UInt256 LL2_Asset
        { get; private set; } = UInt256.Parse("0xdd2bc7bb1335b94a3d11280ef541901b243c7932565fa5975eb06d8cedafc8de");



        public static UInt256 DEXBonusAsset
        { get; private set; } = UInt256.Parse("0xa24f75b23a4965cb132c9a38651bee994a2083bd6a716017f509b22c7a7f2e5c");

        public static UInt160 OTCWithdrawalContractScriptHash 
        { get; private set; } = UInt160.Parse("0x046f69a88fde323a8ca157dbd4d82a98f7d13723");
        static invest()
        {
            BizPublicKeys = PubKeys.Select(p => ECPoint.DecodePoint(p.HexToBytes(), ECCurve.Secp256r1)).ToArray();
            BizAddresses = BizPublicKeys.ToDictionary(n => Contract.CreateSignatureRedeemScript(n).ToScriptHash());
        }
         
    }
}

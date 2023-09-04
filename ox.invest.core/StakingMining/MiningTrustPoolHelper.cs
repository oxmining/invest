using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Wallets;
using OX.Cryptography.ECC;
using System.Security.Policy;
using System.IO;
using OX.Network.P2P;
using System.Runtime;

namespace OX.Mining.StakingMining
{
    public static class MiningTrustPoolHelper
    {
         
        public static Dictionary<string, string> AssetTargets = new Dictionary<string, string>();
        public static Dictionary<string, UInt256> TargetAssets = new Dictionary<string, UInt256>();
        public static ECPoint Truster = invest.MasterAccountPubKey;
        public static ECPoint Trustee = invest.MiningBuyBackFundAccountPubKey;
        static MiningTrustPoolHelper()
        {
            AssetTargets["MLM"] = "ALFDnFuAeShm7vFyFmZhjiBLAHJJhpn8bG";
            AssetTargets["SLM"] = "Aaad8ABGsmun7YWMTTYuC5DDY5tqbeHTFJ";
            AssetTargets["LLM"] = "AaiQJMeqmrunxASM8DdJDquDxcG8nNtHXX";
            AssetTargets["ML2"] = "ATxdqbSJyQNRN8X6zyJDWrDzAYfo3oPyP5";
            AssetTargets["SL2"] = "AQC1NeRC8BfN5QmKTAfHumQi9UUdL38chH";
            AssetTargets["LL2"] = "AMtDeobTS9czi6MPRVDdERfYp7gWaUqCmX";
            AssetTargets["BNS"] = "AUTz1AAwj4Dpk9u6VzWupqT2NHyVd2spfZ";



            TargetAssets["OXC"] = Blockchain.OXC;
            TargetAssets["MLM"] = UInt256.Parse("0x80c531d84f1f1fd04c3be5fe8a2ce8b50831b60e145035ac1ed18281e2133608");
            TargetAssets["SLM"] = UInt256.Parse("0x1e953288acd127a066110ca9e6bbfc2ce8821cd0e10a94fd0a6b3138b452434d");
            TargetAssets["LLM"] = UInt256.Parse("0xb6be9c0e8e8360eceb44f1fa503332f9f63418204757f21e34cf10769d6fd5e4");
            TargetAssets["BNS"] = UInt256.Parse("0xb11e03edb58288218f5e9e12da3ab77cfadaf046cae5b547bd402c6f5b452725");
            TargetAssets["ML2"] = UInt256.Parse("0x70955c804f4263513b671cf18b5ff10a80b5088c08c5aceaa55cdfae44acb2ea");
            TargetAssets["SL2"] = UInt256.Parse("0xcbb321f3db6dddb1b7311f57a8219d3bd822a4a1ea61657d9e5ccdfd7e5696be");
            TargetAssets["LL2"] = UInt256.Parse("0x3a1788400daaa7b489dd498154cc2bf1a2c456c91a5d01f086e1712a072ddffc");
        }
        static UInt160 _trustPoolAddress;
        public static UInt160 TrustPoolAddress
        {
            get
            {
                if (_trustPoolAddress.IsNull())
                    _trustPoolAddress = GetCasinoTrustPoolAddress();
                return _trustPoolAddress;
            }
        }
        static UInt160 GetCasinoTrustPoolAddress()
        {
            AssetTrustTransaction att = new AssetTrustTransaction
            {
                TrustContract = Blockchain.TrustAssetContractScriptHash,
                IsMustRelateTruster = true,
                Truster = Truster,
                Trustee = Trustee,
                Targets = AssetTargets.Select(m => m.Value.ToScriptHash()).OrderBy(p => p).ToArray()
            };
            return att.GetContract().ScriptHash;
        }
    }
}

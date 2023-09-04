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
            TargetAssets["MLM"] = invest.MLM_Asset;
            TargetAssets["SLM"] = invest.SLM_Asset;
            TargetAssets["LLM"] = invest.LLM_Asset;
            TargetAssets["BNS"] = invest.BNS_Asset;
            TargetAssets["ML2"] = invest.ML2_Asset;
            TargetAssets["SL2"] = invest.SL2_Asset;
            TargetAssets["LL2"] = invest.LL2_Asset;
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

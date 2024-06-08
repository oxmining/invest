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
        public const string MLMExchangePoolAddress = "APKrG6vQdBZamPJ8AG7G23bjVRJn4t74nz";
        public const string SLMExchangePoolAddress = "AHTqcyqSD5QvYbmhykfw1dhrqchTNRz3ge";
        public const string LLMExchangePoolAddress = "AUTPhx9WWNekJK8ny73GNNvWsspjUftoTa";
        public static Dictionary<string, string> AssetTargets = new Dictionary<string, string>();
        public static Dictionary<string, UInt256> TargetAssets = new Dictionary<string, UInt256>();
        public static ECPoint Truster = invest.MasterAccountPubKey;
        public static ECPoint Trustee = invest.MiningBuyBackFundAccountPubKey;
        static MiningTrustPoolHelper()
        {
            AssetTargets["MLM"] = MLMExchangePoolAddress;
            AssetTargets["SLM"] = SLMExchangePoolAddress;
            AssetTargets["LLM"] = LLMExchangePoolAddress;
            AssetTargets["ML2"] = "AKHXKpR2MiiMLHH53y7fW9vYZsLZVWx3S7";
            AssetTargets["SL2"] = "APty1oYg7JqbrZJ9S6bGmLPQy95ZuXCsWi";
            AssetTargets["LL2"] = "AS1jQ18oJjgZm7axZciN8DLT1XfsYr9b3e";
            AssetTargets["BNS"] = "AMv9zcQKNbYPF5QpK8Z1L4cnhYR1tNfK7w";



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

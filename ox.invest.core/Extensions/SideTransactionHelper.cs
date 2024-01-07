using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Mining.DEX;

namespace OX.Mining
{
    public static class SideTransactionHelper
    {
        public static readonly Fixed8 MinSidePoolOXC = Fixed8.One * 1000;
        public static bool VerifyRegMainSwap(this SideTransaction tx, out UInt256 Asset,out SwapPairReply swapPairReply)
        {
            Asset = default;
            swapPairReply = default;
            if (!tx.Recipient.Equals(invest.SidePoolAccountPubKey) || tx.Flag != 0 || tx.SideType != SideType.AssetID || !tx.AuthContract.Equals(Blockchain.SideAssetContractScriptHash)) return false;
            if (!tx.GetPublicKeys().Contains(invest.MasterAccountPubKey)) return false;
            if (tx.Attach.IsNullOrEmpty()) return false;
            try
            {
                Asset = tx.Data.AsSerializable<UInt256>();
                swapPairReply = tx.Attach.AsSerializable<SwapPairReply>();
                if (swapPairReply.TargetAssetId != Asset) return false;
                //var sh = tx.GetContract().ScriptHash;
                //var outputs = tx.Outputs.Where(m => m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(sh));
                //if (outputs.IsNullOrEmpty()) return false;
                //if (outputs.Sum(m => m.Value) < MinSidePoolOXC) return false;
                //outputs = tx.Outputs.Where(m => m.AssetId.Equals(assetId) && m.ScriptHash.Equals(sh));
                //if (outputs.IsNullOrEmpty()) return false;
                //if (outputs.Sum(m => m.Value) < MinSidePoolOXC) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool VerifyRegSideSwap(this SideTransaction tx, out UInt256 Asset)
        {
            Asset = default;
            if (!tx.Recipient.Equals(invest.SlaveSidePoolAccountPubKey) || tx.Flag != 1 || tx.SideType != SideType.AssetID || !tx.AuthContract.Equals(Blockchain.SideAssetContractScriptHash)) return false;
            try
            {
                var assetId = tx.Data.AsSerializable<UInt256>();
                Asset = assetId;
                var sh = tx.GetContract().ScriptHash;
                var outputs = tx.Outputs.Where(m => m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(sh));
                if (outputs.IsNullOrEmpty()) return false;
                if (outputs.Sum(m => m.Value) < MinSidePoolOXC) return false;
                outputs = tx.Outputs.Where(m => m.AssetId.Equals(assetId) && m.ScriptHash.Equals(sh));
                if (outputs.IsNullOrEmpty()) return false;
                if (outputs.Sum(m => m.Value) < MinSidePoolOXC) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool VerifyRegSideSwapFee(this SideTransaction tx, Fixed8 SidePoolFeeSetting)
        {
            var outputs = tx.Outputs.Where(m => m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(invest.SlaveSidePoolAccountAddress));
            if (outputs.IsNullOrEmpty()) return false;
            if (outputs.Sum(m => m.Value) < SidePoolFeeSetting) return false;
            return true;
        }
    }
}

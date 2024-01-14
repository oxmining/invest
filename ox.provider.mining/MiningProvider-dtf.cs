using Akka.Util;
using OX.Bapps;
using OX.Cryptography.ECC;
using OX.IO;
using OX.IO.Data.LevelDB;
using OX.Ledger;
using OX.Mining.CheckinMining;
using OX.Mining.DEX;
using OX.Mining.DTF;
using OX.Mining.OTC;
using OX.Mining.StakingMining;
using OX.Mining.Trade;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OX.Mining
{
    public partial class MiningProvider
    {
        public Dictionary<UInt160, TrustFundModel> TrustFunds { get; set; } = new Dictionary<UInt160, TrustFundModel>();
        public Dictionary<OutputKey, LockAssetMerge> DTFLockAssets { get; set; } = new Dictionary<OutputKey, LockAssetMerge>();
        public Dictionary<DTFIDOSummaryKey, Fixed8> DTFIDOSummary { get; set; } = new Dictionary<DTFIDOSummaryKey, Fixed8>();
        public void OnAssetTrustTransaction(WriteBatch batch, Block block, ushort txN, AssetTrustTransaction att)
        {
            if (att.TryVerifyRegTrustFund(out ECPoint trustee))
            {
                var sh = Contract.CreateSignatureRedeemScript(trustee).ToScriptHash();
                if (!this.TrustFunds.ContainsKey(sh))
                {
                    TrustFundModel model = new TrustFundModel
                    {
                        Trustee = trustee,
                        AssetTrustTransaction = att,
                        TrustAddress = att.GetContract().ScriptHash,
                        DividendAddress = trustee.BuildWitnessAddress(),
                        SubscribeAddress = trustee.BuildWitnessAddress(1),
                        ArbitrateAddress = trustee.BuildWitnessAddress(2),
                        Index = block.Index,
                        TxN = txN
                    };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.TrustFundRequest).Add(sh), SliceBuilder.Begin().Add(model));
                    this.TrustFunds[sh] = model;
                }
            }
        }
        public IEnumerable<KeyValuePair<OutputKey, LockAssetMerge>> GetAllDTFLockAssets()
        {
            return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(InvestBizPersistencePrefixes.DTF_LockAsset_Record), (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<OutputKey, LockAssetMerge>(ks.AsSerializable<OutputKey>(), data.AsSerializable<LockAssetMerge>());
            });
        }
        public IEnumerable<KeyValuePair<DTFIDOSummaryKey, Fixed8>> GetAllDTFIDOSummary(UInt160 IDOOwner = default)
        {
            var builder = SliceBuilder.Begin(InvestBizPersistencePrefixes.TrustFundIDOSummary);
            if (IDOOwner.IsNotNull())
                builder = builder.Add(IDOOwner);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<DTFIDOSummaryKey, Fixed8>(ks.AsSerializable<DTFIDOSummaryKey>(), data.AsSerializable<Fixed8>());
            });
        }
    }
    public static partial class MiningPersistenceHelper
    {
        public static void Watch_DTFTransaction(this WriteBatch batch, MiningProvider miningProvider, Block block, ushort TxN, Transaction tx, UInt160[] bizshs)
        {
            batch.WatchSubscribeToTrust(miningProvider, tx);
            batch.WatchTrustToDividend(miningProvider, tx);
            batch.WatchIDOSubscribe(miningProvider, block, TxN, tx);
        }
        public static void WatchSubscribeToTrust(this WriteBatch batch, MiningProvider miningProvider, Transaction tx)
        {
            if (tx.IsNull()) return;
            foreach (var output in tx.Outputs)
            {
                if (output.AssetId == Blockchain.OXC)
                {
                    var tf = miningProvider.TrustFunds.Where(m => m.Value.TrustAddress.Equals(output.ScriptHash)).FirstOrDefault();
                    if (!tf.Equals(new KeyValuePair<UInt160, TrustFundModel>()))
                    {
                        var rfs = tx.References.Values.Select(m => m.ScriptHash);
                        if (rfs.IsNotNullAndEmpty() && rfs.Contains(tf.Value.SubscribeAddress))
                        {
                            tf.Value.TotalSubscribeAmount += output.Value;
                            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.TrustFundRequest).Add(tf.Key), SliceBuilder.Begin().Add(tf.Value));
                        }
                    }
                }
            }
        }
        public static void WatchTrustToDividend(this WriteBatch batch, MiningProvider miningProvider, Transaction tx)
        {
            if (tx.IsNull()) return;
            foreach (var output in tx.Outputs)
            {
                if (output.AssetId == Blockchain.OXC)
                {
                    var tf = miningProvider.TrustFunds.Where(m => m.Value.DividendAddress.Equals(output.ScriptHash)).FirstOrDefault();
                    if (!tf.Equals(new KeyValuePair<UInt160, TrustFundModel>()))
                    {
                        var rfs = tx.References.Values.Select(m => m.ScriptHash);
                        if (rfs.IsNotNullAndEmpty() && rfs.Contains(tf.Value.TrustAddress))
                        {
                            tf.Value.TotalDividendAmount += output.Value;
                            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.TrustFundRequest).Add(tf.Key), SliceBuilder.Begin().Add(tf.Value));
                        }
                    }
                }
            }
        }
        public static void WatchIDOSubscribe(this WriteBatch batch, MiningProvider miningProvider, Block block, ushort TxN, Transaction tx)
        {
            if (tx.IsNull()) return;
            foreach (var output in tx.Outputs)
            {
                if (output.AssetId == Blockchain.OXC && output.Value >= Fixed8.OXT)
                {
                    var tf = miningProvider.TrustFunds.Where(m => m.Value.SubscribeAddress.Equals(output.ScriptHash)).FirstOrDefault();
                    if (!tf.Equals(new KeyValuePair<UInt160, TrustFundModel>()))
                    {
                        var rfsOutput = tx.References.Values.Where(m => m.AssetId == Blockchain.OXC).OrderByDescending(m => m.Value).FirstOrDefault();
                        if (rfsOutput.IsNotNull())
                        {
                            var fromsh = rfsOutput.ScriptHash;
                            if (!fromsh.Equals(tf.Value.TrustAddress) && !fromsh.Equals(tf.Value.SubscribeAddress))
                            {
                                DTFIDOKey key = new DTFIDOKey { TrusteeAddress = tf.Key, IDOOwner = rfsOutput.ScriptHash, TxId = tx.Hash };
                                DTFIDORecord record = new DTFIDORecord { TrusteeAddress = tf.Key, IdoOwner = rfsOutput.ScriptHash, IdoAmount = output.Value, BlockIndex = block.Index, TxN = TxN };
                                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.TrustFundIDORecord).Add(key), SliceBuilder.Begin().Add(record));
                                DTFIDOSummaryKey summaryKey = new DTFIDOSummaryKey { IDOOwner = rfsOutput.ScriptHash, TrusteeAddress = tf.Key };
                                var amount = output.Value;
                                if (miningProvider.DTFIDOSummary.TryGetValue(summaryKey, out var value))
                                {
                                    amount += value;
                                }
                                miningProvider.DTFIDOSummary[summaryKey] = amount;
                                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.TrustFundIDOSummary).Add(summaryKey), SliceBuilder.Begin().Add(amount));
                            }
                        }
                    }
                }
            }
        }
        public static void Save_DTFLockAssetTransaction(this WriteBatch batch, MiningProvider provider, Block block, LockAssetTransaction lat, ushort blockN)
        {
            if (lat.IsNotNull() && lat.LockContract.Equals(Blockchain.LockAssetContractScriptHash) && lat.Recipient.Equals(invest.TrustFundWitnessPubKey) && lat.Attach.IsNotNullAndEmpty())
            {
                var targetSH = new UInt160(lat.Attach);
                if (targetSH.IsNotNull())
                {
                    var sh = lat.GetContract().ScriptHash;
                    //var txid = lat.Hash;
                    for (ushort n = 0; n < lat.Outputs.Length; n++)
                    {
                        var output = lat.Outputs[n];
                        if (output.ScriptHash.Equals(sh))
                        {
                            var key = new OutputKey { TxId = lat.Hash, N = n };
                            var LockAssetMerge = new LockAssetMerge { TargetAddress = targetSH, Tx = lat, Output = output };

                            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.DTF_LockAsset_Record).Add(key), SliceBuilder.Begin().Add(LockAssetMerge));
                            provider.DTFLockAssets[key] = LockAssetMerge;
                        }
                    }
                }
            }
        }
    }
}

using Akka.Util;
using OX;
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
using OX.Mining.AMI;
using Nethereum.Model;
using System.Runtime.CompilerServices;

namespace OX.Mining
{
    public partial class MiningProvider
    {
        public Dictionary<UInt256, Fixed8> TokenDestroy { get; set; } = new Dictionary<UInt256, Fixed8>();
        public void CheckUSDTBlackHoleDestroy(WriteBatch batch, Block block, TransactionOutput output, Transaction tx)
        {
            if (output.ScriptHash.Equals(UInt160.Zero) && output.AssetId.Equals(invest.USDT_Asset))
            {
                if (tx.EthSignatures.IsNotNullAndEmpty() && tx.EthSignatures.Count() <= 2)
                {
                    foreach (var sig in tx.EthSignatures)
                    {
                        try
                        {
                            var stringToSign = tx.InputOutputHash.ToArray().ToHexString();
                            var signer = new Nethereum.Signer.EthereumMessageSigner();
                            var ethAddr = signer.EncodeUTF8AndEcRecover(stringToSign, sig.CreateStringSignature());
                            if (ethAddr.IsNotNullAndEmpty())
                            {
                                batch.Save_AnchorMortgageDestroyRecord(this, block, tx, output, ethAddr);
                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        public void OnEthereumMapTransactionForAnchorMortgageIssue(WriteBatch batch, Block block, EthereumMapTransaction emt, ushort TxN)
        {
            if (emt.VerifyAnchorMortgageIssue(out TransactionOutput output))
            {
                batch.Save_AnchorMortgageIssueRecord(this, block, emt, output);
            }
        }
        public IEnumerable<KeyValuePair<AnchorMortgageIssueKey, TransactionOutput>> GetAnchorMortgageIssueRecords(string ethAddress = default)
        {
            var builder = SliceBuilder.Begin(InvestBizPersistencePrefixes.AMI_USDTCastRecord);
            if (ethAddress.IsNotNullAndEmpty())
                builder = builder.Add(new OX.Mining.StakingMining. StringWrapper(ethAddress.ToLower()));
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<AnchorMortgageIssueKey, TransactionOutput>(ks.AsSerializable<AnchorMortgageIssueKey>(), data.AsSerializable<TransactionOutput>());
            });
        }
        public IEnumerable<KeyValuePair<AnchorMortgageIssueKey, TransactionOutput>> GetAnchorMortgageDestroyRecords(string ethAddress = default)
        {
            var builder = SliceBuilder.Begin(InvestBizPersistencePrefixes.AMI_USDTDestroyRecord);
            if (ethAddress.IsNotNullAndEmpty())
                builder = builder.Add(new OX.Mining.StakingMining.StringWrapper(ethAddress.ToLower()));
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<AnchorMortgageIssueKey, TransactionOutput>(ks.AsSerializable<AnchorMortgageIssueKey>(), data.AsSerializable<TransactionOutput>());
            });
        }

    }
    public static partial class MiningPersistenceHelper
    {
        public static void Save_AnchorMortgageIssueRecord(this WriteBatch batch, MiningProvider provider, Block block, EthereumMapTransaction emt, TransactionOutput output)
        {
            AnchorMortgageIssueKey key = new AnchorMortgageIssueKey
            {
                EthereumAddress = emt.EthereumAddress.ToLower(),
                TxHash = emt.Hash,
                Timestamp = block.Timestamp
            };
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.AMI_USDTCastRecord).Add(key), SliceBuilder.Begin().Add(output));
        }
        public static void Save_AnchorMortgageDestroyRecord(this WriteBatch batch, MiningProvider provider, Block block, Transaction tx, TransactionOutput output, string ethAddress)
        {
            AnchorMortgageIssueKey key = new AnchorMortgageIssueKey
            {
                EthereumAddress = ethAddress.ToLower(),
                TxHash = tx.Hash,
                Timestamp = block.Timestamp
            };
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.AMI_USDTDestroyRecord).Add(key), SliceBuilder.Begin().Add(output));
        }
    }
}

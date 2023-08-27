using System.Text;
using System.Linq;
using System.Collections.Generic;
using OX;
using OX.IO;
using OX.VM;
using OX.Ledger;
using OX.IO.Data.LevelDB;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Cryptography.ECC;
using OX.Wallets;
using OX.Cryptography.AES;
using OX.Cryptography;
using OX.Mining.StakingMining;
using OX.Mining.DEX;
using OX.Mining.Trade;
using OX.Mining.OTC;
using Org.BouncyCastle.Cms;
using Nethereum.Signer.Crypto;
using System.Reflection;

namespace OX.Mining
{
    public static partial class MiningPersistenceHelper
    {
        public static void Save_InvestSettingRecord(this WriteBatch batch, BizRecordModel model, InvestSettingRecord record)
        {
            if (record != default && record.Value != default)
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.Invest_Setting).Add(model.Key), SliceBuilder.Begin().Add(record));
        }
        public static void Save_SwapTraderRedeemRequest(this WriteBatch batch, MiningProvider miningProvider, Block block, AskTransaction at, UInt160 holderSH, OTCDealerRedeemRequest request)
        {
            if (request.IsNotNull() && holderSH.IsNotNull())
            {
                SwapTraderRedeemIndex stri = new SwapTraderRedeemIndex { SwapTraderRedeemRequest = request, ExpireIndex = block.Index + 20000 };
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.OTCDealerRedeemRequest).Add(holderSH), SliceBuilder.Begin().Add(stri));
            }
        }
        public static void On_Event_Engrave(this WriteBatch batch, IMiningProvider persistence, Block block, EventTransaction model, ushort n)
        {
            if (model.IsNotNull())
            {
                try
                {
                    var engrave = model.Data.AsSerializable<Engrave>();
                    if (engrave.IsNotNull())
                    {
                        var k = $"{engrave.BoardTxIndex}-{engrave.BoardTxPosition}";
                        if (k == invest.LockMiningOfficalEventBoardId || k == invest.DEXOfficalEventBoardId)
                            OX.Bapps.Bapp.PushCrossBappMessage(new OX.Bapps.CrossBappMessage() { MessageType = 1, Attachment = k });
                    }
                }
                catch
                {

                }
            }
        }
        public static void Save_MainSwapPair(this WriteBatch batch, MiningProvider miningProvider, Block block, SideTransaction st, SwapPairReply reply)
        {
            if (reply.IsNotNull())
            {
                var sh = st.GetContract().ScriptHash;
                //var hostSH = Contract.CreateSignatureRedeemScript(reply.Host).ToScriptHash();
                var TargetAssetState = Blockchain.Singleton.Store.GetAssets().TryGet(reply.TargetAssetId);
                var merge = new SwapPairMerge { PoolAddress = sh, TxId = st.Hash, SwapPairReply = reply, TargetAssetState = TargetAssetState, Index = block.Index };
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPair).Add(sh), SliceBuilder.Begin().Add(merge));
                miningProvider.SwapPairs[sh] = merge;
            }
        }
        public static void Save_SwapPairStateReply(this WriteBatch batch, MiningProvider miningProvider, Block block, ReplyTransaction rt, SwapPairStateReply reply)
        {
            if (reply.IsNotNull())
            {
                var hostSH = reply.PoolAddress;
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPairState).Add(hostSH), SliceBuilder.Begin().Add(reply));
                miningProvider.SwapPairStates[hostSH] = reply;
            }
        }

        public static void Save_SwapPairExchange(this WriteBatch batch, MiningProvider miningProvider, Block block, ushort TxN, Transaction tx, UInt160[] bizshs)
        {
            if (tx.References.IsNotNullAndEmpty())
            {
                foreach (var reference in tx.References)
                {
                    if (miningProvider.SwapPairs.TryGetValue(reference.Value.ScriptHash, out SwapPairMerge spm))
                    {
                        if (spm.TargetAssetState.AssetId.Equals(reference.Value.AssetId) || Blockchain.OXC.Equals(reference.Value.AssetId))
                        {
                            var attr = tx.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Remark2);
                            if (attr.IsNotNull())
                            {
                                try
                                {
                                    var sop = attr.Data.AsSerializable<SwapVolume>();
                                    if (sop.IsNotNull())
                                    {
                                        SwapPairExchangeKey key = new SwapPairExchangeKey { SH = reference.Value.ScriptHash, Payee = sop.Payee, Timestamp = block.Timestamp, Index = block.Index, TxN = TxN, TxId = tx.Hash };
                                        var vom = new SwapVolumeMerge { Volume = sop, Price = (decimal)sop.PricingAssetVolume.GetInternalValue() / (decimal)sop.TargetAssetVolume.GetInternalValue() };
                                        batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPairExchange).Add(key), SliceBuilder.Begin().Add(vom));
                                        var swapvolumemerge = miningProvider.Get<SwapVolumeMerge>(InvestBizPersistencePrefixes.SwapPairLastExchange, reference.Value.ScriptHash);
                                        long sortIndex = 0;
                                        if (swapvolumemerge.IsNotNull())
                                        {
                                            sortIndex = (long)swapvolumemerge.Volume.BlockIndex * 10000 + (long)swapvolumemerge.Volume.TxN;
                                        }
                                        long newsortIndex = (long)sop.BlockIndex * 10000 + (long)sop.TxN;
                                        if (newsortIndex > sortIndex)
                                            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPairLastExchange).Add(reference.Value.ScriptHash), SliceBuilder.Begin().Add(vom));
                                        //SwapBeforeExchangeKey sbekey = new SwapBeforeExchangeKey { SH = reference.Value.ScriptHash, BlockIndex = sop.BlockIndex, TxN = sop.TxN };
                                        batch.Delete(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPairBeforeExchange).Add(key));
                                    }
                                }
                                catch { }
                            }
                            var attr3 = tx.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Remark3);
                            if (attr3.IsNotNull())
                            {
                                try
                                {
                                    var idor = attr3.Data.AsSerializable<IDORecord>();
                                    if (idor.IsNotNull())
                                    {
                                        SwapIDOKey key = new SwapIDOKey { PoolAddress = idor.PoolAddress, IDOOwner = idor.IdoOwner, TxId = tx.Hash };
                                        batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPairIDORecord).Add(key), SliceBuilder.Begin().Add(idor));
                                    }
                                }
                                catch { }
                            }

                            break;
                        }
                    }
                }
            }
            var sh = tx.Witnesses.FirstOrDefault(m => miningProvider.SwapPairs.ContainsKey(m.ScriptHash) || bizshs.Contains(m.ScriptHash));

            if (sh.IsNull())
            {
                for (ushort i = 0; i < tx.Outputs.Length; i++)
                {
                    var output = tx.Outputs[i];
                    var v = output.Value;
                    if (miningProvider.SwapPairs.TryGetValue(output.ScriptHash, out SwapPairMerge spm))
                    {
                        if (block.Index >= spm.SwapPairReply.Stamp)
                        {
                            SwapBeforeExchangeKey key = new SwapBeforeExchangeKey { SH = output.ScriptHash, BlockIndex = block.Index, TxN = TxN };
                            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPairBeforeExchange).Add(key), SliceBuilder.Begin().Add(output));
                        }
                    }
                }
            }
        }
        public static void Save_SideSwapPairExchange(this WriteBatch batch, MiningProvider miningProvider, Block block, ushort TxN, Transaction tx, UInt160[] bizshs)
        {
            if (tx.References.IsNotNullAndEmpty())
            {
                foreach (var reference in tx.References)
                {
                    if (miningProvider.Side_SwapPairs.TryGetValue(reference.Value.ScriptHash, out SideSwapPairKeyMerge spm))
                    {
                        UInt256 assetId = spm.Value.Data.AsSerializable<UInt256>();
                        if (assetId.Equals(reference.Value.AssetId) || Blockchain.OXC.Equals(reference.Value.AssetId))
                        {
                            var attr = tx.Attributes.FirstOrDefault(mbox => mbox.Usage == TransactionAttributeUsage.Remark4);
                            if (attr.IsNotNull())
                            {
                                try
                                {
                                    var sop = attr.Data.AsSerializable<SideSwapVolume>();
                                    if (sop.IsNotNull())
                                    {
                                        SwapPairExchangeKey key = new SwapPairExchangeKey { SH = reference.Value.ScriptHash, Payee = sop.Payee, Timestamp = block.Timestamp, Index = block.Index, TxN = TxN, TxId = tx.Hash };
                                        var vom = new SideSwapVolumeMerge { Volume = sop, Price = (decimal)sop.PricingAssetVolume.GetInternalValue() / (decimal)sop.TargetAssetVolume.GetInternalValue() };
                                        batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SideSwapPairExchange).Add(key), SliceBuilder.Begin().Add(vom));
                                        var swapvolumemerge = miningProvider.Get<SideSwapVolumeMerge>(InvestBizPersistencePrefixes.SideSwapPairLastExchange, reference.Value.ScriptHash);
                                        long sortIndex = 0;
                                        if (swapvolumemerge.IsNotNull())
                                        {
                                            sortIndex = (long)swapvolumemerge.Volume.BlockIndex * 10000 + (long)swapvolumemerge.Volume.TxN;
                                        }
                                        long newsortIndex = (long)sop.BlockIndex * 10000 + (long)sop.TxN;
                                        if (newsortIndex > sortIndex)
                                            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SideSwapPairLastExchange).Add(reference.Value.ScriptHash), SliceBuilder.Begin().Add(vom));
                                    }
                                }
                                catch { }
                            }
                            break;
                        }
                    }
                }
            }

        }
        public static void Save_SwapPairRequest(this WriteBatch batch, MiningProvider miningProvider, Block block, AskTransaction at, UInt160 holderSH, SwapPairAsk request)
        {
            if (request.IsNotNull() && holderSH.IsNotNull())
            {
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SwapPairRequest).Add(at.Hash), SliceBuilder.Begin().Add(request));
            }
        }
        public static void Save_LockMiningAssetReply(this WriteBatch batch, MiningProvider miningProvider, Block block, ReplyTransaction rt, LockMiningAssetReply reply)
        {
            if (reply.IsNotNull())
            {
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LockMiningAssetReply).Add(new LockMiningAssetKey { AssetId = reply.AssetId, IssueIndex = block.Index }), SliceBuilder.Begin().Add(reply));
            }
        }
        public static void Save_MutualLockMiningAssetReply(this WriteBatch batch, MiningProvider miningProvider, Block block, ReplyTransaction rt, MutualLockMiningAssetReply reply)
        {
            if (reply.IsNotNull())
            {
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MutualLockMiningAssetReply).Add(new MutualLockMiningAssetKey { AssetId = reply.AssetId, IssueIndex = block.Index }), SliceBuilder.Begin().Add(reply));
                miningProvider.MutualLockAssets[reply.AssetId] = reply;
            }
        }
        public static void Save_LevelLockMiningAssetReply(this WriteBatch batch, MiningProvider miningProvider, Block block, ReplyTransaction rt, LevelLockMiningAssetReply reply)
        {
            if (reply.IsNotNull())
            {
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockMiningAssetReply).Add(new LevelLockMiningAssetKey { AssetId = reply.AssetId, IssueIndex = block.Index }), SliceBuilder.Begin().Add(reply));
                miningProvider.LevelLockAsset = reply;
            }
        }
        public static void Save_LockMiningRecords(this WriteBatch batch, MiningProvider miningProvider, Block block, LockAssetTransaction lat, ushort k, TransactionOutput output, UInt160 recipient)
        {
            if (lat.IsNotNull())
            {
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    SelfLockKey key = new SelfLockKey { AssetId = output.AssetId, Holder = recipient, TxId = lat.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LockMiningRecords).Add(key), SliceBuilder.Begin().Add(new LockMiningRecordMerge { AssetId = output.AssetId, TransactionType = lat.Type, TxId = lat.Hash, K = k, Index = block.Index, Timestamp = block.Timestamp, LockExpiration = lat.LockExpiration, Output = output }));
                    batch.Save_TotalLockVolume(miningProvider, output.AssetId, recipient, output.Value, block.Index, lat.LockExpiration);
                }
            }
        }
        public static void Save_LockMiningRecordsForEth(this WriteBatch batch, MiningProvider miningProvider, Block block, EthereumMapTransaction emt, ushort k, TransactionOutput output)
        {
            if (emt.IsNotNull())
            {
                var recipient = emt.EthereumAddress.BuildMapAddress();
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    SelfLockKey key = new SelfLockKey { AssetId = output.AssetId, Holder = recipient, TxId = emt.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LockMiningRecords).Add(key), SliceBuilder.Begin().Add(new LockMiningRecordMerge { AssetId = output.AssetId, TransactionType = emt.Type, TxId = emt.Hash, K = k, Index = block.Index, Timestamp = block.Timestamp, LockExpiration = emt.LockExpirationIndex, Output = output }));
                    batch.Save_TotalLockVolume(miningProvider, output.AssetId, recipient, output.Value, block.Index, emt.LockExpirationIndex);
                }
            }
        }
        public static void Save_LockMiningOXSTotal(this WriteBatch batch, MiningProvider miningProvider, UInt160 Holder, Block block, LockAssetTransaction lat, ushort k, TransactionOutput output)
        {
            if (lat.IsNotNull())
            {
                var amount = miningProvider.Get<Fixed8>(InvestBizPersistencePrefixes.LockMiningOXSTotal, Holder);
                if (amount == default)
                    amount = Fixed8.Zero;
                var l = (output.Value.GetInternalValue() / Fixed8.D) * (lat.LockExpiration - block.Index);
                var amt = amount + new Fixed8(l);
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LockMiningOXSTotal).Add(Holder), SliceBuilder.Begin().Add(amt));
            }
        }
        public static void Save_LockMiningInterestRecords(this WriteBatch batch, MiningProvider miningProvider, Block block, LockAssetTransaction lat, ushort k, TransactionOutput output, UInt160 recipient)
        {
            if (lat.IsNotNull())
            {

                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    SelfLockKey key = new SelfLockKey { AssetId = output.AssetId, Holder = recipient, TxId = lat.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LockMiningInterestRecords).Add(key), SliceBuilder.Begin().Add(new LockMiningRecordMerge { AssetId = output.AssetId, TransactionType = lat.Type, TxId = lat.Hash, K = k, Index = block.Index, Timestamp = block.Timestamp, LockExpiration = lat.LockExpiration, Output = output }));
                }
            }
        }
        public static void Save_LockMiningInterestRecordsForEth(this WriteBatch batch, MiningProvider miningProvider, Block block, EthereumMapTransaction emt, ushort k, TransactionOutput output)
        {
            if (emt.IsNotNull())
            {
                var recipient = emt.EthereumAddress.BuildMapAddress();
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    SelfLockKey key = new SelfLockKey { AssetId = output.AssetId, Holder = recipient, TxId = emt.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LockMiningInterestRecords).Add(key), SliceBuilder.Begin().Add(new LockMiningRecordMerge { AssetId = output.AssetId, TransactionType = emt.Type, TxId = emt.Hash, K = k, Index = block.Index, Timestamp = block.Timestamp, LockExpiration = emt.LockExpirationIndex, Output = output }));
                }
            }
        }
        public static void Save_SideSwapPair(this WriteBatch batch, MiningProvider miningProvider, Block block, SideTransaction st, UInt256 assetId)
        {
            SideSwapPairKey key = new SideSwapPairKey { Owner = Contract.CreateSignatureRedeemScript(st.Recipient).ToScriptHash(), PoolAddress = st.GetContract().ScriptHash, AssetId = assetId, Index = block.Index };
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.SideSwapPair).Add(key), SliceBuilder.Begin().Add(st));
            miningProvider.Side_SwapPairs[key.PoolAddress] = new SideSwapPairKeyMerge { Key = key, Value = st };
        }
        public static void Save_MutualLockNodeForEth(this WriteBatch batch, MiningProvider miningProvider, Block block, Transaction tx, TransactionOutput output, MutualNode parentNode, string ethAddress)
        {
            var from = ethAddress.BuildMapAddress();
            var seedSH = from.GetMutualLockSeed();
            if (!miningProvider.MutualLockNodes.ContainsKey(seedSH))
            {
                byte nodeType = (byte)(output.ScriptHash.Equals(MutualLockHelper.GenesisSeed()) ? 1 : 0);
                var node = new MutualNode { HolderAddress = from, RegIndex = block.Index, NodeType = nodeType, RootSeedAddress = parentNode.IsNotNull() ? parentNode.RootSeedAddress : seedSH, ParentHolder = parentNode.IsNotNull() ? parentNode.HolderAddress : invest.MasterAccountAddress, IsEthMap = true };
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MutualLockNode).Add(seedSH), SliceBuilder.Begin().Add(node));
                miningProvider.MutualLockNodes[seedSH] = node;

            }
        }
        public static void Save_MutualLockNode(this WriteBatch batch, MiningProvider miningProvider, Block block, Transaction tx, TransactionOutput output, MutualNode parentNode)
        {
            var fromPubKey = tx.GetBestWitnessPublicKey();
            if (fromPubKey.IsNotNull())
            {
                var from = Contract.CreateSignatureRedeemScript(fromPubKey).ToScriptHash();
                var seedSH = from.GetMutualLockSeed();
                if (!miningProvider.MutualLockNodes.ContainsKey(seedSH))
                {
                    byte nodeType = (byte)(output.ScriptHash.Equals(MutualLockHelper.GenesisSeed()) ? 1 : 0);
                    var node = new MutualNode { HolderAddress = from, RegIndex = block.Index, NodeType = nodeType, RootSeedAddress = parentNode.IsNotNull() ? parentNode.RootSeedAddress : seedSH, ParentHolder = parentNode.IsNotNull() ? parentNode.HolderAddress : invest.MasterAccountAddress, IsEthMap = false };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MutualLockNode).Add(seedSH), SliceBuilder.Begin().Add(node));
                    miningProvider.MutualLockNodes[seedSH] = node;

                }
            }
        }
        public static void Save_MutualLockRecordForEth(this WriteBatch batch, MiningProvider miningProvider, Block block, EthereumMapTransaction emt, TransactionOutput output)
        {
            if (emt.IsNotNull())
            {
                var recipient = emt.EthereumAddress.BuildMapAddress();
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    MutualLockKey key = new MutualLockKey { Owner = recipient, ParentOwner = miner.ParentHolder, AssetId = output.AssetId, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = emt.LockExpirationIndex, Timestamp = block.Timestamp, TxId = emt.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MutualLockRecords).Add(key), SliceBuilder.Begin().Add(new MutualLockValue { TransactionType = emt.Type, TxId = emt.Hash }));
                    batch.Save_TotalLockVolume(miningProvider, output.AssetId, recipient, output.Value, block.Index, emt.LockExpirationIndex);
                }
            }
        }
        public static void Save_MutualLockRecord(this WriteBatch batch, MiningProvider miningProvider, Block block, LockAssetTransaction lat, TransactionOutput output, UInt160 recipient)
        {
            if (lat.IsNotNull())
            {
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    MutualLockKey key = new MutualLockKey { Owner = recipient, ParentOwner = miner.ParentHolder, AssetId = output.AssetId, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = lat.LockExpiration, Timestamp = block.Timestamp, TxId = lat.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MutualLockRecords).Add(key), SliceBuilder.Begin().Add(new MutualLockValue { TransactionType = lat.Type, TxId = lat.Hash }));
                    batch.Save_TotalLockVolume(miningProvider, output.AssetId, recipient, output.Value, block.Index, lat.LockExpiration);
                }
            }
        }

        public static void Save_MutualLockInterestRecordForEth(this WriteBatch batch, MiningProvider miningProvider, Block block, EthereumMapTransaction emt, TransactionOutput output)
        {
            if (emt.IsNotNull())
            {
                var recipient = emt.EthereumAddress.BuildMapAddress();
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    MutualLockInterestKey key = new MutualLockInterestKey { Owner = miner.HolderAddress, ParentOwner = miner.ParentHolder, AssetId = output.AssetId, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = emt.LockExpirationIndex, Timestamp = block.Timestamp, TxId = emt.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MutualLockInterestRecords).Add(key), SliceBuilder.Begin().Add(new MutualLockValue { TransactionType = emt.Type, TxId = emt.Hash }));
                }
            }
        }
        public static void Save_MutualLockInterestRecord(this WriteBatch batch, MiningProvider miningProvider, Block block, IssueTransaction ist, TransactionOutput output)
        {
            if (ist.IsNotNull())
            {
                if (miningProvider.MutualLockNodes.TryGetValue(output.ScriptHash.GetMutualLockSeed(), out MutualNode miner))
                {
                    MutualLockInterestKey key = new MutualLockInterestKey { Owner = miner.HolderAddress, ParentOwner = miner.ParentHolder, AssetId = output.AssetId, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = 0, Timestamp = block.Timestamp, TxId = ist.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MutualLockInterestRecords).Add(key), SliceBuilder.Begin().Add(new MutualLockValue { TransactionType = ist.Type, TxId = ist.Hash }));
                }
            }
        }
        public static void Save_LevelLockOutRecord(this WriteBatch batch, MiningProvider miningProvider, Block block, Transaction tx, TransactionOutput output, ushort k, UInt160 From, UInt160 To)
        {
            if (tx.IsNotNull())
            {
                LevelLockKey key = new LevelLockKey { Owner = From, From = From, To = To, Amount = output.Value, PledgeIndex = block.Index, Timestamp = block.Timestamp, TxId = tx.Hash };
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockOutRecords).Add(key), SliceBuilder.Begin().Add(new LevelLockTx { TxID = tx.Hash, N = k }));
            }
        }
        public static void Save_LevelLockInRecord(this WriteBatch batch, MiningProvider miningProvider, Block block, Transaction tx, TransactionOutput output, ushort k, UInt160 From, UInt160 To)
        {
            if (tx.IsNotNull())
            {
                LevelLockKey key = new LevelLockKey { Owner = To, From = From, To = To, Amount = output.Value, PledgeIndex = block.Index, Timestamp = block.Timestamp, TxId = tx.Hash };
                var kv = new LevelLockTx { N = k, TxID = tx.Hash };
                var km = new LevelLockValue { Spend = false, Amount = output.Value };
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockInRecords).Add(key), SliceBuilder.Begin().Add(kv));
                batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockInStateRecords).Add(kv), SliceBuilder.Begin().Add(km));
                miningProvider.LevelLockInTx[kv.ToKey()] = km;
            }
        }
        public static void Update_LevelLockValue_spend(this WriteBatch batch, MiningProvider miningProvider, LevelLockTx levellocktx, LevelLockValue llv)
        {
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockInStateRecords).Add(levellocktx), SliceBuilder.Begin().Add(llv));
        }
        public static void Save_LevelLockInterestRecordForEth(this WriteBatch batch, MiningProvider miningProvider, Block block, EthereumMapTransaction emt, TransactionOutput output, UInt160 recipient)
        {
            if (emt.IsNotNull())
            {
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    LevelLockInterestKey key = new LevelLockInterestKey { IsLock = true, Owner = miner.HolderAddress, ParentOwner = miner.ParentHolder, AssetId = output.AssetId, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = emt.LockExpirationIndex, Timestamp = block.Timestamp, TransactionType = emt.Type, TxId = emt.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockInterestRecords).Add(key), SliceBuilder.Begin().Add(emt.Hash));
                }
            }
        }
        public static void Save_LevelLockInterestRecord(this WriteBatch batch, MiningProvider miningProvider, Block block, LockAssetTransaction lat, TransactionOutput output, UInt160 recipient)
        {
            if (lat.IsNotNull())
            {
                if (miningProvider.MutualLockNodes.TryGetValue(recipient.GetMutualLockSeed(), out MutualNode miner))
                {
                    LevelLockInterestKey key = new LevelLockInterestKey { IsLock = true, Owner = miner.HolderAddress, ParentOwner = miner.ParentHolder, AssetId = output.AssetId, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = lat.LockExpiration, Timestamp = block.Timestamp, TransactionType = lat.Type, TxId = lat.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockInterestRecords).Add(key), SliceBuilder.Begin().Add(lat.Hash));
                }
            }
        }
        public static void Save_LevelLockInterestRecord(this WriteBatch batch, MiningProvider miningProvider, Block block, IssueTransaction ist, TransactionOutput output)
        {
            if (ist.IsNotNull())
            {
                if (miningProvider.MutualLockNodes.TryGetValue(output.ScriptHash.GetMutualLockSeed(), out MutualNode miner))
                {
                    LevelLockInterestKey key = new LevelLockInterestKey { IsLock = false, Owner = miner.HolderAddress, ParentOwner = miner.ParentHolder, AssetId = output.AssetId, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = 0, Timestamp = block.Timestamp, TransactionType = ist.Type, TxId = ist.Hash };
                    batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.LevelLockInterestRecords).Add(key), SliceBuilder.Begin().Add(ist.Hash));
                }
            }
        }
        public static void Save_TotalLockVolume(this WriteBatch batch, MiningProvider miningProvider, UInt256 assetId, UInt160 holder, Fixed8 value, uint startIndex, uint endIndex)
        {
            TotalLockVolumeKey key = new TotalLockVolumeKey { AssetId = assetId, Holder = holder };
            var lw = miningProvider.Get<LongWrapper>(InvestBizPersistencePrefixes.TotalMutualLockSpaceTimeLockVolume, key);
            if (lw.IsNull())
            {
                lw = new LongWrapper();
            }
            lw.Value += MutualLockHelper.CalculateValidSpaceTimeVolume(value, startIndex, endIndex);
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.TotalMutualLockSpaceTimeLockVolume).Add(key), SliceBuilder.Begin().Add(lw));
        }
        public static void UpdateCheckinMiningCount(this WriteBatch batch, MiningProvider provider, string ethAddress, uint markIndex)
        {
            StringWrapper sw = new StringWrapper(ethAddress);
            var lw = provider.Get<LongWrapper>(InvestBizPersistencePrefixes.MarkMiningCount, sw);
            if (lw.IsNull())
            {
                lw = new LongWrapper();
            }
            lw.Value += 1;
            batch.Put(SliceBuilder.Begin(InvestBizPersistencePrefixes.MarkMiningCount).Add(sw), SliceBuilder.Begin().Add(lw));
        }
    }
}

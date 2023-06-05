using OX.Bapps;
using OX.Cryptography.ECC;
using OX.IO;
using OX.IO.Data.LevelDB;
using OX.Ledger;
using OX.Mining.DEX;
using OX.Mining.OTC;
using OX.Mining.StakingMining;
using OX.Mining.Trade;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.VM.Types;
using OX.Wallets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OX.Mining
{
    public partial class MiningProvider : BaseBappProvider, IMiningProvider
    {
        Wallet _wallet;
        public override Wallet Wallet { get { return _wallet; } set { _wallet = value; initHashAccounts(); } }
        public UInt160[] bizshs;

        public Dictionary<UInt256, AccountPack> HashAccounts = new Dictionary<UInt256, AccountPack>();
        public Dictionary<UInt160, SwapPairMerge> SwapPairs { get; set; } = new Dictionary<UInt160, SwapPairMerge>();
        public Dictionary<UInt160, SwapPairStateReply> SwapPairStates { get; set; } = new Dictionary<UInt160, SwapPairStateReply>();
        public Dictionary<UInt160, SideSwapPairKeyMerge> Side_SwapPairs { get; set; } = new Dictionary<UInt160, SideSwapPairKeyMerge>();
        /// <summary>
        /// key is node seed address,value is node private address
        /// </summary>
        public Dictionary<UInt160, MutualNode> MutualLockNodes { get; set; } = new Dictionary<UInt160, MutualNode>();

        public Dictionary<UInt256, MutualLockMiningAssetReply> MutualLockAssets { get; set; } = new Dictionary<UInt256, MutualLockMiningAssetReply>();
        public LevelLockMiningAssetReply LevelLockAsset { get; set; } = default;
        public Dictionary<string, LevelLockValue> LevelLockInTx { get; set; } = new Dictionary<string, LevelLockValue>();
        //public UInt160 MutualLockGenesisNodeAddress { get; private set; } = MutualLockHelper.GenesisSeed();
        public MiningProvider(Bapp bapp) : base(bapp)
        {
            Db = DB.Open(Path.GetFullPath($"{WalletIndexDirectory}\\mng_{Message.Magic.ToString("X8")}"), new Options { CreateIfMissing = true });
            OTCDealers = new Dictionary<UInt160, OTC.OTCDealerMerge>(this.GetAll<UInt160, OTCDealerMerge>(InvestBizPersistencePrefixes.OTC_Dealer));
            SwapPairs = new Dictionary<UInt160, SwapPairMerge>(this.GetAll<UInt160, SwapPairMerge>(InvestBizPersistencePrefixes.SwapPair));
            Side_SwapPairs = new Dictionary<UInt160, SideSwapPairKeyMerge>(this.GetAll<SideSwapPairKey, SideTransaction>(InvestBizPersistencePrefixes.SideSwapPair).Select(m => new KeyValuePair<UInt160, SideSwapPairKeyMerge>(m.Key.PoolAddress, new SideSwapPairKeyMerge { Key = m.Key, Value = m.Value })));
            SwapPairStates = new Dictionary<UInt160, SwapPairStateReply>(this.GetAll<UInt160, SwapPairStateReply>(InvestBizPersistencePrefixes.SwapPairState));
            MutualLockNodes = new Dictionary<UInt160, MutualNode>(this.GetAll<UInt160, MutualNode>(InvestBizPersistencePrefixes.MutualLockNode));
            var mutualLockAssetRecords = this.GetAll<MutualLockMiningAssetKey, MutualLockMiningAssetReply>(InvestBizPersistencePrefixes.MutualLockMiningAssetReply);
            if (mutualLockAssetRecords.IsNotNullAndEmpty())
            {
                foreach (var g in mutualLockAssetRecords.GroupBy(m => m.Key.AssetId))
                {
                    var r = g.OrderByDescending(m => m.Key.IssueIndex).FirstOrDefault();
                    MutualLockAssets[g.Key] = r.Value;
                }
            }
            var levelLockAssetRecords = this.GetAll<LevelLockMiningAssetKey, LevelLockMiningAssetReply>(InvestBizPersistencePrefixes.LevelLockMiningAssetReply);
            var levelAssetRecord = levelLockAssetRecords.OrderByDescending(m => m.Key.IssueIndex).FirstOrDefault();
            if (!levelAssetRecord.Equals(new KeyValuePair<LevelLockMiningAssetKey, LevelLockMiningAssetReply>()))
            {
                this.LevelLockAsset = levelAssetRecord.Value;
            }
            var levelLockInStateRecords = this.GetAll<LevelLockTx, LevelLockValue>(InvestBizPersistencePrefixes.LevelLockInStateRecords);
            if (levelLockInStateRecords.IsNotNullAndEmpty())
            {
                foreach (var g in levelLockInStateRecords)
                {
                    this.LevelLockInTx[g.Key.ToKey()] = g.Value;
                }
            }
            bizshs = Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();
        }
        void initHashAccounts()
        {
            if (_wallet.IsNotNull())
            {
                this.HashAccounts.Clear();
                foreach (var act in this._wallet.GetHeldAccounts())
                {
                    var key = act.GetKey();
                    this.HashAccounts[act.ScriptHash.Hash] = new AccountPack { Key = key, Address = act.ScriptHash, PublicKey = key.PublicKey };
                }
            }
        }
        #region IBappProvider

        public override void OnBappEvent(BappEvent bappEvent)
        {

        }
        public override void OnCrossBappMessage(CrossBappMessage message)
        {

        }
        public override void OnRebuild(Wallet wallet)
        {
            WriteBatch batch = new WriteBatch();
            ReadOptions options = new ReadOptions { FillCache = false };
            using (Iterator it = Db.NewIterator(options))
            {
                for (it.SeekToFirst(); it.Valid(); it.Next())
                {
                    batch.Delete(it.Key());
                }
            }
            Db.Write(WriteOptions.Default, batch);
            Bapp.PushEvent(new BappEvent { EventItems = new BappEventItem[] { new BappEventItem() { EventType = InvestBappEventType.ReBuildIndex.Value() } } });

        }
        public override void BeforeOnBlock(Block block)
        { }
        public override void AfterOnBlock(Block block)
        { }
        public override void OnBlock(Block block)
        {
            //byte BlockEventType = 0;
            //List<BizEvent> bizEventArgs = new List<BizEvent>();
            WriteBatch batch = new WriteBatch();
            for (ushort i = 0; i < block.Transactions.Length; i++)
            {
                var tx = block.Transactions[i];
                if (this.Bapp.IsBizTransaction(tx, out BizTransaction biztx))
                {
                    ushort? n = null;
                    if (biztx is BillTransaction bt)
                    {
                        foreach (var record in bt.Records)
                        {
                            var bizModel = InvestBizRecordHelper.BuildModel(record);
                            if (bizModel.Model is InvestSettingRecord InvestSettingRecord) batch.Save_InvestSettingRecord(bizModel, InvestSettingRecord);
                        }
                    }
                    else if (biztx is ReplyTransaction rt)
                    {
                        this.OnMiningReplayTransaction(batch, block, rt);
                    }
                    else if (biztx is AskTransaction at)
                    {
                        this.OnMiningAskTransaction(batch, block, at, out n);
                    }

                    //bizEventArgs.Add(new BizEvent() { Block = block, BizTransaction = biztx, N = n, BizScriptHash = biztx.BizScriptHash });
                }
                else if (tx is LockAssetTransaction lat)
                {
                    OnLockAssetTransaction(batch, block, lat);
                }
                else if (tx is EthereumMapTransaction emt)
                {
                    OnEthereumMapTransaction(batch, block, emt);
                }
                else if (tx is IssueTransaction ist)
                {
                    OnIssueTransaction(batch, block, ist);
                }
                else if (tx is SideTransaction st)
                {
                    OnSideTransaction(batch, block, st);
                }
                else if (tx is EventTransaction eventTx)
                {
                    switch (eventTx.EventType)
                    {
                        case EventType.Board:
                            break;
                        case EventType.Engrave:
                            batch.On_Event_Engrave(this, block, eventTx, i);
                            break;
                        case EventType.Digg:
                            break;
                    }
                }
                //batch.Save_PledgeRecharge(this, block, tx);
                if (tx.Outputs.IsNotNullAndEmpty())
                {
                    for (ushort k = 0; k < tx.Outputs.Length; k++)
                    {
                        TransactionOutput output = tx.Outputs[k];
                        OnPledgeMiningTransaction(batch, this, block, tx, output, k);
                    }
                }
                if (tx.References.IsNotNullAndEmpty())
                {
                    foreach (var input in tx.Inputs)
                    {
                        var LevelLockTx = new LevelLockTx { TxID = input.PrevHash, N = input.PrevIndex };
                        if (this.LevelLockInTx.TryGetValue(LevelLockTx.ToKey(), out LevelLockValue llv))
                        {
                            llv.Spend = true;
                            batch.Update_LevelLockValue_spend(this, LevelLockTx, llv);
                        }
                    }
                }
                batch.Save_SwapPairExchange(this, block, i, tx, bizshs);
                batch.Save_SideSwapPairExchange(this, block, i, tx, bizshs);
            }
            this.Db.Write(WriteOptions.Default, batch);
        }
        public void OnPledgeMiningTransaction(WriteBatch batch, MiningProvider miningProvider, Block block, Transaction tx, TransactionOutput output, ushort k)
        {
            MutualNode parentNode = default;
            var isRootReg = output.ScriptHash.Equals(MutualLockHelper.GenesisSeed());
            var isCommonReg = this.MutualLockNodes.TryGetValue(output.ScriptHash, out parentNode);
            if (isRootReg || isCommonReg)
            {
                //level lock pledge
                if (isCommonReg && output.VerifyLevelLockRecord(this))
                {
                    if (tx.IsOnlyFromEthereumMapAddress(out string ethAddress))
                    {
                        var mapAddress = ethAddress.BuildMapAddress();
                        if (mapAddress == parentNode.ParentHolder)
                        {
                            batch.Save_LevelLockOutRecord(this, block, tx, output, k, mapAddress, parentNode.HolderAddress);
                            batch.Save_LevelLockInRecord(this, block, tx, output, k, mapAddress, parentNode.HolderAddress);
                        }
                    }
                    else
                    {
                        //var leafNode = parentNode;
                        var fromPubKeys = tx.GetPublicKeys();
                        if (fromPubKeys.IsNotNullAndEmpty())
                        {
                            foreach (var pubkey in fromPubKeys)
                            {
                                var from = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                                if (from == parentNode.ParentHolder)
                                {
                                    batch.Save_LevelLockOutRecord(this, block, tx, output, k, from, parentNode.HolderAddress);
                                    batch.Save_LevelLockInRecord(this, block, tx, output, k, from, parentNode.HolderAddress);
                                    break;
                                }
 
                            }                             
                        }
                    }
                }
                //reg miner
                if (output.VerifyMutualLockNodeRegister())
                {
                    if (tx.IsOnlyFromEthereumMapAddress(out string ethAddress))
                    {
                        batch.Save_MutualLockNodeForEth(this, block, tx, output, parentNode, ethAddress);
                    }
                    else
                        batch.Save_MutualLockNode(this, block, tx, output, parentNode);
                }
            }
        }
        public void OnEthereumMapTransaction(WriteBatch batch, Block block, EthereumMapTransaction emt)
        {
            if (emt.EthMapContract.Equals(Blockchain.EthereumMapContractScriptHash))
            {
                OnEthereumMapTransactionForSelfLock(batch, block, emt);
                OnEthereumMapTransactionForMutualLock(batch, block, emt);
                OnEthereumMapTransactionForLevelLock(batch, block, emt);
            }
        }
        public void OnLockAssetTransaction(WriteBatch batch, Block block, LockAssetTransaction lat)
        {
            if (lat.LockContract.Equals(Blockchain.LockAssetContractScriptHash))
            {
                OnLockAssetTransactionForSelfLock(batch, block, lat);
                OnLockAssetTransactionForMutualLock(batch, block, lat);
                OnLockAssetTransactionForLevelLock(batch, block, lat);
            }
        }
        public void OnIssueTransaction(WriteBatch batch, Block block, IssueTransaction ist)
        {
            foreach (var output in ist.Outputs)
            {
                if (this.MutualLockAssets.TryGetValue(output.AssetId, out MutualLockMiningAssetReply MAR))
                {
                    batch.Save_MutualLockInterestRecord(this, block, ist, output);
                }
                if (this.LevelLockAsset.IsNotNull() && this.LevelLockAsset.AssetId.Equals(output.AssetId))
                {
                    batch.Save_LevelLockInterestRecord(this, block, ist, output);
                }
            }
        }
        public void OnLockAssetTransactionForMutualLock(WriteBatch batch, Block block, LockAssetTransaction lat)
        {
            if (!lat.IsTimeLock)
            {
                if (lat.IsSelfLock(out UInt160 sh))
                {
                    var contractSH = lat.GetContract().ScriptHash;
                    var output = lat.Outputs.FirstOrDefault(m => m.ScriptHash.Equals(contractSH));
                    if (output.IsNotNull() && this.MutualLockAssets.TryGetValue(output.AssetId, out MutualLockMiningAssetReply MAR))
                    {
                        if (output.Value >= MAR.MinAmount && output.Value <= MAR.MaxAmount)
                        {
                            if (lat.LockExpiration - block.Index >= 1000000)
                                batch.Save_MutualLockRecord(this, block, lat, output, sh);
                        }
                    }
                }

            }
        }
        public void OnEthereumMapTransactionForMutualLock(WriteBatch batch, Block block, EthereumMapTransaction emt)
        {

            var contractSH = emt.GetContract().ScriptHash;
            var output = emt.Outputs.FirstOrDefault(m => m.ScriptHash.Equals(contractSH));
            if (output.IsNotNull() && this.MutualLockAssets.TryGetValue(output.AssetId, out MutualLockMiningAssetReply MAR))
            {
                if (emt.IsSelfLock())
                {
                    if (output.Value >= MAR.MinAmount && output.Value <= MAR.MaxAmount)
                    {
                        if (emt.LockExpirationIndex - block.Index >= 1000000)
                            batch.Save_MutualLockRecordForEth(this, block, emt, output);
                    }
                }
                if (emt.IsIssue)
                {
                    batch.Save_MutualLockInterestRecordForEth(this, block, emt, output);
                }
            }

        }
        public void OnEthereumMapTransactionForLevelLock(WriteBatch batch, Block block, EthereumMapTransaction emt)
        {
            var contractSH = emt.GetContract().ScriptHash;
            var output = emt.Outputs.FirstOrDefault(m => m.ScriptHash.Equals(contractSH));
            if (output.IsNotNull() && this.LevelLockAsset.IsNotNull() && this.LevelLockAsset.AssetId.Equals(output.AssetId))
            {
                var sh = emt.EthereumAddress.BuildMapAddress();
                if (emt.IsIssue)
                {
                    batch.Save_LevelLockInterestRecordForEth(this, block, emt, output, sh);
                }
            }

        }
        public void OnLockAssetTransactionForLevelLock(WriteBatch batch, Block block, LockAssetTransaction lat)
        {
            if (!lat.IsTimeLock)
            {
                var contractSH = lat.GetContract().ScriptHash;
                var output = lat.Outputs.FirstOrDefault(m => m.ScriptHash.Equals(contractSH));
                if (output.IsNotNull() && this.LevelLockAsset.IsNotNull() && this.LevelLockAsset.AssetId.Equals(output.AssetId))
                {
                    var sh = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
                    if (lat.IsIssue)
                    {
                        batch.Save_LevelLockInterestRecord(this, block, lat, output, sh);
                    }
                }

            }
        }
        public void OnEthereumMapTransactionForSelfLock(WriteBatch batch, Block block, EthereumMapTransaction emt)
        {
            var contractSH = emt.GetContract().ScriptHash;
            for (ushort k = 0; k < emt.Outputs.Length; k++)
            {
                TransactionOutput output = emt.Outputs[k];
                if (output.ScriptHash.Equals(contractSH))
                {
                    var regAssets = this.GetAll<LockMiningAssetKey, LockMiningAssetReply>(InvestBizPersistencePrefixes.LockMiningAssetReply);
                    if (regAssets.IsNotNullAndEmpty())
                    {
                        var asset = regAssets.Select(m => m.Value)?.FirstOrDefault(m => m.AssetId.Equals(output.AssetId));
                        if (asset.IsNotNull() && output.Value >= asset.MinAmount && output.Value <= asset.MaxAmount)
                            batch.Save_LockMiningRecordsForEth(this, block, emt, k, output);
                    }
                }
            }
            if (emt.IsIssue)
            {
                for (ushort k = 0; k < emt.Outputs.Length; k++)
                {
                    TransactionOutput output = emt.Outputs[k];
                    if (output.ScriptHash.Equals(contractSH))
                    {
                        var regAssets = this.GetAll<LockMiningAssetKey, LockMiningAssetReply>(InvestBizPersistencePrefixes.LockMiningAssetReply);
                        if (regAssets.IsNotNullAndEmpty() && regAssets.Select(m => m.Key.AssetId).Contains(output.AssetId))
                        {
                            batch.Save_LockMiningInterestRecordsForEth(this, block, emt, k, output);
                        }
                    }
                }
            }
        }
        public void OnLockAssetTransactionForSelfLock(WriteBatch batch, Block block, LockAssetTransaction lat)
        {
            if (!lat.IsTimeLock)
            {
                if (lat.IsSelfLock(out UInt160 sh))
                {
                    var contractSH = lat.GetContract().ScriptHash;
                    for (ushort k = 0; k < lat.Outputs.Length; k++)
                    {
                        TransactionOutput output = lat.Outputs[k];
                        if (output.ScriptHash.Equals(contractSH))
                        {
                            var regAssets = this.GetAll<LockMiningAssetKey, LockMiningAssetReply>(InvestBizPersistencePrefixes.LockMiningAssetReply);
                            if (regAssets.IsNotNullAndEmpty())
                            {
                                var asset = regAssets.Select(m => m.Value)?.FirstOrDefault(m => m.AssetId.Equals(output.AssetId));
                                if (asset.IsNotNull() && output.Value >= asset.MinAmount && output.Value <= asset.MaxAmount)
                                    batch.Save_LockMiningRecords(this, block, lat, k, output, sh);
                            }
                            if (output.AssetId.Equals(Blockchain.OXS))
                            {
                                batch.Save_LockMiningOXSTotal(this, sh, block, lat, k, output);
                            }
                        }
                    }
                }
                if (lat.IsIssue)
                {
                    var addr = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();

                    var contractSH = lat.GetContract().ScriptHash;
                    for (ushort k = 0; k < lat.Outputs.Length; k++)
                    {
                        TransactionOutput output = lat.Outputs[k];
                        if (output.ScriptHash.Equals(contractSH))
                        {
                            var regAssets = this.GetAll<LockMiningAssetKey, LockMiningAssetReply>(InvestBizPersistencePrefixes.LockMiningAssetReply);
                            if (regAssets.IsNotNullAndEmpty() && regAssets.Select(m => m.Key.AssetId).Contains(output.AssetId))
                            {
                                batch.Save_LockMiningInterestRecords(this, block, lat, k, output, addr);
                            }
                        }
                    }
                }
            }
        }
        public void OnSideTransaction(WriteBatch batch, Block block, SideTransaction st)
        {
            UInt256 AssetId = default;
            if (st.VerifyRegMainSwap(out AssetId, out SwapPairReply swapPairReply))
            {
                batch.Save_MainSwapPair(this, block, st, swapPairReply);
            }
            else if (st.VerifyRegSideSwap(out AssetId))
            {
                var settings = this.GetAllInvestSettings();
                var feesetting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { InvestSettingTypes.SidePairRegFee }));
                if (feesetting.Equals(new KeyValuePair<byte[], InvestSettingRecord>())) return;
                var fee = Fixed8.FromDecimal(decimal.Parse(feesetting.Value.Value));
                if (st.VerifyRegSideSwapFee(fee))
                {
                    batch.Save_SideSwapPair(this, block, st, AssetId);
                }
            }
            else if (st.VerifyOTCDealerTx(out string ethAddress, out OTCSetting setting))
            {
                batch.Save_OTCDealer(this, block, st, ethAddress, setting);
            }
        }
        public void OnMiningReplayTransaction(WriteBatch batch, Block block, ReplyTransaction rt)
        {
            IReadOnlyDictionary<CoinReference, TransactionOutput> rfs = rt.References;
            //var shs = rfs.Values.GroupBy(m => m.ScriptHash).Select(n => n.Key.ToAddress());

            //1.验证Tip标签的真实性,不真实则返回不做后续处理
            switch (rt.DataType)
            {
                //case (byte)InvestType.SwapInGoldReply:
                //    if (rt.GetDataModel<SwapInGoldReply>(bizshs, (byte)InvestType.SwapInGoldReply, out SwapInGoldReply SwapInGoldReply))
                //    {
                //        batch.Save_SwapInGoldReply(this, block, rt, SwapInGoldReply);
                //    }
                //    break;

                case (byte)InvestType.SwapPairStateReply:
                    if (rt.GetDataModel<SwapPairStateReply>(bizshs, (byte)InvestType.SwapPairStateReply, out SwapPairStateReply SwapPairStateReply))
                    {
                        batch.Save_SwapPairStateReply(this, block, rt, SwapPairStateReply);
                    }
                    break;
                case (byte)InvestType.LockMiningAssetReply:
                    if (rt.GetDataModel<LockMiningAssetReply>(bizshs, (byte)InvestType.LockMiningAssetReply, out LockMiningAssetReply LockMiningAssetReply))
                    {
                        batch.Save_LockMiningAssetReply(this, block, rt, LockMiningAssetReply);
                    }
                    break;
                case (byte)InvestType.MutualLockMiningAssetReply:
                    if (rt.GetDataModel<MutualLockMiningAssetReply>(bizshs, (byte)InvestType.MutualLockMiningAssetReply, out MutualLockMiningAssetReply MutualLockMiningAssetReply))
                    {
                        batch.Save_MutualLockMiningAssetReply(this, block, rt, MutualLockMiningAssetReply);
                    }
                    break;
                case (byte)InvestType.LevelLockMiningAssetReply:
                    if (rt.GetDataModel<LevelLockMiningAssetReply>(bizshs, (byte)InvestType.LevelLockMiningAssetReply, out LevelLockMiningAssetReply LevelLockMiningAssetReply))
                    {
                        batch.Save_LevelLockMiningAssetReply(this, block, rt, LevelLockMiningAssetReply);
                    }
                    break;

            }

        }
        public void OnMiningAskTransaction(WriteBatch batch, Block block, AskTransaction at, out ushort? n)
        {
            n = null;
            IReadOnlyDictionary<CoinReference, TransactionOutput> rfs = at.References;
            //var shs = rfs.Values.GroupBy(m => m.ScriptHash).Select(n => n.Key.ToAddress());
            switch (at.DataType)
            {
                case (byte)InvestType.OTCDealerRedeemRequest:
                    if (at.GetDataModel<OTCDealerRedeemRequest>(bizshs, (byte)InvestType.OTCDealerRedeemRequest, out OTCDealerRedeemRequest SwapTraderRedeemRequest))
                    {
                        var holderSH = Contract.CreateSignatureRedeemScript(at.From).ToScriptHash();
                        batch.Save_SwapTraderRedeemRequest(this, block, at, holderSH, SwapTraderRedeemRequest);
                    }
                    break;
            }

        }
        #endregion
        #region IMiningProvider

        public bool TryGetHashAccount(UInt256 PubkeyHash, out AccountPack pack)
        {
            return this.HashAccounts.TryGetValue(PubkeyHash, out pack);
        }
        public IEnumerable<KeyValuePair<byte[], InvestSettingRecord>> GetAllInvestSettings()
        {
            return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(InvestBizPersistencePrefixes.Invest_Setting), (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<byte[], InvestSettingRecord>(ks, data.AsSerializable<InvestSettingRecord>());
            });
        }
        //public IEnumerable<KeyValuePair<MiningHashKey, MinePoolPublishRecord>> GetMinePools(UInt160 holder = default)
        //{
        //    var builder = SliceBuilder.Begin(InvestBizPersistencePrefixes.MinePoolPublish);
        //    if (holder.IsNotNull())
        //        builder = builder.Add(holder);
        //    return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
        //    {
        //        var ks = k.ToArray();
        //        var length = ks.Length - sizeof(byte);
        //        ks = ks.TakeLast(length).ToArray();
        //        byte[] data = v.ToArray();
        //        return new KeyValuePair<MiningHashKey, MinePoolPublishRecord>(ks.AsSerializable<MiningHashKey>(), data.AsSerializable<MinePoolPublishRecord>());
        //    });
        //}
        //public LevelThresholdPublishRecord GetMinePoolLevelThreshold(MiningKey minePoolKey)
        //{
        //    Slice value;
        //    if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(InvestBizPersistencePrefixes.MinePoolLevelThresholdPublish).Add(minePoolKey), out value))
        //    {
        //        byte[] data = value.ToArray();
        //        return data.AsSerializable<LevelThresholdPublishRecord>();
        //    }
        //    else
        //        return default;
        //}
        //public IEnumerable<KeyValuePair<UInt160, MinerData>> GetMyMiners()
        //{
        //    return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(InvestBizPersistencePrefixes.MyMinerPublish), (k, v) =>
        //    {
        //        var ks = k.ToArray();
        //        var length = ks.Length - sizeof(byte);
        //        ks = ks.TakeLast(length).ToArray();
        //        byte[] data = v.ToArray();
        //        return new KeyValuePair<UInt160, MinerData>(ks.AsSerializable<UInt160>(), data.AsSerializable<MinerData>());
        //    });
        //}
        //public MinerData GetMinerData(UInt160 MinerHolder)
        //{
        //    Slice value;
        //    if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(InvestBizPersistencePrefixes.MyMinerPublish).Add(MinerHolder), out value))
        //    {
        //        byte[] data = value.ToArray();
        //        return data.AsSerializable<MinerData>();
        //    }
        //    else
        //        return default;
        //}
        //public IEnumerable<KeyValuePair<MiningHolderKey, MinerParentPrivatePublish>> GetMyLeafMiners()
        //{
        //    return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(InvestBizPersistencePrefixes.MinerParentPublish), (k, v) =>
        //    {
        //        var ks = k.ToArray();
        //        var length = ks.Length - sizeof(byte);
        //        ks = ks.TakeLast(length).ToArray();
        //        byte[] data = v.ToArray();
        //        return new KeyValuePair<MiningHolderKey, MinerParentPrivatePublish>(ks.AsSerializable<MiningHolderKey>(), data.AsSerializable<MinerParentPrivatePublish>());
        //    });
        //}
        #endregion

    }
}

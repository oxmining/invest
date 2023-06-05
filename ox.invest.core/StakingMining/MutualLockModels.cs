using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P.Payloads;
using System.IO;
using OX.VM;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.StakingMining
{
    public class MutualLockSnapshotData
    {
        public MutualLockMiningAssetReply MutualLockMiningAssetReply;
        public MutualNode Node;
        public IEnumerable<MutualLockKey> Self;
        public IEnumerable<MutualLockKey> Leaf;
    }
    public class MutualLockValue : ISerializable
    {
        public TransactionType TransactionType;
        public UInt256 TxId;
        public virtual int Size => sizeof(TransactionType) + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)TransactionType);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            TransactionType = (TransactionType)reader.ReadByte();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class MutualLockKey : ISerializable
    {
        public UInt256 AssetId;
        public UInt160 ParentOwner;
        public UInt160 Owner;
        public uint StartIndex;
        public uint EndIndex;
        public UInt160 LockAddress;
        public Fixed8 Amount;
        public uint Timestamp;
        public UInt256 TxId;
        public virtual int Size => AssetId.Size + ParentOwner.Size + Owner.Size + sizeof(uint) + sizeof(uint) + LockAddress.Size + Amount.Size + sizeof(uint) + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(ParentOwner);
            writer.Write(Owner);
            writer.Write(StartIndex);
            writer.Write(EndIndex);
            writer.Write(LockAddress);
            writer.Write(Amount);
            writer.Write(Timestamp);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            ParentOwner = reader.ReadSerializable<UInt160>();
            Owner = reader.ReadSerializable<UInt160>();
            StartIndex = reader.ReadUInt32();
            EndIndex = reader.ReadUInt32();
            LockAddress = reader.ReadSerializable<UInt160>();
            Amount = reader.ReadSerializable<Fixed8>();
            Timestamp = reader.ReadUInt32();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class MutualLockInterestKey : ISerializable
    {
        public UInt256 AssetId;
        public UInt160 ParentOwner;
        public UInt160 Owner;
        public uint StartIndex;
        public uint EndIndex;
        public UInt160 LockAddress;
        public Fixed8 Amount;
        public uint Timestamp;
        public UInt256 TxId;
        public virtual int Size => AssetId.Size + ParentOwner.Size + Owner.Size + sizeof(uint) + sizeof(uint) + LockAddress.Size + Amount.Size + sizeof(uint) + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(ParentOwner);
            writer.Write(Owner);
            writer.Write(StartIndex);
            writer.Write(EndIndex);
            writer.Write(LockAddress);
            writer.Write(Amount);
            writer.Write(Timestamp);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            ParentOwner = reader.ReadSerializable<UInt160>();
            Owner = reader.ReadSerializable<UInt160>();
            StartIndex = reader.ReadUInt32();
            EndIndex = reader.ReadUInt32();
            LockAddress = reader.ReadSerializable<UInt160>();
            Amount = reader.ReadSerializable<Fixed8>();
            Timestamp = reader.ReadUInt32();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
}

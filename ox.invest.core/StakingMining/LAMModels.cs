using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P.Payloads;
using System.IO;
using OX.VM;
using System.Security.Policy;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.StakingMining
{
    public class LongWrapper : ISerializable
    {
        public ulong Value { get; set; }
        public virtual int Size => sizeof(ulong);
        public LongWrapper() : this(0) { }
        public LongWrapper(ulong value) { this.Value = value; }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            Value = reader.ReadUInt64();
        }
    }
    public class StringWrapper : ISerializable
    {
        public string Text { get; private set; }
        public virtual int Size => Text.GetVarSize();
        public StringWrapper() { }
        public StringWrapper(string text) { this.Text = text; }
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarString(Text);
        }
        public void Deserialize(BinaryReader reader)
        {
            Text = reader.ReadVarString();
        }
    }
    public class LockMiningRecordMerge : ISerializable
    {
        public UInt256 AssetId;
        public TransactionType TransactionType;
        public UInt256 TxId;
        public ushort K;
        public uint Index;
        public uint Timestamp;
        public uint LockExpiration;
        public TransactionOutput Output;
        public virtual int Size => AssetId.Size + sizeof(TransactionType) + TxId.Size + sizeof(ushort) + sizeof(uint) + sizeof(uint) + sizeof(uint) + Output.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write((byte)TransactionType);
            writer.Write(TxId);
            writer.Write(K);
            writer.Write(Index);
            writer.Write(Timestamp);
            writer.Write(LockExpiration);
            writer.Write(Output);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            TransactionType = (TransactionType)reader.ReadByte();
            TxId = reader.ReadSerializable<UInt256>();
            K = reader.ReadUInt16();
            Index = reader.ReadUInt32();
            Timestamp = reader.ReadUInt32();
            LockExpiration = reader.ReadUInt32();
            Output = reader.ReadSerializable<TransactionOutput>();
        }
    }

    public class SelfLockKey : ISerializable
    {
        public UInt256 AssetId;
        public UInt160 Holder;
        public UInt256 TxId;
        public virtual int Size => AssetId.Size + Holder.Size + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Holder);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            Holder = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class LockMiningAssetKey : ISerializable
    {
        public UInt256 AssetId;
        public uint IssueIndex;
        public virtual int Size => AssetId.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(IssueIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            IssueIndex = reader.ReadUInt32();
        }
    }
    public class MutualLockMiningAssetKey : ISerializable
    {
        public UInt256 AssetId;
        public uint IssueIndex;
        public virtual int Size => AssetId.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(IssueIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            IssueIndex = reader.ReadUInt32();
        }
    }
    public class LevelLockMiningAssetKey : ISerializable
    {
        public UInt256 AssetId;
        public uint IssueIndex;
        public virtual int Size => AssetId.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(IssueIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            IssueIndex = reader.ReadUInt32();
        }
    }
    public class MutualNode : ISerializable
    {
        public UInt160 HolderAddress;
        public uint RegIndex;
        /// <summary>
        /// Node Type 0:common node,1:root node ,2:genesis node
        /// </summary>
        public byte NodeType;
        public UInt160 RootSeedAddress;
        public UInt160 ParentHolder;
        public bool IsEthMap;
        public virtual int Size => HolderAddress.Size + sizeof(uint) + sizeof(byte) + RootSeedAddress.Size + ParentHolder.Size + sizeof(bool);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(HolderAddress);
            writer.Write(RegIndex);
            writer.Write(NodeType);
            writer.Write(RootSeedAddress);
            writer.Write(ParentHolder);
            writer.Write(IsEthMap);
        }
        public void Deserialize(BinaryReader reader)
        {
            HolderAddress = reader.ReadSerializable<UInt160>();
            RegIndex = reader.ReadUInt32();
            NodeType = reader.ReadByte();
            RootSeedAddress = reader.ReadSerializable<UInt160>();
            ParentHolder = reader.ReadSerializable<UInt160>();
            IsEthMap = reader.ReadBoolean();
        }
    }
    public class TotalLockVolumeKey : ISerializable
    {
        public UInt256 AssetId;
        public UInt160 Holder;

        public virtual int Size => AssetId.Size + Holder.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Holder);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            Holder = reader.ReadSerializable<UInt160>();
        }
    }
}

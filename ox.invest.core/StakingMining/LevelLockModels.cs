using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P.Payloads;
using System.IO;
using OX.VM;
using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.StakingMining
{
    public class LevelLockKey : ISerializable
    {
        public UInt160 Owner;
        public UInt160 From;
        public UInt160 To;
        public uint PledgeIndex;
        public Fixed8 Amount;
        public uint Timestamp;
        public UInt256 TxId;
        public virtual int Size => Owner.Size + From.Size + To.Size + sizeof(uint) + Amount.Size + sizeof(uint) + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Owner);
            writer.Write(From);
            writer.Write(To);
            writer.Write(PledgeIndex);
            writer.Write(Amount);
            writer.Write(Timestamp);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            Owner = reader.ReadSerializable<UInt160>();
            From = reader.ReadSerializable<UInt160>();
            To = reader.ReadSerializable<UInt160>();
            PledgeIndex = reader.ReadUInt32();
            Amount = reader.ReadSerializable<Fixed8>();
            Timestamp = reader.ReadUInt32();
            TxId= reader.ReadSerializable<UInt256>();
        }
    }
    public class LevelLockTx : ISerializable
    {
        public UInt256 TxID;
        public ushort N;

        public virtual int Size => TxID.Size + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TxID);
            writer.Write(N);

        }
        public void Deserialize(BinaryReader reader)
        {
            TxID = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
        }
        public string ToKey()
        {
            return ToKey(TxID, N);
        }
        public static string ToKey(UInt256 txid, ushort n)
        {
            return $"{txid.ToString()}-{n}";
        }
        public override bool Equals(object obj)
        {
            if (obj is LevelLockTx llt)
            {
                return TxID.Equals(llt.TxID) && N == llt.N;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return TxID.GetHashCode() + N.GetHashCode();
        }
    }
    public class LevelLockValue : ISerializable
    {
        public Fixed8 Amount;
        public bool Spend;

        public virtual int Size => Amount.Size + sizeof(bool);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Amount);
            writer.Write(Spend);

        }
        public void Deserialize(BinaryReader reader)
        {
            Amount = reader.ReadSerializable<Fixed8>();
            Spend = reader.ReadBoolean();
        }
    }
    public class LevelLockInterestKey : ISerializable
    {
        public UInt256 AssetId;
        public UInt160 ParentOwner;
        public UInt160 Owner;
        public uint StartIndex;
        public uint EndIndex;
        public bool IsLock;
        public UInt160 LockAddress;
        public Fixed8 Amount;
        public uint Timestamp;
        public TransactionType TransactionType;
        public UInt256 TxId;
        public virtual int Size => AssetId.Size + ParentOwner.Size + Owner.Size + sizeof(uint) + sizeof(uint) + sizeof(bool) + LockAddress.Size + Amount.Size + sizeof(uint)+sizeof(TransactionType) +TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(ParentOwner);
            writer.Write(Owner);
            writer.Write(StartIndex);
            writer.Write(EndIndex);
            writer.Write(IsLock);
            writer.Write(LockAddress);
            writer.Write(Amount);
            writer.Write(Timestamp);
            writer.Write((byte)TransactionType);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            ParentOwner = reader.ReadSerializable<UInt160>();
            Owner = reader.ReadSerializable<UInt160>();
            StartIndex = reader.ReadUInt32();
            EndIndex = reader.ReadUInt32();
            IsLock = reader.ReadBoolean();
            LockAddress = reader.ReadSerializable<UInt160>();
            Amount = reader.ReadSerializable<Fixed8>();
            Timestamp = reader.ReadUInt32();
            TransactionType =(TransactionType) reader.ReadByte();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
}

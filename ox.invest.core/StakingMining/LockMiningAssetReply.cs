using System;
using System.IO;
using OX.IO;
using OX.Cryptography.ECC;

namespace OX.Mining.StakingMining
{
    [EnumStrings(typeof(LockMiningTerm), "100000区块", "500000区块", "1000000区块", "2000000区块", "4000000区块", "6000000区块", EnumName = "锁仓期限")]
    [EnumEngStrings(typeof(LockMiningTerm), "100000Blocks", "500000Blocks", "1000000Blocks", "2000000Blocks", "4000000Blocks", "6000000Blocks", EnumName = "Lock Term")]
    [Flags]
    public enum LockMiningTerm : byte
    {
        Block_100000 = 1 << 0,
        Block_500000 = 1 << 1,
        Block_1000000 = 1 << 2,
        Block_2000000 = 1 << 3,
        Block_4000000 = 1 << 4,
        Block_6000000 = 1 << 5
    }
    public class LockMiningAssetReply : ISerializable
    {
        public UInt256 AssetId;
        public Fixed8 MinAmount;
        public Fixed8 MaxAmount;
        public uint Interest_100000;
        public uint Interest_500000;
        public uint Interest_1000000;
        public uint Interest_2000000;
        public uint Interest_4000000;
        public uint Interest_6000000;
        public byte[] Mark;
        public virtual int Size => AssetId.Size + MinAmount.Size + MaxAmount.Size + sizeof(uint) * 6 + Mark.GetVarSize();
        public LockMiningAssetReply()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(MinAmount);
            writer.Write(MaxAmount);
            writer.Write(Interest_100000);
            writer.Write(Interest_500000);
            writer.Write(Interest_1000000);
            writer.Write(Interest_2000000);
            writer.Write(Interest_4000000);
            writer.Write(Interest_6000000);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            MinAmount = reader.ReadSerializable<Fixed8>();
            MaxAmount = reader.ReadSerializable<Fixed8>();
            Interest_100000 = reader.ReadUInt32();
            Interest_500000 = reader.ReadUInt32();
            Interest_1000000 = reader.ReadUInt32();
            Interest_2000000 = reader.ReadUInt32();
            Interest_4000000 = reader.ReadUInt32();
            Interest_6000000 = reader.ReadUInt32();
            Mark = reader.ReadVarBytes();
        }
    }
    public class MutualLockMiningAssetReply : ISerializable
    {
        public UInt256 AssetId;
        public Fixed8 MinAmount;
        public Fixed8 MaxAmount;
        public uint AirdropAmount;

        public byte[] Mark;
        public virtual int Size => AssetId.Size + MinAmount.Size + MaxAmount.Size + sizeof(uint) + Mark.GetVarSize();
        public MutualLockMiningAssetReply()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(MinAmount);
            writer.Write(MaxAmount);
            writer.Write(AirdropAmount);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            MinAmount = reader.ReadSerializable<Fixed8>();
            MaxAmount = reader.ReadSerializable<Fixed8>();
            AirdropAmount = reader.ReadUInt32();
            Mark = reader.ReadVarBytes();
        }
    }
    public class LevelLockMiningAssetReply : ISerializable
    {
        public UInt256 AssetId;
        public Fixed8 MinAmount;
        public Fixed8 StepAmount;
        public uint AirdropAmount;

        public byte[] Mark;
        public virtual int Size => AssetId.Size + MinAmount.Size + StepAmount.Size + sizeof(uint) + Mark.GetVarSize();
        public LevelLockMiningAssetReply()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(MinAmount);
            writer.Write(StepAmount);
            writer.Write(AirdropAmount);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            MinAmount = reader.ReadSerializable<Fixed8>();
            StepAmount = reader.ReadSerializable<Fixed8>();
            AirdropAmount = reader.ReadUInt32();
            Mark = reader.ReadVarBytes();
        }
    }
}

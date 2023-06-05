using System;
using System.IO;
using OX.IO;
using OX.Cryptography.ECC;
using OX.SmartContract;
using OX.Wallets;

namespace OX.Mining.DEX
{
    [Flags]
    public enum DividentSlope : byte
    {
        Big_5 = 1 << 0,
        Medium_6 = 1 << 1,
        Small_8 = 1 << 2
    }
    public static class DividentSlopeHelper
    {
        public static decimal ComputeBonusRatio(this DividentSlope slope, Fixed8 PoolTotal, Fixed8 frontAmount, Fixed8 selfAmount)
        {
            return slope.ComputeBonusRatio(PoolTotal.GetInternalValue(), frontAmount.GetInternalValue(), selfAmount.GetInternalValue());
        }
        public static decimal ComputeBonusRatio(this DividentSlope slope, long PoolTotal, long frontAmount, long selfAmount)
        {
            decimal r = 0;
            var x1 = frontAmount / (decimal)PoolTotal;
            var x2 = selfAmount / (decimal)PoolTotal;
            switch (slope)
            {
                case DividentSlope.Big_5:
                    r = (x2 * 4 - x2 * x2 - 2 * x2 * x1) / 3;
                    break;
                case DividentSlope.Medium_6:
                    r = (x2 * 5 - x2 * x2 - 2 * x2 * x1) / 4;
                    break;
                case DividentSlope.Small_8:
                    r = (x2 * 10 - x2 * x2 - 2 * x2 * x1) / 9;
                    break;
            }
            return r;
        }
    }
    public class SwapPairStateReply : ISerializable
    {
        public UInt160 PoolAddress;
        public byte Flag;
        public byte[] Mark;
        public virtual int Size => PoolAddress.Size + sizeof(byte) + Mark.GetVarSize();
        public SwapPairStateReply()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PoolAddress);
            writer.Write(Flag);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            PoolAddress = reader.ReadSerializable<UInt160>();
            Flag = reader.ReadByte();
            Mark = reader.ReadVarBytes();
        }
    }
    public class SwapPairAsk : ISerializable
    {
        public ECPoint Applicant;
        public UInt256 TargetAssetId;
        public UInt256 PricingAssetId;
        public byte LockPercent;
        public uint LockExpire;
        public byte Kind;
        public byte Flag;
        public uint Stamp;
        public byte[] Mark;
        public virtual int Size => Applicant.Size + TargetAssetId.Size + PricingAssetId.Size + sizeof(byte) + sizeof(uint) + sizeof(byte) + sizeof(byte) + sizeof(uint) + Mark.GetVarSize();
        public SwapPairAsk()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Applicant);
            writer.Write(TargetAssetId);
            writer.Write(PricingAssetId);
            writer.Write(LockPercent);
            writer.Write(LockExpire);
            writer.Write(Kind);
            writer.Write(Flag);
            writer.Write(Stamp);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            Applicant = reader.ReadSerializable<ECPoint>();
            TargetAssetId = reader.ReadSerializable<UInt256>();
            PricingAssetId = reader.ReadSerializable<UInt256>();
            LockPercent = reader.ReadByte();
            LockExpire = reader.ReadUInt32();
            Kind = reader.ReadByte();
            Flag = reader.ReadByte();
            Stamp = reader.ReadUInt32();
            Mark = reader.ReadVarBytes();
        }
    }

    public class SwapPairReply : ISerializable
    {
        public UInt256 TargetAssetId;
        public byte LockPercent;
        public uint LockExpire;
        public byte Kind;
        public byte Flag;
        public uint Stamp;
        public byte[] Mark;
        public virtual int Size => TargetAssetId.Size + sizeof(byte) + sizeof(uint) + sizeof(byte) + sizeof(byte) + sizeof(uint) + Mark.GetVarSize();
        public SwapPairReply()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TargetAssetId);
            writer.Write(LockPercent);
            writer.Write(LockExpire);
            writer.Write(Kind);
            writer.Write(Flag);
            writer.Write(Stamp);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            TargetAssetId = reader.ReadSerializable<UInt256>();
            LockPercent = reader.ReadByte();
            LockExpire = reader.ReadUInt32();
            Kind = reader.ReadByte();
            Flag = reader.ReadByte();
            Stamp = reader.ReadUInt32();
            Mark = reader.ReadVarBytes();
        }
    }
    public class SwapPairIDO : ISerializable
    {
        public Fixed8 Price;
        public Fixed8 MinLiquidity;
        public uint IDOLockExpire;
        public byte CommissionPercent;
        public DividentSlope DividentSlope;
        public byte[] Mark;

        public virtual int Size => Price.Size + MinLiquidity.Size + sizeof(uint) + sizeof(byte) + sizeof(DividentSlope) + Mark.GetVarSize();
        public SwapPairIDO()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Price);
            writer.Write(MinLiquidity);
            writer.Write(IDOLockExpire);
            writer.Write(CommissionPercent);
            writer.Write((byte)DividentSlope);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            Price = reader.ReadSerializable<Fixed8>();
            MinLiquidity = reader.ReadSerializable<Fixed8>();
            IDOLockExpire = reader.ReadUInt32();
            CommissionPercent = reader.ReadByte();
            DividentSlope = (DividentSlope)reader.ReadByte();
            Mark = reader.ReadVarBytes();
        }
    }
}

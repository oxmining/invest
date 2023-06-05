using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Mining.Trade;
using OX.Mining.OTC;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.DEX
{
    public class SwapVolume : ISerializable
    {
        public UInt160 Payee;
        public Fixed8 TargetAssetVolume;
        public Fixed8 PricingAssetVolume;
        public Fixed8 TargetBalance;
        public Fixed8 PricingBalance;
        public uint BlockIndex;
        public ushort TxN;
        public bool IsBuy;
        public byte[] Mark;
        public virtual int Size => Payee.Size + TargetAssetVolume.Size + PricingAssetVolume.Size + TargetBalance.Size + PricingBalance.Size + sizeof(uint) + sizeof(ushort) + sizeof(bool) + Mark.GetVarSize();
        public SwapVolume()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Payee);
            writer.Write(TargetAssetVolume);
            writer.Write(PricingAssetVolume);
            writer.Write(TargetBalance);
            writer.Write(PricingBalance);
            writer.Write(BlockIndex);
            writer.Write(TxN);
            writer.Write(IsBuy);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            Payee = reader.ReadSerializable<UInt160>();
            TargetAssetVolume = reader.ReadSerializable<Fixed8>();
            PricingAssetVolume = reader.ReadSerializable<Fixed8>();
            TargetBalance = reader.ReadSerializable<Fixed8>();
            PricingBalance = reader.ReadSerializable<Fixed8>();
            BlockIndex = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
            IsBuy = reader.ReadBoolean();
            Mark = reader.ReadVarBytes();
        }
    }
    public class SideSwapVolume : ISerializable
    {
        public UInt160 Payee;
        public Fixed8 TargetAssetVolume;
        public Fixed8 PricingAssetVolume;
        public Fixed8 TargetBalance;
        public Fixed8 PricingBalance;
        public uint BlockIndex;
        public ushort TxN;
        public bool IsBuy;
        public virtual int Size => Payee.Size + TargetAssetVolume.Size + PricingAssetVolume.Size + TargetBalance.Size + PricingBalance.Size + sizeof(uint) + sizeof(ushort) + sizeof(bool);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Payee);
            writer.Write(TargetAssetVolume);
            writer.Write(PricingAssetVolume);
            writer.Write(TargetBalance);
            writer.Write(PricingBalance);
            writer.Write(BlockIndex);
            writer.Write(TxN);
            writer.Write(IsBuy);
        }
        public void Deserialize(BinaryReader reader)
        {
            Payee = reader.ReadSerializable<UInt160>();
            TargetAssetVolume = reader.ReadSerializable<Fixed8>();
            PricingAssetVolume = reader.ReadSerializable<Fixed8>();
            TargetBalance = reader.ReadSerializable<Fixed8>();
            PricingBalance = reader.ReadSerializable<Fixed8>();
            BlockIndex = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
            IsBuy = reader.ReadBoolean();
        }
    }

    public class IDORecord : ISerializable
    {
        public UInt160 PoolAddress;
        public UInt160 IdoOwner;
        public Fixed8 IdoAmount;
        public UInt256 AssetId;
        public uint BlockIndex;
        public ushort TxN;
        public virtual int Size => PoolAddress.Size + IdoOwner.Size + IdoAmount.Size + AssetId.Size + sizeof(uint) + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PoolAddress);
            writer.Write(IdoOwner);
            writer.Write(IdoAmount);
            writer.Write(AssetId);
            writer.Write(BlockIndex);
            writer.Write(TxN);
        }
        public void Deserialize(BinaryReader reader)
        {
            PoolAddress = reader.ReadSerializable<UInt160>();
            IdoOwner = reader.ReadSerializable<UInt160>();
            IdoAmount = reader.ReadSerializable<Fixed8>();
            AssetId = reader.ReadSerializable<UInt256>();
            BlockIndex = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
        }
    }
    public class SwapIDOKey : ISerializable
    {
        public UInt160 PoolAddress;
        public UInt160 IDOOwner;
        public UInt256 TxId;
        public virtual int Size => PoolAddress.Size + IDOOwner.Size + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PoolAddress);
            writer.Write(IDOOwner);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            PoolAddress = reader.ReadSerializable<UInt160>();
            IDOOwner = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class SwapTxKey : ISerializable
    {
        public UInt160 SH;
        public UInt256 TxId;
        public virtual int Size => SH.Size + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SH);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            SH = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class SwapTxTxKey : ISerializable
    {
        public UInt160 SH;
        public UInt256 PreTxId;
        public UInt256 TxId;
        public virtual int Size => SH.Size + PreTxId.Size + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SH);
            writer.Write(PreTxId);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            SH = reader.ReadSerializable<UInt160>();
            PreTxId = reader.ReadSerializable<UInt256>();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class SwapAssetAmount : ISerializable
    {
        public UInt256 Asset;
        public Fixed8 Amount;
        public virtual int Size => Asset.Size + Amount.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Asset);
            writer.Write(Amount);
        }
        public void Deserialize(BinaryReader reader)
        {
            Asset = reader.ReadSerializable<UInt256>();
            Amount = reader.ReadSerializable<Fixed8>();
        }
    }


    public class SwapTraderRedeemIndex : ISerializable
    {
        public OTCDealerRedeemRequest SwapTraderRedeemRequest;
        public uint ExpireIndex;
        public virtual int Size => SwapTraderRedeemRequest.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SwapTraderRedeemRequest);
            writer.Write(ExpireIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            SwapTraderRedeemRequest = reader.ReadSerializable<OTCDealerRedeemRequest>();
            ExpireIndex = reader.ReadUInt32();
        }
    }
    public class SwapPairMerge : ISerializable
    {
        public UInt160 PoolAddress;
        public SwapPairReply SwapPairReply;
        public UInt256 TxId;
        public AssetState TargetAssetState;
        public uint Index;
        public virtual int Size => PoolAddress.Size + SwapPairReply.Size + TxId.Size + TargetAssetState.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PoolAddress);
            writer.Write(SwapPairReply);
            writer.Write(TxId);
            writer.Write(TargetAssetState);
            writer.Write(Index);
        }
        public void Deserialize(BinaryReader reader)
        {
            PoolAddress = reader.ReadSerializable<UInt160>();
            SwapPairReply = reader.ReadSerializable<SwapPairReply>();
            TxId = reader.ReadSerializable<UInt256>();
            TargetAssetState = reader.ReadSerializable<AssetState>();
            Index = reader.ReadUInt32();
        }
    }
    public class SwapPairPriceKey : ISerializable
    {
        public UInt160 SH;
        public uint Timestamp;
        public uint Index;
        public ushort TxN;
        public UInt256 TxId;
        public ushort N;
        public virtual int Size => SH.Size + sizeof(uint) + sizeof(uint) + sizeof(ushort) + TxId.Size + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SH);
            writer.Write(Timestamp);
            writer.Write(Index);
            writer.Write(TxN);
            writer.Write(TxId);
            writer.Write(N);
        }
        public void Deserialize(BinaryReader reader)
        {
            SH = reader.ReadSerializable<UInt160>();
            Timestamp = reader.ReadUInt32();
            Index = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
        }
    }
    public class SwapPairRechargeMerge : ISerializable
    {
        public TransactionOutput Output;
        public Fixed8 TargetBalance;
        public Fixed8 PriceBalance;
        public Fixed8 TargetAmount;
        public Fixed8 PriceAmount;
        public decimal Price;
        public virtual int Size => Output.Size + TargetBalance.Size + PriceBalance.Size + TargetAmount.Size + PriceAmount.Size + sizeof(decimal);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Output);
            writer.Write(TargetBalance);
            writer.Write(PriceBalance);
            writer.Write(TargetAmount);
            writer.Write(PriceAmount);
            writer.Write(Price);
        }
        public void Deserialize(BinaryReader reader)
        {
            Output = reader.ReadSerializable<TransactionOutput>();
            TargetBalance = reader.ReadSerializable<Fixed8>();
            PriceBalance = reader.ReadSerializable<Fixed8>();
            TargetAmount = reader.ReadSerializable<Fixed8>();
            PriceAmount = reader.ReadSerializable<Fixed8>();
            Price = reader.ReadDecimal();
        }
    }
    public class SwapPairExchangeKey : ISerializable
    {
        public UInt160 SH;
        public UInt160 Payee;
        public uint Timestamp;
        public uint Index;
        public ushort TxN;
        public UInt256 TxId;
        public virtual int Size => SH.Size + Payee.Size + sizeof(uint) + sizeof(uint) + sizeof(ushort) + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SH);
            writer.Write(Payee);
            writer.Write(Timestamp);
            writer.Write(Index);
            writer.Write(TxN);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            SH = reader.ReadSerializable<UInt160>();
            Payee = reader.ReadSerializable<UInt160>();
            Timestamp = reader.ReadUInt32();
            Index = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class SwapVolumeMerge : ISerializable
    {
        public SwapVolume Volume;
        public decimal Price;
        public virtual int Size => Volume.Size + sizeof(decimal);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Volume);
            writer.Write(Price);
        }
        public void Deserialize(BinaryReader reader)
        {
            Volume = reader.ReadSerializable<SwapVolume>();
            Price = reader.ReadDecimal();
        }
    }
    public class SideSwapVolumeMerge : ISerializable
    {
        public SideSwapVolume Volume;
        public decimal Price;
        public virtual int Size => Volume.Size + sizeof(decimal);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Volume);
            writer.Write(Price);
        }
        public void Deserialize(BinaryReader reader)
        {
            Volume = reader.ReadSerializable<SideSwapVolume>();
            Price = reader.ReadDecimal();
        }
    }
    public class SwapBeforeExchangeKey : ISerializable
    {
        public UInt160 SH;
        public uint BlockIndex;
        public ushort TxN;
        public virtual int Size => SH.Size + sizeof(uint) + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SH);
            writer.Write(BlockIndex);
            writer.Write(TxN);
        }
        public void Deserialize(BinaryReader reader)
        {
            SH = reader.ReadSerializable<UInt160>();
            BlockIndex = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
        }
    }
    public class SideSwapPairKey : ISerializable
    {
        public UInt160 Owner;
        public UInt160 PoolAddress;
        public UInt256 AssetId;
        public uint Index;
        public virtual int Size => Owner.Size + PoolAddress.Size + AssetId.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Owner);
            writer.Write(PoolAddress);
            writer.Write(AssetId);
            writer.Write(Index);
        }
        public void Deserialize(BinaryReader reader)
        {
            Owner = reader.ReadSerializable<UInt160>();
            PoolAddress = reader.ReadSerializable<UInt160>();
            AssetId = reader.ReadSerializable<UInt256>();
            Index = reader.ReadUInt32();
        }
    }
    public class SideSwapPairKeyMerge : ISerializable
    {
        public SideSwapPairKey Key;
        public SideTransaction Value;
        public virtual int Size => Key.Size + Value.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            Key = reader.ReadSerializable<SideSwapPairKey>();
            Value = reader.ReadSerializable<SideTransaction>();
        }
    }
}

using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Mining.OTC;
using System.Security.Principal;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.DTF
{
    public class TrustFundModel : ISerializable
    {
        public ECPoint Trustee;
        public UInt160 TrustAddress;
        public UInt160 DividendAddress;
        public UInt160 SubscribeAddress;
        public UInt160 ArbitrateAddress;
        public AssetTrustTransaction AssetTrustTransaction;
        public Fixed8 TotalDividendAmount;
        public Fixed8 TotalSubscribeAmount;
        public uint Index;
        public ushort TxN;
        public virtual int Size => Trustee.Size
                                           + TrustAddress.Size
                                           + DividendAddress.Size
                                           + SubscribeAddress.Size
                                           + ArbitrateAddress.Size
                                           + AssetTrustTransaction.Size
                                           + TotalDividendAmount.Size
                                           + TotalSubscribeAmount.Size
                                           + sizeof(uint)
                                           + sizeof(ushort);
        public TrustFundModel()
        {
            this.TotalDividendAmount = Fixed8.Zero;
            this.TotalSubscribeAmount = Fixed8.Zero;
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Trustee);
            writer.Write(TrustAddress);
            writer.Write(DividendAddress);
            writer.Write(SubscribeAddress);
            writer.Write(ArbitrateAddress);
            writer.Write(AssetTrustTransaction);
            writer.Write(TotalDividendAmount);
            writer.Write(TotalSubscribeAmount);
            writer.Write(Index);
            writer.Write(TxN);
        }
        public void Deserialize(BinaryReader reader)
        {
            Trustee = reader.ReadSerializable<ECPoint>();
            TrustAddress = reader.ReadSerializable<UInt160>();
            DividendAddress = reader.ReadSerializable<UInt160>();
            SubscribeAddress = reader.ReadSerializable<UInt160>();
            ArbitrateAddress = reader.ReadSerializable<UInt160>();
            AssetTrustTransaction = reader.ReadSerializable<AssetTrustTransaction>();
            TotalDividendAmount = reader.ReadSerializable<Fixed8>();
            TotalSubscribeAmount = reader.ReadSerializable<Fixed8>();
            Index = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
        }
        public string ToId()
        {
            return $"{Index}-{TxN}";
        }
    }
    public class DTFIDOKey : ISerializable
    {
        public UInt160 TrusteeAddress;
        public UInt160 IDOOwner;
        public UInt256 TxId;
        public virtual int Size => TrusteeAddress.Size + IDOOwner.Size + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TrusteeAddress);
            writer.Write(IDOOwner);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            TrusteeAddress = reader.ReadSerializable<UInt160>();
            IDOOwner = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
    public class DTFIDORecord : ISerializable
    {
        public UInt160 TrusteeAddress;
        public UInt160 IdoOwner;
        public Fixed8 IdoAmount;
        public uint BlockIndex;
        public ushort TxN;
        public virtual int Size => TrusteeAddress.Size + IdoOwner.Size + IdoAmount.Size + sizeof(uint) + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TrusteeAddress);
            writer.Write(IdoOwner);
            writer.Write(IdoAmount);
            writer.Write(BlockIndex);
            writer.Write(TxN);
        }
        public void Deserialize(BinaryReader reader)
        {
            TrusteeAddress = reader.ReadSerializable<UInt160>();
            IdoOwner = reader.ReadSerializable<UInt160>();
            IdoAmount = reader.ReadSerializable<Fixed8>();
            BlockIndex = reader.ReadUInt32();
            TxN = reader.ReadUInt16();
        }
    }
   
    public class DTFLockAssetMerge : ISerializable
    {
        public UInt160 TargetAddress;
        public LockAssetTransaction Tx;
        public TransactionOutput Output;
        public virtual int Size => TargetAddress.Size + Tx.Size + Output.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TargetAddress);
            writer.Write(Tx);
            writer.Write(Output);
        }
        public void Deserialize(BinaryReader reader)
        {
            TargetAddress = reader.ReadSerializable<UInt160>();
            Tx = reader.ReadSerializable<LockAssetTransaction>();
            Output = reader.ReadSerializable<TransactionOutput>();
        }

    }
    public class DTFIDOSummaryKey : ISerializable
    {
        public UInt160 IDOOwner;
        public UInt160 TrusteeAddress;
        public virtual int Size => IDOOwner.Size + TrusteeAddress.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(IDOOwner);
            writer.Write(TrusteeAddress);
        }
        public void Deserialize(BinaryReader reader)
        {
            IDOOwner = reader.ReadSerializable<UInt160>();
            TrusteeAddress = reader.ReadSerializable<UInt160>();
        }
        public override bool Equals(object obj)
        {
            if (obj is DTFIDOSummaryKey disk)
            {
                return disk.IDOOwner.Equals(this.IDOOwner) && disk.TrusteeAddress.Equals(this.TrusteeAddress);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.TrusteeAddress.GetHashCode() + this.IDOOwner.GetHashCode();
        }
    }
}

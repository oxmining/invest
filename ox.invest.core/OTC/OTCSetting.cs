using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Mining.Trade;
using System;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.OTC
{
    [EnumStrings(typeof(OTCStatus), "开放", "赎回", EnumName = "OTC状态")]
    [EnumEngStrings(typeof(OTCStatus), "Open", "Redeem", EnumName = "OTC State")]
    [Flags]
    public enum OTCStatus : byte
    {
        Open = 1 << 0,
        Redeem = 1 << 1
    }
    public class OTCSetting : ISerializable
    {
        public OTCStatus State;
        public byte EthAsset;
        public byte InRate;
        public byte OutRate;
        public byte[] Mark;
        public virtual int Size => sizeof(OTCStatus) + sizeof(byte) + sizeof(byte) + sizeof(byte) + Mark.GetVarSize();
        public OTCSetting()
        {
            Mark = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)State);
            writer.Write(EthAsset);
            writer.Write(InRate);
            writer.Write(OutRate);
            writer.WriteVarBytes(Mark);
        }
        public void Deserialize(BinaryReader reader)
        {
            State = (OTCStatus)reader.ReadByte();
            EthAsset = reader.ReadByte();
            InRate = reader.ReadByte();
            OutRate = reader.ReadByte();
            Mark = reader.ReadVarBytes();
        }
    }
    public class OTCDealerMerge : ISerializable
    {
        public UInt160 InPoolAddress;
        public string EthAddress;
        public OTCSetting Setting;
        public virtual int Size => InPoolAddress.Size + EthAddress.GetVarSize() + Setting.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(InPoolAddress);
            writer.WriteVarString(EthAddress);
            writer.Write(Setting);
        }
        public void Deserialize(BinaryReader reader)
        {
            InPoolAddress = reader.ReadSerializable<UInt160>();
            EthAddress = reader.ReadVarString();
            Setting = reader.ReadSerializable<OTCSetting>();
        }
    }
}

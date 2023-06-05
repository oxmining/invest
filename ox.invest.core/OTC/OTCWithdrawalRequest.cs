using OX.IO;
using System.IO;
using OX.Cryptography.ECC;

namespace OX.Mining.Trade
{
    public class OTCWithdrawalRequest : ISerializable
    {
        public string ReturnEthAddress;
        public UInt256 AssetId;
        public UInt160 From;
        public string ToEthAddress;
        public uint ExpirationIndex;
        public ECPoint OTCMasterPubKey;
        public virtual int Size =>ReturnEthAddress.GetVarSize()+ AssetId.Size + From.Size + ToEthAddress.GetVarSize() + sizeof(uint) + OTCMasterPubKey.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarString(ReturnEthAddress);
            writer.Write(AssetId);
            writer.Write(From);
            writer.WriteVarString(ToEthAddress);
            writer.Write(ExpirationIndex);
            writer.Write(OTCMasterPubKey);
        }
        public void Deserialize(BinaryReader reader)
        {
            ReturnEthAddress = reader.ReadVarString();
            AssetId = reader.ReadSerializable<UInt256>();
            From = reader.ReadSerializable<UInt160>();
            ToEthAddress = reader.ReadVarString();
            ExpirationIndex = reader.ReadUInt32();
            OTCMasterPubKey = reader.ReadSerializable<ECPoint>();
        }
    }
}

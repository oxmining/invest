using OX.IO;
using System.IO;

namespace OX.Mining.OTC
{
    public class EthDepositInfo : ISerializable
    {
        public string EthAddress;
        public string UnSignedData;
        public string SignedData;
        public string EthTxId;
        public virtual int Size => EthAddress.GetVarSize() + UnSignedData.GetVarSize() + SignedData.GetVarSize() + EthTxId.GetVarSize();

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarString(EthAddress);
            writer.WriteVarString(UnSignedData);
            writer.WriteVarString(SignedData);
            writer.WriteVarString(EthTxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            EthAddress = reader.ReadVarString();
            UnSignedData = reader.ReadVarString();
            SignedData = reader.ReadVarString();
            EthTxId = reader.ReadVarString();
        }
    }
    public class OTCDepositRequest : ISerializable
    {
        public string EthPoolAddress;
        public UInt160 ReceiptAddress;
        public byte[] EthTxInfo_Encrypt;
        public bool AgentFee;

        public virtual int Size => EthPoolAddress.GetVarSize() + ReceiptAddress.Size + EthTxInfo_Encrypt.GetVarSize() + sizeof(bool);

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarString(EthPoolAddress);
            writer.Write(ReceiptAddress);
            writer.WriteVarBytes(EthTxInfo_Encrypt);
            writer.Write(AgentFee);
        }
        public void Deserialize(BinaryReader reader)
        {
            EthPoolAddress = reader.ReadVarString();
            ReceiptAddress = reader.ReadSerializable<UInt160>();
            EthTxInfo_Encrypt = reader.ReadVarBytes();
            AgentFee = reader.ReadBoolean();
        }
    }
}

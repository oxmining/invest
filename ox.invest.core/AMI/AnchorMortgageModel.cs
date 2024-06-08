using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Mining.Trade;
using OX.Mining.OTC;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX.Mining.AMI
{
    public class AnchorMortgageIssueKey : ISerializable
    {
        public string EthereumAddress;
        public UInt256 TxHash;
        public uint Timestamp;
        public virtual int Size => EthereumAddress.GetVarSize() + TxHash.Size + sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarString(EthereumAddress);
            writer.Write(TxHash);
            writer.Write(Timestamp);
        }
        public void Deserialize(BinaryReader reader)
        {
            EthereumAddress = reader.ReadVarString();
            TxHash = reader.ReadSerializable<UInt256>();
            Timestamp = reader.ReadUInt32();
        }
    }


}

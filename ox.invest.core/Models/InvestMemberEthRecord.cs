using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OX.IO;
using OX.Cryptography;
using OX.Network.P2P.Payloads;
using OX.Network.P2P;
using OX.Wallets;

namespace OX
{
    public class InvestMemberEthRecord : BizModel
    {
        public uint MId;
        public string EthAddress;
        public byte[] Remarks;
        private UInt256 _hash = null;
        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    _hash = new UInt256(Crypto.Default.Hash256(this.GetHashData()));
                }
                return _hash;
            }
        }
        public override int Size => base.Size + sizeof(uint) + EthAddress.GetVarSize() + Remarks.GetVarSize();
        public InvestMemberEthRecord() : base((byte)InvestBizModelType.InvestMemberEth)
        {
            this.EthAddress = "0";
            this.Remarks = new byte[] { 0x00 };
        }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.Write(MId);
            writer.WriteVarString(EthAddress);
            writer.WriteVarBytes(Remarks);
        }
        public override void DeserializeBizModel(BinaryReader reader)
        {
            MId = reader.ReadUInt32();
            EthAddress = reader.ReadVarString();
            Remarks = reader.ReadVarBytes();
        }
    }
}

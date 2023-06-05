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

namespace OX
{
    public class InvestMemberVarRecord : BizModel
    {
        public byte State;
        public byte Level;
        public uint BreedIndex;

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
        public override int Size => base.Size + sizeof(byte) + sizeof(byte) + sizeof(uint);
        public InvestMemberVarRecord() : base((byte)InvestBizModelType.InvestMemberVar)
        { }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.Write(State);
            writer.Write(Level);
            writer.Write(BreedIndex);
        }
        public override void DeserializeBizModel(BinaryReader reader)
        {
            State = reader.ReadByte();
            Level = reader.ReadByte();
            BreedIndex = reader.ReadUInt32();
        }
    }
}

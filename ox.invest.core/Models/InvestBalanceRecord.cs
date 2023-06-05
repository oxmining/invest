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
    public class InvestBalanceRecord : BizModel
    {
        public uint Mid;
        public byte AccountKind;
        public Fixed8 OXS;
        public Fixed8 OXC;
        public Fixed8 TCS;
        public Fixed8 TCC;

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
        public override int Size => base.Size + sizeof(uint) + sizeof(byte) + OXS.Size + OXC.Size + TCS.Size + TCC.Size;
        public InvestBalanceRecord() : base((byte)InvestBizModelType.InvestBalance)
        { }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.Write(Mid);
            writer.Write(AccountKind);
            writer.Write(OXS);
            writer.Write(OXC);
            writer.Write(TCS);
            writer.Write(TCC);
        }
        public override void DeserializeBizModel(BinaryReader reader)
        {
            Mid = reader.ReadUInt32();
            AccountKind = reader.ReadByte();
            OXS = reader.ReadSerializable<Fixed8>();
            OXC = reader.ReadSerializable<Fixed8>();
            TCS = reader.ReadSerializable<Fixed8>();
            TCC = reader.ReadSerializable<Fixed8>();
        }
    }
}

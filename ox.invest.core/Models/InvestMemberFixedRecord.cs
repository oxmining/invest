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
    public class InvestMemberFixedRecord : BizModel
    {
        public uint MId;
        public uint PId;
        public UInt160 BreedScriptHash;
        public UInt160 InvestScriptHash;
        public UInt160 CasinoScriptHash;
        public UInt160 ExchangeScriptHash;
        public UInt160 PrivateScriptHash;
        public UInt160 OtherScriptHash;
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
        public override int Size => base.Size + sizeof(uint) + sizeof(uint) + BreedScriptHash.Size + InvestScriptHash.Size + CasinoScriptHash.Size + ExchangeScriptHash.Size + PrivateScriptHash.Size + OtherScriptHash.Size;
        public InvestMemberFixedRecord() : base((byte)InvestBizModelType.InvestMemberFixed)
        { }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.Write(MId);
            writer.Write(PId);
            writer.Write(BreedScriptHash);
            writer.Write(InvestScriptHash);
            writer.Write(CasinoScriptHash);
            writer.Write(ExchangeScriptHash);
            writer.Write(PrivateScriptHash);
            writer.Write(OtherScriptHash);
        }
        public override void DeserializeBizModel(BinaryReader reader)
        {
            MId = reader.ReadUInt32();
            PId = reader.ReadUInt32();
            BreedScriptHash = reader.ReadSerializable<UInt160>();
            InvestScriptHash = reader.ReadSerializable<UInt160>();
            CasinoScriptHash = reader.ReadSerializable<UInt160>();
            ExchangeScriptHash = reader.ReadSerializable<UInt160>();
            PrivateScriptHash = reader.ReadSerializable<UInt160>();
            OtherScriptHash = reader.ReadSerializable<UInt160>();
        }
    }
}

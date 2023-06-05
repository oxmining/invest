using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using OX.IO;
using OX.Cryptography;
using OX.Network.P2P.Payloads;
using OX.Network.P2P;

namespace OX
{
    public class PledgePersistenceKey : ISerializable
    {
        public byte Prefix = InvestBizPersistencePrefixes.Pledge_Indexer;
        public UInt160 From;
        public UInt160 Deposit;
        public UInt256 TxId;
        public ushort N;

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
        public virtual int Size => sizeof(byte) + From.Size + Deposit.Size + TxId.Size + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Prefix);
            writer.Write(From);
            writer.Write(Deposit);
            writer.Write(TxId);
            writer.Write(N);
        }
        public void Deserialize(BinaryReader reader)
        {
            Prefix = reader.ReadByte();
            From = reader.ReadSerializable<UInt160>();
            Deposit = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
        }
    }
    public class PledgeReversePersistenceKey : ISerializable
    {
        public byte Prefix = InvestBizPersistencePrefixes.Pledge_Indexer_Reverse;
        public UInt160 Deposit;
        public UInt160 From;
        public UInt256 TxId;
        public ushort N;

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
        public virtual int Size => sizeof(byte) + Deposit.Size + From.Size + TxId.Size + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Prefix);
            writer.Write(Deposit);
            writer.Write(From);
            writer.Write(TxId);
            writer.Write(N);
        }
        public void Deserialize(BinaryReader reader)
        {
            Prefix = reader.ReadByte();
            Deposit = reader.ReadSerializable<UInt160>();
            From = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
        }
    }
}

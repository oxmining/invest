using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OX.IO;
using OX.IO.Json;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Cryptography.ECC;
using OX.Cryptography;
using OX.SmartContract;

namespace OX
{
    public class PledgeProof : ISignatureTarget
    {
        public uint Timespan;
        public ECPoint PublicKey { get; set; }
        public int Size => sizeof(uint) + PublicKey.Size;
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
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Timespan);
            writer.Write(PublicKey);
        }
        public void Deserialize(BinaryReader reader)
        {
            Timespan = reader.ReadUInt32();
            PublicKey = reader.ReadSerializable<ECPoint>();
        }
    }
}

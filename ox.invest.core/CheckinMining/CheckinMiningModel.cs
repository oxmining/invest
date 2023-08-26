using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;

namespace OX.Mining.CheckinMining
{
    public class CheckinMark : ISerializable
    {
        public uint BlockIndex;
        public byte Kind;

        public virtual int Size => sizeof(uint) + sizeof(byte);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BlockIndex);
            writer.Write(Kind);
        }
        public void Deserialize(BinaryReader reader)
        {
            BlockIndex = reader.ReadUInt32();
            Kind = reader.ReadByte();
        }
    }
}

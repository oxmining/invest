using OX.IO;
using System.IO;

namespace OX.Mining.OTC
{
    public class OTCDealerRedeemRequest : ISerializable
    {
        public UInt160 RedeenSH;
        public UInt256 Asset;
        public Fixed8 MaxAmount;

        public virtual int Size => RedeenSH.Size + Asset.Size + MaxAmount.Size;
        public OTCDealerRedeemRequest()
        {
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RedeenSH);
            writer.Write(Asset);
            writer.Write(MaxAmount);

        }
        public void Deserialize(BinaryReader reader)
        {
            RedeenSH = reader.ReadSerializable<UInt160>();
            Asset = reader.ReadSerializable<UInt256>();
            MaxAmount = reader.ReadSerializable<Fixed8>();
        }
    }
}

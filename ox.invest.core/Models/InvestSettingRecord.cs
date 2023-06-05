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
    public class InvestSettingRecord : BizModel
    {
        public string Value;
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
        public override int Size => base.Size + Value.GetVarSize();
        public InvestSettingRecord() : base((byte)InvestBizModelType.InvestSetting)
        { }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.WriteVarString(Value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader">数据来源</param>
        public override void DeserializeBizModel(BinaryReader reader)
        {
            Value = reader.ReadVarString();
        }
    }
}

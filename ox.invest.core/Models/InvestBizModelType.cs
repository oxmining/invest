using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO.Caching;
using OX.Network.P2P.Payloads;

namespace OX
{
    //不能与其他BizModelType的定义冲突
    public enum InvestBizModelType : byte
    {
        [ReflectionCache(typeof(InvestSettingRecord))]
        InvestSetting = 0x00,
        [ReflectionCache(typeof(InvestMemberFixedRecord))]
        InvestMemberFixed = 0x01,
        [ReflectionCache(typeof(InvestMemberVarRecord))]
        InvestMemberVar = 0x02,
        [ReflectionCache(typeof(InvestBalanceRecord))]
        InvestBalance = 0x03,
        //[ReflectionCache(typeof(CasinoSettingRecord))]
        //CasinoSetting = 0x10,
        [ReflectionCache(typeof(InvestMemberEthRecord))]
        InvestMemberEth = 0x11
    }
}

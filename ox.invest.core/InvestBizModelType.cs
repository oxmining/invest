using OX.IO.Caching;
using OX.Mining.Trade;

namespace OX.Mining
{
    //不能与其他BizModelType的定义冲突
    public enum InvestBizModelType : byte
    {
        [ReflectionCache(typeof(InvestSettingRecord))]
        InvestSetting = 0x01    
    }
}

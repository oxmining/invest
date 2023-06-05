using OX.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Mining
{
    public class EnumItem<T> where T : struct
    {
        public T Target { get; private set; }
        public EnumItem(T obj)
        {
            this.Target = obj;
        }
        public override string ToString()
        {
            return UIHelper.LocalString(Target.StringValue(), Target.EngStringValue());
        }
    }
}

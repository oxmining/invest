using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX
{
    public class PledgeAccountSummary
    {
        public UInt160 From;
        public UInt160 Deposit;
        public uint DepositMId;
        public Fixed8 OXSSum = Fixed8.Zero;
        public Fixed8 OXCSum = Fixed8.Zero;
        public bool IsSelf;
    }
    public class PledgeReverseAccountSummary
    {
        public UInt160 Deposit;
        public UInt160 From;
        public Fixed8 OXSSum = Fixed8.Zero;
        public Fixed8 OXCSum = Fixed8.Zero;
        public bool IsSelf;
    }
}

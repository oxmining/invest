using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.Wallets;

namespace OX
{
    public class WalletAccountProfile : IWalletAccountProfile
    {
        public string Address { get; set; }
        public InvestMemberFixedRecord InvestMemberFixedRecord { get; set; }
        public InvestMemberVarRecord InvestMemberVarRecord { get; set; }
        public InvestMemberEthRecord InvestMemberEthRecord { get; set; }
        public InvestBalanceRecord[] BalanceRecords { get; set; }
    }
}

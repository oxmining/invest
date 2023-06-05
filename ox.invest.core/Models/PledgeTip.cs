using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OX.IO;
using OX.IO.Json;
using OX.Network.P2P.Payloads;

namespace OX
{
    public enum PledgeType : byte
    {
        Mortage = 0x01,
        RedeemPay = 0x02,
        RedeemRequest = 0x03
    }
}

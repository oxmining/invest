using OX.Wallets.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Web.Models
{
    public class BeatHubMessage : IHubMessage
    {
        public IOXID OXID { get; set; }
        public byte[] MessageData { get; set; }
    }
}

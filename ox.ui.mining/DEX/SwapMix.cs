using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using OX.Network.P2P;
using OX.Cryptography;
using OX.Cryptography.ECC;
using OX.UI.MAM;
using OX.Mining.DEX;

namespace OX.UI.Mining.DEX
{
    public class PendingItem
    {
        public SwapTxKey SwapTxKey;
        public SwapTxAmountKey SwapTxAmountKey;
        public string EthAddress;
    }
}

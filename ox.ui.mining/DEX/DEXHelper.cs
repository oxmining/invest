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
using System.Text;
using OX.Network.P2P;
using OX.Cryptography;
using OX.Cryptography.ECC;
using OX.Mining;

namespace OX.UI.Mining.DEX
{
    public static class DEXHelper
    {

        public static bool VerifyHolderOXS(this IMiningProvider provider, UInt160 sh, out Fixed8 balance, out Fixed8 settingFee)
        {
            var HolderBalanceOK = false;
            using var snapshot = Blockchain.Singleton.GetSnapshot();
            var acts = snapshot.Accounts.GetAndChange(sh, () => null);
            balance = acts.IsNotNull() ? acts.GetBalance(Blockchain.OXS) : Fixed8.Zero;
            var settings = provider.GetAllInvestSettings();
            settingFee = Fixed8.Zero;
            var setting = settings.FirstOrDefault(m => m.Key.SequenceEqual(new[] { InvestSettingTypes.SwapTraderOXS }));
            if (setting.Equals(new KeyValuePair<byte[], InvestSettingRecord>()))
                HolderBalanceOK = true;
            else
            {
                settingFee = Fixed8.FromDecimal(decimal.Parse(setting.Value.Value));
                if (balance >= settingFee)
                    HolderBalanceOK = true;
            }
            return HolderBalanceOK;
        }
        public static bool VerifyDepositOXC(this IMiningProvider provider, UInt160 sh, out Fixed8 balance, out Fixed8 settingFee)
        {
            var HolderBalanceOK = false;
            using var snapshot = Blockchain.Singleton.GetSnapshot();
            var acts = snapshot.Accounts.GetAndChange(sh, () => null);
            balance = acts.IsNotNull() ? acts.GetBalance(Blockchain.OXC) : Fixed8.Zero;
            var settings = provider.GetAllInvestSettings();
            settingFee = Fixed8.Zero;
            var setting = settings.FirstOrDefault(m => m.Key.SequenceEqual(new[] { InvestSettingTypes.SwapTraderDeposit }));
            if (setting.Equals(new KeyValuePair<byte[], InvestSettingRecord>()))
                HolderBalanceOK = true;
            else
            {
                settingFee = Fixed8.FromDecimal(decimal.Parse(setting.Value.Value));
                if (balance >= settingFee)
                    HolderBalanceOK = true;
            }
            return HolderBalanceOK;
        }
    }
}

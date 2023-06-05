using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using System.Collections.Generic;
using System.Linq;
using OX.Mining;
using OX.UI.Mining;

namespace OX.UI.Swap
{
    public partial class SwapRule : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        #region Constructor Region

        public SwapRule()
        {
            InitializeComponent();
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                var settings = bizPlugin.GetAllInvestSettings();
                //this.Clear();
                //AddSetting(settings, InvestSettingTypes.SwapTraderFee, UIHelper.LocalString("汇兑商开户费", "Swap trader account fee"), "OXC");
                //AddSetting(settings, InvestSettingTypes.SwapTraderOXS, UIHelper.LocalString("汇兑商OXS最低保证金", "Swap trader min OXS deposit"), "OXS");
                //AddSetting(settings, InvestSettingTypes.SwapTraderDeposit, UIHelper.LocalString("汇兑商OXC最低保证金", "Swap trader min OXC deposit"), "OXC");
                AddSetting(settings, InvestSettingTypes.SwapExternalToken, UIHelper.LocalString("兑换代币名称", "Swap token name"), "");
                AddSetting(settings, InvestSettingTypes.DEXBonusToken, UIHelper.LocalString("交易奖励资产", "DEX Bonus Asset Id"), "");
                AddSetting(settings, InvestSettingTypes.SidePairRegFee, UIHelper.LocalString("边池交易对注册费", "Side Exchange Pair Reg Fee"), "OXC");
            }
        }
        void AddSetting(IEnumerable<KeyValuePair<byte[], InvestSettingRecord>> settings, byte settingKey, string name, string suffix)
        {
            var setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { settingKey }));
            if (setting.Equals(new KeyValuePair<byte[], InvestSettingRecord>()))
                return;
            string text = $"{name}  :  {setting.Value.Value}  {suffix}";
            DarkListItem node = new DarkListItem(text);
            this.lstConsole.Items.Add(node);
        }
        public void Clear()
        {
            this.lstConsole.Items.Clear();
        }
        #endregion
        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be) { }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void OnBlock(Block block)
        {
        }
        public void BeforeOnBlock(Block block) { }
        public void AfterOnBlock(Block block) { }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
        }
        public void OnRebuild()
        {
           
        }

        #endregion
    }
}

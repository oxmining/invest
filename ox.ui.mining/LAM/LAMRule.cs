using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using System.Collections.Generic;
using System.Linq;
using OX.Mining;
using OX.UI.Mining;

namespace OX.UI.LAM
{
    public partial class LAMRule : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        #region Constructor Region

        public LAMRule()
        {
            InitializeComponent();
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default)
            {
                var settings = bizPlugin.GetAllInvestSettings();
                //this.Clear();
                AddSetting(settings, InvestSettingTypes.NodeBonusTotalLockVolume, UIHelper.LocalString("矿机推荐奖励锁仓量门槛", "Miner reward lock volume threshold"), "");
                //AddSetting(settings, InvestSettingTypes.NodeBonusOXSMinLock, UIHelper.LocalString("矿机推荐奖励最低锁仓数", "Miner reward min lock number"), "OXS");
                AddSetting(settings, InvestSettingTypes.NodeTeamBonusOXSLockVolume, UIHelper.LocalString("团队管理奖励OXS锁仓量门槛", "Team manage OXS lock volume threshold"), "");
                AddSetting(settings, InvestSettingTypes.NodeTeamBonusOXSMinLock, UIHelper.LocalString("团队管理奖励OXS最低锁仓数", "Team manage OXS min lock number"), "OXS");

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

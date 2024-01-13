using OX.Bapps;
using OX.Mining;
using OX.Network.P2P.Payloads;
using OX.Cryptography.ECC;
using System.Collections.Generic;
using OX.Wallets;

namespace OX.UI.Mining
{
    public class MiningBapp : Bapp
    {
        public override string MatchKernelVersion => "1.0.2";
        public override ECPoint[] BizPublicKeys => invest.BizPublicKeys;

        public override IBappProvider BuildBappProvider()
        {
            return new MiningProvider(this);
        }
        public override IBappApi BuildBappApi()
        {
            return default;
        }
        public override IBappUi BuildBappUi()
        {
            return new MiningUI(this);
        }
        protected override void InitBapp()
        {
        }
        public override SideScope[] GetSideScopes()
        {
            return new SideScope[] {
             new SideScope {
              MasterAddress=invest.SidePoolAccountAddress,
               Description=UIHelper.LocalString("主池交易对边际信托","Master Swap Side Trust")
             },
             new SideScope {
              MasterAddress=invest.SlaveSidePoolAccountAddress,
               Description=UIHelper.LocalString("边池交易对边际信托","Slave Swap Side Trust")
             }
            };
        }
    }

}

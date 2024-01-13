using OX.Bapps;
using OX.IO;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using System.Collections.Generic;
using System.Linq;
using OX.UI.Swap;
using OX.UI.LAM;
using OX.Mining;
using OX.UI.Mining.DEX;
using OX.UI.Mining.DTF;

namespace OX.UI.Mining
{
    public class MiningUI : IBappUi
    {
        public Bapp Bapp { get; set; }
        Dictionary<string, IUIModule> _modules = new Dictionary<string, IUIModule>();
        public IUIModule[] Modules { get { return _modules.Values.ToArray(); } }
        public MiningUI(Bapp bapp)
        {
            Bapp = bapp;
            LAMModule lammodule = new LAMModule(bapp);
            _modules[lammodule.ModuleName] = lammodule;
            DEXModule dexmodule = new DEXModule(bapp);
            _modules[dexmodule.ModuleName] = dexmodule;
            OTCModule otcmodule = new OTCModule(bapp);
            _modules[otcmodule.ModuleName] = otcmodule;
            DTFModule dtfmodule = new DTFModule(bapp);
            _modules[dtfmodule.ModuleName] = dtfmodule;
        }
        public void OnBappEvent(BappEvent bappEvent)
        {
            foreach (var m in Modules)
                if (m is Module module)
                    module.OnBappEvent(bappEvent);
        }
        public void OnCrossBappMessage(CrossBappMessage message)
        {
            foreach (var m in Modules)
                if (m is Module module)
                    module.OnCrossBappMessage(message);
        }
        public void OnBlock(Block block)
        {
            foreach (var m in Modules)
                if (m is Module module)
                    module.OnBlock(block);
        }
        public void BeforeOnBlock(Block block)
        {
            foreach (var m in Modules)
                if (m is Module module)
                    module.BeforeOnBlock(block);
        }
        public void AfterOnBlock(Block block)
        {
            foreach (var m in Modules)
                if (m is Module module)
                    module.AfterOnBlock(block);
        }
        public void OnRebuild()
        {
            foreach (var m in Modules)
                if (m is Module module)
                    module.OnRebuild();
        }
    }
}

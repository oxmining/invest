
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using OX.Wallets;
using OX.Wallets.Authentication;
using OX.Wallets.States;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using OX.MetaMask;
using System.Collections.Generic;
using OX.Bapps;
using OX.Mining;
using OX.UI.Mining;
using System.Linq;
namespace OX.Web.Models
{
    public abstract class MiningComponentBase : StatesComponentBase
    {
        protected MiningWebBox Box;
        //protected string EthAddress;
        //protected int? Chain;
        protected bool Valid
        {
            get
            {
                if (this.Box.IsNull()) return false;
                if (this.Box.Notecase.IsNull()) return false;
                if (this.Box.Notecase.Wallet.IsNull()) return false;
                if (this.EthID.IsNull()) return false;
                return true;
            }
        }
        protected bool ValidMainChain
        {
            get
            {
                return this.ChainID.HasValue && this.Chain == Chain.Mainnet;
            }
        }
        protected bool ValidChain
        {
            get
            {
                if (this.ChainID.HasValue)
                {
                    var Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                    if (Provider.IsNotNull())
                    {
                        var settings = Provider.GetAllInvestSettings();
                        var setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { InvestSettingTypes.ValidEthChain }));
                        if (!setting.Equals(new KeyValuePair<byte[], InvestSettingRecord>()))
                        {
                            if (setting.Value.Value.IsNotNullAndEmpty())
                            {
                                foreach (var c in setting.Value.Value.Split('-'))
                                {
                                    if (this.ChainID.Value.ToString() == c) return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }
        protected Chain[] ValidChains
        {
            get
            {
                List<Chain> result = new List<Chain>();
                if (this.ChainID.HasValue)
                {
                    var Provider = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                    if (Provider.IsNotNull())
                    {
                        var settings = Provider.GetAllInvestSettings();
                        var setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { InvestSettingTypes.ValidEthChain }));
                        if (!setting.Equals(new KeyValuePair<byte[], InvestSettingRecord>()))
                        {
                            if (setting.Value.Value.IsNotNullAndEmpty())
                            {
                                foreach (var c in setting.Value.Value.Split('-'))
                                {
                                    result.Add((Chain)int.Parse(c));
                                }
                            }
                        }
                    }
                }
                return result.ToArray();
            }
        }
        protected override void OnStateInit()
        {
            Box = WebBox.GetWebBox<MiningWebBox>();
            this.OnMiningInit();
        }
        protected abstract void OnMiningInit();
        protected override void StateDispatcher_NodeStateNotice(INodeStateMessage message)
        {
        }
        protected override void StateDispatcher_MixStateNotice(IMixStateMessage message)
        {
        }
        protected override void StateDispatcher_ServerStateNotice(IServerStateMessage message)
        {
        }
        protected override void IMetaMaskService_OnDisconnectEvent()
        {
            Console.WriteLine("Disconnect");
        }

        protected override void IMetaMaskService_OnConnectEvent()
        {
            Console.WriteLine("Connect");
        }

        protected override async Task MetaMaskService_ChainChangedEvent((long, Chain) arg)
        {
            Console.WriteLine("Chain Changed");
            await GetSelectedNetwork();
            StateHasChanged();
        }

        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            Console.WriteLine("Account Changed");
            await GetSelectedAddress();
            StateHasChanged();
        }

    }
}

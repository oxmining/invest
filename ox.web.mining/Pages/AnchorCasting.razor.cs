
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using OX.Wallets;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using OX.Network.P2P.Payloads;
using OX;
using OX.IO;
using OX.Cryptography.ECC;
using OX.Ledger;
using OX.SmartContract;
using OX.Cryptography;
using OX.Web.Models;
using OX.Wallets.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using OX.Wallets.Authentication;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using OX.Wallets.States;
using OX.Bapps;
using OX.Wallets.Eths;
using OX.Mining;
using OX.UI.Mining;
using OX.Mining.DTF;
using System.Collections.Specialized;
using OX.Mining.AMI;
using OX.Mining.StakingMining;
using NBitcoin.Secp256k1;
using OX.MetaMask;
using StringWrapper = OX.Mining.StakingMining.StringWrapper;

namespace OX.Web.Pages
{
    public partial class AnchorCasting
    {
        public override string PageTitle => this.WebLocalString("锚定铸造USDT", "Anchor Casting USDT");
        IEnumerable<KeyValuePair<AnchorMortgageIssueKey, TransactionOutput>> CastingRecords;
        IEnumerable<KeyValuePair<AnchorMortgageIssueKey, TransactionOutput>> DestroyRecords;
        public AnchorCastViewModel Model { get; set; } = new AnchorCastViewModel { Amount = 100m };
        string MortgageAddress;
        string msg;
        protected override void OnMiningInit()
        {
            Init();
        }

        void Init()
        {
            if (this.Valid)
            {
                var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>() as MiningProvider;
                if (bizPlugin.IsNotNull())
                {
                    MortgageAddress = bizPlugin.GetAnchorMortgageIssuePoolAddress();
                    CastingRecords = bizPlugin.GetAll<AnchorMortgageIssueKey, TransactionOutput>(InvestBizPersistencePrefixes.AMI_USDTCastRecord, new StringWrapper(this.EthID.EthAddress.ToLower()));
                    DestroyRecords = bizPlugin.GetAll<AnchorMortgageIssueKey, TransactionOutput>(InvestBizPersistencePrefixes.AMI_USDTDestroyRecord, new StringWrapper(this.EthID.EthAddress.ToLower()));
                }
            }
        }
        async void Cast()
        {
            if (this.Valid && this.MortgageAddress.IsNotNullAndEmpty())
            {
                if (this.Model.Amount >= 100M)
                {
                    try
                    {
                        var ethtxid = await this.MetaMaskService.SendUSDT(this.MortgageAddress, this.Model.Amount);
                        if (ethtxid.IsNotNullAndEmpty())
                        {
                            this.msg = this.WebLocalString($"以太坊交易 {ethtxid}已经尝试", $"Ethereum transaction {ethtxid} has been attempted");
                            StateHasChanged();
                        }
                    }
                    catch (UserDeniedException e)
                    {
                        this.msg = this.WebLocalString($"已经拒绝交易", $"Transaction has been rejected");
                        StateHasChanged();
                    }
                }
                else
                {
                    this.msg = this.WebLocalString($"最低铸造额为100USDT", $"The minimum casting amount is 100 USDT");
                    StateHasChanged();
                }
            }
        }
        void Destroy()
        {
            if (this.Valid)
            {
                var url = $"/_pc/wallet/transferasset/{invest.USDT_Asset.ToString()}/1/{UInt160.Zero.ToAddress()}/{this.Model.Amount.ToString()}";
                this.NavigationManager.NavigateTo(url);
            }
        }
    }
}

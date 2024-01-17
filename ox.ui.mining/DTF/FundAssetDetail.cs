using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using OX.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OX.SmartContract;
using OX.Mining.DTF;
using OX.Mining;
using OX.Mining.DEX;

namespace OX.UI.Mining.DTF
{
    public partial class FundAssetDetail : DarkDialog
    {
        INotecase Operater;
        UInt160 TrusteeAddress;
        TrustFundModel TFModel;
        public FundAssetDetail(INotecase notecase, UInt160 trusteeAddress, TrustFundModel tfModel)
        {
            this.Operater = notecase;
            this.TrusteeAddress = trusteeAddress;
            this.TFModel = tfModel;
            InitializeComponent();
            this.Text = UIHelper.LocalString($"信托资产明细:{tfModel.TrustAddress.ToAddress()}", $"Trust Asset Detail:{tfModel.TrustAddress.ToAddress()}");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FundAssetDetail_Load(object sender, EventArgs e)
        {
            this.lb_commonAsset.Text = UIHelper.LocalString("未锁仓资产", "Unlocked Asset");
            this.lb_lockAsset.Text = UIHelper.LocalString("锁仓资产", "Locked Asset");
            this.lb_rateList.Text = UIHelper.LocalString("分红率排行", "Dividend Rate Sorting");
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(this.TFModel.TrustAddress, () => null);
            if (acts.IsNotNull())
            {
                foreach (var b in acts.Balances)
                {
                    var assetId = b.Key;
                    var assetname = string.Empty;

                    if (assetId.Equals(Blockchain.OXS))
                        assetname = "OXS";
                    else if (assetId.Equals(Blockchain.OXC))
                        assetname = "OXC";
                    else
                    {
                        var state = Blockchain.Singleton.Store.GetAssets().TryGet(assetId);
                        assetname = state.GetName();
                    }
                    this.dlv_unlockAsset.Items.Add(new DarkListItem { Text = $"{assetname}  :    {b.Value}" });
                }
            }
            var bizPlugin = Bapp.GetBappProvider<MiningBapp, IMiningProvider>();
            if (bizPlugin != default && this.Operater.IsNotNull() && this.Operater.Wallet.IsNotNull())
            {
                Dictionary<UInt160, decimal> rates = new Dictionary<UInt160, decimal>();
                var idos = bizPlugin.GetAll<DTFIDOKey, DTFIDORecord>(InvestBizPersistencePrefixes.TrustFundIDORecord, this.TrusteeAddress);
                if (idos.IsNotNullAndEmpty())
                {
                    decimal totalRatio = 0;
                    var total = idos.Sum(m => m.Value.IdoAmount.GetInternalValue());
                    long t = 0;
                    foreach (var ido in idos.OrderBy(m => (long)m.Value.BlockIndex * 10000 + (long)m.Value.TxN))
                    {
                        var f = ido.Value.IdoAmount.GetInternalValue();
                        var ratio = DividentSlope.Big_5.ComputeBonusRatio(total, t, f);
                        t += f;
                        if (this.Operater.Wallet.ContainsAndHeld(ido.Value.IdoOwner))
                        {
                            totalRatio += ratio;
                        }
                        decimal r = ratio;
                        if (rates.TryGetValue(ido.Value.IdoOwner, out decimal d))
                            r += d;
                        rates[ido.Value.IdoOwner] = r;
                    }
                    this.lb_myRatio.Text = UIHelper.LocalString($"我合计的分红率: {totalRatio.ToString("f6")}", $"My Total dividend rate: {totalRatio.ToString("f6")}");
                }
                foreach (var rp in rates.OrderByDescending(m => m.Value))
                {
                    this.dlv_rateList.Items.Add(new DarkListItem { Text = $"{rp.Key.ToAddress()}  :    {rp.Value.ToString("f6")}" });
                }
                foreach (var la in bizPlugin.GetAllDTFLockAssets())
                {
                    if (la.Value.TargetAddress == this.TFModel.TrustAddress)
                    {
                        var assetId = la.Value.Output.AssetId;
                        var assetname = string.Empty;

                        if (assetId.Equals(Blockchain.OXS))
                            assetname = "OXS";
                        else if (assetId.Equals(Blockchain.OXC))
                            assetname = "OXC";
                        else
                        {
                            var state = Blockchain.Singleton.Store.GetAssets().TryGet(assetId);
                            assetname = state.GetName();
                        }
                        this.dlv_lockAsset.Items.Add(new DarkListItem { Text = $"{assetname}  :    {la.Value.Output.Value}   ->{la.Value.Tx.LockExpiration}" });
                    }
                }
            }
        }
    }
}

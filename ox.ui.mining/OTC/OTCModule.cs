using OX.Bapps;
using OX.IO;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.SmartContract;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OX.Ledger;
using OX.IO.Json;
using System.Net.WebSockets;
using OX.Mining;
using OX.UI.Mining;
using System.Security.Principal;
using OX.Mining.DEX;
using OX.UI.OTC;

namespace OX.UI.Swap
{
    public class OTCModule : Module
    {
        public override string ModuleName { get { return "otcmodule"; } }
        public override uint Index { get { return 14; } }

        public INotecase Operater;

        public OTCModule(Bapp bapp) : base(bapp)
        {
        }
        public override void InitEvents() { }
        public override void InitWindows()
        {
            ToolStripMenuItem swapMenu = new ToolStripMenuItem();
            swapMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            swapMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            swapMenu.Name = "otcMenu";
            swapMenu.Size = new System.Drawing.Size(39, 21);
            swapMenu.Text = UIHelper.LocalString("场外交易", "OTC");

            ToolStripMenuItem regOTCDealerMenu = new ToolStripMenuItem();
            regOTCDealerMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            regOTCDealerMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            regOTCDealerMenu.Name = "regOTCDealerMenu";
            regOTCDealerMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            regOTCDealerMenu.Size = new System.Drawing.Size(170, 22);
            regOTCDealerMenu.Text = UIHelper.LocalString("场外出金", "OTC sale");
            regOTCDealerMenu.Click += NewTraderMenu_Click;

            if (OXRunTime.RunMode == RunMode.Server|| OXRunTime.RunMode == RunMode.Mix)
            {
                ToolStripMenuItem depositMenu = new ToolStripMenuItem();
                depositMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
                depositMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                depositMenu.Name = "depositMenu";
                depositMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
                depositMenu.Size = new System.Drawing.Size(170, 22);
                depositMenu.Text = UIHelper.LocalString("场外入金", "OTC buy");
                depositMenu.Click += DepositMenu_Click;

                swapMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
               depositMenu
            });
            }

            swapMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
               regOTCDealerMenu
            });
            this.Container.TopMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            swapMenu});
        }

        private void DepositMenu_Click(object sender, EventArgs e)
        {
            OXRunTime.GoWeb("/_pc/invest/deposit");
        }

        private void NewTraderMenu_Click(object sender, EventArgs e)
        {
            new RegOTCDealerForm(this.Operater).ShowDialog();
        }

        public override void OnCrossBappMessage(CrossBappMessage message)
        {

        }


        public override void OnBappEvent(BappEvent be)
        {

        }

        public override void HeartBeat(HeartBeatContext context)
        {

        }
        public override void OnBlock(Block block)
        {

        }
        public override void BeforeOnBlock(Block block)
        {

        }
        public override void AfterOnBlock(Block block)
        {

        }
        public override void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;

        }
        public override void OnRebuild()
        {

        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {

        }


    }
}

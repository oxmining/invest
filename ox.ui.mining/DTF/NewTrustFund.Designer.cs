using OX.Wallets.UI.Controls;
namespace OX.UI.Mining.DTF
{
    partial class NewTrustFund
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTrustFund));
            panel = new System.Windows.Forms.Panel();
            bt_copy = new DarkButton();
            tb_trustAddr = new DarkTextBox();
            lb_trustAddr = new DarkLabel();
            bt_Close = new DarkButton();
            tb_balance = new DarkTextBox();
            lb_balance = new DarkLabel();
            bt_OK = new DarkButton();
            cbAccounts = new DarkComboBox();
            lb_truster = new DarkLabel();
            panel.SuspendLayout();
            SuspendLayout();
            // 
            // panel
            // 
            panel.Controls.Add(bt_copy);
            panel.Controls.Add(tb_trustAddr);
            panel.Controls.Add(lb_trustAddr);
            panel.Controls.Add(bt_Close);
            panel.Controls.Add(tb_balance);
            panel.Controls.Add(lb_balance);
            panel.Controls.Add(bt_OK);
            panel.Controls.Add(cbAccounts);
            panel.Controls.Add(lb_truster);
            resources.ApplyResources(panel, "panel");
            panel.Name = "panel";
            panel.Paint += panel_Paint;
            // 
            // bt_copy
            // 
            resources.ApplyResources(bt_copy, "bt_copy");
            bt_copy.Name = "bt_copy";
            bt_copy.SpecialBorderColor = null;
            bt_copy.SpecialFillColor = null;
            bt_copy.SpecialTextColor = null;
            bt_copy.Click += bt_copy_Click;
            // 
            // tb_trustAddr
            // 
            tb_trustAddr.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_trustAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_trustAddr.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_trustAddr, "tb_trustAddr");
            tb_trustAddr.Name = "tb_trustAddr";
            tb_trustAddr.ReadOnly = true;
            // 
            // lb_trustAddr
            // 
            resources.ApplyResources(lb_trustAddr, "lb_trustAddr");
            lb_trustAddr.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_trustAddr.Name = "lb_trustAddr";
            // 
            // bt_Close
            // 
            resources.ApplyResources(bt_Close, "bt_Close");
            bt_Close.Name = "bt_Close";
            bt_Close.SpecialBorderColor = null;
            bt_Close.SpecialFillColor = null;
            bt_Close.SpecialTextColor = null;
            bt_Close.Click += bt_Close_Click;
            // 
            // tb_balance
            // 
            tb_balance.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_balance.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_balance, "tb_balance");
            tb_balance.Name = "tb_balance";
            tb_balance.ReadOnly = true;
            // 
            // lb_balance
            // 
            resources.ApplyResources(lb_balance, "lb_balance");
            lb_balance.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_balance.Name = "lb_balance";
            // 
            // bt_OK
            // 
            resources.ApplyResources(bt_OK, "bt_OK");
            bt_OK.Name = "bt_OK";
            bt_OK.SpecialBorderColor = null;
            bt_OK.SpecialFillColor = null;
            bt_OK.SpecialTextColor = null;
            bt_OK.Click += bt_OK_Click;
            // 
            // cbAccounts
            // 
            cbAccounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(cbAccounts, "cbAccounts");
            cbAccounts.Name = "cbAccounts";
            cbAccounts.SpecialBorderColor = null;
            cbAccounts.SpecialFillColor = null;
            cbAccounts.SpecialTextColor = null;
            cbAccounts.SelectedIndexChanged += cbAccounts_SelectedIndexChanged;
            // 
            // lb_truster
            // 
            resources.ApplyResources(lb_truster, "lb_truster");
            lb_truster.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_truster.Name = "lb_truster";
            // 
            // NewTrustFund
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewTrustFund";
            FormClosing += ClaimForm_FormClosing;
            Load += NewEvent_Load;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private   DarkComboBox cbAccounts;
        private  DarkLabel lb_truster;
        private DarkTextBox tb_balance;
        private DarkLabel lb_balance;
        private DarkButton bt_OK;
        private DarkButton bt_Close;
        private DarkButton bt_copy;
        private DarkTextBox tb_trustAddr;
        private DarkLabel lb_trustAddr;
    }
}
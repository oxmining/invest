
namespace OX.UI.OTC
{
    partial class RegOTCDealerForm
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
            lb_balance = new Wallets.UI.Controls.DarkLabel();
            lb_accounts = new Wallets.UI.Controls.DarkLabel();
            cb_accounts = new Wallets.UI.Controls.DarkComboBox();
            cb_ethAccounts = new Wallets.UI.Controls.DarkComboBox();
            lb_ethAddress = new Wallets.UI.Controls.DarkLabel();
            tb_infeerate = new Wallets.UI.Controls.DarkTextBox();
            lb_infeerate = new Wallets.UI.Controls.DarkLabel();
            lb_State = new Wallets.UI.Controls.DarkLabel();
            cb_state = new Wallets.UI.Controls.DarkComboBox();
            tb_balance = new Wallets.UI.Controls.DarkTextBox();
            lb_amount = new Wallets.UI.Controls.DarkLabel();
            tb_amount = new Wallets.UI.Controls.DarkTextBox();
            lb_inpool_addr = new Wallets.UI.Controls.DarkLabel();
            lb_inpool_addr_v = new Wallets.UI.Controls.DarkLabel();
            lb_inpool_balance = new Wallets.UI.Controls.DarkLabel();
            lb_inpool_balance_v = new Wallets.UI.Controls.DarkLabel();
            darkLabel1 = new Wallets.UI.Controls.DarkLabel();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(166, 18);
            btnCancel.Click += btnCancel_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new System.Drawing.Point(18, 18);
            // 
            // btnYes
            // 
            btnYes.Location = new System.Drawing.Point(18, 18);
            // 
            // btnNo
            // 
            btnNo.Location = new System.Drawing.Point(18, 18);
            // 
            // btnRetry
            // 
            btnRetry.Location = new System.Drawing.Point(708, 18);
            // 
            // btnIgnore
            // 
            btnIgnore.Location = new System.Drawing.Point(708, 18);
            // 
            // lb_balance
            // 
            lb_balance.AutoSize = true;
            lb_balance.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_balance.Location = new System.Drawing.Point(27, 131);
            lb_balance.Name = "lb_balance";
            lb_balance.Size = new System.Drawing.Size(106, 24);
            lb_balance.TabIndex = 2;
            lb_balance.Text = "darkLabel1";
            // 
            // lb_accounts
            // 
            lb_accounts.AutoSize = true;
            lb_accounts.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_accounts.Location = new System.Drawing.Point(27, 81);
            lb_accounts.Name = "lb_accounts";
            lb_accounts.Size = new System.Drawing.Size(106, 24);
            lb_accounts.TabIndex = 6;
            lb_accounts.Text = "darkLabel1";
            // 
            // cb_accounts
            // 
            cb_accounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_accounts.FormattingEnabled = true;
            cb_accounts.Location = new System.Drawing.Point(298, 78);
            cb_accounts.Name = "cb_accounts";
            cb_accounts.Size = new System.Drawing.Size(709, 31);
            cb_accounts.SpecialBorderColor = null;
            cb_accounts.SpecialFillColor = null;
            cb_accounts.SpecialTextColor = null;
            cb_accounts.TabIndex = 7;
            cb_accounts.SelectedIndexChanged += cb_accounts_SelectedIndexChanged;
            // 
            // cb_ethAccounts
            // 
            cb_ethAccounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_ethAccounts.FormattingEnabled = true;
            cb_ethAccounts.Location = new System.Drawing.Point(298, 23);
            cb_ethAccounts.Name = "cb_ethAccounts";
            cb_ethAccounts.Size = new System.Drawing.Size(709, 31);
            cb_ethAccounts.SpecialBorderColor = null;
            cb_ethAccounts.SpecialFillColor = null;
            cb_ethAccounts.SpecialTextColor = null;
            cb_ethAccounts.TabIndex = 9;
            cb_ethAccounts.SelectedIndexChanged += cb_ethAccounts_SelectedIndexChanged;
            // 
            // lb_ethAddress
            // 
            lb_ethAddress.AutoSize = true;
            lb_ethAddress.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_ethAddress.Location = new System.Drawing.Point(27, 26);
            lb_ethAddress.Name = "lb_ethAddress";
            lb_ethAddress.Size = new System.Drawing.Size(106, 24);
            lb_ethAddress.TabIndex = 8;
            lb_ethAddress.Text = "darkLabel1";
            // 
            // tb_infeerate
            // 
            tb_infeerate.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_infeerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_infeerate.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_infeerate.Location = new System.Drawing.Point(301, 206);
            tb_infeerate.Name = "tb_infeerate";
            tb_infeerate.Size = new System.Drawing.Size(154, 30);
            tb_infeerate.TabIndex = 18;
            tb_infeerate.Text = "0";
            tb_infeerate.TextChanged += tb_infeerate_TextChanged;
            // 
            // lb_infeerate
            // 
            lb_infeerate.AutoSize = true;
            lb_infeerate.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_infeerate.Location = new System.Drawing.Point(30, 212);
            lb_infeerate.Name = "lb_infeerate";
            lb_infeerate.Size = new System.Drawing.Size(106, 24);
            lb_infeerate.TabIndex = 17;
            lb_infeerate.Text = "darkLabel1";
            // 
            // lb_State
            // 
            lb_State.AutoSize = true;
            lb_State.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_State.Location = new System.Drawing.Point(556, 212);
            lb_State.Name = "lb_State";
            lb_State.Size = new System.Drawing.Size(106, 24);
            lb_State.TabIndex = 25;
            lb_State.Text = "darkLabel1";
            // 
            // cb_state
            // 
            cb_state.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_state.FormattingEnabled = true;
            cb_state.Location = new System.Drawing.Point(827, 209);
            cb_state.Name = "cb_state";
            cb_state.Size = new System.Drawing.Size(183, 31);
            cb_state.SpecialBorderColor = null;
            cb_state.SpecialFillColor = null;
            cb_state.SpecialTextColor = null;
            cb_state.TabIndex = 29;
            // 
            // tb_balance
            // 
            tb_balance.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_balance.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_balance.Location = new System.Drawing.Point(298, 125);
            tb_balance.Name = "tb_balance";
            tb_balance.ReadOnly = true;
            tb_balance.Size = new System.Drawing.Size(183, 30);
            tb_balance.TabIndex = 31;
            tb_balance.Text = "0";
            // 
            // lb_amount
            // 
            lb_amount.AutoSize = true;
            lb_amount.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_amount.Location = new System.Drawing.Point(554, 131);
            lb_amount.Name = "lb_amount";
            lb_amount.Size = new System.Drawing.Size(106, 24);
            lb_amount.TabIndex = 32;
            lb_amount.Text = "darkLabel1";
            // 
            // tb_amount
            // 
            tb_amount.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_amount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_amount.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_amount.Location = new System.Drawing.Point(827, 125);
            tb_amount.Name = "tb_amount";
            tb_amount.Size = new System.Drawing.Size(183, 30);
            tb_amount.TabIndex = 33;
            tb_amount.Text = "0";
            // 
            // lb_inpool_addr
            // 
            lb_inpool_addr.AutoSize = true;
            lb_inpool_addr.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_inpool_addr.Location = new System.Drawing.Point(30, 279);
            lb_inpool_addr.Name = "lb_inpool_addr";
            lb_inpool_addr.Size = new System.Drawing.Size(106, 24);
            lb_inpool_addr.TabIndex = 36;
            lb_inpool_addr.Text = "darkLabel1";
            // 
            // lb_inpool_addr_v
            // 
            lb_inpool_addr_v.AutoSize = true;
            lb_inpool_addr_v.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_inpool_addr_v.Location = new System.Drawing.Point(301, 279);
            lb_inpool_addr_v.Name = "lb_inpool_addr_v";
            lb_inpool_addr_v.Size = new System.Drawing.Size(0, 24);
            lb_inpool_addr_v.TabIndex = 37;
            // 
            // lb_inpool_balance
            // 
            lb_inpool_balance.AutoSize = true;
            lb_inpool_balance.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_inpool_balance.Location = new System.Drawing.Point(30, 339);
            lb_inpool_balance.Name = "lb_inpool_balance";
            lb_inpool_balance.Size = new System.Drawing.Size(106, 24);
            lb_inpool_balance.TabIndex = 38;
            lb_inpool_balance.Text = "darkLabel1";
            // 
            // lb_inpool_balance_v
            // 
            lb_inpool_balance_v.AutoSize = true;
            lb_inpool_balance_v.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_inpool_balance_v.Location = new System.Drawing.Point(301, 339);
            lb_inpool_balance_v.Name = "lb_inpool_balance_v";
            lb_inpool_balance_v.Size = new System.Drawing.Size(0, 24);
            lb_inpool_balance_v.TabIndex = 39;
            // 
            // darkLabel1
            // 
            darkLabel1.AutoSize = true;
            darkLabel1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkLabel1.Location = new System.Drawing.Point(461, 210);
            darkLabel1.Name = "darkLabel1";
            darkLabel1.Size = new System.Drawing.Size(26, 24);
            darkLabel1.TabIndex = 40;
            darkLabel1.Text = "%";
            // 
            // RegOTCDealerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1045, 497);
            Controls.Add(darkLabel1);
            Controls.Add(lb_inpool_balance_v);
            Controls.Add(lb_inpool_balance);
            Controls.Add(lb_inpool_addr_v);
            Controls.Add(lb_inpool_addr);
            Controls.Add(tb_amount);
            Controls.Add(lb_amount);
            Controls.Add(tb_balance);
            Controls.Add(cb_state);
            Controls.Add(lb_State);
            Controls.Add(tb_infeerate);
            Controls.Add(lb_infeerate);
            Controls.Add(cb_ethAccounts);
            Controls.Add(lb_ethAddress);
            Controls.Add(cb_accounts);
            Controls.Add(lb_accounts);
            Controls.Add(lb_balance);
            DialogButtons = Wallets.UI.Forms.DarkDialogButton.OkCancel;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RegOTCDealerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "RegOTCDealerForm";
            Load += RegMinerForm_Load;
            Controls.SetChildIndex(lb_balance, 0);
            Controls.SetChildIndex(lb_accounts, 0);
            Controls.SetChildIndex(cb_accounts, 0);
            Controls.SetChildIndex(lb_ethAddress, 0);
            Controls.SetChildIndex(cb_ethAccounts, 0);
            Controls.SetChildIndex(lb_infeerate, 0);
            Controls.SetChildIndex(tb_infeerate, 0);
            Controls.SetChildIndex(lb_State, 0);
            Controls.SetChildIndex(cb_state, 0);
            Controls.SetChildIndex(tb_balance, 0);
            Controls.SetChildIndex(lb_amount, 0);
            Controls.SetChildIndex(tb_amount, 0);
            Controls.SetChildIndex(lb_inpool_addr, 0);
            Controls.SetChildIndex(lb_inpool_addr_v, 0);
            Controls.SetChildIndex(lb_inpool_balance, 0);
            Controls.SetChildIndex(lb_inpool_balance_v, 0);
            Controls.SetChildIndex(darkLabel1, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Wallets.UI.Controls.DarkLabel lb_balance;
        private Wallets.UI.Controls.DarkLabel lb_accounts;
        private Wallets.UI.Controls.DarkComboBox cb_accounts;
        private Wallets.UI.Controls.DarkComboBox cb_ethAccounts;
        private Wallets.UI.Controls.DarkLabel lb_ethAddress;
        private Wallets.UI.Controls.DarkTextBox tb_infeerate;
        private Wallets.UI.Controls.DarkLabel lb_infeerate;
        private Wallets.UI.Controls.DarkLabel lb_State;
        private Wallets.UI.Controls.DarkComboBox cb_state;
        private Wallets.UI.Controls.DarkTextBox tb_balance;
        private Wallets.UI.Controls.DarkLabel lb_amount;
        private Wallets.UI.Controls.DarkTextBox tb_amount;
        private Wallets.UI.Controls.DarkLabel lb_inpool_addr;
        private Wallets.UI.Controls.DarkLabel lb_inpool_addr_v;
        private Wallets.UI.Controls.DarkLabel lb_inpool_balance;
        private Wallets.UI.Controls.DarkLabel lb_inpool_balance_v;
        private Wallets.UI.Controls.DarkLabel darkLabel1;
    }
}
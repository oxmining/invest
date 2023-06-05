
namespace OX.UI.LAM
{
    partial class SelfLockForm
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
            this.lb_assetName = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_assetName = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_assetId = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_accounts = new OX.Wallets.UI.Controls.DarkLabel();
            this.cb_accounts = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_amount = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_blockexpire = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_assetId = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_amount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_balance = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_balance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_blockexpire = new OX.Wallets.UI.Controls.DarkTextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(18, 18);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(18, 18);
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(18, 18);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(18, 18);
            // 
            // btnRetry
            // 
            this.btnRetry.Location = new System.Drawing.Point(708, 18);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Location = new System.Drawing.Point(708, 18);
            // 
            // lb_assetName
            // 
            this.lb_assetName.AutoSize = true;
            this.lb_assetName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_assetName.Location = new System.Drawing.Point(30, 24);
            this.lb_assetName.Name = "lb_assetName";
            this.lb_assetName.Size = new System.Drawing.Size(106, 24);
            this.lb_assetName.TabIndex = 2;
            this.lb_assetName.Text = "darkLabel1";
            // 
            // tb_assetName
            // 
            this.tb_assetName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_assetName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_assetName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_assetName.Location = new System.Drawing.Point(232, 22);
            this.tb_assetName.Name = "tb_assetName";
            this.tb_assetName.ReadOnly = true;
            this.tb_assetName.Size = new System.Drawing.Size(338, 30);
            this.tb_assetName.TabIndex = 3;
            // 
            // tb_assetId
            // 
            this.tb_assetId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_assetId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_assetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_assetId.Location = new System.Drawing.Point(232, 72);
            this.tb_assetId.Name = "tb_assetId";
            this.tb_assetId.ReadOnly = true;
            this.tb_assetId.Size = new System.Drawing.Size(769, 30);
            this.tb_assetId.TabIndex = 5;
            // 
            // lb_accounts
            // 
            this.lb_accounts.AutoSize = true;
            this.lb_accounts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_accounts.Location = new System.Drawing.Point(30, 128);
            this.lb_accounts.Name = "lb_accounts";
            this.lb_accounts.Size = new System.Drawing.Size(106, 24);
            this.lb_accounts.TabIndex = 6;
            this.lb_accounts.Text = "darkLabel1";
            // 
            // cb_accounts
            // 
            this.cb_accounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_accounts.FormattingEnabled = true;
            this.cb_accounts.Location = new System.Drawing.Point(232, 125);
            this.cb_accounts.Name = "cb_accounts";
            this.cb_accounts.Size = new System.Drawing.Size(771, 31);
            this.cb_accounts.SpecialBorderColor = null;
            this.cb_accounts.SpecialFillColor = null;
            this.cb_accounts.SpecialTextColor = null;
            this.cb_accounts.TabIndex = 7;
            this.cb_accounts.SelectedIndexChanged += new System.EventHandler(this.cb_accounts_SelectedIndexChanged);
            // 
            // lb_amount
            // 
            this.lb_amount.AutoSize = true;
            this.lb_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_amount.Location = new System.Drawing.Point(30, 188);
            this.lb_amount.Name = "lb_amount";
            this.lb_amount.Size = new System.Drawing.Size(106, 24);
            this.lb_amount.TabIndex = 8;
            this.lb_amount.Text = "darkLabel1";
            // 
            // lb_blockexpire
            // 
            this.lb_blockexpire.AutoSize = true;
            this.lb_blockexpire.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_blockexpire.Location = new System.Drawing.Point(30, 248);
            this.lb_blockexpire.Name = "lb_blockexpire";
            this.lb_blockexpire.Size = new System.Drawing.Size(106, 24);
            this.lb_blockexpire.TabIndex = 10;
            this.lb_blockexpire.Text = "darkLabel1";
            // 
            // lb_assetId
            // 
            this.lb_assetId.AutoSize = true;
            this.lb_assetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_assetId.Location = new System.Drawing.Point(30, 78);
            this.lb_assetId.Name = "lb_assetId";
            this.lb_assetId.Size = new System.Drawing.Size(106, 24);
            this.lb_assetId.TabIndex = 11;
            this.lb_assetId.Text = "darkLabel1";
            // 
            // tb_amount
            // 
            this.tb_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_amount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_amount.Location = new System.Drawing.Point(232, 186);
            this.tb_amount.Name = "tb_amount";
            this.tb_amount.Size = new System.Drawing.Size(254, 30);
            this.tb_amount.TabIndex = 12;
            this.tb_amount.Text = "1000";
            this.tb_amount.TextChanged += new System.EventHandler(this.tb_blockexpire_TextChanged);
            // 
            // lb_balance
            // 
            this.lb_balance.AutoSize = true;
            this.lb_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_balance.Location = new System.Drawing.Point(533, 192);
            this.lb_balance.Name = "lb_balance";
            this.lb_balance.Size = new System.Drawing.Size(106, 24);
            this.lb_balance.TabIndex = 13;
            this.lb_balance.Text = "darkLabel1";
            // 
            // tb_balance
            // 
            this.tb_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_balance.Location = new System.Drawing.Point(747, 186);
            this.tb_balance.Name = "tb_balance";
            this.tb_balance.ReadOnly = true;
            this.tb_balance.Size = new System.Drawing.Size(254, 30);
            this.tb_balance.TabIndex = 14;
            // 
            // tb_blockexpire
            // 
            this.tb_blockexpire.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_blockexpire.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_blockexpire.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_blockexpire.Location = new System.Drawing.Point(232, 246);
            this.tb_blockexpire.Name = "tb_blockexpire";
            this.tb_blockexpire.Size = new System.Drawing.Size(338, 30);
            this.tb_blockexpire.TabIndex = 15;
            this.tb_blockexpire.Text = "1001000";
            this.tb_blockexpire.TextChanged += new System.EventHandler(this.tb_blockexpire_TextChanged);
            // 
            // SelfLockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 414);
            this.Controls.Add(this.tb_blockexpire);
            this.Controls.Add(this.tb_balance);
            this.Controls.Add(this.lb_balance);
            this.Controls.Add(this.tb_amount);
            this.Controls.Add(this.lb_assetId);
            this.Controls.Add(this.lb_blockexpire);
            this.Controls.Add(this.lb_amount);
            this.Controls.Add(this.cb_accounts);
            this.Controls.Add(this.lb_accounts);
            this.Controls.Add(this.tb_assetId);
            this.Controls.Add(this.tb_assetName);
            this.Controls.Add(this.lb_assetName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelfLockForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegMinerForm";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.lb_assetName, 0);
            this.Controls.SetChildIndex(this.tb_assetName, 0);
            this.Controls.SetChildIndex(this.tb_assetId, 0);
            this.Controls.SetChildIndex(this.lb_accounts, 0);
            this.Controls.SetChildIndex(this.cb_accounts, 0);
            this.Controls.SetChildIndex(this.lb_amount, 0);
            this.Controls.SetChildIndex(this.lb_blockexpire, 0);
            this.Controls.SetChildIndex(this.lb_assetId, 0);
            this.Controls.SetChildIndex(this.tb_amount, 0);
            this.Controls.SetChildIndex(this.lb_balance, 0);
            this.Controls.SetChildIndex(this.tb_balance, 0);
            this.Controls.SetChildIndex(this.tb_blockexpire, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wallets.UI.Controls.DarkLabel lb_assetName;
        private Wallets.UI.Controls.DarkTextBox tb_assetName;
        private Wallets.UI.Controls.DarkTextBox tb_assetId;
        private Wallets.UI.Controls.DarkLabel lb_accounts;
        private Wallets.UI.Controls.DarkComboBox cb_accounts;
        private Wallets.UI.Controls.DarkLabel lb_amount;
        private Wallets.UI.Controls.DarkLabel lb_blockexpire;
        private Wallets.UI.Controls.DarkLabel lb_assetId;
        private Wallets.UI.Controls.DarkTextBox tb_amount;
        private Wallets.UI.Controls.DarkLabel lb_balance;
        private Wallets.UI.Controls.DarkTextBox tb_balance;
        private Wallets.UI.Controls.DarkTextBox tb_blockexpire;
    }
}
using OX.Wallets;
using OX.Wallets.UI.Forms;

namespace OX.UI.Swap
{
    partial class SideSwapTrustRecharge
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
            lb_asset = new Wallets.UI.Controls.DarkLabel();
            lb_balance = new Wallets.UI.Controls.DarkLabel();
            lb_amount = new Wallets.UI.Controls.DarkLabel();
            rbTargetAsset = new Wallets.UI.Controls.DarkRadioButton();
            rbPriceAsset = new Wallets.UI.Controls.DarkRadioButton();
            textBox3 = new Wallets.UI.Controls.DarkTextBox();
            textBox2 = new Wallets.UI.Controls.DarkTextBox();
            cb_accounts = new Wallets.UI.Controls.DarkComboBox();
            lb_accounts = new Wallets.UI.Controls.DarkLabel();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(285, 19);
            btnCancel.Padding = new System.Windows.Forms.Padding(7, 10, 7, 10);
            // 
            // btnClose
            // 
            btnClose.Location = new System.Drawing.Point(16, 19);
            btnClose.Padding = new System.Windows.Forms.Padding(7, 10, 7, 10);
            // 
            // btnYes
            // 
            btnYes.Location = new System.Drawing.Point(16, 19);
            btnYes.Margin = new System.Windows.Forms.Padding(0, 0, 9, 0);
            btnYes.Padding = new System.Windows.Forms.Padding(7, 10, 7, 10);
            // 
            // btnNo
            // 
            btnNo.Location = new System.Drawing.Point(16, 19);
            btnNo.Margin = new System.Windows.Forms.Padding(0, 0, 9, 0);
            btnNo.Padding = new System.Windows.Forms.Padding(7, 10, 7, 10);
            // 
            // btnRetry
            // 
            btnRetry.Location = new System.Drawing.Point(644, 19);
            btnRetry.Padding = new System.Windows.Forms.Padding(7, 10, 7, 10);
            // 
            // btnIgnore
            // 
            btnIgnore.Location = new System.Drawing.Point(644, 19);
            btnIgnore.Padding = new System.Windows.Forms.Padding(7, 10, 7, 10);
            // 
            // lb_asset
            // 
            lb_asset.AutoSize = true;
            lb_asset.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_asset.Location = new System.Drawing.Point(38, 116);
            lb_asset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_asset.Name = "lb_asset";
            lb_asset.Size = new System.Drawing.Size(54, 25);
            lb_asset.TabIndex = 2;
            lb_asset.Text = "资产:";
            // 
            // lb_balance
            // 
            lb_balance.AutoSize = true;
            lb_balance.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_balance.Location = new System.Drawing.Point(38, 180);
            lb_balance.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_balance.Name = "lb_balance";
            lb_balance.Size = new System.Drawing.Size(54, 25);
            lb_balance.TabIndex = 3;
            lb_balance.Text = "余额:";
            // 
            // lb_amount
            // 
            lb_amount.AutoSize = true;
            lb_amount.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_amount.Location = new System.Drawing.Point(38, 260);
            lb_amount.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_amount.Name = "lb_amount";
            lb_amount.Size = new System.Drawing.Size(54, 25);
            lb_amount.TabIndex = 5;
            lb_amount.Text = "金额:";
            // 
            // rbTargetAsset
            // 
            rbTargetAsset.AutoSize = true;
            rbTargetAsset.Location = new System.Drawing.Point(334, 114);
            rbTargetAsset.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            rbTargetAsset.Name = "rbTargetAsset";
            rbTargetAsset.Size = new System.Drawing.Size(72, 29);
            rbTargetAsset.SpecialBorderColor = null;
            rbTargetAsset.SpecialFillColor = null;
            rbTargetAsset.SpecialTextColor = null;
            rbTargetAsset.TabIndex = 6;
            rbTargetAsset.Text = "OXS";
            rbTargetAsset.CheckedChanged += rbGTC_CheckedChanged;
            // 
            // rbPriceAsset
            // 
            rbPriceAsset.AutoSize = true;
            rbPriceAsset.Checked = true;
            rbPriceAsset.Location = new System.Drawing.Point(206, 114);
            rbPriceAsset.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            rbPriceAsset.Name = "rbPriceAsset";
            rbPriceAsset.Size = new System.Drawing.Size(73, 29);
            rbPriceAsset.SpecialBorderColor = null;
            rbPriceAsset.SpecialFillColor = null;
            rbPriceAsset.SpecialTextColor = null;
            rbPriceAsset.TabIndex = 7;
            rbPriceAsset.TabStop = true;
            rbPriceAsset.Text = "OXC";
            rbPriceAsset.CheckedChanged += rbGTC_CheckedChanged;
            // 
            // textBox3
            // 
            textBox3.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox3.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            textBox3.Location = new System.Drawing.Point(187, 175);
            textBox3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new System.Drawing.Size(682, 31);
            textBox3.TabIndex = 8;
            // 
            // textBox2
            // 
            textBox2.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            textBox2.Location = new System.Drawing.Point(187, 255);
            textBox2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(682, 31);
            textBox2.TabIndex = 10;
            textBox2.TextChanged += textBox_TextChanged;
            // 
            // cb_accounts
            // 
            cb_accounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_accounts.FormattingEnabled = true;
            cb_accounts.Location = new System.Drawing.Point(191, 48);
            cb_accounts.Name = "cb_accounts";
            cb_accounts.Size = new System.Drawing.Size(678, 32);
            cb_accounts.SpecialBorderColor = null;
            cb_accounts.SpecialFillColor = null;
            cb_accounts.SpecialTextColor = null;
            cb_accounts.TabIndex = 12;
            cb_accounts.SelectedIndexChanged += cb_accounts_SelectedIndexChanged;
            // 
            // lb_accounts
            // 
            lb_accounts.AutoSize = true;
            lb_accounts.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_accounts.Location = new System.Drawing.Point(38, 48);
            lb_accounts.Name = "lb_accounts";
            lb_accounts.Size = new System.Drawing.Size(54, 25);
            lb_accounts.TabIndex = 11;
            lb_accounts.Text = "账户:";
            // 
            // SideSwapTrustRecharge
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(904, 395);
            Controls.Add(cb_accounts);
            Controls.Add(lb_accounts);
            Controls.Add(textBox2);
            Controls.Add(textBox3);
            Controls.Add(rbPriceAsset);
            Controls.Add(rbTargetAsset);
            Controls.Add(lb_amount);
            Controls.Add(lb_balance);
            Controls.Add(lb_asset);
            Margin = new System.Windows.Forms.Padding(10, 12, 10, 12);
            Name = "SideSwapTrustRecharge";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "转账";
            Load += PayToDialog_Load;
            Controls.SetChildIndex(lb_asset, 0);
            Controls.SetChildIndex(lb_balance, 0);
            Controls.SetChildIndex(lb_amount, 0);
            Controls.SetChildIndex(rbTargetAsset, 0);
            Controls.SetChildIndex(rbPriceAsset, 0);
            Controls.SetChildIndex(textBox3, 0);
            Controls.SetChildIndex(textBox2, 0);
            Controls.SetChildIndex(lb_accounts, 0);
            Controls.SetChildIndex(cb_accounts, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OX.Wallets.UI.Controls.DarkLabel lb_asset;
        private OX.Wallets.UI.Controls.DarkLabel lb_balance;
        private OX.Wallets.UI.Controls.DarkLabel lb_amount;
        private OX.Wallets.UI.Controls.DarkRadioButton rbTargetAsset;
        private OX.Wallets.UI.Controls.DarkRadioButton rbPriceAsset;
        private OX.Wallets.UI.Controls.DarkTextBox textBox3;
        private OX.Wallets.UI.Controls.DarkTextBox textBox2;
        private Wallets.UI.Controls.DarkComboBox cb_accounts;
        private Wallets.UI.Controls.DarkLabel lb_accounts;
    }
}
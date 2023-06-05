using OX.Wallets;
namespace OX.UI.Swap
{
    partial class SwapTrustRecharge
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
            this.darkLabel1 = new OX.Wallets.UI.Controls.DarkLabel();
            this.darkLabel2 = new OX.Wallets.UI.Controls.DarkLabel();
            this.darkLabel4 = new OX.Wallets.UI.Controls.DarkLabel();
            this.rbTargetAsset = new OX.Wallets.UI.Controls.DarkRadioButton();
            this.rbPriceAsset = new OX.Wallets.UI.Controls.DarkRadioButton();
            this.textBox3 = new OX.Wallets.UI.Controls.DarkTextBox();
            this.textBox2 = new OX.Wallets.UI.Controls.DarkTextBox();
            this.cb_accounts = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_accounts = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_msg = new OX.Wallets.UI.Controls.DarkLabel();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(42, 111);
            this.darkLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(50, 24);
            this.darkLabel1.TabIndex = 2;
            this.darkLabel1.Text = "资产:";
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(42, 173);
            this.darkLabel2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(50, 24);
            this.darkLabel2.TabIndex = 3;
            this.darkLabel2.Text = "余额:";
            // 
            // darkLabel4
            // 
            this.darkLabel4.AutoSize = true;
            this.darkLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel4.Location = new System.Drawing.Point(42, 250);
            this.darkLabel4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.darkLabel4.Name = "darkLabel4";
            this.darkLabel4.Size = new System.Drawing.Size(50, 24);
            this.darkLabel4.TabIndex = 5;
            this.darkLabel4.Text = "金额:";
            // 
            // rbTargetAsset
            // 
            this.rbTargetAsset.AutoSize = true;
            this.rbTargetAsset.Location = new System.Drawing.Point(275, 107);
            this.rbTargetAsset.Margin = new System.Windows.Forms.Padding(6);
            this.rbTargetAsset.Name = "rbTargetAsset";
            this.rbTargetAsset.Size = new System.Drawing.Size(72, 28);
            this.rbTargetAsset.SpecialBorderColor = null;
            this.rbTargetAsset.SpecialFillColor = null;
            this.rbTargetAsset.SpecialTextColor = null;
            this.rbTargetAsset.TabIndex = 6;
            this.rbTargetAsset.Text = "OXS";
            this.rbTargetAsset.CheckedChanged += new System.EventHandler(this.rbGTC_CheckedChanged);
            // 
            // rbPriceAsset
            // 
            this.rbPriceAsset.AutoSize = true;
            this.rbPriceAsset.Checked = true;
            this.rbPriceAsset.Location = new System.Drawing.Point(134, 107);
            this.rbPriceAsset.Margin = new System.Windows.Forms.Padding(6);
            this.rbPriceAsset.Name = "rbPriceAsset";
            this.rbPriceAsset.Size = new System.Drawing.Size(74, 28);
            this.rbPriceAsset.SpecialBorderColor = null;
            this.rbPriceAsset.SpecialFillColor = null;
            this.rbPriceAsset.SpecialTextColor = null;
            this.rbPriceAsset.TabIndex = 7;
            this.rbPriceAsset.TabStop = true;
            this.rbPriceAsset.Text = "OXC";
            this.rbPriceAsset.CheckedChanged += new System.EventHandler(this.rbGTC_CheckedChanged);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBox3.Location = new System.Drawing.Point(130, 165);
            this.textBox3.Margin = new System.Windows.Forms.Padding(6);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(600, 30);
            this.textBox3.TabIndex = 8;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBox2.Location = new System.Drawing.Point(130, 242);
            this.textBox2.Margin = new System.Windows.Forms.Padding(6);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(600, 30);
            this.textBox2.TabIndex = 10;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // cb_accounts
            // 
            this.cb_accounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_accounts.FormattingEnabled = true;
            this.cb_accounts.Location = new System.Drawing.Point(134, 43);
            this.cb_accounts.Name = "cb_accounts";
            this.cb_accounts.Size = new System.Drawing.Size(596, 31);
            this.cb_accounts.SpecialBorderColor = null;
            this.cb_accounts.SpecialFillColor = null;
            this.cb_accounts.SpecialTextColor = null;
            this.cb_accounts.TabIndex = 12;
            this.cb_accounts.SelectedIndexChanged += new System.EventHandler(this.cb_accounts_SelectedIndexChanged);
            // 
            // lb_accounts
            // 
            this.lb_accounts.AutoSize = true;
            this.lb_accounts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_accounts.Location = new System.Drawing.Point(42, 46);
            this.lb_accounts.Name = "lb_accounts";
            this.lb_accounts.Size = new System.Drawing.Size(50, 24);
            this.lb_accounts.TabIndex = 11;
            this.lb_accounts.Text = "账户:";
            // 
            // lb_msg
            // 
            this.lb_msg.AutoSize = true;
            this.lb_msg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_msg.Location = new System.Drawing.Point(42, 311);
            this.lb_msg.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_msg.Name = "lb_msg";
            this.lb_msg.Size = new System.Drawing.Size(50, 24);
            this.lb_msg.TabIndex = 13;
            this.lb_msg.Text = "金额:";
            // 
            // SwapRecharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 435);
            this.Controls.Add(this.lb_msg);
            this.Controls.Add(this.cb_accounts);
            this.Controls.Add(this.lb_accounts);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.rbPriceAsset);
            this.Controls.Add(this.rbTargetAsset);
            this.Controls.Add(this.darkLabel4);
            this.Controls.Add(this.darkLabel2);
            this.Controls.Add(this.darkLabel1);
            this.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.Name = "SwapRecharge";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "转账";
            this.Load += new System.EventHandler(this.PayToDialog_Load);
            this.Controls.SetChildIndex(this.darkLabel1, 0);
            this.Controls.SetChildIndex(this.darkLabel2, 0);
            this.Controls.SetChildIndex(this.darkLabel4, 0);
            this.Controls.SetChildIndex(this.rbTargetAsset, 0);
            this.Controls.SetChildIndex(this.rbPriceAsset, 0);
            this.Controls.SetChildIndex(this.textBox3, 0);
            this.Controls.SetChildIndex(this.textBox2, 0);
            this.Controls.SetChildIndex(this.lb_accounts, 0);
            this.Controls.SetChildIndex(this.cb_accounts, 0);
            this.Controls.SetChildIndex(this.lb_msg, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OX.Wallets.UI.Controls.DarkLabel darkLabel1;
        private OX.Wallets.UI.Controls.DarkLabel darkLabel2;
        private OX.Wallets.UI.Controls.DarkLabel darkLabel4;
        private OX.Wallets.UI.Controls.DarkRadioButton rbTargetAsset;
        private OX.Wallets.UI.Controls.DarkRadioButton rbPriceAsset;
        private OX.Wallets.UI.Controls.DarkTextBox textBox3;
        private OX.Wallets.UI.Controls.DarkTextBox textBox2;
        private Wallets.UI.Controls.DarkComboBox cb_accounts;
        private Wallets.UI.Controls.DarkLabel lb_accounts;
        private Wallets.UI.Controls.DarkLabel lb_msg;
    }
}
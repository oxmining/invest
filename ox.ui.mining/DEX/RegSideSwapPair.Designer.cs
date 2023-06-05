
namespace OX.UI.Swap
{
    partial class RegSideSwapPair
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
            this.tb_targetAssetId = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_targetAssetId = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_targetBalance = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_oxcBalance = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_target_balance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_target_amount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_oxc_amount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_oxc_balance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_assetMsg = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_poolAddress = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_poolAddress = new OX.Wallets.UI.Controls.DarkTextBox();
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
            // tb_targetAssetId
            // 
            this.tb_targetAssetId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_targetAssetId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_targetAssetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_targetAssetId.Location = new System.Drawing.Point(242, 35);
            this.tb_targetAssetId.Margin = new System.Windows.Forms.Padding(6);
            this.tb_targetAssetId.Name = "tb_targetAssetId";
            this.tb_targetAssetId.Size = new System.Drawing.Size(731, 30);
            this.tb_targetAssetId.TabIndex = 48;
            this.tb_targetAssetId.TextChanged += new System.EventHandler(this.tb_targetAssetId_TextChanged);
            // 
            // lb_targetAssetId
            // 
            this.lb_targetAssetId.AutoSize = true;
            this.lb_targetAssetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_targetAssetId.Location = new System.Drawing.Point(30, 37);
            this.lb_targetAssetId.Name = "lb_targetAssetId";
            this.lb_targetAssetId.Size = new System.Drawing.Size(106, 24);
            this.lb_targetAssetId.TabIndex = 47;
            this.lb_targetAssetId.Text = "darkLabel1";
            // 
            // lb_targetBalance
            // 
            this.lb_targetBalance.AutoSize = true;
            this.lb_targetBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_targetBalance.Location = new System.Drawing.Point(30, 149);
            this.lb_targetBalance.Name = "lb_targetBalance";
            this.lb_targetBalance.Size = new System.Drawing.Size(106, 24);
            this.lb_targetBalance.TabIndex = 49;
            this.lb_targetBalance.Text = "darkLabel1";
            // 
            // lb_oxcBalance
            // 
            this.lb_oxcBalance.AutoSize = true;
            this.lb_oxcBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_oxcBalance.Location = new System.Drawing.Point(30, 224);
            this.lb_oxcBalance.Name = "lb_oxcBalance";
            this.lb_oxcBalance.Size = new System.Drawing.Size(106, 24);
            this.lb_oxcBalance.TabIndex = 50;
            this.lb_oxcBalance.Text = "darkLabel1";
            // 
            // tb_target_balance
            // 
            this.tb_target_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_target_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_target_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_target_balance.Location = new System.Drawing.Point(242, 147);
            this.tb_target_balance.Margin = new System.Windows.Forms.Padding(6);
            this.tb_target_balance.Name = "tb_target_balance";
            this.tb_target_balance.ReadOnly = true;
            this.tb_target_balance.Size = new System.Drawing.Size(275, 30);
            this.tb_target_balance.TabIndex = 51;
            // 
            // tb_target_amount
            // 
            this.tb_target_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_target_amount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_target_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_target_amount.Location = new System.Drawing.Point(705, 149);
            this.tb_target_amount.Margin = new System.Windows.Forms.Padding(6);
            this.tb_target_amount.Name = "tb_target_amount";
            this.tb_target_amount.Size = new System.Drawing.Size(268, 30);
            this.tb_target_amount.TabIndex = 52;
            this.tb_target_amount.TextChanged += new System.EventHandler(this.tb_target_amount_TextChanged);
            // 
            // tb_oxc_amount
            // 
            this.tb_oxc_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_oxc_amount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_oxc_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_oxc_amount.Location = new System.Drawing.Point(705, 222);
            this.tb_oxc_amount.Margin = new System.Windows.Forms.Padding(6);
            this.tb_oxc_amount.Name = "tb_oxc_amount";
            this.tb_oxc_amount.Size = new System.Drawing.Size(268, 30);
            this.tb_oxc_amount.TabIndex = 54;
            this.tb_oxc_amount.TextChanged += new System.EventHandler(this.tb_oxc_amount_TextChanged);
            // 
            // tb_oxc_balance
            // 
            this.tb_oxc_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_oxc_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_oxc_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_oxc_balance.Location = new System.Drawing.Point(242, 220);
            this.tb_oxc_balance.Margin = new System.Windows.Forms.Padding(6);
            this.tb_oxc_balance.Name = "tb_oxc_balance";
            this.tb_oxc_balance.ReadOnly = true;
            this.tb_oxc_balance.Size = new System.Drawing.Size(275, 30);
            this.tb_oxc_balance.TabIndex = 53;
            // 
            // lb_assetMsg
            // 
            this.lb_assetMsg.AutoSize = true;
            this.lb_assetMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_assetMsg.Location = new System.Drawing.Point(246, 92);
            this.lb_assetMsg.Name = "lb_assetMsg";
            this.lb_assetMsg.Size = new System.Drawing.Size(0, 24);
            this.lb_assetMsg.TabIndex = 55;
            // 
            // lb_poolAddress
            // 
            this.lb_poolAddress.AutoSize = true;
            this.lb_poolAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_poolAddress.Location = new System.Drawing.Point(30, 283);
            this.lb_poolAddress.Name = "lb_poolAddress";
            this.lb_poolAddress.Size = new System.Drawing.Size(106, 24);
            this.lb_poolAddress.TabIndex = 56;
            this.lb_poolAddress.Text = "darkLabel1";
            // 
            // tb_poolAddress
            // 
            this.tb_poolAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_poolAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_poolAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_poolAddress.Location = new System.Drawing.Point(242, 277);
            this.tb_poolAddress.Margin = new System.Windows.Forms.Padding(6);
            this.tb_poolAddress.Name = "tb_poolAddress";
            this.tb_poolAddress.ReadOnly = true;
            this.tb_poolAddress.Size = new System.Drawing.Size(731, 30);
            this.tb_poolAddress.TabIndex = 57;
            // 
            // RegSideSwapPair
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 418);
            this.Controls.Add(this.tb_poolAddress);
            this.Controls.Add(this.lb_poolAddress);
            this.Controls.Add(this.lb_assetMsg);
            this.Controls.Add(this.tb_oxc_amount);
            this.Controls.Add(this.tb_oxc_balance);
            this.Controls.Add(this.tb_target_amount);
            this.Controls.Add(this.tb_target_balance);
            this.Controls.Add(this.lb_oxcBalance);
            this.Controls.Add(this.lb_targetBalance);
            this.Controls.Add(this.tb_targetAssetId);
            this.Controls.Add(this.lb_targetAssetId);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegSideSwapPair";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegMinerForm";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.lb_targetAssetId, 0);
            this.Controls.SetChildIndex(this.tb_targetAssetId, 0);
            this.Controls.SetChildIndex(this.lb_targetBalance, 0);
            this.Controls.SetChildIndex(this.lb_oxcBalance, 0);
            this.Controls.SetChildIndex(this.tb_target_balance, 0);
            this.Controls.SetChildIndex(this.tb_target_amount, 0);
            this.Controls.SetChildIndex(this.tb_oxc_balance, 0);
            this.Controls.SetChildIndex(this.tb_oxc_amount, 0);
            this.Controls.SetChildIndex(this.lb_assetMsg, 0);
            this.Controls.SetChildIndex(this.lb_poolAddress, 0);
            this.Controls.SetChildIndex(this.tb_poolAddress, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Wallets.UI.Controls.DarkTextBox tb_targetAssetId;
        private Wallets.UI.Controls.DarkLabel lb_targetAssetId;
        private Wallets.UI.Controls.DarkLabel lb_targetBalance;
        private Wallets.UI.Controls.DarkLabel lb_oxcBalance;
        private Wallets.UI.Controls.DarkTextBox tb_target_balance;
        private Wallets.UI.Controls.DarkTextBox tb_target_amount;
        private Wallets.UI.Controls.DarkTextBox tb_oxc_amount;
        private Wallets.UI.Controls.DarkTextBox tb_oxc_balance;
        private Wallets.UI.Controls.DarkLabel lb_assetMsg;
        private Wallets.UI.Controls.DarkLabel lb_poolAddress;
        private Wallets.UI.Controls.DarkTextBox tb_poolAddress;
    }
}
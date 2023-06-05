
namespace OX.UI.LAM
{
    partial class IssueMutualLockMiningAssetcs
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
            this.tb_AssetId = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_AssetId = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_AssetName = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_cap = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_AssetName = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_cap = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_issued = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_issued = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_minAmount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_minAmount = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_maxAmount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_maxAmount = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_airdropratio = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_airdropratio = new OX.Wallets.UI.Controls.DarkLabel();
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
            // tb_AssetId
            // 
            this.tb_AssetId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_AssetId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_AssetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_AssetId.Location = new System.Drawing.Point(242, 29);
            this.tb_AssetId.Margin = new System.Windows.Forms.Padding(6);
            this.tb_AssetId.Name = "tb_AssetId";
            this.tb_AssetId.Size = new System.Drawing.Size(731, 30);
            this.tb_AssetId.TabIndex = 48;
            this.tb_AssetId.TextChanged += new System.EventHandler(this.tb_targetAssetId_TextChanged);
            // 
            // lb_AssetId
            // 
            this.lb_AssetId.AutoSize = true;
            this.lb_AssetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_AssetId.Location = new System.Drawing.Point(30, 31);
            this.lb_AssetId.Name = "lb_AssetId";
            this.lb_AssetId.Size = new System.Drawing.Size(106, 24);
            this.lb_AssetId.TabIndex = 47;
            this.lb_AssetId.Text = "darkLabel1";
            // 
            // lb_AssetName
            // 
            this.lb_AssetName.AutoSize = true;
            this.lb_AssetName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_AssetName.Location = new System.Drawing.Point(30, 92);
            this.lb_AssetName.Name = "lb_AssetName";
            this.lb_AssetName.Size = new System.Drawing.Size(106, 24);
            this.lb_AssetName.TabIndex = 49;
            this.lb_AssetName.Text = "darkLabel1";
            // 
            // lb_cap
            // 
            this.lb_cap.AutoSize = true;
            this.lb_cap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_cap.Location = new System.Drawing.Point(30, 151);
            this.lb_cap.Name = "lb_cap";
            this.lb_cap.Size = new System.Drawing.Size(106, 24);
            this.lb_cap.TabIndex = 50;
            this.lb_cap.Text = "darkLabel1";
            // 
            // tb_AssetName
            // 
            this.tb_AssetName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_AssetName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_AssetName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_AssetName.Location = new System.Drawing.Point(242, 90);
            this.tb_AssetName.Margin = new System.Windows.Forms.Padding(6);
            this.tb_AssetName.Name = "tb_AssetName";
            this.tb_AssetName.ReadOnly = true;
            this.tb_AssetName.Size = new System.Drawing.Size(391, 30);
            this.tb_AssetName.TabIndex = 51;
            // 
            // tb_cap
            // 
            this.tb_cap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_cap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_cap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_cap.Location = new System.Drawing.Point(242, 147);
            this.tb_cap.Margin = new System.Windows.Forms.Padding(6);
            this.tb_cap.Name = "tb_cap";
            this.tb_cap.ReadOnly = true;
            this.tb_cap.Size = new System.Drawing.Size(391, 30);
            this.tb_cap.TabIndex = 53;
            // 
            // lb_issued
            // 
            this.lb_issued.AutoSize = true;
            this.lb_issued.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_issued.Location = new System.Drawing.Point(30, 212);
            this.lb_issued.Name = "lb_issued";
            this.lb_issued.Size = new System.Drawing.Size(106, 24);
            this.lb_issued.TabIndex = 58;
            this.lb_issued.Text = "darkLabel1";
            // 
            // tb_issued
            // 
            this.tb_issued.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_issued.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_issued.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_issued.Location = new System.Drawing.Point(242, 206);
            this.tb_issued.Margin = new System.Windows.Forms.Padding(6);
            this.tb_issued.Name = "tb_issued";
            this.tb_issued.ReadOnly = true;
            this.tb_issued.Size = new System.Drawing.Size(391, 30);
            this.tb_issued.TabIndex = 59;
            // 
            // tb_minAmount
            // 
            this.tb_minAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_minAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_minAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_minAmount.Location = new System.Drawing.Point(242, 268);
            this.tb_minAmount.Margin = new System.Windows.Forms.Padding(6);
            this.tb_minAmount.Name = "tb_minAmount";
            this.tb_minAmount.Size = new System.Drawing.Size(391, 30);
            this.tb_minAmount.TabIndex = 62;
            this.tb_minAmount.Text = "1000";
            // 
            // lb_minAmount
            // 
            this.lb_minAmount.AutoSize = true;
            this.lb_minAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_minAmount.Location = new System.Drawing.Point(30, 274);
            this.lb_minAmount.Name = "lb_minAmount";
            this.lb_minAmount.Size = new System.Drawing.Size(106, 24);
            this.lb_minAmount.TabIndex = 61;
            this.lb_minAmount.Text = "darkLabel1";
            // 
            // tb_maxAmount
            // 
            this.tb_maxAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_maxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_maxAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_maxAmount.Location = new System.Drawing.Point(242, 324);
            this.tb_maxAmount.Margin = new System.Windows.Forms.Padding(6);
            this.tb_maxAmount.Name = "tb_maxAmount";
            this.tb_maxAmount.Size = new System.Drawing.Size(391, 30);
            this.tb_maxAmount.TabIndex = 64;
            this.tb_maxAmount.Text = "10000000";
            // 
            // lb_maxAmount
            // 
            this.lb_maxAmount.AutoSize = true;
            this.lb_maxAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_maxAmount.Location = new System.Drawing.Point(30, 330);
            this.lb_maxAmount.Name = "lb_maxAmount";
            this.lb_maxAmount.Size = new System.Drawing.Size(106, 24);
            this.lb_maxAmount.TabIndex = 63;
            this.lb_maxAmount.Text = "darkLabel1";
            // 
            // tb_airdropratio
            // 
            this.tb_airdropratio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_airdropratio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_airdropratio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_airdropratio.Location = new System.Drawing.Point(242, 383);
            this.tb_airdropratio.Margin = new System.Windows.Forms.Padding(6);
            this.tb_airdropratio.Name = "tb_airdropratio";
            this.tb_airdropratio.Size = new System.Drawing.Size(391, 30);
            this.tb_airdropratio.TabIndex = 66;
            this.tb_airdropratio.Text = "1";
            // 
            // lb_airdropratio
            // 
            this.lb_airdropratio.AutoSize = true;
            this.lb_airdropratio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_airdropratio.Location = new System.Drawing.Point(30, 389);
            this.lb_airdropratio.Name = "lb_airdropratio";
            this.lb_airdropratio.Size = new System.Drawing.Size(106, 24);
            this.lb_airdropratio.TabIndex = 65;
            this.lb_airdropratio.Text = "darkLabel1";
            // 
            // IssueMutualLockMiningAssetcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 532);
            this.Controls.Add(this.tb_airdropratio);
            this.Controls.Add(this.lb_airdropratio);
            this.Controls.Add(this.tb_maxAmount);
            this.Controls.Add(this.lb_maxAmount);
            this.Controls.Add(this.tb_minAmount);
            this.Controls.Add(this.lb_minAmount);
            this.Controls.Add(this.tb_issued);
            this.Controls.Add(this.lb_issued);
            this.Controls.Add(this.tb_cap);
            this.Controls.Add(this.tb_AssetName);
            this.Controls.Add(this.lb_cap);
            this.Controls.Add(this.lb_AssetName);
            this.Controls.Add(this.tb_AssetId);
            this.Controls.Add(this.lb_AssetId);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IssueMutualLockMiningAssetcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegMinerForm";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.lb_AssetId, 0);
            this.Controls.SetChildIndex(this.tb_AssetId, 0);
            this.Controls.SetChildIndex(this.lb_AssetName, 0);
            this.Controls.SetChildIndex(this.lb_cap, 0);
            this.Controls.SetChildIndex(this.tb_AssetName, 0);
            this.Controls.SetChildIndex(this.tb_cap, 0);
            this.Controls.SetChildIndex(this.lb_issued, 0);
            this.Controls.SetChildIndex(this.tb_issued, 0);
            this.Controls.SetChildIndex(this.lb_minAmount, 0);
            this.Controls.SetChildIndex(this.tb_minAmount, 0);
            this.Controls.SetChildIndex(this.lb_maxAmount, 0);
            this.Controls.SetChildIndex(this.tb_maxAmount, 0);
            this.Controls.SetChildIndex(this.lb_airdropratio, 0);
            this.Controls.SetChildIndex(this.tb_airdropratio, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Wallets.UI.Controls.DarkTextBox tb_AssetId;
        private Wallets.UI.Controls.DarkLabel lb_AssetId;
        private Wallets.UI.Controls.DarkLabel lb_AssetName;
        private Wallets.UI.Controls.DarkLabel lb_cap;
        private Wallets.UI.Controls.DarkTextBox tb_AssetName;
        private Wallets.UI.Controls.DarkTextBox tb_cap;
        private Wallets.UI.Controls.DarkLabel lb_issued;
        private Wallets.UI.Controls.DarkTextBox tb_issued;
        private Wallets.UI.Controls.DarkTextBox tb_minAmount;
        private Wallets.UI.Controls.DarkLabel lb_minAmount;
        private Wallets.UI.Controls.DarkTextBox tb_maxAmount;
        private Wallets.UI.Controls.DarkLabel lb_maxAmount;
        private Wallets.UI.Controls.DarkTextBox tb_airdropratio;
        private Wallets.UI.Controls.DarkLabel lb_airdropratio;
    }
}
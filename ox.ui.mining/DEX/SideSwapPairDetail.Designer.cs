
namespace OX.UI.Swap
{
    partial class SideSwapPairDetail
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
            this.groupBox2 = new OX.Wallets.UI.Controls.DarkGroupBox();
            this.tb_TargetAssetBalance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_TargetAsset = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_PricingAsset = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_PricingAssetBalance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.bt_copy = new OX.Wallets.UI.Controls.DarkButton();
            this.tb_poolAddress = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_poolAddress = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_assetId = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_assetId = new OX.Wallets.UI.Controls.DarkLabel();
            this.textBox1 = new OX.Wallets.UI.Controls.DarkTextBox();
            this.label2 = new OX.Wallets.UI.Controls.DarkLabel();
            this.label3 = new OX.Wallets.UI.Controls.DarkLabel();
            this.textBox3 = new OX.Wallets.UI.Controls.DarkTextBox();
            this.textBox2 = new OX.Wallets.UI.Controls.DarkTextBox();
            this.label4 = new OX.Wallets.UI.Controls.DarkLabel();
            this.label5 = new OX.Wallets.UI.Controls.DarkLabel();
            this.textBox4 = new OX.Wallets.UI.Controls.DarkTextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(166, 18);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.groupBox2.Controls.Add(this.tb_TargetAssetBalance);
            this.groupBox2.Controls.Add(this.lb_TargetAsset);
            this.groupBox2.Controls.Add(this.lb_PricingAsset);
            this.groupBox2.Controls.Add(this.tb_PricingAssetBalance);
            this.groupBox2.Controls.Add(this.bt_copy);
            this.groupBox2.Controls.Add(this.tb_poolAddress);
            this.groupBox2.Controls.Add(this.lb_poolAddress);
            this.groupBox2.Controls.Add(this.tb_assetId);
            this.groupBox2.Controls.Add(this.lb_assetId);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Location = new System.Drawing.Point(35, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Size = new System.Drawing.Size(793, 352);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Asset Details";
            // 
            // tb_TargetAssetBalance
            // 
            this.tb_TargetAssetBalance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_TargetAssetBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_TargetAssetBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_TargetAssetBalance.Location = new System.Drawing.Point(107, 287);
            this.tb_TargetAssetBalance.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tb_TargetAssetBalance.Name = "tb_TargetAssetBalance";
            this.tb_TargetAssetBalance.ReadOnly = true;
            this.tb_TargetAssetBalance.Size = new System.Drawing.Size(220, 30);
            this.tb_TargetAssetBalance.TabIndex = 20;
            // 
            // lb_TargetAsset
            // 
            this.lb_TargetAsset.AutoSize = true;
            this.lb_TargetAsset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_TargetAsset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_TargetAsset.Location = new System.Drawing.Point(9, 291);
            this.lb_TargetAsset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_TargetAsset.Name = "lb_TargetAsset";
            this.lb_TargetAsset.Size = new System.Drawing.Size(48, 24);
            this.lb_TargetAsset.TabIndex = 19;
            this.lb_TargetAsset.Text = "Cap:";
            // 
            // lb_PricingAsset
            // 
            this.lb_PricingAsset.AutoSize = true;
            this.lb_PricingAsset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_PricingAsset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_PricingAsset.Location = new System.Drawing.Point(450, 291);
            this.lb_PricingAsset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_PricingAsset.Name = "lb_PricingAsset";
            this.lb_PricingAsset.Size = new System.Drawing.Size(68, 24);
            this.lb_PricingAsset.TabIndex = 21;
            this.lb_PricingAsset.Text = "Issued:";
            // 
            // tb_PricingAssetBalance
            // 
            this.tb_PricingAssetBalance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_PricingAssetBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_PricingAssetBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_PricingAssetBalance.Location = new System.Drawing.Point(550, 287);
            this.tb_PricingAssetBalance.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tb_PricingAssetBalance.Name = "tb_PricingAssetBalance";
            this.tb_PricingAssetBalance.ReadOnly = true;
            this.tb_PricingAssetBalance.Size = new System.Drawing.Size(222, 30);
            this.tb_PricingAssetBalance.TabIndex = 22;
            // 
            // bt_copy
            // 
            this.bt_copy.Location = new System.Drawing.Point(665, 225);
            this.bt_copy.Name = "bt_copy";
            this.bt_copy.Padding = new System.Windows.Forms.Padding(5);
            this.bt_copy.Size = new System.Drawing.Size(111, 34);
            this.bt_copy.SpecialBorderColor = null;
            this.bt_copy.SpecialFillColor = null;
            this.bt_copy.SpecialTextColor = null;
            this.bt_copy.TabIndex = 18;
            this.bt_copy.Text = "darkButton1";
            this.bt_copy.Click += new System.EventHandler(this.bt_copy_Click);
            // 
            // tb_poolAddress
            // 
            this.tb_poolAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_poolAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_poolAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_poolAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_poolAddress.Location = new System.Drawing.Point(107, 228);
            this.tb_poolAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tb_poolAddress.Name = "tb_poolAddress";
            this.tb_poolAddress.ReadOnly = true;
            this.tb_poolAddress.Size = new System.Drawing.Size(523, 30);
            this.tb_poolAddress.TabIndex = 13;
            // 
            // lb_poolAddress
            // 
            this.lb_poolAddress.AutoSize = true;
            this.lb_poolAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_poolAddress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_poolAddress.Location = new System.Drawing.Point(9, 230);
            this.lb_poolAddress.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_poolAddress.Name = "lb_poolAddress";
            this.lb_poolAddress.Size = new System.Drawing.Size(72, 24);
            this.lb_poolAddress.TabIndex = 12;
            this.lb_poolAddress.Text = "Admin:";
            // 
            // tb_assetId
            // 
            this.tb_assetId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_assetId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_assetId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_assetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_assetId.Location = new System.Drawing.Point(109, 53);
            this.tb_assetId.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tb_assetId.Name = "tb_assetId";
            this.tb_assetId.ReadOnly = true;
            this.tb_assetId.Size = new System.Drawing.Size(667, 30);
            this.tb_assetId.TabIndex = 11;
            // 
            // lb_assetId
            // 
            this.lb_assetId.AutoSize = true;
            this.lb_assetId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_assetId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_assetId.Location = new System.Drawing.Point(9, 57);
            this.lb_assetId.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_assetId.Name = "lb_assetId";
            this.lb_assetId.Size = new System.Drawing.Size(71, 24);
            this.lb_assetId.TabIndex = 10;
            this.lb_assetId.Text = "Owner:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBox1.Location = new System.Drawing.Point(109, 94);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(667, 30);
            this.textBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(9, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Owner:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(11, 139);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "Admin:";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBox3.Location = new System.Drawing.Point(109, 176);
            this.textBox3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(220, 30);
            this.textBox3.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBox2.Location = new System.Drawing.Point(109, 135);
            this.textBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(667, 30);
            this.textBox2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(11, 180);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Cap:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(452, 180);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "Issued:";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBox4.Location = new System.Drawing.Point(552, 176);
            this.textBox4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(222, 30);
            this.textBox4.TabIndex = 9;
            // 
            // SideSwapPairDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 479);
            this.Controls.Add(this.groupBox2);
            this.DialogButtons = OX.Wallets.UI.Forms.DarkDialogButton.OkCancel;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SideSwapPairDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegMinerForm";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Wallets.UI.Controls.DarkGroupBox groupBox2;
        private Wallets.UI.Controls.DarkTextBox textBox1;
        private Wallets.UI.Controls.DarkLabel label2;
        private Wallets.UI.Controls.DarkLabel label3;
        private Wallets.UI.Controls.DarkTextBox textBox3;
        private Wallets.UI.Controls.DarkTextBox textBox2;
        private Wallets.UI.Controls.DarkLabel label4;
        private Wallets.UI.Controls.DarkLabel label5;
        private Wallets.UI.Controls.DarkTextBox textBox4;
        private Wallets.UI.Controls.DarkTextBox tb_assetId;
        private Wallets.UI.Controls.DarkLabel lb_assetId;
        private Wallets.UI.Controls.DarkTextBox tb_poolAddress;
        private Wallets.UI.Controls.DarkLabel lb_poolAddress;
        private Wallets.UI.Controls.DarkButton bt_copy;
        private Wallets.UI.Controls.DarkTextBox tb_TargetAssetBalance;
        private Wallets.UI.Controls.DarkLabel lb_TargetAsset;
        private Wallets.UI.Controls.DarkLabel lb_PricingAsset;
        private Wallets.UI.Controls.DarkTextBox tb_PricingAssetBalance;
    }
}
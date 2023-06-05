
namespace OX.UI.LAM
{
    partial class ViewMutualLockSeed
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
            this.lb_accounts = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_seedAddress = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_seedAddress = new OX.Wallets.UI.Controls.DarkTextBox();
            this.bt_copy = new OX.Wallets.UI.Controls.DarkButton();
            this.tb_address = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_genesisSeed = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_genesisSeed = new OX.Wallets.UI.Controls.DarkTextBox();
            this.bt_copyGenesisSeed = new OX.Wallets.UI.Controls.DarkButton();
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
            // lb_accounts
            // 
            this.lb_accounts.AutoSize = true;
            this.lb_accounts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_accounts.Location = new System.Drawing.Point(30, 120);
            this.lb_accounts.Name = "lb_accounts";
            this.lb_accounts.Size = new System.Drawing.Size(106, 24);
            this.lb_accounts.TabIndex = 6;
            this.lb_accounts.Text = "darkLabel1";
            // 
            // lb_seedAddress
            // 
            this.lb_seedAddress.AutoSize = true;
            this.lb_seedAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_seedAddress.Location = new System.Drawing.Point(30, 188);
            this.lb_seedAddress.Name = "lb_seedAddress";
            this.lb_seedAddress.Size = new System.Drawing.Size(106, 24);
            this.lb_seedAddress.TabIndex = 8;
            this.lb_seedAddress.Text = "darkLabel1";
            // 
            // tb_seedAddress
            // 
            this.tb_seedAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_seedAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_seedAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_seedAddress.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.tb_seedAddress.Location = new System.Drawing.Point(232, 182);
            this.tb_seedAddress.MaxLength = 20;
            this.tb_seedAddress.Name = "tb_seedAddress";
            this.tb_seedAddress.ReadOnly = true;
            this.tb_seedAddress.ShortcutsEnabled = false;
            this.tb_seedAddress.Size = new System.Drawing.Size(526, 30);
            this.tb_seedAddress.TabIndex = 51;
            // 
            // bt_copy
            // 
            this.bt_copy.Location = new System.Drawing.Point(775, 179);
            this.bt_copy.Name = "bt_copy";
            this.bt_copy.Padding = new System.Windows.Forms.Padding(5);
            this.bt_copy.Size = new System.Drawing.Size(137, 34);
            this.bt_copy.SpecialBorderColor = null;
            this.bt_copy.SpecialFillColor = null;
            this.bt_copy.SpecialTextColor = null;
            this.bt_copy.TabIndex = 55;
            this.bt_copy.Text = "darkButton1";
            this.bt_copy.Click += new System.EventHandler(this.bt_copy_Click);
            // 
            // tb_address
            // 
            this.tb_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_address.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_address.Location = new System.Drawing.Point(232, 118);
            this.tb_address.Name = "tb_address";
            this.tb_address.Size = new System.Drawing.Size(526, 30);
            this.tb_address.TabIndex = 56;
            this.tb_address.TextChanged += new System.EventHandler(this.tb_address_TextChanged);
            // 
            // lb_genesisSeed
            // 
            this.lb_genesisSeed.AutoSize = true;
            this.lb_genesisSeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_genesisSeed.Location = new System.Drawing.Point(30, 32);
            this.lb_genesisSeed.Name = "lb_genesisSeed";
            this.lb_genesisSeed.Size = new System.Drawing.Size(106, 24);
            this.lb_genesisSeed.TabIndex = 57;
            this.lb_genesisSeed.Text = "darkLabel1";
            // 
            // tb_genesisSeed
            // 
            this.tb_genesisSeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_genesisSeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_genesisSeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_genesisSeed.Location = new System.Drawing.Point(232, 30);
            this.tb_genesisSeed.Name = "tb_genesisSeed";
            this.tb_genesisSeed.ReadOnly = true;
            this.tb_genesisSeed.Size = new System.Drawing.Size(526, 30);
            this.tb_genesisSeed.TabIndex = 58;
            // 
            // bt_copyGenesisSeed
            // 
            this.bt_copyGenesisSeed.Location = new System.Drawing.Point(775, 27);
            this.bt_copyGenesisSeed.Name = "bt_copyGenesisSeed";
            this.bt_copyGenesisSeed.Padding = new System.Windows.Forms.Padding(5);
            this.bt_copyGenesisSeed.Size = new System.Drawing.Size(137, 34);
            this.bt_copyGenesisSeed.SpecialBorderColor = null;
            this.bt_copyGenesisSeed.SpecialFillColor = null;
            this.bt_copyGenesisSeed.SpecialTextColor = null;
            this.bt_copyGenesisSeed.TabIndex = 59;
            this.bt_copyGenesisSeed.Text = "darkButton1";
            this.bt_copyGenesisSeed.Click += new System.EventHandler(this.bt_copyGenesisSeed_Click);
            // 
            // ViewMutualLockSeed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 327);
            this.Controls.Add(this.bt_copyGenesisSeed);
            this.Controls.Add(this.tb_genesisSeed);
            this.Controls.Add(this.lb_genesisSeed);
            this.Controls.Add(this.tb_address);
            this.Controls.Add(this.bt_copy);
            this.Controls.Add(this.tb_seedAddress);
            this.Controls.Add(this.lb_seedAddress);
            this.Controls.Add(this.lb_accounts);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewMutualLockSeed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ViewMutualLockSeed";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.lb_accounts, 0);
            this.Controls.SetChildIndex(this.lb_seedAddress, 0);
            this.Controls.SetChildIndex(this.tb_seedAddress, 0);
            this.Controls.SetChildIndex(this.bt_copy, 0);
            this.Controls.SetChildIndex(this.tb_address, 0);
            this.Controls.SetChildIndex(this.lb_genesisSeed, 0);
            this.Controls.SetChildIndex(this.tb_genesisSeed, 0);
            this.Controls.SetChildIndex(this.bt_copyGenesisSeed, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Wallets.UI.Controls.DarkLabel lb_accounts;
        private Wallets.UI.Controls.DarkLabel lb_seedAddress;
        private Wallets.UI.Controls.DarkTextBox tb_seedAddress;
        private Wallets.UI.Controls.DarkButton bt_copy;
        private Wallets.UI.Controls.DarkTextBox tb_address;
        private Wallets.UI.Controls.DarkLabel lb_genesisSeed;
        private Wallets.UI.Controls.DarkTextBox tb_genesisSeed;
        private Wallets.UI.Controls.DarkButton bt_copyGenesisSeed;
    }
}
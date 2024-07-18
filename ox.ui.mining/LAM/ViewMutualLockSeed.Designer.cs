
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
            lb_accounts = new Wallets.UI.Controls.DarkLabel();
            lb_seedAddress = new Wallets.UI.Controls.DarkLabel();
            tb_seedAddress = new Wallets.UI.Controls.DarkTextBox();
            bt_copy = new Wallets.UI.Controls.DarkButton();
            tb_address = new Wallets.UI.Controls.DarkTextBox();
            lb_genesisSeed = new Wallets.UI.Controls.DarkLabel();
            tb_genesisSeed = new Wallets.UI.Controls.DarkTextBox();
            bt_copyGenesisSeed = new Wallets.UI.Controls.DarkButton();
            lb_msg = new Wallets.UI.Controls.DarkLabel();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(18, 18);
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
            // lb_accounts
            // 
            lb_accounts.AutoSize = true;
            lb_accounts.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_accounts.Location = new System.Drawing.Point(30, 120);
            lb_accounts.Name = "lb_accounts";
            lb_accounts.Size = new System.Drawing.Size(106, 24);
            lb_accounts.TabIndex = 6;
            lb_accounts.Text = "darkLabel1";
            // 
            // lb_seedAddress
            // 
            lb_seedAddress.AutoSize = true;
            lb_seedAddress.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_seedAddress.Location = new System.Drawing.Point(30, 188);
            lb_seedAddress.Name = "lb_seedAddress";
            lb_seedAddress.Size = new System.Drawing.Size(106, 24);
            lb_seedAddress.TabIndex = 8;
            lb_seedAddress.Text = "darkLabel1";
            // 
            // tb_seedAddress
            // 
            tb_seedAddress.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_seedAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_seedAddress.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_seedAddress.ImeMode = System.Windows.Forms.ImeMode.Disable;
            tb_seedAddress.Location = new System.Drawing.Point(232, 182);
            tb_seedAddress.MaxLength = 20;
            tb_seedAddress.Name = "tb_seedAddress";
            tb_seedAddress.ReadOnly = true;
            tb_seedAddress.ShortcutsEnabled = false;
            tb_seedAddress.Size = new System.Drawing.Size(526, 30);
            tb_seedAddress.TabIndex = 51;
            // 
            // bt_copy
            // 
            bt_copy.Location = new System.Drawing.Point(775, 179);
            bt_copy.Name = "bt_copy";
            bt_copy.Padding = new System.Windows.Forms.Padding(5);
            bt_copy.Size = new System.Drawing.Size(137, 34);
            bt_copy.SpecialBorderColor = null;
            bt_copy.SpecialFillColor = null;
            bt_copy.SpecialTextColor = null;
            bt_copy.TabIndex = 55;
            bt_copy.Text = "darkButton1";
            bt_copy.Click += bt_copy_Click;
            // 
            // tb_address
            // 
            tb_address.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_address.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_address.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_address.Location = new System.Drawing.Point(232, 118);
            tb_address.Name = "tb_address";
            tb_address.Size = new System.Drawing.Size(526, 30);
            tb_address.TabIndex = 56;
            tb_address.TextChanged += tb_address_TextChanged;
            // 
            // lb_genesisSeed
            // 
            lb_genesisSeed.AutoSize = true;
            lb_genesisSeed.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_genesisSeed.Location = new System.Drawing.Point(30, 32);
            lb_genesisSeed.Name = "lb_genesisSeed";
            lb_genesisSeed.Size = new System.Drawing.Size(106, 24);
            lb_genesisSeed.TabIndex = 57;
            lb_genesisSeed.Text = "darkLabel1";
            // 
            // tb_genesisSeed
            // 
            tb_genesisSeed.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_genesisSeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_genesisSeed.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_genesisSeed.Location = new System.Drawing.Point(232, 30);
            tb_genesisSeed.Name = "tb_genesisSeed";
            tb_genesisSeed.ReadOnly = true;
            tb_genesisSeed.Size = new System.Drawing.Size(526, 30);
            tb_genesisSeed.TabIndex = 58;
            // 
            // bt_copyGenesisSeed
            // 
            bt_copyGenesisSeed.Location = new System.Drawing.Point(775, 27);
            bt_copyGenesisSeed.Name = "bt_copyGenesisSeed";
            bt_copyGenesisSeed.Padding = new System.Windows.Forms.Padding(5);
            bt_copyGenesisSeed.Size = new System.Drawing.Size(137, 34);
            bt_copyGenesisSeed.SpecialBorderColor = null;
            bt_copyGenesisSeed.SpecialFillColor = null;
            bt_copyGenesisSeed.SpecialTextColor = null;
            bt_copyGenesisSeed.TabIndex = 59;
            bt_copyGenesisSeed.Text = "darkButton1";
            bt_copyGenesisSeed.Click += bt_copyGenesisSeed_Click;
            // 
            // lb_msg
            // 
            lb_msg.AutoSize = true;
            lb_msg.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_msg.Location = new System.Drawing.Point(232, 235);
            lb_msg.Name = "lb_msg";
            lb_msg.Size = new System.Drawing.Size(0, 24);
            lb_msg.TabIndex = 60;
            // 
            // ViewMutualLockSeed
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(930, 396);
            Controls.Add(lb_msg);
            Controls.Add(bt_copyGenesisSeed);
            Controls.Add(tb_genesisSeed);
            Controls.Add(lb_genesisSeed);
            Controls.Add(tb_address);
            Controls.Add(bt_copy);
            Controls.Add(tb_seedAddress);
            Controls.Add(lb_seedAddress);
            Controls.Add(lb_accounts);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ViewMutualLockSeed";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "ViewMutualLockSeed";
            Load += RegMinerForm_Load;
            Controls.SetChildIndex(lb_accounts, 0);
            Controls.SetChildIndex(lb_seedAddress, 0);
            Controls.SetChildIndex(tb_seedAddress, 0);
            Controls.SetChildIndex(bt_copy, 0);
            Controls.SetChildIndex(tb_address, 0);
            Controls.SetChildIndex(lb_genesisSeed, 0);
            Controls.SetChildIndex(tb_genesisSeed, 0);
            Controls.SetChildIndex(bt_copyGenesisSeed, 0);
            Controls.SetChildIndex(lb_msg, 0);
            ResumeLayout(false);
            PerformLayout();
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
        private Wallets.UI.Controls.DarkLabel lb_msg;
    }
}
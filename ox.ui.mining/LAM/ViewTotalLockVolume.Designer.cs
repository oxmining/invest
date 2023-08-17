
namespace OX.UI.LAM
{
    partial class ViewTotalLockVolume
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
            lb_holder = new Wallets.UI.Controls.DarkLabel();
            lb_volume = new Wallets.UI.Controls.DarkLabel();
            tb_volume = new Wallets.UI.Controls.DarkTextBox();
            bt_query = new Wallets.UI.Controls.DarkButton();
            tb_holder = new Wallets.UI.Controls.DarkTextBox();
            lb_assetId = new Wallets.UI.Controls.DarkLabel();
            tb_assetId = new Wallets.UI.Controls.DarkTextBox();
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
            // lb_holder
            // 
            lb_holder.AutoSize = true;
            lb_holder.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_holder.Location = new System.Drawing.Point(30, 91);
            lb_holder.Name = "lb_holder";
            lb_holder.Size = new System.Drawing.Size(106, 24);
            lb_holder.TabIndex = 6;
            lb_holder.Text = "darkLabel1";
            // 
            // lb_volume
            // 
            lb_volume.AutoSize = true;
            lb_volume.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_volume.Location = new System.Drawing.Point(30, 160);
            lb_volume.Name = "lb_volume";
            lb_volume.Size = new System.Drawing.Size(106, 24);
            lb_volume.TabIndex = 8;
            lb_volume.Text = "darkLabel1";
            // 
            // tb_volume
            // 
            tb_volume.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_volume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_volume.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_volume.ImeMode = System.Windows.Forms.ImeMode.Disable;
            tb_volume.Location = new System.Drawing.Point(232, 154);
            tb_volume.MaxLength = 20;
            tb_volume.Name = "tb_volume";
            tb_volume.ReadOnly = true;
            tb_volume.ShortcutsEnabled = false;
            tb_volume.Size = new System.Drawing.Size(512, 30);
            tb_volume.TabIndex = 51;
            tb_volume.Text = "0";
            // 
            // bt_query
            // 
            bt_query.Location = new System.Drawing.Point(775, 150);
            bt_query.Name = "bt_query";
            bt_query.Padding = new System.Windows.Forms.Padding(5);
            bt_query.Size = new System.Drawing.Size(137, 34);
            bt_query.SpecialBorderColor = null;
            bt_query.SpecialFillColor = null;
            bt_query.SpecialTextColor = null;
            bt_query.TabIndex = 55;
            bt_query.Text = "darkButton1";
            bt_query.Click += bt_copy_Click;
            // 
            // tb_holder
            // 
            tb_holder.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_holder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_holder.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_holder.Location = new System.Drawing.Point(232, 89);
            tb_holder.Name = "tb_holder";
            tb_holder.Size = new System.Drawing.Size(680, 30);
            tb_holder.TabIndex = 56;
            // 
            // lb_assetId
            // 
            lb_assetId.AutoSize = true;
            lb_assetId.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_assetId.Location = new System.Drawing.Point(30, 32);
            lb_assetId.Name = "lb_assetId";
            lb_assetId.Size = new System.Drawing.Size(106, 24);
            lb_assetId.TabIndex = 57;
            lb_assetId.Text = "darkLabel1";
            // 
            // tb_assetId
            // 
            tb_assetId.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_assetId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_assetId.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_assetId.Location = new System.Drawing.Point(232, 30);
            tb_assetId.Name = "tb_assetId";
            tb_assetId.Size = new System.Drawing.Size(680, 30);
            tb_assetId.TabIndex = 58;
            // 
            // ViewTotalLockVolume
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(930, 292);
            Controls.Add(tb_assetId);
            Controls.Add(lb_assetId);
            Controls.Add(tb_holder);
            Controls.Add(bt_query);
            Controls.Add(tb_volume);
            Controls.Add(lb_volume);
            Controls.Add(lb_holder);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ViewTotalLockVolume";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "ViewMutualLockSeed";
            Load += RegMinerForm_Load;
            Controls.SetChildIndex(lb_holder, 0);
            Controls.SetChildIndex(lb_volume, 0);
            Controls.SetChildIndex(tb_volume, 0);
            Controls.SetChildIndex(bt_query, 0);
            Controls.SetChildIndex(tb_holder, 0);
            Controls.SetChildIndex(lb_assetId, 0);
            Controls.SetChildIndex(tb_assetId, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Wallets.UI.Controls.DarkLabel lb_holder;
        private Wallets.UI.Controls.DarkLabel lb_volume;
        private Wallets.UI.Controls.DarkTextBox tb_volume;
        private Wallets.UI.Controls.DarkButton bt_query;
        private Wallets.UI.Controls.DarkTextBox tb_holder;
        private Wallets.UI.Controls.DarkLabel lb_assetId;
        private Wallets.UI.Controls.DarkTextBox tb_assetId;
    }
}
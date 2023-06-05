
namespace OX.UI.LAM
{
    partial class SpaceTimeSnapshot
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
            this.tb_BlockIndex = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_BlockIndex = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_SelfSTVol = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_LeafSTVol = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_SelfSTVol = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_LeafSTVol = new OX.Wallets.UI.Controls.DarkTextBox();
            this.bt_pre = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_next = new OX.Wallets.UI.Controls.DarkButton();
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
            // tb_BlockIndex
            // 
            this.tb_BlockIndex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_BlockIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_BlockIndex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_BlockIndex.Location = new System.Drawing.Point(269, 29);
            this.tb_BlockIndex.Margin = new System.Windows.Forms.Padding(6);
            this.tb_BlockIndex.Name = "tb_BlockIndex";
            this.tb_BlockIndex.ReadOnly = true;
            this.tb_BlockIndex.Size = new System.Drawing.Size(468, 30);
            this.tb_BlockIndex.TabIndex = 48;
            this.tb_BlockIndex.TextChanged += new System.EventHandler(this.tb_targetAssetId_TextChanged);
            // 
            // lb_BlockIndex
            // 
            this.lb_BlockIndex.AutoSize = true;
            this.lb_BlockIndex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_BlockIndex.Location = new System.Drawing.Point(30, 31);
            this.lb_BlockIndex.Name = "lb_BlockIndex";
            this.lb_BlockIndex.Size = new System.Drawing.Size(106, 24);
            this.lb_BlockIndex.TabIndex = 47;
            this.lb_BlockIndex.Text = "darkLabel1";
            // 
            // lb_SelfSTVol
            // 
            this.lb_SelfSTVol.AutoSize = true;
            this.lb_SelfSTVol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_SelfSTVol.Location = new System.Drawing.Point(30, 92);
            this.lb_SelfSTVol.Name = "lb_SelfSTVol";
            this.lb_SelfSTVol.Size = new System.Drawing.Size(106, 24);
            this.lb_SelfSTVol.TabIndex = 49;
            this.lb_SelfSTVol.Text = "darkLabel1";
            // 
            // lb_LeafSTVol
            // 
            this.lb_LeafSTVol.AutoSize = true;
            this.lb_LeafSTVol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_LeafSTVol.Location = new System.Drawing.Point(30, 151);
            this.lb_LeafSTVol.Name = "lb_LeafSTVol";
            this.lb_LeafSTVol.Size = new System.Drawing.Size(106, 24);
            this.lb_LeafSTVol.TabIndex = 50;
            this.lb_LeafSTVol.Text = "darkLabel1";
            // 
            // tb_SelfSTVol
            // 
            this.tb_SelfSTVol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_SelfSTVol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SelfSTVol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_SelfSTVol.Location = new System.Drawing.Point(269, 90);
            this.tb_SelfSTVol.Margin = new System.Windows.Forms.Padding(6);
            this.tb_SelfSTVol.Name = "tb_SelfSTVol";
            this.tb_SelfSTVol.ReadOnly = true;
            this.tb_SelfSTVol.Size = new System.Drawing.Size(468, 30);
            this.tb_SelfSTVol.TabIndex = 51;
            // 
            // tb_LeafSTVol
            // 
            this.tb_LeafSTVol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_LeafSTVol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_LeafSTVol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_LeafSTVol.Location = new System.Drawing.Point(269, 147);
            this.tb_LeafSTVol.Margin = new System.Windows.Forms.Padding(6);
            this.tb_LeafSTVol.Name = "tb_LeafSTVol";
            this.tb_LeafSTVol.ReadOnly = true;
            this.tb_LeafSTVol.Size = new System.Drawing.Size(468, 30);
            this.tb_LeafSTVol.TabIndex = 53;
            // 
            // bt_pre
            // 
            this.bt_pre.Location = new System.Drawing.Point(272, 209);
            this.bt_pre.Name = "bt_pre";
            this.bt_pre.Padding = new System.Windows.Forms.Padding(5);
            this.bt_pre.Size = new System.Drawing.Size(227, 34);
            this.bt_pre.SpecialBorderColor = null;
            this.bt_pre.SpecialFillColor = null;
            this.bt_pre.SpecialTextColor = null;
            this.bt_pre.TabIndex = 66;
            this.bt_pre.Text = "darkButton1";
            this.bt_pre.Click += new System.EventHandler(this.bt_pre_Click);
            // 
            // bt_next
            // 
            this.bt_next.Location = new System.Drawing.Point(510, 209);
            this.bt_next.Name = "bt_next";
            this.bt_next.Padding = new System.Windows.Forms.Padding(5);
            this.bt_next.Size = new System.Drawing.Size(227, 34);
            this.bt_next.SpecialBorderColor = null;
            this.bt_next.SpecialFillColor = null;
            this.bt_next.SpecialTextColor = null;
            this.bt_next.TabIndex = 67;
            this.bt_next.Text = "darkButton2";
            this.bt_next.Click += new System.EventHandler(this.bt_next_Click);
            // 
            // SpaceTimeSnapshot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 338);
            this.Controls.Add(this.bt_next);
            this.Controls.Add(this.bt_pre);
            this.Controls.Add(this.tb_LeafSTVol);
            this.Controls.Add(this.tb_SelfSTVol);
            this.Controls.Add(this.lb_LeafSTVol);
            this.Controls.Add(this.lb_SelfSTVol);
            this.Controls.Add(this.tb_BlockIndex);
            this.Controls.Add(this.lb_BlockIndex);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpaceTimeSnapshot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SpaceTimeSnapshot";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.lb_BlockIndex, 0);
            this.Controls.SetChildIndex(this.tb_BlockIndex, 0);
            this.Controls.SetChildIndex(this.lb_SelfSTVol, 0);
            this.Controls.SetChildIndex(this.lb_LeafSTVol, 0);
            this.Controls.SetChildIndex(this.tb_SelfSTVol, 0);
            this.Controls.SetChildIndex(this.tb_LeafSTVol, 0);
            this.Controls.SetChildIndex(this.bt_pre, 0);
            this.Controls.SetChildIndex(this.bt_next, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Wallets.UI.Controls.DarkTextBox tb_BlockIndex;
        private Wallets.UI.Controls.DarkLabel lb_BlockIndex;
        private Wallets.UI.Controls.DarkLabel lb_SelfSTVol;
        private Wallets.UI.Controls.DarkLabel lb_LeafSTVol;
        private Wallets.UI.Controls.DarkTextBox tb_SelfSTVol;
        private Wallets.UI.Controls.DarkTextBox tb_LeafSTVol;
        private Wallets.UI.Controls.DarkButton bt_pre;
        private Wallets.UI.Controls.DarkButton bt_next;
    }
}
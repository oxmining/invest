namespace OX.UI.Swap
{
    partial class SideSwapPairControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_pairname = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_priceBalance = new OX.Wallets.UI.Controls.DarkLabel();
            this.bt_showKLine = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_goSwap = new OX.Wallets.UI.Controls.DarkButton();
            this.lb_price = new OX.Wallets.UI.Controls.DarkLabel();
            this.bt_detail = new OX.Wallets.UI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // lb_pairname
            // 
            this.lb_pairname.AutoSize = true;
            this.lb_pairname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_pairname.Location = new System.Drawing.Point(28, 22);
            this.lb_pairname.Name = "lb_pairname";
            this.lb_pairname.Size = new System.Drawing.Size(106, 24);
            this.lb_pairname.TabIndex = 7;
            this.lb_pairname.Text = "darkLabel1";
            // 
            // lb_priceBalance
            // 
            this.lb_priceBalance.AutoSize = true;
            this.lb_priceBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_priceBalance.Location = new System.Drawing.Point(28, 65);
            this.lb_priceBalance.Name = "lb_priceBalance";
            this.lb_priceBalance.Size = new System.Drawing.Size(106, 24);
            this.lb_priceBalance.TabIndex = 8;
            this.lb_priceBalance.Text = "darkLabel1";
            // 
            // bt_showKLine
            // 
            this.bt_showKLine.Location = new System.Drawing.Point(188, 104);
            this.bt_showKLine.Name = "bt_showKLine";
            this.bt_showKLine.Padding = new System.Windows.Forms.Padding(5);
            this.bt_showKLine.Size = new System.Drawing.Size(130, 34);
            this.bt_showKLine.SpecialBorderColor = null;
            this.bt_showKLine.SpecialFillColor = null;
            this.bt_showKLine.SpecialTextColor = null;
            this.bt_showKLine.TabIndex = 13;
            this.bt_showKLine.Text = "darkButton1";
            this.bt_showKLine.Click += new System.EventHandler(this.bt_showKLine_Click);
            // 
            // bt_goSwap
            // 
            this.bt_goSwap.Location = new System.Drawing.Point(355, 104);
            this.bt_goSwap.Name = "bt_goSwap";
            this.bt_goSwap.Padding = new System.Windows.Forms.Padding(5);
            this.bt_goSwap.Size = new System.Drawing.Size(130, 34);
            this.bt_goSwap.SpecialBorderColor = null;
            this.bt_goSwap.SpecialFillColor = null;
            this.bt_goSwap.SpecialTextColor = null;
            this.bt_goSwap.TabIndex = 14;
            this.bt_goSwap.Text = "darkButton1";
            this.bt_goSwap.Click += new System.EventHandler(this.bt_goSwap_Click);
            // 
            // lb_price
            // 
            this.lb_price.AutoSize = true;
            this.lb_price.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_price.Location = new System.Drawing.Point(280, 22);
            this.lb_price.Name = "lb_price";
            this.lb_price.Size = new System.Drawing.Size(106, 24);
            this.lb_price.TabIndex = 15;
            this.lb_price.Text = "darkLabel1";
            // 
            // bt_detail
            // 
            this.bt_detail.Location = new System.Drawing.Point(18, 104);
            this.bt_detail.Name = "bt_detail";
            this.bt_detail.Padding = new System.Windows.Forms.Padding(5);
            this.bt_detail.Size = new System.Drawing.Size(130, 34);
            this.bt_detail.SpecialBorderColor = null;
            this.bt_detail.SpecialFillColor = null;
            this.bt_detail.SpecialTextColor = null;
            this.bt_detail.TabIndex = 16;
            this.bt_detail.Text = "darkButton1";
            this.bt_detail.Click += new System.EventHandler(this.bt_detail_Click);
            // 
            // SideSwapPairControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.bt_detail);
            this.Controls.Add(this.lb_price);
            this.Controls.Add(this.bt_goSwap);
            this.Controls.Add(this.bt_showKLine);
            this.Controls.Add(this.lb_priceBalance);
            this.Controls.Add(this.lb_pairname);
            this.Name = "SideSwapPairControl";
            this.Size = new System.Drawing.Size(503, 145);
            this.Load += new System.EventHandler(this.SideSwapPairControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wallets.UI.Controls.DarkLabel lb_pairname;
        private Wallets.UI.Controls.DarkLabel lb_priceBalance;
        private Wallets.UI.Controls.DarkButton bt_showKLine;
        private Wallets.UI.Controls.DarkButton bt_goSwap;
        private Wallets.UI.Controls.DarkLabel lb_price;
        private Wallets.UI.Controls.DarkButton bt_detail;
    }
}

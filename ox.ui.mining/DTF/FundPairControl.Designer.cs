namespace OX.UI.DTF
{
    partial class FundPairControl
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
            lb_trusteeAddress = new Wallets.UI.Controls.DarkLabel();
            lb_totalSubscribe = new Wallets.UI.Controls.DarkLabel();
            bt_copySubscibeAddress = new Wallets.UI.Controls.DarkButton();
            bt_trustAssetDetail = new Wallets.UI.Controls.DarkButton();
            lb_totalDivident = new Wallets.UI.Controls.DarkLabel();
            lb_trustOXC = new Wallets.UI.Controls.DarkLabel();
            lb_subscribeAddress = new Wallets.UI.Controls.DarkLabel();
            bt_refresh = new Wallets.UI.Controls.DarkButton();
            bt_copyTrustAddress = new Wallets.UI.Controls.DarkButton();
            lb_trustAddress = new Wallets.UI.Controls.DarkLabel();
            lb_myTotalSubscribe = new Wallets.UI.Controls.DarkLabel();
            lb_fundId = new Wallets.UI.Controls.DarkLabel();
            SuspendLayout();
            // 
            // lb_trusteeAddress
            // 
            lb_trusteeAddress.AutoSize = true;
            lb_trusteeAddress.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_trusteeAddress.Location = new System.Drawing.Point(28, 63);
            lb_trusteeAddress.Name = "lb_trusteeAddress";
            lb_trusteeAddress.Size = new System.Drawing.Size(106, 24);
            lb_trusteeAddress.TabIndex = 7;
            lb_trusteeAddress.Text = "darkLabel1";
            // 
            // lb_totalSubscribe
            // 
            lb_totalSubscribe.AutoSize = true;
            lb_totalSubscribe.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_totalSubscribe.Location = new System.Drawing.Point(29, 188);
            lb_totalSubscribe.Name = "lb_totalSubscribe";
            lb_totalSubscribe.Size = new System.Drawing.Size(106, 24);
            lb_totalSubscribe.TabIndex = 8;
            lb_totalSubscribe.Text = "darkLabel1";
            lb_totalSubscribe.Click += lb_totalSubscribe_Click;
            // 
            // bt_copySubscibeAddress
            // 
            bt_copySubscibeAddress.Location = new System.Drawing.Point(355, 367);
            bt_copySubscibeAddress.Name = "bt_copySubscibeAddress";
            bt_copySubscibeAddress.Padding = new System.Windows.Forms.Padding(5);
            bt_copySubscibeAddress.Size = new System.Drawing.Size(227, 34);
            bt_copySubscibeAddress.SpecialBorderColor = null;
            bt_copySubscibeAddress.SpecialFillColor = null;
            bt_copySubscibeAddress.SpecialTextColor = null;
            bt_copySubscibeAddress.TabIndex = 13;
            bt_copySubscibeAddress.Text = "darkButton1";
            bt_copySubscibeAddress.Click += bt_showKLine_Click;
            // 
            // bt_trustAssetDetail
            // 
            bt_trustAssetDetail.Location = new System.Drawing.Point(598, 367);
            bt_trustAssetDetail.Name = "bt_trustAssetDetail";
            bt_trustAssetDetail.Padding = new System.Windows.Forms.Padding(5);
            bt_trustAssetDetail.Size = new System.Drawing.Size(227, 34);
            bt_trustAssetDetail.SpecialBorderColor = null;
            bt_trustAssetDetail.SpecialFillColor = null;
            bt_trustAssetDetail.SpecialTextColor = null;
            bt_trustAssetDetail.TabIndex = 14;
            bt_trustAssetDetail.Text = "darkButton1";
            bt_trustAssetDetail.Click += bt_trustAssetDetail_Click;
            // 
            // lb_totalDivident
            // 
            lb_totalDivident.AutoSize = true;
            lb_totalDivident.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_totalDivident.Location = new System.Drawing.Point(29, 227);
            lb_totalDivident.Name = "lb_totalDivident";
            lb_totalDivident.Size = new System.Drawing.Size(106, 24);
            lb_totalDivident.TabIndex = 17;
            lb_totalDivident.Text = "darkLabel1";
            // 
            // lb_trustOXC
            // 
            lb_trustOXC.AutoSize = true;
            lb_trustOXC.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_trustOXC.Location = new System.Drawing.Point(28, 270);
            lb_trustOXC.Name = "lb_trustOXC";
            lb_trustOXC.Size = new System.Drawing.Size(106, 24);
            lb_trustOXC.TabIndex = 18;
            lb_trustOXC.Text = "darkLabel1";
            // 
            // lb_subscribeAddress
            // 
            lb_subscribeAddress.AutoSize = true;
            lb_subscribeAddress.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_subscribeAddress.Location = new System.Drawing.Point(29, 142);
            lb_subscribeAddress.Name = "lb_subscribeAddress";
            lb_subscribeAddress.Size = new System.Drawing.Size(106, 24);
            lb_subscribeAddress.TabIndex = 19;
            lb_subscribeAddress.Text = "darkLabel1";
            // 
            // bt_refresh
            // 
            bt_refresh.Location = new System.Drawing.Point(17, 367);
            bt_refresh.Name = "bt_refresh";
            bt_refresh.Padding = new System.Windows.Forms.Padding(5);
            bt_refresh.Size = new System.Drawing.Size(81, 34);
            bt_refresh.SpecialBorderColor = null;
            bt_refresh.SpecialFillColor = null;
            bt_refresh.SpecialTextColor = null;
            bt_refresh.TabIndex = 20;
            bt_refresh.Text = "bt_refresh";
            bt_refresh.Click += darkButton1_Click;
            // 
            // bt_copyTrustAddress
            // 
            bt_copyTrustAddress.Location = new System.Drawing.Point(113, 367);
            bt_copyTrustAddress.Name = "bt_copyTrustAddress";
            bt_copyTrustAddress.Padding = new System.Windows.Forms.Padding(5);
            bt_copyTrustAddress.Size = new System.Drawing.Size(227, 34);
            bt_copyTrustAddress.SpecialBorderColor = null;
            bt_copyTrustAddress.SpecialFillColor = null;
            bt_copyTrustAddress.SpecialTextColor = null;
            bt_copyTrustAddress.TabIndex = 21;
            bt_copyTrustAddress.Text = "darkButton1";
            bt_copyTrustAddress.Click += bt_copyTrustAddress_Click;
            // 
            // lb_trustAddress
            // 
            lb_trustAddress.AutoSize = true;
            lb_trustAddress.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_trustAddress.Location = new System.Drawing.Point(29, 103);
            lb_trustAddress.Name = "lb_trustAddress";
            lb_trustAddress.Size = new System.Drawing.Size(106, 24);
            lb_trustAddress.TabIndex = 22;
            lb_trustAddress.Text = "darkLabel1";
            // 
            // lb_myTotalSubscribe
            // 
            lb_myTotalSubscribe.AutoSize = true;
            lb_myTotalSubscribe.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_myTotalSubscribe.Location = new System.Drawing.Point(29, 310);
            lb_myTotalSubscribe.Name = "lb_myTotalSubscribe";
            lb_myTotalSubscribe.Size = new System.Drawing.Size(106, 24);
            lb_myTotalSubscribe.TabIndex = 23;
            lb_myTotalSubscribe.Text = "darkLabel1";
            // 
            // lb_fundId
            // 
            lb_fundId.AutoSize = true;
            lb_fundId.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_fundId.Location = new System.Drawing.Point(28, 22);
            lb_fundId.Name = "lb_fundId";
            lb_fundId.Size = new System.Drawing.Size(106, 24);
            lb_fundId.TabIndex = 24;
            lb_fundId.Text = "darkLabel1";
            // 
            // FundPairControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(lb_fundId);
            Controls.Add(lb_myTotalSubscribe);
            Controls.Add(lb_trustAddress);
            Controls.Add(bt_copyTrustAddress);
            Controls.Add(bt_refresh);
            Controls.Add(lb_subscribeAddress);
            Controls.Add(lb_trustOXC);
            Controls.Add(lb_totalDivident);
            Controls.Add(bt_trustAssetDetail);
            Controls.Add(bt_copySubscibeAddress);
            Controls.Add(lb_totalSubscribe);
            Controls.Add(lb_trusteeAddress);
            Name = "FundPairControl";
            Size = new System.Drawing.Size(840, 416);
            Load += SideSwapPairControl_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Wallets.UI.Controls.DarkLabel lb_trusteeAddress;
        private Wallets.UI.Controls.DarkLabel lb_totalSubscribe;
        private Wallets.UI.Controls.DarkButton bt_copySubscibeAddress;
        private Wallets.UI.Controls.DarkButton bt_trustAssetDetail;
        private Wallets.UI.Controls.DarkLabel lb_totalDivident;
        private Wallets.UI.Controls.DarkLabel lb_trustOXC;
        private Wallets.UI.Controls.DarkLabel lb_subscribeAddress;
        private Wallets.UI.Controls.DarkButton bt_refresh;
        private Wallets.UI.Controls.DarkButton bt_copyTrustAddress;
        private Wallets.UI.Controls.DarkLabel lb_trustAddress;
        private Wallets.UI.Controls.DarkLabel lb_myTotalSubscribe;
        private Wallets.UI.Controls.DarkLabel lb_fundId;
    }
}

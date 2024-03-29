﻿
namespace OX.UI.Mining.DTF
{
    partial class FundAssetDetail
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
            dlv_unlockAsset = new Wallets.UI.Controls.DarkListView();
            dlv_lockAsset = new Wallets.UI.Controls.DarkListView();
            lb_commonAsset = new Wallets.UI.Controls.DarkLabel();
            lb_lockAsset = new Wallets.UI.Controls.DarkLabel();
            lb_myRatio = new Wallets.UI.Controls.DarkLabel();
            dlv_rateList = new Wallets.UI.Controls.DarkListView();
            lb_rateList = new Wallets.UI.Controls.DarkLabel();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Click += btnOk_Click;
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
            // dlv_unlockAsset
            // 
            dlv_unlockAsset.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dlv_unlockAsset.Location = new System.Drawing.Point(13, 130);
            dlv_unlockAsset.Name = "dlv_unlockAsset";
            dlv_unlockAsset.Size = new System.Drawing.Size(482, 865);
            dlv_unlockAsset.TabIndex = 46;
            dlv_unlockAsset.Text = "darkListView1";
            // 
            // dlv_lockAsset
            // 
            dlv_lockAsset.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dlv_lockAsset.Location = new System.Drawing.Point(527, 130);
            dlv_lockAsset.Name = "dlv_lockAsset";
            dlv_lockAsset.Size = new System.Drawing.Size(500, 865);
            dlv_lockAsset.TabIndex = 47;
            dlv_lockAsset.Text = "darkListView1";
            // 
            // lb_commonAsset
            // 
            lb_commonAsset.AutoSize = true;
            lb_commonAsset.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_commonAsset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_commonAsset.Location = new System.Drawing.Point(13, 85);
            lb_commonAsset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_commonAsset.Name = "lb_commonAsset";
            lb_commonAsset.Size = new System.Drawing.Size(86, 24);
            lb_commonAsset.TabIndex = 48;
            lb_commonAsset.Text = "Claim to:";
            // 
            // lb_lockAsset
            // 
            lb_lockAsset.AutoSize = true;
            lb_lockAsset.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_lockAsset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_lockAsset.Location = new System.Drawing.Point(527, 85);
            lb_lockAsset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_lockAsset.Name = "lb_lockAsset";
            lb_lockAsset.Size = new System.Drawing.Size(86, 24);
            lb_lockAsset.TabIndex = 49;
            lb_lockAsset.Text = "Claim to:";
            // 
            // lb_myRatio
            // 
            lb_myRatio.AutoSize = true;
            lb_myRatio.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_myRatio.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_myRatio.Location = new System.Drawing.Point(1060, 33);
            lb_myRatio.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_myRatio.Name = "lb_myRatio";
            lb_myRatio.Size = new System.Drawing.Size(0, 24);
            lb_myRatio.TabIndex = 50;
            // 
            // dlv_rateList
            // 
            dlv_rateList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dlv_rateList.Location = new System.Drawing.Point(1060, 130);
            dlv_rateList.Name = "dlv_rateList";
            dlv_rateList.Size = new System.Drawing.Size(805, 865);
            dlv_rateList.TabIndex = 51;
            dlv_rateList.Text = "darkListView1";
            // 
            // lb_rateList
            // 
            lb_rateList.AutoSize = true;
            lb_rateList.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_rateList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_rateList.Location = new System.Drawing.Point(1060, 85);
            lb_rateList.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_rateList.Name = "lb_rateList";
            lb_rateList.Size = new System.Drawing.Size(86, 24);
            lb_rateList.TabIndex = 52;
            lb_rateList.Text = "Claim to:";
            // 
            // FundAssetDetail
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1883, 1093);
            Controls.Add(lb_rateList);
            Controls.Add(dlv_rateList);
            Controls.Add(lb_myRatio);
            Controls.Add(lb_lockAsset);
            Controls.Add(lb_commonAsset);
            Controls.Add(dlv_lockAsset);
            Controls.Add(dlv_unlockAsset);
            Name = "FundAssetDetail";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "MorePrize";
            Load += FundAssetDetail_Load;
            Controls.SetChildIndex(dlv_unlockAsset, 0);
            Controls.SetChildIndex(dlv_lockAsset, 0);
            Controls.SetChildIndex(lb_commonAsset, 0);
            Controls.SetChildIndex(lb_lockAsset, 0);
            Controls.SetChildIndex(lb_myRatio, 0);
            Controls.SetChildIndex(dlv_rateList, 0);
            Controls.SetChildIndex(lb_rateList, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Wallets.UI.Controls.DarkListView dlv_unlockAsset;
        private Wallets.UI.Controls.DarkListView dlv_lockAsset;
        private Wallets.UI.Controls.DarkLabel lb_commonAsset;
        private Wallets.UI.Controls.DarkLabel lb_lockAsset;
        private Wallets.UI.Controls.DarkLabel lb_myRatio;
        private Wallets.UI.Controls.DarkListView dlv_rateList;
        private Wallets.UI.Controls.DarkLabel lb_rateList;
    }
}
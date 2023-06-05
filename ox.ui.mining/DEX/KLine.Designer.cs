using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
namespace OX.UI.Swap
{
    partial class KLine
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RoundPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // RoundPanel
            // 
            this.RoundPanel.AutoScroll = true;
            this.RoundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RoundPanel.Location = new System.Drawing.Point(0, 0);
            this.RoundPanel.Name = "RoundPanel";
            this.RoundPanel.Size = new System.Drawing.Size(1453, 543);
            this.RoundPanel.TabIndex = 0;
            // 
            // KLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RoundPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "KLine";
            this.Size = new System.Drawing.Size(1453, 543);
            this.Load += new System.EventHandler(this.KLine_Load);
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.Panel RoundPanel;
    }
}

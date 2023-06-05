
namespace OX.UI.Swap
{
    partial class MySwapRecords
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
            this.lst_ido = new OX.Wallets.UI.Controls.DarkListView();
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
            // lst_ido
            // 
            this.lst_ido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lst_ido.Location = new System.Drawing.Point(0, 0);
            this.lst_ido.Name = "lst_ido";
            this.lst_ido.Size = new System.Drawing.Size(1371, 971);
            this.lst_ido.TabIndex = 15;
            this.lst_ido.Text = "darkListView1";
            // 
            // MySwapRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 1054);
            this.Controls.Add(this.lst_ido);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MySwapRecords";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegMinerForm";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.lst_ido, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private Wallets.UI.Controls.DarkListView lst_ido;
    }
}
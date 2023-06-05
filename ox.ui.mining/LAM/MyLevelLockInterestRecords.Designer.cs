using OX.Wallets.UI.Config;
using OX.Wallets.UI.Docking;
using OX.Wallets;

namespace OX.UI.LAM
{
    partial class MyLevelLockInterestRecords
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
            this.treePools = new OX.Wallets.UI.Controls.DarkTreeView();
            this.SuspendLayout();
            // 
            // treeAsset
            // 
            this.treePools.AllowMoveNodes = true;
            this.treePools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treePools.Location = new System.Drawing.Point(0, 25);
            this.treePools.MaxDragChange = 20;
            this.treePools.MultiSelect = true;
            this.treePools.Name = "treeRooms";
            this.treePools.ShowIcons = true;
            this.treePools.Size = new System.Drawing.Size(280, 425);
            this.treePools.TabIndex = 1;
            this.treePools.Text = "darkTreeView1";
            // 
            // DockAsset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treePools);
            this.DefaultDockArea = OX.Wallets.UI.Docking.DarkDockArea.Right;
            this.DockText = UIHelper.LocalString("我的级锁出矿记录", "My Level Lock Interest Records");
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.Icon = global::OX.GT.Icons.properties_16xLG;
            this.Name = "MyLevelLockInterestRecords";
            this.SerializationKey = "MyLevelLockInterestRecords";
            this.Size = new System.Drawing.Size(280, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private OX.Wallets.UI.Controls.DarkTreeView treePools;

    }
}

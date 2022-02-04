
namespace Inverse.Windows
{
    partial class FrmNewConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewConnection));
            this.picSqlite = new System.Windows.Forms.PictureBox();
            this.picMssqlServer = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSqlite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMssqlServer)).BeginInit();
            this.SuspendLayout();
            // 
            // picSqlite
            // 
            this.picSqlite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSqlite.Image = ((System.Drawing.Image)(resources.GetObject("picSqlite.Image")));
            this.picSqlite.Location = new System.Drawing.Point(183, 12);
            this.picSqlite.Name = "picSqlite";
            this.picSqlite.Size = new System.Drawing.Size(166, 143);
            this.picSqlite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSqlite.TabIndex = 5;
            this.picSqlite.TabStop = false;
            this.picSqlite.Click += new System.EventHandler(this.picSqlite_Click);
            // 
            // picMssqlServer
            // 
            this.picMssqlServer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMssqlServer.Image = ((System.Drawing.Image)(resources.GetObject("picMssqlServer.Image")));
            this.picMssqlServer.Location = new System.Drawing.Point(25, 33);
            this.picMssqlServer.Name = "picMssqlServer";
            this.picMssqlServer.Size = new System.Drawing.Size(129, 107);
            this.picMssqlServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMssqlServer.TabIndex = 6;
            this.picMssqlServer.TabStop = false;
            this.picMssqlServer.Click += new System.EventHandler(this.picMssqlServer_Click);
            // 
            // FrmNewConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(375, 173);
            this.Controls.Add(this.picMssqlServer);
            this.Controls.Add(this.picSqlite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmNewConnection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New";
            ((System.ComponentModel.ISupportInitialize)(this.picSqlite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMssqlServer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSqlite;
        private System.Windows.Forms.PictureBox picMssqlServer;
    }
}

namespace Inverse.Desktop
{
    partial class DatabaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseForm));
            picSqlite = new System.Windows.Forms.PictureBox();
            picMssqlServer = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)picSqlite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picMssqlServer).BeginInit();
            SuspendLayout();
            // 
            // picSqlite
            // 
            picSqlite.Cursor = System.Windows.Forms.Cursors.Hand;
            picSqlite.Image = (System.Drawing.Image)resources.GetObject("picSqlite.Image");
            picSqlite.Location = new System.Drawing.Point(214, 31);
            picSqlite.Name = "picSqlite";
            picSqlite.Size = new System.Drawing.Size(120, 107);
            picSqlite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picSqlite.TabIndex = 5;
            picSqlite.TabStop = false;
            picSqlite.Click += picSqlite_Click;
            // 
            // picMssqlServer
            // 
            picMssqlServer.Cursor = System.Windows.Forms.Cursors.Hand;
            picMssqlServer.Image = (System.Drawing.Image)resources.GetObject("picMssqlServer.Image");
            picMssqlServer.Location = new System.Drawing.Point(35, 31);
            picMssqlServer.Name = "picMssqlServer";
            picMssqlServer.Size = new System.Drawing.Size(129, 107);
            picMssqlServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picMssqlServer.TabIndex = 6;
            picMssqlServer.TabStop = false;
            picMssqlServer.Click += picMssqlServer_Click;
            // 
            // DatabaseForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(375, 173);
            Controls.Add(picMssqlServer);
            Controls.Add(picSqlite);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "DatabaseForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "New Database";
            ((System.ComponentModel.ISupportInitialize)picSqlite).EndInit();
            ((System.ComponentModel.ISupportInitialize)picMssqlServer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox picSqlite;
        private System.Windows.Forms.PictureBox picMssqlServer;
    }
}
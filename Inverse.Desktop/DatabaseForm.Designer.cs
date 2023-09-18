
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
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            label1 = new System.Windows.Forms.Label();
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
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(104, 176);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(166, 15);
            linkLabel1.TabIndex = 7;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Continue with a new database";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(178, 161);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(18, 15);
            label1.TabIndex = 8;
            label1.Text = "or";
            // 
            // DatabaseForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(375, 216);
            Controls.Add(label1);
            Controls.Add(linkLabel1);
            Controls.Add(picMssqlServer);
            Controls.Add(picSqlite);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "DatabaseForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "New Database";
            ((System.ComponentModel.ISupportInitialize)picSqlite).EndInit();
            ((System.ComponentModel.ISupportInitialize)picMssqlServer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox picSqlite;
        private System.Windows.Forms.PictureBox picMssqlServer;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
    }
}
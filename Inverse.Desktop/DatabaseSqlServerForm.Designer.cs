namespace Inverse.Desktop
{
    partial class DatabaseSqlServerForm
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
            label1 = new System.Windows.Forms.Label();
            txtServer = new System.Windows.Forms.TextBox();
            txtDatabase = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            btnRevert = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            txtPassword = new System.Windows.Forms.MaskedTextBox();
            chkWindowsAuth = new System.Windows.Forms.CheckBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(29, 11);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Server";
            // 
            // txtServer
            // 
            txtServer.Location = new System.Drawing.Point(29, 29);
            txtServer.Name = "txtServer";
            txtServer.Size = new System.Drawing.Size(316, 23);
            txtServer.TabIndex = 0;
            txtServer.Leave += txtServer_Leave;
            // 
            // txtDatabase
            // 
            txtDatabase.Location = new System.Drawing.Point(29, 80);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new System.Drawing.Size(316, 23);
            txtDatabase.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(29, 62);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(55, 15);
            label2.TabIndex = 2;
            label2.Text = "Database";
            // 
            // btnRevert
            // 
            btnRevert.Location = new System.Drawing.Point(246, 259);
            btnRevert.Name = "btnRevert";
            btnRevert.Size = new System.Drawing.Size(99, 30);
            btnRevert.TabIndex = 5;
            btnRevert.Text = "Revert";
            btnRevert.UseVisualStyleBackColor = true;
            btnRevert.Click += btnRevert_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(29, 203);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(57, 15);
            label3.TabIndex = 7;
            label3.Text = "Password";
            // 
            // txtUsername
            // 
            txtUsername.Enabled = false;
            txtUsername.Location = new System.Drawing.Point(29, 170);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(316, 23);
            txtUsername.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(29, 152);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(60, 15);
            label4.TabIndex = 5;
            label4.Text = "Username";
            // 
            // txtPassword
            // 
            txtPassword.Enabled = false;
            txtPassword.Location = new System.Drawing.Point(29, 221);
            txtPassword.Name = "txtPassword";
            txtPassword.PromptChar = '*';
            txtPassword.Size = new System.Drawing.Size(316, 23);
            txtPassword.TabIndex = 4;
            // 
            // chkWindowsAuth
            // 
            chkWindowsAuth.AutoSize = true;
            chkWindowsAuth.Checked = true;
            chkWindowsAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWindowsAuth.Location = new System.Drawing.Point(29, 119);
            chkWindowsAuth.Name = "chkWindowsAuth";
            chkWindowsAuth.Size = new System.Drawing.Size(157, 19);
            chkWindowsAuth.TabIndex = 2;
            chkWindowsAuth.Text = "Windows Authentication";
            chkWindowsAuth.UseVisualStyleBackColor = true;
            chkWindowsAuth.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = System.Drawing.Color.Lime;
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(351, 32);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(14, 15);
            label5.TabIndex = 8;
            label5.Text = "V";
            label5.Visible = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = System.Drawing.Color.Lime;
            label6.ForeColor = System.Drawing.Color.White;
            label6.Location = new System.Drawing.Point(351, 88);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(14, 15);
            label6.TabIndex = 9;
            label6.Text = "V";
            label6.Visible = false;
            // 
            // DatabaseSqlServerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(378, 314);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(chkWindowsAuth);
            Controls.Add(txtPassword);
            Controls.Add(label3);
            Controls.Add(txtUsername);
            Controls.Add(label4);
            Controls.Add(btnRevert);
            Controls.Add(txtDatabase);
            Controls.Add(label2);
            Controls.Add(txtServer);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "DatabaseSqlServerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "MSSQLServer Database";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox txtPassword;
        private System.Windows.Forms.CheckBox chkWindowsAuth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
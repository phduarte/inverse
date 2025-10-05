namespace Inverse.Desktop;

partial class DatabaseSqliteForm
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
        txtFilename = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        btnRevert = new System.Windows.Forms.Button();
        btnSearch = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // txtFilename
        // 
        txtFilename.Location = new System.Drawing.Point(12, 30);
        txtFilename.Name = "txtFilename";
        txtFilename.ReadOnly = true;
        txtFilename.Size = new System.Drawing.Size(285, 23);
        txtFilename.TabIndex = 3;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 12);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(55, 15);
        label1.TabIndex = 4;
        label1.Text = "Filename";
        // 
        // btnRevert
        // 
        btnRevert.Enabled = false;
        btnRevert.Location = new System.Drawing.Point(237, 84);
        btnRevert.Name = "btnRevert";
        btnRevert.Size = new System.Drawing.Size(113, 38);
        btnRevert.TabIndex = 6;
        btnRevert.Text = "Revert";
        btnRevert.UseVisualStyleBackColor = true;
        btnRevert.Click += btnRevert_Click;
        // 
        // btnSearch
        // 
        btnSearch.Location = new System.Drawing.Point(303, 30);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new System.Drawing.Size(47, 23);
        btnSearch.TabIndex = 7;
        btnSearch.Text = "...";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        // 
        // DatabaseSqliteForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.White;
        ClientSize = new System.Drawing.Size(362, 134);
        Controls.Add(btnSearch);
        Controls.Add(btnRevert);
        Controls.Add(txtFilename);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        Name = "DatabaseSqliteForm";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "SQLite Database";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private System.Windows.Forms.TextBox txtFilename;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnRevert;
    private System.Windows.Forms.Button btnSearch;
}
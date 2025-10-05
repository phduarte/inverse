namespace Inverse.Desktop;

partial class AboutForm
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
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        linkLabel1 = new System.Windows.Forms.LinkLabel();
        label4 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        label1.Location = new System.Drawing.Point(76, 61);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(188, 21);
        label1.TabIndex = 0;
        label1.Text = "Database Studio Designer";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(101, 89);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(142, 15);
        label2.TabIndex = 0;
        label2.Text = "Developed by phduarte87";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(145, 113);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(60, 15);
        label3.TabIndex = 0;
        label3.Text = "2021-2023";
        // 
        // linkLabel1
        // 
        linkLabel1.AutoSize = true;
        linkLabel1.Location = new System.Drawing.Point(68, 138);
        linkLabel1.Name = "linkLabel1";
        linkLabel1.Size = new System.Drawing.Size(204, 15);
        linkLabel1.TabIndex = 1;
        linkLabel1.TabStop = true;
        linkLabel1.Text = "https://github.com/phduarte/inverse";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        label4.Location = new System.Drawing.Point(115, 21);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(111, 30);
        label4.TabIndex = 0;
        label4.Text = "InverseDB";
        // 
        // FrmAbout
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(341, 174);
        Controls.Add(linkLabel1);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label4);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        Name = "FrmAbout";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "About";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.Label label4;
}
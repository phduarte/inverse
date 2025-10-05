namespace Inverse.Desktop;

partial class DatabaseEditForm
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
        textBoxName = new System.Windows.Forms.TextBox();
        button1 = new System.Windows.Forms.Button();
        comboBoxProvider = new System.Windows.Forms.ComboBox();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        textBoxConnectionString = new System.Windows.Forms.TextBox();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(39, 15);
        label1.TabIndex = 0;
        label1.Text = "Name";
        // 
        // textBoxName
        // 
        textBoxName.Location = new System.Drawing.Point(12, 27);
        textBoxName.Name = "textBoxName";
        textBoxName.Size = new System.Drawing.Size(272, 23);
        textBoxName.TabIndex = 1;
        textBoxName.TextChanged += textBoxName_TextChanged;
        // 
        // button1
        // 
        button1.Location = new System.Drawing.Point(209, 229);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(75, 23);
        button1.TabIndex = 2;
        button1.Text = "Save";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // comboBoxProvider
        // 
        comboBoxProvider.FormattingEnabled = true;
        comboBoxProvider.Location = new System.Drawing.Point(12, 76);
        comboBoxProvider.Name = "comboBoxProvider";
        comboBoxProvider.Size = new System.Drawing.Size(272, 23);
        comboBoxProvider.TabIndex = 3;
        comboBoxProvider.SelectedIndexChanged += comboBoxProvider_SelectedIndexChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(12, 58);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(51, 15);
        label2.TabIndex = 0;
        label2.Text = "Provider";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(12, 113);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(100, 15);
        label3.TabIndex = 4;
        label3.Text = "ConnectionString";
        // 
        // textBoxConnectionString
        // 
        textBoxConnectionString.Location = new System.Drawing.Point(12, 132);
        textBoxConnectionString.Multiline = true;
        textBoxConnectionString.Name = "textBoxConnectionString";
        textBoxConnectionString.Size = new System.Drawing.Size(272, 91);
        textBoxConnectionString.TabIndex = 1;
        textBoxConnectionString.TextChanged += textBoxName_TextChanged;
        // 
        // DatabaseEditForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(300, 264);
        Controls.Add(label3);
        Controls.Add(comboBoxProvider);
        Controls.Add(button1);
        Controls.Add(textBoxConnectionString);
        Controls.Add(textBoxName);
        Controls.Add(label2);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        Name = "DatabaseEditForm";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Database";
        FormClosing += DatabaseEditForm_FormClosing;
        Load += DatabaseEditForm_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.ComboBox comboBoxProvider;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textBoxConnectionString;
}
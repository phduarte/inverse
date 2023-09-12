namespace Inverse.Desktop
{
    partial class FrmTableEdit
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
            lblName = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            tabPage2 = new System.Windows.Forms.TabPage();
            txtNotes = new System.Windows.Forms.TextBox();
            ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Required = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ColumnPK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ColumnReferences = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(12, 9);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(39, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(12, 35);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(402, 23);
            txtName.TabIndex = 1;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new System.Drawing.Point(12, 64);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(844, 381);
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(836, 353);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Columns";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { ColumnName, ColumnDescription, ColumnType, Required, ColumnPK, ColumnReferences, ColumnTag });
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new System.Drawing.Size(830, 347);
            dataGridView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(txtNotes);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(836, 353);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Notes";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            txtNotes.Location = new System.Drawing.Point(3, 3);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new System.Drawing.Size(830, 347);
            txtNotes.TabIndex = 0;
            // 
            // ColumnName
            // 
            ColumnName.HeaderText = "Name";
            ColumnName.Name = "ColumnName";
            // 
            // ColumnDescription
            // 
            ColumnDescription.HeaderText = "Description";
            ColumnDescription.Name = "ColumnDescription";
            // 
            // ColumnType
            // 
            ColumnType.HeaderText = "Type";
            ColumnType.Name = "ColumnType";
            // 
            // Required
            // 
            Required.HeaderText = "Required";
            Required.Name = "Required";
            // 
            // ColumnPK
            // 
            ColumnPK.HeaderText = "Key";
            ColumnPK.Name = "ColumnPK";
            // 
            // ColumnReferences
            // 
            ColumnReferences.HeaderText = "References";
            ColumnReferences.Name = "ColumnReferences";
            ColumnReferences.ReadOnly = true;
            // 
            // ColumnTag
            // 
            ColumnTag.HeaderText = "Tag";
            ColumnTag.Name = "ColumnTag";
            ColumnTag.Visible = false;
            // 
            // FrmTableEdit
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(868, 457);
            Controls.Add(tabControl1);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Name = "FrmTableEdit";
            Text = "Table";
            FormClosing += FrmTableEdit_FormClosing;
            Load += FrmTableEdit_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Required;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnPK;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReferences;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTag;
    }
}
namespace Inverse.Desktop;

partial class TableForm
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
        components = new System.ComponentModel.Container();
        lblName = new System.Windows.Forms.Label();
        txtName = new System.Windows.Forms.TextBox();
        tabControl1 = new System.Windows.Forms.TabControl();
        tabPageColumns = new System.Windows.Forms.TabPage();
        dataGridView1 = new System.Windows.Forms.DataGridView();
        ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
        dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
        Required = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        ColumnPK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        DefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
        ColumnReferences = new System.Windows.Forms.DataGridViewTextBoxColumn();
        ColumnTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
        contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
        editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        removeFKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        tabPageSeed = new System.Windows.Forms.TabPage();
        dataGridViewSeed = new System.Windows.Forms.DataGridView();
        tabPageNotes = new System.Windows.Forms.TabPage();
        flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        button1 = new System.Windows.Forms.Button();
        txtNote = new System.Windows.Forms.TextBox();
        tabControl1.SuspendLayout();
        tabPageColumns.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        contextMenuStrip1.SuspendLayout();
        tabPageSeed.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridViewSeed).BeginInit();
        tabPageNotes.SuspendLayout();
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
        txtName.KeyDown += txtName_KeyDown;
        // 
        // tabControl1
        // 
        tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        tabControl1.Controls.Add(tabPageColumns);
        tabControl1.Controls.Add(tabPageSeed);
        tabControl1.Controls.Add(tabPageNotes);
        tabControl1.Location = new System.Drawing.Point(12, 64);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new System.Drawing.Size(938, 381);
        tabControl1.TabIndex = 2;
        // 
        // tabPageColumns
        // 
        tabPageColumns.Controls.Add(dataGridView1);
        tabPageColumns.Location = new System.Drawing.Point(4, 24);
        tabPageColumns.Name = "tabPageColumns";
        tabPageColumns.Padding = new System.Windows.Forms.Padding(3);
        tabPageColumns.Size = new System.Drawing.Size(930, 353);
        tabPageColumns.TabIndex = 0;
        tabPageColumns.Text = "Columns";
        tabPageColumns.UseVisualStyleBackColor = true;
        // 
        // dataGridView1
        // 
        dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { ColumnName, ColumnDescription, dataGridViewComboBoxColumn1, Required, ColumnPK, DefaultValue, ColumnReferences, ColumnTag });
        dataGridView1.ContextMenuStrip = contextMenuStrip1;
        dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
        dataGridView1.Location = new System.Drawing.Point(3, 3);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new System.Drawing.Size(924, 347);
        dataGridView1.TabIndex = 0;
        dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        dataGridView1.RowsAdded += dataGridView1_RowsAdded;
        dataGridView1.RowsRemoved += dataGridView1_RowsRemoved;
        dataGridView1.UserAddedRow += dataGridView1_UserAddedRow;
        dataGridView1.UserDeletedRow += dataGridView1_UserDeletedRow;
        dataGridView1.UserDeletingRow += dataGridView1_UserDeletingRow;
        // 
        // ColumnName
        // 
        ColumnName.HeaderText = "Name";
        ColumnName.Name = "ColumnName";
        ColumnName.Width = 150;
        // 
        // ColumnDescription
        // 
        ColumnDescription.HeaderText = "Description";
        ColumnDescription.Name = "ColumnDescription";
        ColumnDescription.Width = 230;
        // 
        // dataGridViewComboBoxColumn1
        // 
        dataGridViewComboBoxColumn1.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
        dataGridViewComboBoxColumn1.HeaderText = "Type";
        dataGridViewComboBoxColumn1.Items.AddRange(new object[] { "int", "bigint", "bit", "decimal", "money", "smallmoney", "real", "date", "time", "datetime2", "datetimeoffset", "smalldate", "tinyint", "long", "numeric", "smallint", "uniqueidentifier", "datetime", "varchar(10)", "varchar(20)", "varchar(30)", "varchar(40)", "varchar(50)", "varchar(100)", "varchar(200)", "varchar(255)", "varchar(300)", "varchar(400)", "varchar(500)", "char", "text", "nvarchar", "nchar", "ntext", "binary", "image", "varbinary", "xml", "rowversion", "table", "cursor", "sql_variant", "hierarchyid", "geometry", "geography" });
        dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
        dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
        dataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
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
        // DefaultValue
        // 
        DefaultValue.HeaderText = "Default";
        DefaultValue.Name = "DefaultValue";
        // 
        // ColumnReferences
        // 
        ColumnReferences.HeaderText = "FK";
        ColumnReferences.Name = "ColumnReferences";
        ColumnReferences.ReadOnly = true;
        // 
        // ColumnTag
        // 
        ColumnTag.HeaderText = "Tag";
        ColumnTag.Name = "ColumnTag";
        ColumnTag.Visible = false;
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem, moveUpToolStripMenuItem, moveDownToolStripMenuItem, removeFKToolStripMenuItem });
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new System.Drawing.Size(139, 114);
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
        editToolStripMenuItem.Text = "Edit";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // deleteToolStripMenuItem
        // 
        deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
        deleteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
        deleteToolStripMenuItem.Text = "Delete";
        deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
        // 
        // moveUpToolStripMenuItem
        // 
        moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
        moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
        moveUpToolStripMenuItem.Text = "Move Up";
        moveUpToolStripMenuItem.Click += moveUpToolStripMenuItem_Click;
        // 
        // moveDownToolStripMenuItem
        // 
        moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
        moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
        moveDownToolStripMenuItem.Text = "Move Down";
        moveDownToolStripMenuItem.Click += moveDownToolStripMenuItem_Click;
        // 
        // removeFKToolStripMenuItem
        // 
        removeFKToolStripMenuItem.Name = "removeFKToolStripMenuItem";
        removeFKToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
        removeFKToolStripMenuItem.Text = "Remove FK";
        removeFKToolStripMenuItem.Click += removeFKToolStripMenuItem_Click;
        // 
        // tabPageSeed
        // 
        tabPageSeed.Controls.Add(dataGridViewSeed);
        tabPageSeed.Location = new System.Drawing.Point(4, 24);
        tabPageSeed.Name = "tabPageSeed";
        tabPageSeed.Padding = new System.Windows.Forms.Padding(3);
        tabPageSeed.Size = new System.Drawing.Size(930, 353);
        tabPageSeed.TabIndex = 2;
        tabPageSeed.Text = "Seed";
        tabPageSeed.UseVisualStyleBackColor = true;
        // 
        // dataGridViewSeed
        // 
        dataGridViewSeed.AllowUserToOrderColumns = true;
        dataGridViewSeed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridViewSeed.Dock = System.Windows.Forms.DockStyle.Fill;
        dataGridViewSeed.Location = new System.Drawing.Point(3, 3);
        dataGridViewSeed.Name = "dataGridViewSeed";
        dataGridViewSeed.Size = new System.Drawing.Size(924, 347);
        dataGridViewSeed.TabIndex = 0;
        // 
        // tabPageNotes
        // 
        tabPageNotes.Controls.Add(flowLayoutPanel1);
        tabPageNotes.Controls.Add(button1);
        tabPageNotes.Controls.Add(txtNote);
        tabPageNotes.Location = new System.Drawing.Point(4, 24);
        tabPageNotes.Name = "tabPageNotes";
        tabPageNotes.Padding = new System.Windows.Forms.Padding(3);
        tabPageNotes.Size = new System.Drawing.Size(930, 353);
        tabPageNotes.TabIndex = 1;
        tabPageNotes.Text = "Notes";
        tabPageNotes.UseVisualStyleBackColor = true;
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        flowLayoutPanel1.AutoScroll = true;
        flowLayoutPanel1.Location = new System.Drawing.Point(3, 6);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new System.Drawing.Size(924, 289);
        flowLayoutPanel1.TabIndex = 3;
        // 
        // button1
        // 
        button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        button1.Location = new System.Drawing.Point(848, 301);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(79, 49);
        button1.TabIndex = 2;
        button1.Text = "Save";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // txtNote
        // 
        txtNote.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        txtNote.Location = new System.Drawing.Point(3, 301);
        txtNote.Multiline = true;
        txtNote.Name = "txtNote";
        txtNote.Size = new System.Drawing.Size(839, 49);
        txtNote.TabIndex = 1;
        txtNote.KeyDown += txtNote_KeyDown;
        // 
        // TableForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(962, 457);
        Controls.Add(tabControl1);
        Controls.Add(txtName);
        Controls.Add(lblName);
        KeyPreview = true;
        Name = "TableForm";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Table";
        FormClosing += FrmTableEdit_FormClosing;
        Load += FrmTableEdit_Load;
        tabControl1.ResumeLayout(false);
        tabPageColumns.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        contextMenuStrip1.ResumeLayout(false);
        tabPageSeed.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridViewSeed).EndInit();
        tabPageNotes.ResumeLayout(false);
        tabPageNotes.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPageColumns;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.TabPage tabPageNotes;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox txtNote;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeFKToolStripMenuItem;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescription;
    private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
    private System.Windows.Forms.DataGridViewCheckBoxColumn Required;
    private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnPK;
    private System.Windows.Forms.DataGridViewTextBoxColumn DefaultValue;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReferences;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTag;
    private System.Windows.Forms.TabPage tabPageSeed;
    private System.Windows.Forms.DataGridView dataGridViewSeed;
}

namespace Inverse.Desktop;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        panel1 = new System.Windows.Forms.Panel();
        contextMenuStripDatabase = new System.Windows.Forms.ContextMenuStrip(components);
        addTableToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
        propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        contextMenuStripTable = new System.Windows.Forms.ContextMenuStrip(components);
        editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
        bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        sendToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
        deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        statusStrip1 = new System.Windows.Forms.StatusStrip();
        toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        menuStrip1 = new System.Windows.Forms.MenuStrip();
        fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        emptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
        saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        addTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
        databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        diagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        arrangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        releaseTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
        readOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        showHiddenTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        showToolTipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
        cardinalityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        noneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
        numberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        bachmanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        crowsFeetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        iDEF1XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
        windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        hideMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolTip1 = new System.Windows.Forms.ToolTip(components);
        contextMenuStripColumn = new System.Windows.Forms.ContextMenuStrip(components);
        editToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
        setAsPrimaryKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        setAsForeignKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        removeForeignKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        removePrimaryKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        contextMenuStripDatabase.SuspendLayout();
        contextMenuStripTable.SuspendLayout();
        flowLayoutPanel1.SuspendLayout();
        statusStrip1.SuspendLayout();
        menuStrip1.SuspendLayout();
        contextMenuStripColumn.SuspendLayout();
        SuspendLayout();
        // 
        // panel1
        // 
        panel1.BackColor = System.Drawing.Color.White;
        panel1.ContextMenuStrip = contextMenuStripDatabase;
        panel1.Location = new System.Drawing.Point(3, 3);
        panel1.Name = "panel1";
        panel1.Size = new System.Drawing.Size(1066, 462);
        panel1.TabIndex = 0;
        panel1.Paint += panel1_Paint;
        panel1.MouseDoubleClick += panel1_MouseDoubleClick;
        panel1.MouseDown += panel1_MouseDown;
        panel1.MouseMove += panel1_MouseMove;
        panel1.MouseUp += panel1_MouseUp;
        // 
        // contextMenuStripDatabase
        // 
        contextMenuStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addTableToolStripMenuItem1, toolStripSeparator11, propertiesToolStripMenuItem });
        contextMenuStripDatabase.Name = "contextMenuStrip2";
        contextMenuStripDatabase.Size = new System.Drawing.Size(128, 54);
        contextMenuStripDatabase.Opening += contextMenuStripDatabase_Opening;
        // 
        // addTableToolStripMenuItem1
        // 
        addTableToolStripMenuItem1.Name = "addTableToolStripMenuItem1";
        addTableToolStripMenuItem1.Size = new System.Drawing.Size(127, 22);
        addTableToolStripMenuItem1.Text = "Add Table";
        addTableToolStripMenuItem1.Click += addTableToolStripMenuItem_Click;
        // 
        // toolStripSeparator11
        // 
        toolStripSeparator11.Name = "toolStripSeparator11";
        toolStripSeparator11.Size = new System.Drawing.Size(124, 6);
        // 
        // propertiesToolStripMenuItem
        // 
        propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
        propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
        propertiesToolStripMenuItem.Text = "Properties";
        propertiesToolStripMenuItem.Click += propertiesToolStripMenuItem_Click;
        // 
        // contextMenuStripTable
        // 
        contextMenuStripTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { editToolStripMenuItem, hideToolStripMenuItem, showToolStripMenuItem, toolStripSeparator5, bringToFrontToolStripMenuItem, sendToBackToolStripMenuItem, toolStripSeparator6, deleteToolStripMenuItem });
        contextMenuStripTable.Name = "contextMenuStrip1";
        contextMenuStripTable.Size = new System.Drawing.Size(148, 148);
        contextMenuStripTable.Opening += contextMenuStrip1_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
        editToolStripMenuItem.Text = "&Edit";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // hideToolStripMenuItem
        // 
        hideToolStripMenuItem.Name = "hideToolStripMenuItem";
        hideToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
        hideToolStripMenuItem.Text = "&Hide";
        hideToolStripMenuItem.Click += hideToolStripMenuItem_Click;
        // 
        // showToolStripMenuItem
        // 
        showToolStripMenuItem.Name = "showToolStripMenuItem";
        showToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
        showToolStripMenuItem.Text = "&Show";
        showToolStripMenuItem.Click += showToolStripMenuItem_Click;
        // 
        // toolStripSeparator5
        // 
        toolStripSeparator5.Name = "toolStripSeparator5";
        toolStripSeparator5.Size = new System.Drawing.Size(144, 6);
        // 
        // bringToFrontToolStripMenuItem
        // 
        bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
        bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
        bringToFrontToolStripMenuItem.Text = "Bring to Front";
        bringToFrontToolStripMenuItem.Click += bringToFrontToolStripMenuItem_Click;
        // 
        // sendToBackToolStripMenuItem
        // 
        sendToBackToolStripMenuItem.Name = "sendToBackToolStripMenuItem";
        sendToBackToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
        sendToBackToolStripMenuItem.Text = "Send to Back";
        sendToBackToolStripMenuItem.Click += sendToBackToolStripMenuItem_Click;
        // 
        // toolStripSeparator6
        // 
        toolStripSeparator6.Name = "toolStripSeparator6";
        toolStripSeparator6.Size = new System.Drawing.Size(144, 6);
        // 
        // deleteToolStripMenuItem
        // 
        deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
        deleteToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
        deleteToolStripMenuItem.Text = "&Delete";
        deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        flowLayoutPanel1.AutoScroll = true;
        flowLayoutPanel1.Controls.Add(panel1);
        flowLayoutPanel1.Location = new System.Drawing.Point(0, 24);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new System.Drawing.Size(1081, 487);
        flowLayoutPanel1.TabIndex = 0;
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
        statusStrip1.Location = new System.Drawing.Point(0, 512);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new System.Drawing.Size(1081, 22);
        statusStrip1.TabIndex = 8;
        statusStrip1.Text = "statusStrip1";
        // 
        // toolStripStatusLabel1
        // 
        toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
        toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        toolStripStatusLabel1.Size = new System.Drawing.Size(89, 17);
        toolStripStatusLabel1.Text = "Mouse position";
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem1, diagramToolStripMenuItem, themeToolStripMenuItem, windowToolStripMenuItem, helpToolStripMenuItem });
        menuStrip1.Location = new System.Drawing.Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new System.Drawing.Size(1081, 24);
        menuStrip1.TabIndex = 9;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripMenuItem, toolStripSeparator3, openToolStripMenuItem, closeToolStripMenuItem, toolStripSeparator4, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator2, exportToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        fileToolStripMenuItem.Text = "&File";
        // 
        // newToolStripMenuItem
        // 
        newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { projectToolStripMenuItem, emptyToolStripMenuItem });
        newToolStripMenuItem.Name = "newToolStripMenuItem";
        newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
        newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
        newToolStripMenuItem.Text = "&New";
        // 
        // projectToolStripMenuItem
        // 
        projectToolStripMenuItem.Name = "projectToolStripMenuItem";
        projectToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
        projectToolStripMenuItem.Text = "Project";
        projectToolStripMenuItem.Click += newToolStripMenuItem_Click;
        // 
        // emptyToolStripMenuItem
        // 
        emptyToolStripMenuItem.Name = "emptyToolStripMenuItem";
        emptyToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
        emptyToolStripMenuItem.Text = "Blank";
        emptyToolStripMenuItem.Click += emptyToolStripMenuItem_Click;
        // 
        // toolStripSeparator3
        // 
        toolStripSeparator3.Name = "toolStripSeparator3";
        toolStripSeparator3.Size = new System.Drawing.Size(143, 6);
        // 
        // openToolStripMenuItem
        // 
        openToolStripMenuItem.Name = "openToolStripMenuItem";
        openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
        openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
        openToolStripMenuItem.Text = "Open";
        openToolStripMenuItem.Click += openToolStripMenuItem_Click;
        // 
        // closeToolStripMenuItem
        // 
        closeToolStripMenuItem.Enabled = false;
        closeToolStripMenuItem.Name = "closeToolStripMenuItem";
        closeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
        closeToolStripMenuItem.Text = "Close";
        closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
        // 
        // toolStripSeparator4
        // 
        toolStripSeparator4.Name = "toolStripSeparator4";
        toolStripSeparator4.Size = new System.Drawing.Size(143, 6);
        // 
        // saveToolStripMenuItem
        // 
        saveToolStripMenuItem.Enabled = false;
        saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
        saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
        saveToolStripMenuItem.Text = "Save";
        saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
        // 
        // saveAsToolStripMenuItem
        // 
        saveAsToolStripMenuItem.Enabled = false;
        saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
        saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
        saveAsToolStripMenuItem.Text = "Save as";
        saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
        // 
        // exportToolStripMenuItem
        // 
        exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { scriptToolStripMenuItem, imageToolStripMenuItem });
        exportToolStripMenuItem.Enabled = false;
        exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        exportToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
        exportToolStripMenuItem.Text = "Export";
        // 
        // scriptToolStripMenuItem
        // 
        scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
        scriptToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
        scriptToolStripMenuItem.Text = "Script";
        scriptToolStripMenuItem.Click += scriptToolStripMenuItem_Click;
        // 
        // imageToolStripMenuItem
        // 
        imageToolStripMenuItem.Name = "imageToolStripMenuItem";
        imageToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
        imageToolStripMenuItem.Text = "Image";
        imageToolStripMenuItem.Click += imageToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
        exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // editToolStripMenuItem1
        // 
        editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { addTableToolStripMenuItem, toolStripSeparator9, databaseToolStripMenuItem });
        editToolStripMenuItem1.Name = "editToolStripMenuItem1";
        editToolStripMenuItem1.Size = new System.Drawing.Size(39, 20);
        editToolStripMenuItem1.Text = "Edit";
        // 
        // addTableToolStripMenuItem
        // 
        addTableToolStripMenuItem.Name = "addTableToolStripMenuItem";
        addTableToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
        addTableToolStripMenuItem.Text = "Add Table";
        addTableToolStripMenuItem.Click += addTableToolStripMenuItem_Click;
        // 
        // toolStripSeparator9
        // 
        toolStripSeparator9.Name = "toolStripSeparator9";
        toolStripSeparator9.Size = new System.Drawing.Size(124, 6);
        // 
        // databaseToolStripMenuItem
        // 
        databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
        databaseToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
        databaseToolStripMenuItem.Text = "Database";
        databaseToolStripMenuItem.Click += databaseToolStripMenuItem_Click;
        // 
        // diagramToolStripMenuItem
        // 
        diagramToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { refreshToolStripMenuItem, selectAllToolStripMenuItem, arrangeToolStripMenuItem, releaseTablesToolStripMenuItem, toolStripSeparator7, readOnlyToolStripMenuItem, showHiddenTablesToolStripMenuItem, showToolTipsToolStripMenuItem, toolStripSeparator8, cardinalityToolStripMenuItem });
        diagramToolStripMenuItem.Name = "diagramToolStripMenuItem";
        diagramToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
        diagramToolStripMenuItem.Text = "Diagram";
        // 
        // refreshToolStripMenuItem
        // 
        refreshToolStripMenuItem.Enabled = false;
        refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
        refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
        refreshToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        refreshToolStripMenuItem.Text = "Refresh";
        refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
        // 
        // selectAllToolStripMenuItem
        // 
        selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
        selectAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
        selectAllToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        selectAllToolStripMenuItem.Text = "Select All";
        selectAllToolStripMenuItem.Click += selectAllToolStripMenuItem_Click;
        // 
        // arrangeToolStripMenuItem
        // 
        arrangeToolStripMenuItem.Name = "arrangeToolStripMenuItem";
        arrangeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        arrangeToolStripMenuItem.Text = "Arrange";
        arrangeToolStripMenuItem.Click += arrangeToolStripMenuItem_Click;
        // 
        // releaseTablesToolStripMenuItem
        // 
        releaseTablesToolStripMenuItem.Enabled = false;
        releaseTablesToolStripMenuItem.Name = "releaseTablesToolStripMenuItem";
        releaseTablesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        releaseTablesToolStripMenuItem.Text = "Release Tables";
        releaseTablesToolStripMenuItem.Click += releaseTablesToolStripMenuItem_Click;
        // 
        // toolStripSeparator7
        // 
        toolStripSeparator7.Name = "toolStripSeparator7";
        toolStripSeparator7.Size = new System.Drawing.Size(178, 6);
        // 
        // readOnlyToolStripMenuItem
        // 
        readOnlyToolStripMenuItem.CheckOnClick = true;
        readOnlyToolStripMenuItem.Name = "readOnlyToolStripMenuItem";
        readOnlyToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        readOnlyToolStripMenuItem.Text = "Read Only";
        readOnlyToolStripMenuItem.Click += readOnlyToolStripMenuItem_Click;
        // 
        // showHiddenTablesToolStripMenuItem
        // 
        showHiddenTablesToolStripMenuItem.CheckOnClick = true;
        showHiddenTablesToolStripMenuItem.Name = "showHiddenTablesToolStripMenuItem";
        showHiddenTablesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        showHiddenTablesToolStripMenuItem.Text = "Show Hidden Tables";
        showHiddenTablesToolStripMenuItem.CheckStateChanged += showHiddenTablesToolStripMenuItem_CheckStateChanged;
        // 
        // showToolTipsToolStripMenuItem
        // 
        showToolTipsToolStripMenuItem.Checked = true;
        showToolTipsToolStripMenuItem.CheckOnClick = true;
        showToolTipsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        showToolTipsToolStripMenuItem.Name = "showToolTipsToolStripMenuItem";
        showToolTipsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        showToolTipsToolStripMenuItem.Text = "Show Tool Tips";
        // 
        // toolStripSeparator8
        // 
        toolStripSeparator8.Name = "toolStripSeparator8";
        toolStripSeparator8.Size = new System.Drawing.Size(178, 6);
        // 
        // cardinalityToolStripMenuItem
        // 
        cardinalityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { noneToolStripMenuItem1, toolStripSeparator12, numberToolStripMenuItem, bachmanToolStripMenuItem, crowsFeetToolStripMenuItem, iDEF1XToolStripMenuItem });
        cardinalityToolStripMenuItem.Name = "cardinalityToolStripMenuItem";
        cardinalityToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        cardinalityToolStripMenuItem.Text = "Cardinality";
        // 
        // noneToolStripMenuItem1
        // 
        noneToolStripMenuItem1.CheckOnClick = true;
        noneToolStripMenuItem1.Name = "noneToolStripMenuItem1";
        noneToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
        noneToolStripMenuItem1.Text = "None";
        noneToolStripMenuItem1.Click += noneToolStripMenuItem1_Click;
        // 
        // toolStripSeparator12
        // 
        toolStripSeparator12.Name = "toolStripSeparator12";
        toolStripSeparator12.Size = new System.Drawing.Size(134, 6);
        // 
        // numberToolStripMenuItem
        // 
        numberToolStripMenuItem.CheckOnClick = true;
        numberToolStripMenuItem.Name = "numberToolStripMenuItem";
        numberToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        numberToolStripMenuItem.Text = "UML";
        numberToolStripMenuItem.Click += umlToolStripMenuItem_Click;
        // 
        // bachmanToolStripMenuItem
        // 
        bachmanToolStripMenuItem.Enabled = false;
        bachmanToolStripMenuItem.Name = "bachmanToolStripMenuItem";
        bachmanToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        bachmanToolStripMenuItem.Text = "Bachman";
        // 
        // crowsFeetToolStripMenuItem
        // 
        crowsFeetToolStripMenuItem.Checked = true;
        crowsFeetToolStripMenuItem.CheckOnClick = true;
        crowsFeetToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        crowsFeetToolStripMenuItem.Name = "crowsFeetToolStripMenuItem";
        crowsFeetToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        crowsFeetToolStripMenuItem.Text = "Crow's Foot";
        crowsFeetToolStripMenuItem.Click += crowsFootToolStripMenuItem_Click;
        // 
        // iDEF1XToolStripMenuItem
        // 
        iDEF1XToolStripMenuItem.Enabled = false;
        iDEF1XToolStripMenuItem.Name = "iDEF1XToolStripMenuItem";
        iDEF1XToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
        iDEF1XToolStripMenuItem.Text = "IDEF1X";
        // 
        // themeToolStripMenuItem
        // 
        themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { defaultToolStripMenuItem, toolStripSeparator10 });
        themeToolStripMenuItem.Name = "themeToolStripMenuItem";
        themeToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
        themeToolStripMenuItem.Text = "Theme";
        // 
        // defaultToolStripMenuItem
        // 
        defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
        defaultToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
        defaultToolStripMenuItem.Text = "Default";
        defaultToolStripMenuItem.Click += defaultToolStripMenuItem_Click;
        // 
        // toolStripSeparator10
        // 
        toolStripSeparator10.Name = "toolStripSeparator10";
        toolStripSeparator10.Size = new System.Drawing.Size(109, 6);
        // 
        // windowToolStripMenuItem
        // 
        windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { fullScreenToolStripMenuItem, hideMenuToolStripMenuItem });
        windowToolStripMenuItem.Name = "windowToolStripMenuItem";
        windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
        windowToolStripMenuItem.Text = "Window";
        // 
        // fullScreenToolStripMenuItem
        // 
        fullScreenToolStripMenuItem.CheckOnClick = true;
        fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
        fullScreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
        fullScreenToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
        fullScreenToolStripMenuItem.Text = "Full Screen";
        fullScreenToolStripMenuItem.Click += fullScreenToolStripMenuItem_Click;
        // 
        // hideMenuToolStripMenuItem
        // 
        hideMenuToolStripMenuItem.Name = "hideMenuToolStripMenuItem";
        hideMenuToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Space;
        hideMenuToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
        hideMenuToolStripMenuItem.Text = "Hide Menu";
        hideMenuToolStripMenuItem.Click += hideMenuToolStripMenuItem_Click;
        // 
        // helpToolStripMenuItem
        // 
        helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
        helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        helpToolStripMenuItem.Text = "Help";
        // 
        // aboutToolStripMenuItem
        // 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
        aboutToolStripMenuItem.Text = "About";
        aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
        // 
        // contextMenuStripColumn
        // 
        contextMenuStripColumn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { editToolStripMenuItem2, setAsPrimaryKeyToolStripMenuItem, setAsForeignKeyToolStripMenuItem, removeForeignKeyToolStripMenuItem, removePrimaryKeyToolStripMenuItem });
        contextMenuStripColumn.Name = "contextMenuStripColumn";
        contextMenuStripColumn.Size = new System.Drawing.Size(183, 136);
        contextMenuStripColumn.Opening += contextMenuStripColumn_Opening;
        // 
        // editToolStripMenuItem2
        // 
        editToolStripMenuItem2.Name = "editToolStripMenuItem2";
        editToolStripMenuItem2.Size = new System.Drawing.Size(182, 22);
        editToolStripMenuItem2.Text = "Edit";
        editToolStripMenuItem2.Click += editToolStripMenuItem2_Click;
        // 
        // setAsPrimaryKeyToolStripMenuItem
        // 
        setAsPrimaryKeyToolStripMenuItem.Name = "setAsPrimaryKeyToolStripMenuItem";
        setAsPrimaryKeyToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
        setAsPrimaryKeyToolStripMenuItem.Text = "Set as Primary Key";
        setAsPrimaryKeyToolStripMenuItem.Click += setAsPrimaryKeyToolStripMenuItem_Click;
        // 
        // setAsForeignKeyToolStripMenuItem
        // 
        setAsForeignKeyToolStripMenuItem.Name = "setAsForeignKeyToolStripMenuItem";
        setAsForeignKeyToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
        setAsForeignKeyToolStripMenuItem.Text = "Set as Foreign Key";
        setAsForeignKeyToolStripMenuItem.Click += setAsForeignKeyToolStripMenuItem_Click;
        // 
        // removeForeignKeyToolStripMenuItem
        // 
        removeForeignKeyToolStripMenuItem.Name = "removeForeignKeyToolStripMenuItem";
        removeForeignKeyToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
        removeForeignKeyToolStripMenuItem.Text = "Remove Foreign Key";
        removeForeignKeyToolStripMenuItem.Click += removeForeignKeyToolStripMenuItem_Click;
        // 
        // removePrimaryKeyToolStripMenuItem
        // 
        removePrimaryKeyToolStripMenuItem.Name = "removePrimaryKeyToolStripMenuItem";
        removePrimaryKeyToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
        removePrimaryKeyToolStripMenuItem.Text = "Remove Primary key";
        removePrimaryKeyToolStripMenuItem.Click += removePrimaryKeyToolStripMenuItem_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.White;
        ClientSize = new System.Drawing.Size(1081, 534);
        Controls.Add(statusStrip1);
        Controls.Add(menuStrip1);
        Controls.Add(flowLayoutPanel1);
        Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        MainMenuStrip = menuStrip1;
        Name = "MainForm";
        Text = "InverseDB";
        WindowState = System.Windows.Forms.FormWindowState.Maximized;
        FormClosing += MainForm_FormClosing;
        Load += MainForm_Load;
        SizeChanged += Main_SizeChanged;
        KeyDown += MainForm_KeyDown;
        KeyUp += MainForm_KeyUp;
        contextMenuStripDatabase.ResumeLayout(false);
        contextMenuStripTable.ResumeLayout(false);
        flowLayoutPanel1.ResumeLayout(false);
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        contextMenuStripColumn.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripTable;
    private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem diagramToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showHiddenTablesToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem arrangeToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem bringToFrontToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem sendToBackToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripDatabase;
    private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem releaseTablesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cardinalityToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem numberToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem crowsFeetToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    private System.Windows.Forms.ToolStripMenuItem showToolTipsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem addTableToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
    private System.Windows.Forms.ToolStripMenuItem readOnlyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
    private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem hideMenuToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addTableToolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
    private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
    private System.Windows.Forms.ToolStripMenuItem bachmanToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem iDEF1XToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem emptyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripColumn;
    private System.Windows.Forms.ToolStripMenuItem setAsPrimaryKeyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem setAsForeignKeyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem removeForeignKeyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removePrimaryKeyToolStripMenuItem;
}


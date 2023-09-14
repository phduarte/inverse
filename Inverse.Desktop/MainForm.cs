using Inverse.Application;
using Inverse.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class MainForm : Form
    {
        private readonly IDatabaseService _databaseService;
        private readonly StringFormat _textAlignLeft = new(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Near
        };
        private readonly StringFormat _textAlignRight = new(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Far
        };
        private readonly StringFormat _textAlignLeftTop = new(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Near,
            Alignment = StringAlignment.Near
        };
        private readonly StringFormat _textAlignCenter = new(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };
        private readonly StringFormat _imageTextAlignCenter = new(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        private readonly StringFormat _imageTextSignature = new(StringFormatFlags.DirectionRightToLeft);

        private string _connectionString;
        private Provider _provider;
        private Database _database = new(Provider.MSSQLServer);
        private Point _pressedPoint = Point.Empty;
        private Table _currentTable;
        private Point _currentPoint = Point.Empty;
        private Point _pressedPointCorrection = Point.Empty;
        private readonly IList<Table> _selectedTables = new List<Table>();

        private readonly IList<TableViewStatus> _tablePositions = new List<TableViewStatus>();
        DateTime _lastUpdate = DateTime.MinValue;
        protected bool HasStateChange => _lastUpdate > DateTime.Now.AddSeconds(-5);

        private Theme Theme = new();

        public MainForm(params string[] args)
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _databaseService.InstallPlugins();

            panel1.SetDoubleBuffered();
            editToolStripMenuItem1.Visible = diagramToolStripMenuItem.Visible = false;

            if (args.Length > 0)
            {
                var filename = args.Select(s => new FileInfo(s)).Where(f => f.Exists).FirstOrDefault()?.FullName;

                if (!string.IsNullOrEmpty(filename))
                {
                    OpenFile(filename);
                }
                else
                if (GetValueFromArgs("-provider", args) is string p)
                {
                    var provider = Enum.Parse<Provider>(p);
                    var connectionString = GetValueFromArgs("-connectionstring", args);

                    UseDatabase(provider, connectionString);
                }

                ChangeTheme(GetValueFromArgs("-theme", args));
            }
        }

        private string GetValueFromArgs(string argName, params string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals(argName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return null;
        }

        public void UseDatabase(Database database)
        {
            _database = database;
            _connectionString = database.ConnectionString;
            _provider = database.Provider;
            Arrange().GetAwaiter();
            panel1.Refresh();
        }

        public void UseDatabase(Provider provider, string connectionString)
        {
            var database = _databaseService.LoadDatabase(provider, connectionString);
            _currentFilename = string.Empty;
            UseDatabase(database);
        }

        private async Task Arrange()
        {
            var left = Table.MARGIN;
            var top = Table.MARGIN;
            var tabelas = new List<Table>();
            const int DELAY = 0;

            panel1.SuspendLayout();

            foreach (var t in _database.Tables)
            {
                t.Hide();
                t.MoveTo(0, 0);
            }

            panel1.ResumeLayout();

            var mainTable = _database.Tables.OrderByDescending(_ => _.Columns.Count(_ => _.IsForeignKey && !_.IsPrimaryKey)).FirstOrDefault();

            if (mainTable != null)
            {
                var center = Width / 2;
                var middle = Height / 2;

                SetPosition(mainTable, ref center, ref middle);

                tabelas.Add(mainTable);
            }

            foreach (var t in _database.Tables)
            {
                if (tabelas.Contains(t))
                {
                    continue;
                }

                var relacionadas = t.ForeignKeys.Select(_ => _.RelatedTable);

                foreach (var r in relacionadas)
                {
                    var tb = _database.Tables.First(_ => _.Name.Equals(r));
                    var alturaDaMae = t.Top == 0 ? top : t.Top;

                    if (tabelas.Contains(tb))
                    {
                        continue;
                    }

                    SetPosition(tb, ref left, ref alturaDaMae);

                    tabelas.Add(tb);

                    await Task.Delay(DELAY);
                    panel1.Invalidate();
                }

                SetPosition(t, ref left, ref top);

                tabelas.Add(t);

                await Task.Delay(DELAY);
                panel1.Invalidate();
            }
        }

        private void SetPosition(Table table, ref int left, ref int top)
        {
            var intersect = false;

            do
            {
                var width = table.Columns.Select(x => x.Name.Length).Max() * LayoutDefinition.Chars.WIDTH;
                var height = (table.Columns.Count + 1) * Table.HEIGHT;
                var layout = new Rectangle(left, top, width, height);

                if (layout.Right > panel1.Width)
                {
                    left = Table.MARGIN;
                    top = _database.Tables.Max(_ => _.Bottom) + Table.MARGIN;
                }

                table.Left = layout.Left;
                table.Top = layout.Top;

                left += layout.Width + Table.MARGIN;

                var overridenTable = _database.Tables.Except(new List<Table> { table }).FirstOrDefault(x => x.IsHover(table.Left, table.Top));

                intersect = overridenTable is not null;

            } while (intersect);

            table.Show();
        }

        private void ResetPanelSize()
        {
            panel1.Invalidate();
            panel1.Width = flowLayoutPanel1.Width;
            panel1.Height = flowLayoutPanel1.Height;
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            ResetPanelSize();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = GetActiveTable() is null;
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetActiveTable() is Table activeTable)
            {
                activeTable.Hide();
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetActiveTable() is Table activeTable)
            {
                activeTable.Show();
            }
        }

        private Table GetActiveTable()
        {
            return _database.Tables.LastOrDefault(f => f.IsHover(_currentPoint.X, _currentPoint.Y));
        }

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetActiveTable() is Table table)
            {
                _database.BringToFront(table);
            }
        }

        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetActiveTable() is Table table)
            {
                _database.SendToBack(table);
            }
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderColor(Brushes.Gray);
            //panel1.Invalidate();
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderColor(Brushes.White);
            //panel1.Invalidate();
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderColor(Brushes.Black);
            //panel1.Invalidate();
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderColor(Brushes.Transparent);
            //panel1.Invalidate();
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderColor(Brushes.Blue);
            //panel1.Invalidate();
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderColor(Brushes.Orange);
            //panel1.Invalidate();
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderColor(Brushes.Red);
            //panel1.Invalidate();
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Portable Network Graphics|*.png",
                FileName = _database.Name
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filename = dialog.FileName;
                var format = System.Drawing.Imaging.ImageFormat.Png;
                var bmp = new Bitmap(panel1.Width, panel1.Height);

                panel1.SuspendLayout();

                try
                {
                    panel1.BorderStyle = BorderStyle.FixedSingle;
                    panel1.BorderStyle = BorderStyle.None;
                    panel1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                    var fonte = new Font("Arial", 10);
                    var g = Graphics.FromImage(bmp);

                    g.ScaleTransform(3, 3);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawString(_database.Name, fonte, Brushes.Black, new Rectangle(0, 0, panel1.Width, 30), _imageTextAlignCenter);
                    g.DrawString($"{Program.Name} - {DateTime.Today:dd/MM/yyyy}", fonte, Brushes.Black, new Rectangle(0, bmp.Height - 20, bmp.Width, 20), _imageTextSignature);
                    g.Flush();

                    bmp.Save(filename, format);

                    if (MessageBox.Show("Do you want to open the exported image file?", "Open file", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        OpenImage(filename);
                    }
                }
                finally
                {

                    panel1.ResumeLayout();
                }
            }
        }

        private bool UserWantsClose()
        {
            return MessageBox.Show("Unsaved changes will be lost. Are you sure?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void noneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem1.Checked = true;
            numberToolStripMenuItem.Checked = false;
            crowsFeetToolStripMenuItem.Checked = false;
        }

        private void numberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem1.Checked = false;
            numberToolStripMenuItem.Checked = true;
            crowsFeetToolStripMenuItem.Checked = false;
        }

        private void crowsFeetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem1.Checked = false;
            numberToolStripMenuItem.Checked = false;
            crowsFeetToolStripMenuItem.Checked = true;
        }

        private void UpdateStatus(string message)
        {
            toolStripStatusLabel1.Text = message;
            _lastUpdate = DateTime.Now;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (readOnlyToolStripMenuItem.Checked)
            {
                return;
            }

            if (GetActiveTable() is Table activeTable)
            {
                new TableForm(activeTable).ShowDialog();

                panel1.Invalidate();
            }
        }

        private void addTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (readOnlyToolStripMenuItem.Checked)
            {
                return;
            }

            var newTable = new Table
            {
                Name = "New Table",
            };

            _database.Add(newTable);

            panel1.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (readOnlyToolStripMenuItem.Checked)
            {
                return;
            }

            if (GetActiveTable() is Table activeTable)
            {
                if (MessageBox.Show($"You're trying to delete the table \"{activeTable.Name}\". \n\nAre you sure?", Program.Name, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _database.Remove(activeTable);
                }

                panel1.Invalidate();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private static void OpenImage(string imagePath)
        {
            var process = new Process();
            process.StartInfo.FileName = "explorer";
            process.StartInfo.Arguments = imagePath;
            process.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var themes = Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                .Select(x => new FileInfo(x))
                .Where(x => x.Name.StartsWith("Theme") && x.Extension.Equals(".json"));

            foreach (var theme in themes)
            {
                var split = theme.Name.Split('.');

                if (split.Length < 3)
                {
                    continue;
                }

                var menuItem = new ToolStripMenuItem();

                menuItem.Text = split[1];
                menuItem.Click += (s, e) =>
                {
                    ChangeTheme(menuItem.Text);
                };

                themeToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private void ChangeTheme(string selectedTheme)
        {
            Theme = ThemeManager.Load(selectedTheme);

            panel1.BackColor = Theme.Canvas.Background.AsColor();
            panel1.ForeColor = Theme.Canvas.Text.AsColor();
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTheme("");
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullScreenMode.Toggle(this, fullScreenToolStripMenuItem.Checked);
            statusStrip1.Visible = !fullScreenToolStripMenuItem.Checked;
        }

        private void hideMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip1.Visible = !menuStrip1.Visible;
        }
    }
}

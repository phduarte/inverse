using Inverse.Domain.Models;
using Inverse.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inverse.Plugin;
using System.Drawing.Drawing2D;

namespace Inverse.Windows
{
    public partial class FrmMain : Form
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

        private string _connectionString;
        private Provider _provider;
        private Database _database = new(Provider.MSSQLServer);
        private Point _pressedPoint = Point.Empty;
        private Table _currentTable;
        private Point _currentPoint = Point.Empty;
        private Point _pressedPointCorrection = Point.Empty;
        //private readonly IList<Table> _selectedTables = new List<Table>();
        //private readonly Control tempControl = new Button();

        public FrmMain()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _databaseService.InstallPlugins();

            panel1.SetDoubleBuffered();
            //flowLayoutPanel1.Controls.Add(tempControl);
            //flowLayoutPanel1.ScrollControlIntoView(tempControl);
            //flowLayoutPanel1.Controls.Remove(tempControl);
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
            var left = LayoutDefinition.Tables.MARGIN;
            var top = LayoutDefinition.Tables.MARGIN;
            var tabelas = new List<Table>();
            const int DELAY = 100;

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
                var height = (table.Columns.Count + 1) * LayoutDefinition.Columns.HEIGHT;
                var layout = new Rectangle(left, top, width, height);

                if (layout.Right > panel1.Width)
                {
                    left = LayoutDefinition.Tables.MARGIN;
                    top = _database.Tables.Max(_ => _.Bottom) + LayoutDefinition.Tables.MARGIN;
                }

                table.Left = layout.Left;
                table.Top = layout.Top;

                left += layout.Width + LayoutDefinition.Tables.MARGIN;

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
            Theme.Table.SetBorderColor(Brushes.Gray);
            panel1.Invalidate();
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Theme.Table.SetBorderColor(Brushes.White);
            panel1.Invalidate();
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Theme.Table.SetBorderColor(Brushes.Transparent);
            panel1.Invalidate();
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Theme.Table.SetBorderColor(Brushes.Transparent);
            panel1.Invalidate();
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Theme.Table.SetBorderColor(Brushes.Blue);
            panel1.Invalidate();
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Theme.Table.SetBorderColor(Brushes.Orange);
            panel1.Invalidate();
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Theme.Table.SetBorderColor(Brushes.Red);
            panel1.Invalidate();
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Portable Network Graphics|*.Png"
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

                    var fonte = new Font("Arial", 8);
                    var g = Graphics.FromImage(bmp);

                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawString($"InverseDB Studio - {DateTime.Today:dd/MM/yyyy}", fonte, Brushes.Black, new Rectangle(0, bmp.Height - 20, bmp.Width, 20), new StringFormat(StringFormatFlags.DirectionRightToLeft));
                    g.Flush();

                    bmp.Save(filename, format);
                }
                finally
                {
                    panel1.ResumeLayout();
                }
            }
        }
    }
}

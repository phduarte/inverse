using Inverse.Domain.Model;
using Inverse.Domain.Services;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public partial class FrmMain : Form
    {
        const int MARGIN = 50;
        readonly IDatabaseService _service;

        private string _connectionString;
        private Provider _provider;
        private Database _database = new Database(Provider.MSSQLServer);
        StringFormat _textFormat = new StringFormat(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Near
        };

        StringFormat _textFormatTitle = new StringFormat(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };
        Point _pressedPoint = Point.Empty;
        Table _currentTable = null;
        Point _currentPoint = Point.Empty;

        public FrmMain()
        {
            InitializeComponent();
            panel1.SetDoubleBuffered();
            _service = new DatabaseService();
        }

        public void UseDatabase(Database database)
        {
            _database = database;
            _connectionString = database.ConnectionString;
            _provider = database.Provider;
            Arrange();
            panel1.Refresh();
        }

        public void UseDatabase(Provider provider, string connectionString)
        {
            var database = _service.LoadDatabase(provider, connectionString);
            UseDatabase(database);
        }

        private void Arrange()
        {
            var left = MARGIN;
            var top = MARGIN;
            const int COLUMN_HEIGHT = 30;

            foreach (var t in _database.Tables.OrderByDescending(x => x.Columns.OfType<ForeignKey>().Count()))
            {
                var width = t.Columns.Select(x => x.Name.Length).Max() * 10;
                var height = (t.Columns.Count + 1) * COLUMN_HEIGHT;
                var layout = new Rectangle(left, top, width, height);

                t.Left = layout.Left;
                t.Top = layout.Top;

                left += layout.Width + MARGIN;
            }
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
            return _database.Tables.FirstOrDefault(f => f.IsHover(_currentPoint.X, _currentPoint.Y));
        }
    }
}

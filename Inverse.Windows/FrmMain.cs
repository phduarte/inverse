using Inverse.Domain.Model;
using Inverse.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public partial class FrmMain : Form
    {
        private readonly IDatabaseService _databaseService;

        private string _connectionString;
        private Provider _provider;
        private Database _database = new Database(Provider.MSSQLServer);
        private StringFormat _textFormat = new StringFormat(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Near
        };

        private StringFormat _textFormatTitle = new StringFormat(StringFormatFlags.NoClip)
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };
        private Point _pressedPoint = Point.Empty;
        private Table _currentTable = null;
        private Point _currentPoint = Point.Empty;

        public FrmMain()
        {
            InitializeComponent();
            panel1.SetDoubleBuffered();
            _databaseService = new DatabaseService();
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
            UseDatabase(database);
        }

        private const int COLUMN_HEIGHT = 30;

        private async Task Arrange()
        {
            var left = LayoutDefinition.Tables.MARGIN;
            var top = LayoutDefinition.Tables.MARGIN;
            var tabelas = new List<Table>();
            const int DELAY = 200;

            foreach (var t in _database.Tables)
            {
                t.MoveTo(0, 0);
            }

            var dic = new Dictionary<string, int>();

            foreach (var r in _database.Tables.SelectMany(c => c.ForeignKeys.Select(x => x.RelatedTable)))
            {
                if (dic.ContainsKey(r))
                {
                    dic[r]++;
                }
                else
                {
                    dic.Add(r, 1);
                }
            }

            // coloca a que tem mais relacionamentos no centro da tela.
            var maior = dic.OrderByDescending(x => x.Value).First();

            var temMais = _database.Tables.FirstOrDefault(x => x.Name.Equals(maior.Key));

            if (temMais != null)
            {
                var centro = Width / 2;
                var meio = Height / 2;

                SetPosition(temMais, ref centro, ref meio);

                tabelas.Add(temMais);
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
                var width = table.Columns.Select(x => x.Name.Length).Max() * 10;
                var height = (table.Columns.Count + 1) * COLUMN_HEIGHT;
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

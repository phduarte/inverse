using EngenhariaReversaDb.Domain.Model;
using EngenhariaReversaDb.Domain.Services;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace EngenhariaReversaDb.Windows
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_database.IsEmpty)
                return;

            var g = e.Graphics;
            var bordaTabela = new Pen(Brushes.Black, 2);
            var bordaTabelaSelecionada = new Pen(Brushes.Blue, 2);
            const int COLUMN_HEIGHT = 30;

            var linha = new Pen(Brushes.Black, 1);

            // desenhar relacionamentos
            foreach (var table in _database.Tables)
            {
                foreach (var source in table.Columns.OfType<ForeignKey>())
                {
                    var table2 = _database.Tables.First(x => x.Name.Equals(source.RelatedTable));
                    var target = table2.Columns.First(x => x.Name.Equals(source.RelatedColumn));

                    var isGoingToRight = table.Center < table2.Center;
                    var isGoingToLeft = table2.Center < table.Center;

                    if (isGoingToRight)
                    {
                        var midway = source.Right + ((target.Left - source.Right) / 2);

                        g.DrawLine(bordaTabela, midway, source.Middle, midway, target.Middle);
                        g.DrawLine(bordaTabela, source.Right, source.Middle, midway, source.Middle);
                        g.DrawLine(bordaTabela, midway, target.Middle, target.Left, target.Middle);
                    }
                    else if (isGoingToLeft)
                    {
                        var meio = target.Right + ((source.Left - target.Right) / 2);

                        g.DrawLine(bordaTabela, meio, source.Middle, meio, target.Middle);
                        g.DrawLine(bordaTabela, target.Right, target.Middle, meio, target.Middle);
                        g.DrawLine(bordaTabela, meio, source.Middle, source.Left, source.Middle);
                    }
                    else
                    {
                        g.DrawLine(bordaTabela, source.Right, source.Top, target.Left, target.Middle);
                    }

                    //if (target is PrimaryKey pk)
                    //{
                    //    if (isGoingToRight)
                    //    {
                    //        var areaNomeColunaPk = new RectangleF(target.Left - 20, target.Top, 50, 50);
                    //        g.DrawString("1", Font, Brushes.Black, areaNomeColunaPk, _textFormat);

                    //        var areaNomeColunaFk = new RectangleF(source.Right + 10, source.Top, 50, 50);
                    //        g.DrawString("N", Font, Brushes.Black, areaNomeColunaFk, _textFormat);
                    //    }
                    //    else
                    //    {
                    //        var areaNomeColunaPK = new RectangleF(target.Left - 20, target.Top, 50, 50);
                    //        g.DrawString("U", Font, Brushes.Black, areaNomeColunaPK, _textFormat);

                    //        var areaNomeColunaFk = new RectangleF(source.Right + 10, source.Top, 50, 50);
                    //        g.DrawString("0:N", Font, Brushes.Black, areaNomeColunaFk, _textFormat);
                    //    }
                    //}
                }
            }

            foreach (var table in _database.Tables.OrderByDescending(x => x.Columns.OfType<ForeignKey>().Count()))
            {
                var x = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                var layout = new Rectangle(table.Left, table.Top, table.Width, table.Height);

                g.FillRectangle(Brushes.White, layout);

                if (table.IsHover(_currentPoint.X, _currentPoint.Y))
                {
                    g.DrawRectangle(bordaTabelaSelecionada, layout);
                }
                else
                {
                    g.DrawRectangle(bordaTabela, layout);
                }


                // título da tabela
                var areaTitulo = new RectangleF(table.Left, table.Top, table.Width, COLUMN_HEIGHT);
                g.FillRectangle(Brushes.Gray, areaTitulo);
                g.DrawString(table.Name, Font, Brushes.White, areaTitulo, _textFormatTitle);

                const int MARGIN_COLUMN_NAME = 30;

                foreach (var col in table.Columns)
                {
                    var areaNomeColuna = new RectangleF(col.Left + MARGIN_COLUMN_NAME, col.Top, col.Width - MARGIN_COLUMN_NAME, col.Height);

                    if (col is ForeignKey || col is PrimaryKey)
                    {
                        var areaChaveColuna = new RectangleF(col.Left + 2, col.Top, MARGIN_COLUMN_NAME, col.Height);
                        g.DrawString(col.Prefix, new Font(Font, FontStyle.Bold), Brushes.Black, areaChaveColuna, _textFormat);
                    }

                    if (col.Required)
                    {
                        g.DrawString(col.Name, new Font(Font, FontStyle.Bold), Brushes.Black, areaNomeColuna, _textFormat);
                    }
                    else
                    {
                        g.DrawString(col.Name, Font, Brushes.Black, areaNomeColuna, _textFormat);
                    }

                    g.DrawLine(linha, new Point(col.Left, col.Top), new Point(col.Left + col.Width, col.Top));
                }

                g.SmoothingMode = x;
            }

            panel1.Width = Math.Max(flowLayoutPanel1.Width, _database.Tables.Max(x => x.Right) + MARGIN);
            panel1.Height = Math.Max(flowLayoutPanel1.Height, _database.Tables.Max(x => x.Bottom) + MARGIN);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pressedPoint = e.Location;
                _currentTable = _database.Tables.FirstOrDefault(x => x.IsHover(e.X, e.Y));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _currentPoint = e.Location;

            if (_pressedPoint != Point.Empty && _currentTable != null)
            {
                _currentTable.MoveTo(e.X, e.Y);
                panel1.Invalidate();
            }

            _currentTable = null;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentTable is not null)
            {
                _currentTable.MoveTo(e.X, e.Y);
            }

            panel1.Invalidate();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (_database.IsEmpty)
                return;

            var dialog = new SaveFileDialog
            {
                Filter = "Arquivo de Modelo de dados|*.dm",
                DefaultExt = ".dm"
            };

            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            using (var sw = new StreamWriter(dialog.FileName))
            {
                sw.WriteLine($"<database name=\"{_database.Name}\" id=\"{_database.Id}\" provider=\"{_database.Provider}\" connectionstring=\"{_database.ConnectionString}\">");
                sw.WriteLine($"    <tables>");
                foreach (var table in _database.Tables)
                {
                    sw.WriteLine($"        <table id=\"{table.Id}\" name=\"{table.Name}\" left=\"{table.Left}\" top=\"{table.Top}\">");
                    sw.WriteLine($"            <columns>");

                    foreach (var column in table.Columns)
                    {
                        if (column is ForeignKey fk)
                        {
                            sw.WriteLine($"                <column id=\"{column.Id}\" name=\"{column.Name}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.Required}\" class=\"{column.GetType().Name}\" relatedTable=\"{fk.RelatedTable}\" relatedColumn=\"{fk.RelatedColumn}\"/>");
                        }
                        else if (column is PrimaryKey pk)
                        {
                            sw.WriteLine($"                <column id=\"{pk.Id}\" name=\"{pk.Name}\" index=\"{pk.Index}\" type=\"{pk.Type}\" required=\"{pk.Required}\" class=\"{pk.GetType().Name}\" />");
                        }
                        else
                        {
                            sw.WriteLine($"                <column id=\"{column.Id}\" name=\"{column.Name}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.Required}\" class=\"{column.GetType().Name}\" />");
                        }
                    }

                    sw.WriteLine($"            </columns>");
                    sw.WriteLine($"        </table>");
                }
                sw.WriteLine($"    </tables>");
                sw.WriteLine($"</database>");
            }
        }

        private void btnLoad_Click(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Arquivo de Modelo de dados|*.dm";

            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.FileName))
            {
                var xml = new XmlDocument();
                xml.Load(dialog.FileName);
                var doc = xml.DocumentElement;

                var dbName = doc.GetAttribute("name");
                var dbGuid = doc.GetAttribute("id");
                var dbProvider = doc.GetAttribute("provider");
                var dbConnectionString = doc.GetAttribute("connectionstring");
                var dbId = Guid.Parse(dbGuid);

                var database = new Database(Enum.Parse<Provider>(dbProvider))
                {
                    Id = dbId,
                    Name = dbName,
                    ConnectionString = dbConnectionString
                };

                var tables = doc.SelectNodes("//table");

                foreach (XmlNode xmlTable in tables)
                {
                    var tbGuid = xmlTable.Attributes["id"]?.Value ?? Guid.NewGuid().ToString();
                    var tbName = xmlTable.Attributes["name"].Value;
                    var tbLeft = xmlTable.Attributes["left"].Value;
                    var tbTop = xmlTable.Attributes["top"].Value;
                    var table = new Table
                    {
                        Id = tbGuid,
                        Name = tbName,
                        Database = database,
                        Left = int.Parse(tbLeft),
                        Top = int.Parse(tbTop)
                    };

                    var xmlColumns = xmlTable.SelectNodes(".//column");

                    foreach (XmlNode xmlColumn in xmlColumns)
                    {
                        var colGuid = xmlColumn.Attributes["id"]?.Value ?? Guid.NewGuid().ToString();
                        var colName = xmlColumn.Attributes["name"].Value;
                        var colIndex = xmlColumn.Attributes["index"].Value;
                        var colType = xmlColumn.Attributes["type"].Value;
                        var colRequired = xmlColumn.Attributes["required"].Value;
                        var colClass = xmlColumn.Attributes["class"].Value;

                        if (colClass.Equals(nameof(Column)))
                        {
                            var column = new Column
                            {
                                Id = colGuid,
                                Name = colName,
                                Type = colType,
                                Table = table,
                                Index = int.Parse(colIndex),
                                Required = bool.Parse(colRequired)
                            };

                            table.Add(column);
                        }
                        else if (colClass.Equals(nameof(ForeignKey)))
                        {
                            var colRelatedTable = xmlColumn.Attributes["relatedTable"].Value;
                            var colRelatedColumn = xmlColumn.Attributes["relatedColumn"].Value;

                            var column = new ForeignKey
                            {
                                Id = colGuid,
                                Name = colName,
                                Type = colType,
                                Table = table,
                                Index = int.Parse(colIndex),
                                Required = bool.Parse(colRequired),
                                RelatedColumn = colRelatedColumn,
                                RelatedTable = colRelatedTable
                            };

                            table.Add(column);
                        }
                        else if (colClass.Equals(nameof(PrimaryKey)))
                        {
                            var column = new PrimaryKey
                            {
                                Id = colGuid,
                                Name = colName,
                                Type = colType,
                                Table = table,
                                Index = int.Parse(colIndex),
                                Required = bool.Parse(colRequired)
                            };

                            table.Add(column);
                        }
                    }

                    database.Tables.Add(table);
                }

                _database = database;
                panel1.Invalidate();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var form = new FrmNewConnection(this);
            form.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString))
                return;

            var database = _service.LoadDatabase(_provider, _connectionString);

            UseDatabase(database);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _database = new Database(_provider);
            ResetPanelSize();
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

        private void btnScripting_Click(object sender, EventArgs e)
        {
            if (_database.IsEmpty)
                return;

            var dialog = new SaveFileDialog
            {
                Filter = "Structured Query Language|*.sql",
                DefaultExt = ".sql"
            };

            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            _service.Export(_database, dialog.FileName);

            MessageBox.Show("Script exportado com sucesso.");
        }
    }
}

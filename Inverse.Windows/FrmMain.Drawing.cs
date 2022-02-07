using Inverse.Domain.Model;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public partial class FrmMain
    {
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_database.IsEmpty)
                return;

            var g = e.Graphics;
            var bordaTabela = new Pen(Brushes.Black, 2);
            var bordaTabelaSelecionada = new Pen(Brushes.DarkOrange, 2);
            const int COLUMN_HEIGHT = 30;

            var linha = new Pen(Brushes.Black, 1);
            var tables = _database.Tables.Where(x => showHiddenTablesToolStripMenuItem.Checked || !x.IsHidden);

            // desenhar relacionamentos
            foreach (var table in tables)
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

            foreach (var table in tables.OrderByDescending(x => x.Columns.OfType<ForeignKey>().Count()))
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

            panel1.Width = Math.Max(flowLayoutPanel1.Width, _database.Tables.Max(x => x.Right) + LayoutDefinition.Tables.MARGIN);
            panel1.Height = Math.Max(flowLayoutPanel1.Height, _database.Tables.Max(x => x.Bottom) + LayoutDefinition.Tables.MARGIN);
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
            if (_pressedPoint != Point.Empty && _currentTable != null)
            {
                _currentTable.MoveTo(e.X, e.Y);
                panel1.Invalidate();
            }

            _currentTable = null;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            _currentPoint = e.Location;
            toolStripStatusLabel1.Text = $"X={_currentPoint.X},Y={_currentPoint.Y}";

            if (_currentTable is not null)
            {
                _currentTable.MoveTo(e.X, e.Y);
            }

            panel1.Invalidate();
        }
    }
}

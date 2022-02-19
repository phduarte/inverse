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
            //var original = panel1.BackColor;

            //panel1.BackColor = Color.FromArgb(0, 30, 30, 30);

            if (_database.IsEmpty)
                return;

            const int PK_SEP_PADDING = 8;

            var g = e.Graphics;
            var tables = _database.Tables.Where(x => showHiddenTablesToolStripMenuItem.Checked || !x.IsHidden).OrderBy(_ => _.Index);
            var x = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            var font = new Font(Font.FontFamily, 8);

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

                        g.DrawLine(Theme.Table.Border, midway, source.Middle, midway, target.Middle);
                        g.DrawLine(Theme.Table.Border, source.Right, source.Middle, midway, source.Middle);
                        g.DrawLine(Theme.Table.Border, midway, target.Middle, target.Left, target.Middle);
                    }
                    else if (isGoingToLeft)
                    {
                        var meio = target.Right + ((source.Left - target.Right) / 2);

                        g.DrawLine(Theme.Table.Border, meio, source.Middle, meio, target.Middle);
                        g.DrawLine(Theme.Table.Border, target.Right, target.Middle, meio, target.Middle);
                        g.DrawLine(Theme.Table.Border, meio, source.Middle, source.Left, source.Middle);
                    }
                    else
                    {
                        g.DrawLine(Theme.Table.Border, source.Right, source.Top, target.Left, target.Middle);
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

            foreach (var table in tables.OrderBy(_ => _.Index))
            {
                var layout = new Rectangle(table.Left, table.Top, table.Width, table.Height);

                // tabela selecionada
                if (table.IsHover(_currentPoint.X, _currentPoint.Y))
                {
                    g.FillRectangle(Theme.Table.SelectedColumn, layout);
                    g.DrawRectangle(Theme.Table.BorderSelected, layout);
                }
                else
                {
                    g.FillRectangle(Theme.Table.Column, layout);
                    g.DrawRectangle(Theme.Table.Border, layout);
                }

                // título da tabela
                var areaTitulo = new RectangleF(table.Left + Theme.Table.Border.Size / 2, table.Top, table.Width - Theme.Table.Border.Size / 2, LayoutDefinition.Columns.HEIGHT);
                g.FillRectangle(Theme.Table.Title, areaTitulo);
                g.DrawString(table.Name, Font, Theme.Table.TitleFont, areaTitulo, _textAlignCenter);

                var pks = table.Columns.Where(_ => _.IsPrimaryKey);
                var separatorPksY = table.Top + LayoutDefinition.Columns.HEIGHT;

                foreach (var col in pks)
                {
                    var areaChaveColuna = new RectangleF(col.Left + 2, col.Top, LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                    var areaNomeColuna = new RectangleF(col.Left + LayoutDefinition.Columns.PREFIX_WIDTH, col.Top, col.Width - LayoutDefinition.Columns.TYPE_WIDTH - LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                    var areaTipoColuna = new RectangleF(col.Right - LayoutDefinition.Columns.TYPE_WIDTH, col.Top, LayoutDefinition.Columns.TYPE_WIDTH, col.Height);

                    g.DrawString(col.Prefix, font, Theme.Table.Font, areaChaveColuna, _textAlignLeft);

                    if (col.Required)
                    {
                        g.DrawString(col.Name, new Font(font, FontStyle.Bold), Theme.Table.Font, areaNomeColuna, _textAlignLeft);
                        g.DrawString(col.Type, font, Theme.Table.Font, areaTipoColuna, _textAlignLeft);
                    }
                    else
                    {
                        g.DrawString(col.Name, font, Theme.Table.Font, areaNomeColuna, _textAlignLeft);
                        g.DrawString(col.Type, font, Theme.Table.Font, areaTipoColuna, _textAlignLeft);
                    }
                    separatorPksY += col.Height;
                }

                // separador das chaves primárias
                g.DrawLine(Theme.Table.Border, new Point(table.Left + PK_SEP_PADDING, separatorPksY), new Point(table.Right - PK_SEP_PADDING, separatorPksY));

                foreach (var col in table.Columns.Except(pks))
                {
                    var areaNomeColuna = new RectangleF(col.Left + LayoutDefinition.Columns.PREFIX_WIDTH, col.Top, col.Width - LayoutDefinition.Columns.TYPE_WIDTH - LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                    var areaTipoColuna = new RectangleF(col.Right - LayoutDefinition.Columns.TYPE_WIDTH, col.Top, LayoutDefinition.Columns.TYPE_WIDTH, col.Height);

                    //if (col.IsHover(_currentPoint.X, _currentPoint.Y))
                    //{
                    //    g.FillRectangle(Brushes.Yellow, areaNomeColuna);
                    //}

                    if (col is ForeignKey)
                    {
                        var areaChaveColuna = new RectangleF(col.Left + 2, col.Top, LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                        g.DrawString(col.Prefix, font, Theme.Table.ForeignKeyColor, areaChaveColuna, _textAlignLeft);

                        if (col.Required)
                        {
                            g.DrawString(col.Name, new Font(font, FontStyle.Bold | FontStyle.Italic), Theme.Table.ForeignKeyColor, areaNomeColuna, _textAlignLeft);
                            g.DrawString(col.Type, font, Theme.Table.ForeignKeyColor, areaTipoColuna, _textAlignLeft);
                        }
                        else
                        {
                            g.DrawString(col.Name, new Font(font, FontStyle.Italic), Theme.Table.ForeignKeyColor, areaNomeColuna, _textAlignLeft);
                            g.DrawString(col.Type, font, Theme.Table.ForeignKeyColor, areaTipoColuna, _textAlignLeft);
                        }
                    }
                    else
                    {
                        if (col.Required)
                        {
                            g.DrawString(col.Name, new Font(font, FontStyle.Bold), Theme.Table.Font, areaNomeColuna, _textAlignLeft);
                            g.DrawString(col.Type, font, Theme.Table.Font, areaTipoColuna, _textAlignLeft);
                        }
                        else
                        {
                            g.DrawString(col.Name, font, Theme.Table.Font, areaNomeColuna, _textAlignLeft);
                            g.DrawString(col.Type, font, Theme.Table.Font, areaTipoColuna, _textAlignLeft);
                        }
                    }

                    //g.DrawLine(Theme.Table.ColumnSeparator, new Point(col.Left, col.Top), new Point(col.Left + col.Width, col.Top));
                }
            }
            g.SmoothingMode = x;

            panel1.Width = Math.Max(flowLayoutPanel1.Width, _database.Tables.Max(x => x.Right) + LayoutDefinition.Tables.MARGIN);
            panel1.Height = Math.Max(flowLayoutPanel1.Height, _database.Tables.Max(x => x.Bottom) + LayoutDefinition.Tables.MARGIN);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pressedPoint = e.Location;
                _currentTable = GetActiveTable();
                var x = _pressedPoint.X - _currentTable.Left;
                var y = _pressedPoint.Y - _currentTable.Top;
                _pressedPointCorrection = new Point(x, y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _pressedPoint != Point.Empty && _currentTable != null)
            {
                _currentTable.MoveTo(e.X - _pressedPointCorrection.X, e.Y - _pressedPointCorrection.Y);
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
                _currentTable.MoveTo(e.X - _pressedPointCorrection.X, e.Y - _pressedPointCorrection.Y);

                //var scrollY = flowLayoutPanel1.AutoScrollPosition.Y;
                //var scrollX = flowLayoutPanel1.AutoScrollPosition.X;

                //if (scrollX > e.X)
                //{
                //}

                //SetTempControl();
                //ScrollFlowPanel();
            }

            panel1.Invalidate();
        }

        //private void SetTempControl()
        //{
        //    tempControl.Left = _currentTable.Left;
        //    tempControl.Width = _currentTable.Width;
        //    tempControl.Height = _currentTable.Height;
        //    tempControl.Top = _currentTable.Top;
        //}
    }
}

using Inverse.Domain;
using Inverse.Domain.Columns;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Inverse.Windows
{
    public partial class FrmMain
    {
        private void Draw(Graphics g)
        {
            var tables = _database.Tables.Where(x => showHiddenTablesToolStripMenuItem.Checked || !x.IsHidden).OrderBy(_ => _.Index);
            var x = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            
            DrawRelationships(g, tables);
            DrawTables(g, tables);

            g.SmoothingMode = x;
        }

        private void DrawTables(Graphics g, IOrderedEnumerable<Domain.Tables.Table> tables)
        {
            const int PK_SEP_PADDING = 8;
            var fontRegular = new Font(Font.FontFamily, 8);
            var fontBold = new Font(fontRegular, FontStyle.Bold);
            var fontItalic = new Font(fontRegular, FontStyle.Italic);
            var fontBoldItalic = new Font(fontRegular, FontStyle.Bold | FontStyle.Italic);

            foreach (var table in tables.OrderBy(_ => _.Index))
            {
                var layout = new Rectangle(table.Left, table.Top, table.Width, table.Height);
                var isTableHover = table.IsHover(_currentPoint.X, _currentPoint.Y);
                var isTabSelected = _selectedTables.Contains(table);
                var tableBackgroundColor = isTableHover || isTabSelected ? Theme.Table.Background.SelectedColor : Theme.Table.Background.Color;
                var tableBorderColor = isTableHover ? Theme.Table.Border.SelectedColor : Theme.Table.Border.Color;

                g.FillRectangle(tableBackgroundColor, layout);
                g.DrawRectangle(tableBorderColor, layout);

                Pen tableBorderPen = Theme.Table.Border.GetPen(isTableHover);

                // título da tabela
                var titlePadding = (int)Math.Ceiling(Theme.Table.Border.Size / 2.0);
                var areaTitulo = new RectangleF(table.Left + titlePadding, table.Top + titlePadding, table.Width - titlePadding, LayoutDefinition.Columns.HEIGHT - titlePadding);
                
                g.FillRectangle(Theme.Table.Title.Background.Color, areaTitulo);
                g.DrawString(table.Name, Font, Theme.Table.Title.Text.Color, areaTitulo, _textAlignCenter); ;
                g.DrawLine(tableBorderPen, new Point((int)areaTitulo.Left, (int)areaTitulo.Bottom), new Point((int)areaTitulo.Right, (int)areaTitulo.Bottom));

                var pks = table.Columns.Where(_ => _.IsPrimaryKey);
                DrawColumns(g, PK_SEP_PADDING, fontRegular, fontBold, fontItalic, fontBoldItalic, table, tableBorderColor, pks);
            }
        }

        private void DrawColumns(Graphics g, int PK_SEP_PADDING, Font fontRegular, Font fontBold, Font fontItalic, Font fontBoldItalic, Domain.Tables.Table table, Color tableBorderColor, IEnumerable<Column> pks)
        {
            var separatorPksY = table.Top + LayoutDefinition.Columns.HEIGHT;

            if (pks.Any())
            {
                foreach (var col in pks)
                {
                    var areaChaveColuna = new RectangleF(col.Left + 2, col.Top, LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                    var areaNomeColuna = new RectangleF(col.Left + LayoutDefinition.Columns.PREFIX_WIDTH, col.Top, col.Width - LayoutDefinition.Columns.TYPE_WIDTH - LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                    var areaTipoColuna = new RectangleF(col.Right - LayoutDefinition.Columns.TYPE_WIDTH, col.Top, LayoutDefinition.Columns.TYPE_WIDTH, col.Height);
                    var columnIsHover = col.IsHover(_currentPoint.X, _currentPoint.Y);
                    var fontColor = columnIsHover ?
                        Theme.Table.Text.SelectedColor :
                        Theme.Table.Text.Color;

                    g.DrawString(col.Prefix, fontRegular, fontColor, areaChaveColuna, _textAlignLeft);

                    var fontStyle = col.Required ? new Font(fontRegular, FontStyle.Bold) : fontRegular;

                    g.DrawString(col.Name, fontStyle, fontColor, areaNomeColuna, _textAlignLeft);
                    g.DrawString(col.Type, fontRegular, fontColor, areaTipoColuna, _textAlignLeft);

                    separatorPksY += col.Height;
                }

                // separador das chaves primárias
                g.DrawLine(tableBorderColor, new Point(table.Left + PK_SEP_PADDING, separatorPksY), new Point(table.Right - PK_SEP_PADDING, separatorPksY));
            }

            foreach (var col in table.Columns.Except(pks))
            {
                var areaNomeColuna = new RectangleF(col.Left + LayoutDefinition.Columns.PREFIX_WIDTH, col.Top, col.Width - LayoutDefinition.Columns.TYPE_WIDTH - LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                var areaTipoColuna = new RectangleF(col.Right - LayoutDefinition.Columns.TYPE_WIDTH, col.Top, LayoutDefinition.Columns.TYPE_WIDTH, col.Height);
                var columnIsHover = col.IsHover(_currentPoint.X, _currentPoint.Y);
                var backgroundColor = columnIsHover ?
                    Theme.Table.Column.Background.SelectedColor :
                    Theme.Table.Column.Background.Color;

                g.FillRectangle(backgroundColor, areaNomeColuna);

                if (col is ForeignKey)
                {
                    var fontColor = columnIsHover ? Theme.Table.ForeignKeyText.SelectedColor : Theme.Table.ForeignKeyText.Color;
                    var areaChaveColuna = new RectangleF(col.Left + 2, col.Top, LayoutDefinition.Columns.PREFIX_WIDTH, col.Height);
                    var nameFontStyle = col.Required ? fontBoldItalic : fontItalic;

                    g.DrawString(col.Prefix, fontRegular, fontColor, areaChaveColuna, _textAlignLeft);
                    g.DrawString(col.Name, nameFontStyle, fontColor, areaNomeColuna, _textAlignLeft);
                    g.DrawString(col.Type, fontRegular, fontColor, areaTipoColuna, _textAlignLeft);
                }
                else
                {
                    var fontColor = columnIsHover ? Theme.Table.Column.Text.SelectedColor : Theme.Table.Column.Text.Color;
                    var nameFontStyle = col.Required ? fontBold : fontRegular;

                    g.DrawString(col.Name, nameFontStyle, fontColor, areaNomeColuna, _textAlignLeft);
                    g.DrawString(col.Type, fontRegular, fontColor, areaTipoColuna, _textAlignLeft);
                }

                g.DrawLine(Theme.Table.Separator.Color, new Point(col.Left, col.Top), new Point(col.Left + col.Width, col.Top));
            }
        }

        private void DrawRelationships(Graphics g, IOrderedEnumerable<Domain.Tables.Table> tables)
        {
            var relationBorder = Theme.Table.Border.GetPen();

            foreach (var table in tables)
            {
                foreach (var source in table.Columns.OfType<ForeignKey>())
                {
                    var destTable = _database.Tables.First(x => x.Name.Equals(source.RelatedTable));
                    var target = destTable.Columns.First(x => x.Name.Equals(source.RelatedColumn));

                    var isGoingToRight = source.Right < target.Left;
                    var isGoingToLeft = source.Left > target.Right;
                    var isGoingToDown = source.Bottom < target.Top;
                    var isGoingToUp = source.Top > target.Bottom;

                    if (isGoingToRight)
                    {
                        var midway = source.Right + ((target.Left - source.Right) / 2);

                        g.DrawLine(relationBorder, midway, source.Middle, midway, target.Middle);
                        g.DrawLine(relationBorder, source.Right, source.Middle, midway, source.Middle);
                        g.DrawLine(relationBorder, midway, target.Middle, target.Left, target.Middle);
                    }
                    else if (isGoingToLeft)
                    {
                        var midway = target.Right + ((source.Left - target.Right) / 2);

                        g.DrawLine(relationBorder, midway, source.Middle, midway, target.Middle);
                        g.DrawLine(relationBorder, target.Right, target.Middle, midway, target.Middle);
                        g.DrawLine(relationBorder, midway, source.Middle, source.Left, source.Middle);
                    }
                    else if (isGoingToUp)
                    {
                        var midway = table.Top + ((destTable.Bottom - table.Top) / 2);

                        g.DrawLine(relationBorder, table.Center, table.Top, table.Center, midway);
                        g.DrawLine(relationBorder, table.Center, midway, destTable.Center, midway);
                        g.DrawLine(relationBorder, destTable.Center, midway, destTable.Center, destTable.Bottom);
                    }
                    else if (isGoingToDown)
                    {
                        var midway = table.Bottom + ((destTable.Top - table.Bottom) / 2);

                        g.DrawLine(relationBorder, table.Center, table.Bottom, table.Center, midway);
                        g.DrawLine(relationBorder, table.Center, midway, destTable.Center, midway);
                        g.DrawLine(relationBorder, destTable.Center, midway, destTable.Center, destTable.Top);
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

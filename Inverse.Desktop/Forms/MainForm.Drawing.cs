using Inverse.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Inverse.Desktop;

public partial class MainForm
{
    private void Draw(Graphics g)
    {
        var tables = _database.Tables.Where(x => showHiddenTablesToolStripMenuItem.Checked || !x.IsHidden).OrderBy(_ => _.Index);
        var x = g.SmoothingMode;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        DrawRelationships(g, tables);
        DrawTables(g, tables);

        if (isControlPressed
            && _originColumn is not null
            && !_pressedPoint.IsEmpty
            && !_currentPoint.IsEmpty)
        {
            g.DrawLine(Theme.Relationship.Line.AsPen(isSelected: true), _pressedPoint, _currentPoint);
        }

        DrawSelection(g);

        g.SmoothingMode = x;
    }

    private void DrawSelection(Graphics g)
    {
        if (isSelecting && !_pressedPoint.IsEmpty && !_currentPoint.IsEmpty)
        {
            var color = Theme.Selection.Line.AsColor();
            var rect = new Rectangle(
                Math.Min(_pressedPoint.X, _currentPoint.X),
                Math.Min(_pressedPoint.Y, _currentPoint.Y),
                Math.Abs(_pressedPoint.X - _currentPoint.X),
                Math.Abs(_pressedPoint.Y - _currentPoint.Y));

            using var pen = new Pen(color, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };

            g.DrawRectangle(pen, rect);
        }
    }

    private void DrawTables(Graphics g, IOrderedEnumerable<Table> tables)
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
            var isTabSelected = _selectedTables.Contains(table) || _activeTable == table;

            var tableBackgroundColor = Theme.Column.Background.AsBrush(isTableHover, isTabSelected);
            var tableBorderColor = Theme.Table.Line.AsPen(isTableHover, isTabSelected);

            g.FillRectangle(tableBackgroundColor, layout);
            g.DrawRectangle(tableBorderColor, layout);

            // título da tabela
            var titlePadding = (int)Math.Ceiling(Theme.Table.Line.Size / 2.0);
            var areaTitulo = new RectangleF(table.Left + titlePadding, table.Top + titlePadding, table.Width - titlePadding, Column.HEIGHT - titlePadding);

            g.FillRectangle(Theme.Table.Background.AsBrush(isTableHover, isTabSelected), areaTitulo);
            g.DrawString(table.Name, Font, Theme.Table.Text.AsBrush(isTableHover, isTabSelected), areaTitulo, _textAlignCenter);
            g.DrawLine(Theme.Table.Line.AsPen(isTableHover, isTabSelected), new Point((int)areaTitulo.Left, (int)areaTitulo.Bottom), new Point((int)areaTitulo.Right, (int)areaTitulo.Bottom));

            var pks = table.Columns.Where(_ => _.IsPrimaryKey);

            DrawColumns(g,
                PK_SEP_PADDING,
                fontRegular,
                fontBold,
                fontItalic,
                fontBoldItalic,
                table,
                tableBorderColor,
                isTabSelected,
                pks);

            if (table.Comments.Any())
            {
                var rect = GetCommentButton(table);

                g.FillEllipse(Theme.Balloon.Background.AsBrush(), rect);
                g.DrawString("i", Font, Theme.Balloon.Text.AsBrush(), new Point(rect.Left + 5, rect.Top));
            }
        }
    }

    private void DrawColumns(
        Graphics g,
        int PK_SEP_PADDING,
        Font fontRegular,
        Font fontBold,
        Font fontItalic,
        Font fontBoldItalic,
        Table table,
        Pen tableBorderColor,
        bool isTableSelected,
        IEnumerable<Column> pks)
    {
        var separatorPksY = table.Top + Column.HEIGHT;

        if (pks.Any())
        {
            foreach (var col in pks)
            {
                var areaChaveColuna = new RectangleF(col.Left + 2, col.Top, Column.PREFIX_WIDTH, col.Height);
                var areaNomeColuna = new RectangleF(col.Left + Column.PREFIX_WIDTH, col.Top, col.Width - Column.TYPE_WIDTH - Column.PREFIX_WIDTH, col.Height);
                var areaTipoColuna = new RectangleF(col.Right - Column.TYPE_WIDTH, col.Top, Column.TYPE_WIDTH, col.Height);
                var columnIsHover = col.IsHover(_currentPoint.X, _currentPoint.Y);
                var fontStyle = col.IsRequired ? new Font(fontRegular, FontStyle.Bold) : fontRegular;

                g.FillRectangle(Theme.PrimaryKey.Background.AsBrush(columnIsHover, isTableSelected), areaChaveColuna);
                g.FillRectangle(Theme.PrimaryKey.Background.AsBrush(columnIsHover, isTableSelected), areaNomeColuna);
                g.FillRectangle(Theme.PrimaryKey.Background.AsBrush(columnIsHover, isTableSelected), areaTipoColuna);

                g.DrawString(col.Prefix, fontRegular, Theme.Prefix.Text.AsBrush(columnIsHover, isTableSelected), areaChaveColuna, _textAlignLeft);
                g.DrawString(col.Name, fontStyle, Theme.PrimaryKey.Text.AsBrush(columnIsHover, isTableSelected), areaNomeColuna, _textAlignLeft);
                g.DrawString(col.Type, fontRegular, Theme.Type.Text.AsBrush(columnIsHover, isTableSelected), areaTipoColuna, _textAlignLeft);

                separatorPksY += col.Height;
            }

            // separador das chaves primárias
            g.DrawLine(Theme.Column.Line.AsPen(), new Point(table.Left + PK_SEP_PADDING, separatorPksY), new Point(table.Right - PK_SEP_PADDING, separatorPksY));
        }

        foreach (var col in table.Columns.Except(pks))
        {
            var areaNomeColuna = new RectangleF(col.Left + Column.PREFIX_WIDTH, col.Top, col.Width - Column.TYPE_WIDTH - Column.PREFIX_WIDTH, col.Height);
            var areaTipoColuna = new RectangleF(col.Right - Column.TYPE_WIDTH, col.Top, Column.TYPE_WIDTH, col.Height);
            var columnIsHover = col.IsHover(_currentPoint.X, _currentPoint.Y);
            var areaChaveColuna = new RectangleF(col.Left + 2, col.Top, Column.PREFIX_WIDTH, col.Height);

            if (col is ForeignKey)
            {
                var nameFontStyle = col.IsRequired ? fontBoldItalic : fontItalic;

                g.FillRectangle(Theme.ForeignKey.Background.AsBrush(columnIsHover, isTableSelected), areaChaveColuna);
                g.FillRectangle(Theme.ForeignKey.Background.AsBrush(columnIsHover, isTableSelected), areaNomeColuna);
                g.FillRectangle(Theme.ForeignKey.Background.AsBrush(columnIsHover, isTableSelected), areaTipoColuna);

                g.DrawString(col.Prefix, fontRegular, Theme.Prefix.Text.AsBrush(columnIsHover, isTableSelected), areaChaveColuna, _textAlignLeft);
                g.DrawString(col.Name, nameFontStyle, Theme.ForeignKey.Text.AsBrush(columnIsHover, isTableSelected), areaNomeColuna, _textAlignLeft);
                g.DrawString(col.Type, fontRegular, Theme.Type.Text.AsBrush(columnIsHover, isTableSelected), areaTipoColuna, _textAlignLeft);
            }
            else
            {
                var nameFontStyle = col.IsRequired ? fontBold : fontRegular;

                g.FillRectangle(Theme.Column.Background.AsBrush(columnIsHover, isTableSelected), areaChaveColuna);
                g.FillRectangle(Theme.Column.Background.AsBrush(columnIsHover, isTableSelected), areaNomeColuna);
                g.FillRectangle(Theme.Column.Background.AsBrush(columnIsHover, isTableSelected), areaTipoColuna);

                g.DrawString(col.Name, nameFontStyle, Theme.Column.Text.AsBrush(columnIsHover, isTableSelected), areaNomeColuna, _textAlignLeft);
                g.DrawString(col.Type, fontRegular, Theme.Type.Text.AsBrush(columnIsHover, isTableSelected), areaTipoColuna, _textAlignLeft);
            }

            g.DrawLine(Theme.Column.Line.AsPen(columnIsHover, isTableSelected), new Point(col.Left, col.Top), new Point(col.Left + col.Width, col.Top));
        }
    }

    List<(int X, int Y)> _selectedRelationship = new();

    private void DrawRelationships(Graphics g, IOrderedEnumerable<Table> tables)
    {
        _selectedRelationship.Clear();

        var line = new List<(int X, int Y)>();

        var relationBorder = Theme.Relationship.Line.AsPen();
        var canvasText = Theme.Canvas.Text.AsBrush();

        var temp = new Pen(Brushes.Red, 2);
        const int VINTE = 20;
        const int DOZE = 12;
        const int SETE = 7;
        const int DEZ = 10;
        const int CINCO = 5;

        foreach (var table in tables)
        {
            foreach (var source in table.ForeignKeys)
            {
                var destTable = _database.Tables.FirstOrDefault(x => x.Name.Equals(source.RelatedTable));

                if (destTable is null || !destTable.Columns.Any()) continue;
                
                var target = destTable.Columns.First(x => x.Name.Equals(source.RelatedColumn));

                var isGoingToRight = source.Right < target.Left;
                var isGoingToLeft = source.Left > target.Right;
                var isGoingToDown = source.Bottom < target.Top;
                var isGoingToUp = source.Top > target.Bottom;

                if (isGoingToRight)
                {
                    var midway = source.Right + ((target.Left - source.Right) / 2);

                    line = new List<(int X, int Y)>
                    {
                        new(midway, source.Middle),
                        new(midway, target.Middle),
                        new(source.Right, source.Middle),
                        new(midway, source.Middle),
                        new(midway, target.Middle),
                        new(target.Left, target.Middle)
                    };

                    if (target.IsPrimaryKey)
                    {
                        if (crowsFeetToolStripMenuItem.Checked)
                        {
                            // dest
                            g.DrawLine(relationBorder, target.Left - DOZE, target.Middle - CINCO, target.Left - DOZE, target.Middle + CINCO);

                            // source
                            if (source.IsOneOrNone)
                            {
                                var circleArea = new Rectangle(source.Right + DOZE + CINCO, source.Middle - CINCO, DEZ, DEZ);
                                g.DrawLine(relationBorder, source.Right + DOZE, source.Middle - CINCO, source.Right + DOZE, source.Middle + CINCO);
                                g.FillEllipse(Theme.Canvas.Background.AsBrush(), circleArea);
                                g.DrawEllipse(relationBorder, circleArea);
                            }
                            else
                            {
                                g.DrawLine(relationBorder, source.Right, source.Middle + SETE, source.Right + DEZ, source.Middle);
                                g.DrawLine(relationBorder, source.Right, source.Middle - SETE, source.Right + DEZ, source.Middle);

                                //if (source.Required)
                                //{
                                //    g.DrawLine(relationBorder, source.Right + DOZE, source.Middle - CINCO, source.Right + DOZE, source.Middle + CINCO);
                                //}
                            }
                        }
                        else if (numberToolStripMenuItem.Checked)
                        {
                            // one
                            g.DrawString("1", DefaultFont, canvasText, new PointF(target.Left - VINTE, target.Middle));

                            // many
                            g.DrawString("N", DefaultFont, canvasText, new PointF(source.Right + DEZ, source.Middle));
                        }
                    }
                    else
                    {
                        g.DrawLine(temp, source.Right - 5, source.Middle - 5, source.Right - 5, source.Middle + 5);
                    }
                }
                else if (isGoingToLeft)
                {
                    var midway = target.Right + ((source.Left - target.Right) / 2);

                    line = new List<(int X, int Y)>
                    {
                        new(midway, source.Middle),
                        new(midway, target.Middle),
                        new(target.Right, target.Middle),
                        new(midway, target.Middle),
                        new(midway, source.Middle),
                        new(source.Left, source.Middle)
                    };

                    if (target.IsPrimaryKey)
                    {
                        if (crowsFeetToolStripMenuItem.Checked)
                        {
                            // dest
                            g.DrawLine(relationBorder, target.Right + DOZE, target.Middle - CINCO, target.Right + DOZE, target.Middle + CINCO);

                            // source
                            if (source.IsOneOrNone)
                            {
                                var circleArea = new Rectangle(source.Left - DOZE - DEZ - CINCO, source.Middle - CINCO, DEZ, DEZ);
                                g.DrawLine(relationBorder, source.Left - DOZE, source.Middle - CINCO, source.Left - DOZE, source.Middle + CINCO);
                                g.FillEllipse(Theme.Canvas.Background.AsBrush(), circleArea);
                                g.DrawEllipse(relationBorder, circleArea);
                            }
                            else
                            {
                                g.DrawLine(relationBorder, source.Left, source.Middle - SETE, source.Left - DEZ, source.Middle);
                                g.DrawLine(relationBorder, source.Left, source.Middle + SETE, source.Left - DEZ, source.Middle);

                                //if (source.Required)
                                //{
                                //    g.DrawLine(relationBorder, source.Left - DOZE, source.Middle - CINCO, source.Left - DOZE, source.Middle + CINCO);
                                //}
                            }
                        }
                        else if (numberToolStripMenuItem.Checked)
                        {
                            // dest
                            g.DrawString("1", DefaultFont, canvasText, new PointF(target.Right + DEZ, target.Top - DefaultFont.Size));

                            // source
                            g.DrawString("N", DefaultFont, canvasText, new PointF(source.Left - VINTE, source.Top - DefaultFont.Size));
                        }
                    }
                    else
                    {
                        g.DrawLine(temp, midway, source.Middle, source.Left, source.Middle);
                    }
                }
                else if (isGoingToUp)
                {
                    var midway = table.Top + ((destTable.Bottom - table.Top) / 2);

                    line = new List<(int X, int Y)>
                    {
                        new(table.Center, table.Top),
                        new(table.Center, midway),
                        new(table.Center, midway),
                        new(destTable.Center, midway),
                        new(destTable.Center, midway),
                        new(destTable.Center, destTable.Bottom),
                    };

                    if (table.Top > destTable.Bottom)
                    {
                        if (target.IsPrimaryKey)
                        {
                            if (crowsFeetToolStripMenuItem.Checked)
                            {
                                // dest
                                g.DrawLine(relationBorder, destTable.Center - CINCO, destTable.Bottom + DOZE, destTable.Center + CINCO, destTable.Bottom + DOZE);

                                // source
                                if (source.IsOneOrNone)
                                {
                                    var circleArea = new Rectangle(table.Center - CINCO, table.Top - DOZE - DEZ - CINCO, DEZ, DEZ);
                                    g.DrawLine(relationBorder, table.Center - CINCO, table.Top - DOZE, table.Center + CINCO, table.Top - DOZE);
                                    g.FillEllipse(Theme.Canvas.Background.AsBrush(), circleArea);
                                    g.DrawEllipse(relationBorder, circleArea);
                                }
                                else
                                {
                                    g.DrawLine(relationBorder, table.Center - SETE, table.Top, table.Center, table.Top - DEZ);
                                    g.DrawLine(relationBorder, table.Center + SETE, table.Top, table.Center, table.Top - DEZ);

                                    //if (source.Required)
                                    //{
                                    //    g.DrawLine(relationBorder, table.Center - CINCO, table.Top - DOZE, table.Center + CINCO, table.Top - DOZE);
                                    //}
                                }
                            }
                            else if (numberToolStripMenuItem.Checked)
                            {
                                // dest
                                g.DrawString("1", DefaultFont, canvasText, new PointF(destTable.Center, destTable.Bottom));

                                // source
                                g.DrawString("N", DefaultFont, canvasText, new PointF(table.Center, table.Top - VINTE));
                            }
                        }
                        else
                        {
                            g.DrawLine(temp, table.Center - 10, table.Bottom + 10, table.Center, table.Bottom);
                            g.DrawLine(temp, table.Center + 10, table.Bottom + 10, table.Center, table.Bottom);
                        }
                    }
                }
                else if (isGoingToDown)
                {
                    var midway = table.Bottom + ((destTable.Top - table.Bottom) / 2);

                    line = new List<(int X, int Y)>
                    {
                        new(table.Center, table.Bottom),
                        new(table.Center, midway),
                        new(table.Center, midway),
                        new(destTable.Center, midway),
                        new(destTable.Center, midway),
                        new(destTable.Center, destTable.Top)
                    };

                    if (destTable.Top > table.Bottom)
                    {
                        if (target.IsPrimaryKey)
                        {
                            if (crowsFeetToolStripMenuItem.Checked)
                            {
                                // dest
                                g.DrawLine(relationBorder, destTable.Center - CINCO, destTable.Top - DOZE, destTable.Center + CINCO, destTable.Top - DOZE);

                                // source
                                if (source.IsOneOrNone)
                                {
                                    var circleArea = new Rectangle(table.Center - CINCO, table.Bottom + DOZE + CINCO, DEZ, DEZ);
                                    g.DrawLine(relationBorder, table.Center - CINCO, table.Bottom + DOZE, table.Center + CINCO, table.Bottom + DOZE);
                                    g.FillEllipse(Theme.Canvas.Background.AsBrush(), circleArea);
                                    g.DrawEllipse(relationBorder, circleArea);
                                }
                                else
                                {
                                    g.DrawLine(relationBorder, table.Center, table.Bottom + DEZ, table.Center - SETE, table.Bottom);
                                    g.DrawLine(relationBorder, table.Center, table.Bottom + DEZ, table.Center + SETE, table.Bottom);

                                    //if (source.Required)
                                    //{
                                    //    g.DrawLine(relationBorder, table.Center - CINCO, table.Bottom + DOZE, table.Center + CINCO, table.Bottom + DOZE);
                                    //}
                                }
                            }
                            else if (numberToolStripMenuItem.Checked)
                            {
                                // dest
                                g.DrawString("1", DefaultFont, canvasText, new PointF(destTable.Center, destTable.Top - VINTE));

                                // source
                                g.DrawString("N", DefaultFont, canvasText, new PointF(table.Center, table.Bottom + DefaultFont.Size));
                            }
                        }
                        else
                        {
                            g.DrawLine(temp, table.Center - 10, table.Bottom + 10, table.Center, table.Bottom);
                            g.DrawLine(temp, table.Center + 10, table.Bottom + 10, table.Center, table.Bottom);
                        }
                    }
                }

                foreach (var p in line.Chunk(2))
                {
                    g.DrawLine(relationBorder, p[0].X, p[0].Y, p[1].X, p[1].Y);
                }

                if (line.Any(p => p.X == _currentPoint.X || p.X <= _currentPoint.Y))
                {
                    _selectedRelationship.AddRange(line);
                }
            }
        }
    }

    public static Rectangle GetCommentButton(Table table)
    {
        return new Rectangle(table.Right - 8, table.Top - 8, 16, 16);
    }
}
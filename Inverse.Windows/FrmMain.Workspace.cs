using Inverse.Domain;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public partial class FrmMain
    {
        bool isDragging = false;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_database.IsEmpty)
                return;

            Draw(e.Graphics);

            panel1.Width = Math.Max(flowLayoutPanel1.Width, _database.Tables.Where(t => showHiddenTablesToolStripMenuItem.Checked || !t.IsHidden).Max(x => x.Right) + LayoutDefinition.Tables.MARGIN);
            panel1.Height = Math.Max(flowLayoutPanel1.Height, _database.Tables.Where(t => showHiddenTablesToolStripMenuItem.Checked || !t.IsHidden).Max(x => x.Bottom) + LayoutDefinition.Tables.MARGIN);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pressedPoint = e.Location;
                _currentTable = GetActiveTable();

                if (_currentTable is not null)
                {
                    var x = _pressedPoint.X - _currentTable.Left;
                    var y = _pressedPoint.Y - _currentTable.Top;
                    _pressedPointCorrection = new Point(x, y);
                    isDragging = true;
                }
                else
                {
                    _selectedTables.Clear();
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                if (!_selectedTables.Any())
                {
                    _currentTable.MoveTo(e.X - _pressedPointCorrection.X, e.Y - _pressedPointCorrection.Y);
                    panel1.Invalidate();
                }

                isDragging = false;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            _currentPoint = e.Location;

            if (!HasStateChange)
            {
                toolStripStatusLabel1.Text = $"X={_currentPoint.X},Y={_currentPoint.Y}";
            }

            if (isDragging)
            {
                var startPointX = _currentTable.Left;
                var startPointY = _currentTable.Top;
                var endPointX = e.X - _pressedPointCorrection.X;
                var endPointY = e.Y - _pressedPointCorrection.Y;
                var offsetX = endPointX - startPointX;
                var offsetY = endPointY - startPointY;
                var wasMoved = offsetX != 0 || offsetY != 0;

                if (_selectedTables.Any())
                {
                    if (wasMoved && _selectedTables.All(x => x.CanMoveOffset(offsetX, offsetY)))
                    {
                        foreach (var s in _selectedTables)
                        {
                            s.MoveOffset(offsetX, offsetY);
                        }
                        isSavePending = true;
                    }
                }
                else
                {
                    _currentTable.MoveTo(endPointX, endPointY);
                    isSavePending = true;
                }
            }

            panel1.Invalidate();
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    releaseTablesToolStripMenuItem_Click(sender, e);
                    break;
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    if (e.Alt)
                    {
                        var isHorizontalMove = e.KeyCode == Keys.Left || e.KeyCode == Keys.Right;
                        var isVerticalMove = e.KeyCode == Keys.Up || e.KeyCode == Keys.Down;
                        var x = isHorizontalMove ? (38 - e.KeyValue) * -1 : 0;
                        var y = isVerticalMove ? (39 - e.KeyValue) * -1 : 0;

                        if (_selectedTables.Any())
                        {
                            if (_selectedTables.All(c => c.CanMoveOffset(x, y)))
                            {
                                foreach (var t in _selectedTables)
                                {
                                    t.MoveOffset(x, y);
                                }
                            }
                        }
                        else if (_currentTable is not null)
                        {
                            _currentTable.MoveOffset(x, y);
                        }

                        isSavePending = true;
                        panel1.Invalidate();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

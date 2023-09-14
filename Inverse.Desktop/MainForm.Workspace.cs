using Inverse.Domain;
using Inverse.Extensions;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class MainForm
    {
        bool isDragging = false;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_database.IsEmpty)
                return;

            Draw(e.Graphics);

            panel1.Width = Math.Max(flowLayoutPanel1.Width, _database.Tables.Where(t => showHiddenTablesToolStripMenuItem.Checked || !t.IsHidden).Max(x => x.Right) + Table.MARGIN);
            panel1.Height = Math.Max(flowLayoutPanel1.Height, _database.Tables.Where(t => showHiddenTablesToolStripMenuItem.Checked || !t.IsHidden).Max(x => x.Bottom) + Table.MARGIN);
        }

        private bool isControlPressed = false;

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

                    if (isControlPressed
                        && !_selectedTables.Contains(_currentTable))
                    {
                        _selectedTables.Add(_currentTable);
                    }
                    else
                    {
                        isDragging = true;
                    }
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
            //else if (showToolTipsToolStripMenuItem.Checked
            //    && toolTip1.Active
            //    && GetActiveTable() is Table table
            //    //&& _currentPoint.IsBetween(table.GetCommentButton())
            //    )
            //{
            //    toolTip1.Show(table.Notes, panel1);
            //}
            //else
            //{
            //    toolTip1.Hide(panel1);
            //}

            panel1.Invalidate();
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (readOnlyToolStripMenuItem.Checked)
            {
                return;
            }

            var newTable = new Table
            {
                Name = "New Table",
                Left = e.Location.X,
                Top = e.Location.Y,
            };

            _database.Add(newTable);

            panel1.Invalidate();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !(!isSavePending || (isSavePending && UserWantsClose()));
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
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
                    if (e.Shift)
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
                case Keys.F11:
                    fullScreenToolStripMenuItem_Click(sender, e);
                    break;
                case Keys.Alt:
                    if (fullScreenToolStripMenuItem.Checked)
                    {
                        menuStrip1.Visible = !menuStrip1.Visible;
                    }
                    break;
                default:
                    break;
            }

            isControlPressed = e.KeyCode == Keys.ControlKey;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            isControlPressed = false;
        }
    }
}

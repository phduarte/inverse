using Inverse.Domain;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class MainForm
    {
        private Table _activeTable = null;
        private Column _activeColumn = null;
        private bool isDragging = false;
        private bool isControlPressed = false;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_database.IsEmpty)
                return;

            Draw(e.Graphics);

            panel1.Width = Math.Max(flowLayoutPanel1.Width, _database.Tables.Where(t => showHiddenTablesToolStripMenuItem.Checked || !t.IsHidden).Max(x => x.Right) + Table.MARGIN);
            panel1.Height = Math.Max(flowLayoutPanel1.Height, _database.Tables.Where(t => showHiddenTablesToolStripMenuItem.Checked || !t.IsHidden).Max(x => x.Bottom) + Table.MARGIN);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pressedPoint = e.Location;
                _originColumn = _database.GetColumnByPosition(_pressedPoint.X, _pressedPoint.Y);

                if (_activeTable is not null)
                {
                    var x = _pressedPoint.X - _activeTable.Left;
                    var y = _pressedPoint.Y - _activeTable.Top;
                    _pressedPointCorrection = new Point(x, y);

                    if (isControlPressed
                        && !_selectedTables.Contains(_activeTable))
                    {
                        _selectedTables.Add(_activeTable);
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
            else if (e.Button == MouseButtons.Right)
            {
                panel1.ContextMenuStrip = _activeTable is null
                    ? contextMenuStripDatabase
                    : contextMenuStripTable;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_originColumn is not null
                && _activeTable is not null
                && _activeColumn is Column destColumn
                )
            {
                //var destColumn = _activeTable.Columns.FirstOrDefault(s => s.Name.Equals(_originColumn.Name));

                if (destColumn != null && !destColumn.Id.Equals(_originColumn.Id))
                {
                    _originColumn.Table.Join(_activeTable, _originColumn, destColumn);
                    _selectedTables.Clear();
                }
            }

            _originColumn = null;

            if (isDragging)
            {
                if (!_selectedTables.Any())
                {
                    _activeTable?.MoveTo(e.X - _pressedPointCorrection.X, e.Y - _pressedPointCorrection.Y);
                    panel1.Invalidate();
                }

                isDragging = false;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            _currentPoint = e.Location;
            _activeTable = _database.GetTableByPosition(_currentPoint.X, _currentPoint.Y);
            _activeColumn = _database.GetColumnByPosition(_currentPoint.X, _currentPoint.Y);

            if (!HasStateChange)
            {
                toolStripStatusLabel1.Text = $"X={_currentPoint.X},Y={_currentPoint.Y}";
            }

            if (isDragging && _activeTable is not null)
            {
                var startPointX = _activeTable.Left;
                var startPointY = _activeTable.Top;
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
                    _activeTable.MoveTo(endPointX, endPointY);
                    isSavePending = true;
                }
            }

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
            ToggleMenuButtons();
            panel1.Invalidate();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !(!isSavePending || (isSavePending && UserWantsClose()));

            if (!e.Cancel)
            {
                FullScreenMode.Toggle(this, false);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    releaseTablesToolStripMenuItem_Click(sender, e);
                    _pressedPoint = Point.Empty;
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
                        else if (_activeTable is not null)
                        {
                            _activeTable.MoveOffset(x, y);
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
using Inverse.Domain;
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
                }
                else
                {
                    _selectedTables.Clear();
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _pressedPoint != Point.Empty && _currentTable != null)
            {
                if (!_selectedTables.Any())
                {
                    _currentTable.MoveTo(e.X - _pressedPointCorrection.X, e.Y - _pressedPointCorrection.Y);
                    panel1.Invalidate();
                }
            }

            _currentTable = null;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            _currentPoint = e.Location;
            toolStripStatusLabel1.Text = $"X={_currentPoint.X},Y={_currentPoint.Y}";

            if (_currentTable is not null)
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
                    }
                }
                else
                {
                    _currentTable.MoveTo(endPointX, endPointY);
                }

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
    }
}

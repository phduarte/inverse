using Inverse.Desktop.Themes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class MainForm
    {
        private void ChangeTheme(string selectedTheme)
        {
            Theme = ThemeManager.Load(selectedTheme);

            BackColor = statusStrip1.BackColor = menuStrip1.BackColor = panel1.BackColor = Theme.Canvas.Background.AsColor();
            ForeColor = statusStrip1.ForeColor = menuStrip1.ForeColor = panel1.ForeColor = Theme.Canvas.Text.AsColor();
        }

        private void ChangeTheme(ToolStripMenuItem menuItem)
        {
            ChangeTheme(menuItem.Text);

            foreach (var control in themeToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>())
            {
                control.Checked = false;
            }

            menuItem.Checked = true;
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeTheme("");

            foreach (var control in themeToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>())
            {
                control.Checked = false;
            }

            defaultToolStripMenuItem.Checked=true;
        }
    }
}

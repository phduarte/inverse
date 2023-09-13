using System;
using System.Drawing;

namespace Inverse.Desktop
{
    public partial class MainForm
    {
        private void borderThinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderSize(1);
            //panel1.Invalidate();
        }

        private void borderBoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBorderSize(2);
            //panel1.Invalidate();
        }

        private void fontBlackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColor(Brushes.Black);
            //Theme.Table.SetBorderColor(Brushes.Black);
            //panel1.Invalidate();
        }

        private void fontBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColor(Brushes.Blue);
            //Theme.Table.SetBorderColor(Brushes.Blue);
            //panel1.Invalidate();
        }

        private void fontOrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColor(Brushes.Orange);
            //Theme.Table.SetBorderColor(Brushes.Orange);
            //panel1.Invalidate();
        }

        private void fontRedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColor(Brushes.Red);
            //Theme.Table.SetBorderColor(Brushes.Red);
            //panel1.Invalidate();
        }

        private void foreColorSelectedOrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColorSelected(Brushes.Orange);
            //Theme.Table.SetBorderColorSelected(Brushes.Orange);
            //panel1.Invalidate();
        }

        private void foreColorSelectedblueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColorSelected(Brushes.Blue);
            //Theme.Table.SetBorderColorSelected(Brushes.Blue);
            //panel1.Invalidate();
        }
        private void foreColorSelectedBlackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColorSelected(Brushes.Black);
            //Theme.Table.SetBorderColorSelected(Brushes.Black);
            //panel1.Invalidate();
        }

        private void foreColorSelectedRedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTextColorSelected(Brushes.Red);
            //Theme.Table.SetBorderColorSelected(Brushes.Red);
            //panel1.Invalidate();
        }

        private void panelBackgroundColorLightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.ForeColor =
            //    statusStrip1.ForeColor =
            //    menuStrip1.ForeColor =
            //    flowLayoutPanel1.ForeColor =
            //    panel1.ForeColor = System.Drawing.Color.Black;

            //this.BackColor =
            //    statusStrip1.BackColor =
            //    menuStrip1.BackColor =
            //    flowLayoutPanel1.BackColor =
            //    panel1.BackColor = System.Drawing.Color.White;

            //Theme.Table.Title.SetTheme(Brushes.Transparent, Brushes.Black);
            //Theme.Table.ForeignKeyText.Color = Brushes.Black;
            //Theme.Table.ForeignKeyText.SelectedColor = Brushes.OrangeRed;

            //Theme.Table.SetBackgroundColor(Brushes.White);
            //Theme.Table.SetBackgroundColorSelected(Brushes.LightYellow);
            //Theme.Table.SetTextColor(Brushes.Black);
            //Theme.Table.SetTextColorSelected(Brushes.OrangeRed);
            //Theme.Table.SetBorderColor(Brushes.Black);
            //Theme.Table.SetBorderColorSelected(Brushes.OrangeRed);
            //panel1.Invalidate();
        }

        private void panelBackgroundColorDarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Theme = ThemeManager.Load("Dark");

            //this.ForeColor =
            //    statusStrip1.ForeColor =
            //    menuStrip1.ForeColor =
            //    flowLayoutPanel1.ForeColor =
            //    panel1.ForeColor = System.Drawing.Color.White;

            //this.BackColor =
            //    statusStrip1.BackColor =
            //    menuStrip1.BackColor =
            //    flowLayoutPanel1.BackColor =
            //    panel1.BackColor = System.Drawing.Color.Black;

            //Theme.Table.Title.Text.Color = Brushes.DarkGray;
            //Theme.Table.Title.Background.Color = Brushes.Transparent;
            //Theme.Table.Title.Text.SelectedColor = Brushes.DarkGray;

            //Theme.Table.ForeignKeyText.Color = Brushes.White;
            //Theme.Table.ForeignKeyText.SelectedColor = Brushes.DarkGray;

            //Theme.Table.SetBackgroundColor(Brushes.Black);
            //Theme.Table.SetBackgroundColorSelected(Brushes.Black);
            //Theme.Table.SetTextColor(Brushes.White);
            //Theme.Table.SetTextColorSelected(Brushes.DarkGray);
            //Theme.Table.SetBorderColor(Brushes.White);
            //Theme.Table.SetBorderColorSelected(Brushes.DarkGray);
            //panel1.Invalidate();
        }

        private void backgroundColorSelectedYellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBackgroundColorSelected(Brushes.LightYellow);
            //panel1.Invalidate();
        }

        private void backgroundColorBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetBackgroundColorSelected(Brushes.AliceBlue);
            //panel1.Invalidate();
        }

        private void titleGrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTitle(Brushes.White, Brushes.Gray);
            //panel1.Invalidate();
        }

        private void titleWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTitle(Brushes.Black, Brushes.White);
            //panel1.Invalidate();
        }

        private void titleBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Theme.Table.SetTitle(Brushes.White, Brushes.SteelBlue);
            //panel1.Invalidate();
        }
    }
}

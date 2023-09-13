using Inverse.Domain;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class TableForm : Form
    {
        private Table _table;

        public TableForm(Table table)
        {
            InitializeComponent();
            _table = table;
        }

        private void FrmTableEdit_Load(object sender, EventArgs e)
        {
            txtName.Text = _table.Name;

            foreach (var column in _table.Columns)
            {
                dataGridView1.Rows.Add(
                    column.Name,
                    column.Description,
                    column.Type,
                    column.IsRequired,
                    column.IsPrimaryKey,
                    column.IsForeignKey.ToString(),
                    column
                    );
            }

            foreach (var comment in _table.Comments.OrderBy(x => x.Date))
            {
                AddComment(comment.Date, comment.Author, comment.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddComment(DateTime.Now, Environment.UserName, txtNote.Text);
        }

        private void FrmTableEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            var index = _table.Columns.Count;

            _table.Name = txtName.Text;
            _table.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var columnName = row.Cells[0].Value as string;
                var columnDesc = row.Cells[1].Value as string;
                var columnType = row.Cells[2].Value as string;
                var columnIsRequired = row.Cells[3].Value;
                var columnIsPrimaryKey = row.Cells[4].Value;
                var column = row.Cells[6].Value as Column;

                if (columnName is null && columnType is null)
                    break;

                if (column is null)
                {
                    column = new Column
                    {
                        Name = columnName,
                        Description = columnDesc,
                        Type = columnType,
                        Index = index++,
                        IsRequired = Convert.ToBoolean(columnIsRequired)
                    };
                }
                else
                {
                    column.Name = columnName;
                    column.Description = columnDesc;
                    column.Type = columnType;
                    column.IsRequired = Convert.ToBoolean(columnIsRequired);
                }

                _table.Add(column);
            }

            foreach (Label note in flowLayoutPanel1.Controls)
            {
                var campos = note.Text.Split('\n');
                var headers = campos[0].Split('-');
                var text = string.Join("<br />", campos.Skip(1));
                var date = Convert.ToDateTime(headers[0]);
                var author = headers[1].Replace(":", string.Empty);

                var comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Author = author,
                    Text = text
                };

                _table.Add(comment);
            }
        }

        private void txtNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                button1_Click(sender, e);
                txtNote.ResetText();
                e.SuppressKeyPress = true;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem toolstrip
                        && toolstrip.Owner is ContextMenuStrip contextMenu
                        && contextMenu.SourceControl is Label label)
            {
                if (MessageBox.Show(string.Format("Are you sure you want to remove BOL {0} from this Job?", label.Text), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    flowLayoutPanel1.Controls.Remove(label);
                }
            }
            else
            {
                MessageBox.Show("Invalid item selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddComment(DateTime date, string author, string text)
        {
            var height = (txtNote.Text.Split('\n').Length * 18) + 20;

            var label = new Label
            {
                Text = $"{date:dd/MM/yyyy HH:mm:ss.fff}-{author}:\n{text}",
                Width = flowLayoutPanel1.Width - 15,
                Height = height,
                BackColor = System.Drawing.Color.LightBlue,
                Margin = new Padding(0, 0, 0, 5),
                Padding = new(5),
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                ContextMenuStrip = contextMenuStrip1
            };

            flowLayoutPanel1.Controls.Add(label);
            flowLayoutPanel1.ScrollControlIntoView(label);
        }
    }
}

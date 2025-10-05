using Inverse.Desktop.Extensions;
using Inverse.Domain;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Inverse.Desktop;

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
        BindGridView();

        foreach (var comment in _table.Comments.OrderBy(x => x.Date))
        {
            AddComment(comment.Date, comment.Author, comment.Text);
        }

        dataGridViewSeed.DataSource = null;
        dataGridViewSeed.FillWithJson(_table.SeedData);
        //dataGridViewSeed.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
    }

    private void BindGridView()
    {
        dataGridView1.Rows.Clear();

        var types = _table.Columns.Select(c => c.Type);

        foreach (var type in types)
        {
            if (!dataGridViewComboBoxColumn1.Items.Contains(type))
            {
                dataGridViewComboBoxColumn1.Items.Add(type);
            }
        }

        foreach (var column in _table.Columns)
        {
            var fk = column as ForeignKey;
            var fkName = fk?.SimpleName ?? string.Empty;

            var idx = dataGridView1.Rows.Add(
                    column.Name,
                    column.Description,
                    column.Type,
                    column.IsRequired,
                    column.IsPrimaryKey,
                    column.DefaultValue,
                    fkName,
                    column
                    );

            dataGridView1.Rows[idx].Tag = column;
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
        _table.SeedData = dataGridViewSeed.AsJson();

        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            var columnName = row.Cells[0].Value as string;
            var columnDesc = row.Cells[1].Value as string;
            var columnType = row.Cells[2].Value as string;
            var columnIsRequired = row.Cells[3].Value;
            var columnIsPrimaryKey = row.Cells[4].Value;
            var defaultValue = row.Cells[5].Value as string;
            var column = row.Cells[7].Value as Column;

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
                    DefaultValue = defaultValue,
                    IsRequired = Convert.ToBoolean(columnIsRequired)
                };

                if (Convert.ToBoolean(columnIsPrimaryKey))
                {
                    column = PrimaryKey.Parse(column);
                }
            }
            else
            {
                column.Name = columnName;
                column.Description = columnDesc;
                column.Type = columnType;
                column.DefaultValue = defaultValue;
                column.IsRequired = Convert.ToBoolean(columnIsRequired);
            }

            _table.AddColumn(column);
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

            _table.AddComment(comment);
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
                    && contextMenu.SourceControl is DataGridView dgv)
        {
            if (MessageBox.Show("Are you sure you want to remove the selected columns", "Confirme Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow r in dgv.SelectedRows)
                {
                    var column = r.Tag as Column;
                    _table.RemoveColumn(column);
                }
                BindGridView();
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

    private void txtName_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            Close();
        }
    }

    private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedRows = GetSelectedColumns();

        foreach (var column in selectedRows)
        {
            _table.MoveColumnUp(column);
        }

        BindGridView();

        Select(selectedRows);
    }

    private void Select(IList<Column> selectedRows)
    {
        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            var ob = row.Cells[7].Value as Column;
            row.Selected = selectedRows.Contains(ob);
        }
    }

    private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedRows = GetSelectedColumns();

        foreach (var column in selectedRows)
        {
            _table.MoveColumnDown(column);
        }

        BindGridView();
        Select(selectedRows);
    }

    private IList<Column> GetSelectedColumns()
        => dataGridView1.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(r => r.Cells[7].Value as Column)
                .ToList();

    private void removeFKToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView1
            .SelectedCells
            .OfType<DataGridViewCell>()
            .FirstOrDefault() is DataGridViewCell cell && cell.OwningRow.Cells[7].Value is ForeignKey fk)
        {
            _table.ChangeToColumn(fk);
        }

        BindGridView();
    }

    private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
        var newRow = dataGridView1.Rows[e.RowIndex];
        var columnName = newRow.Cells[0].FormattedValue.ToString();

        if (string.IsNullOrEmpty(columnName))
        {
            return;
        }

        dataGridViewSeed.Columns.Add(new DataGridViewColumn
        {
            CellTemplate = new DataGridViewTextBoxCell(),
            Name = columnName,
            HeaderText = columnName,
            ValueType = typeof(string),
            Width = 100,
            ReadOnly = false,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
    }

    private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
        var existingColumnNames = dataGridView1.Rows.Cast<DataGridViewRow>().Select(c => c.Cells[0].Value.ToString());

        if (existingColumnNames.IsNullOrEmpty())
        {
            return;
        }

        foreach (DataGridViewColumn c in dataGridViewSeed.Columns)
        {
            if (!existingColumnNames.Contains(c.Name))
            {
                dataGridViewSeed.Columns.Remove(c);
            }
        }
    }

    private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {

    }

    private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
    {

    }

    private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {

    }

    private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        // deve atualizar o nome da coluna na seed quando o nome da coluna for alterado
        if (e.ColumnIndex == 0) // Nome da coluna
        {
            var editedRow = dataGridView1.Rows[e.RowIndex];
            var oldColumn = editedRow.Tag as Column;
            var newColumnName = editedRow.Cells[0].Value?.ToString();
            if (oldColumn is not null && !string.IsNullOrEmpty(newColumnName) && oldColumn.Name != newColumnName)
            {
                // Atualiza o nome da coluna na seed
                var seedColumn = dataGridViewSeed.Columns
                    .OfType<DataGridViewColumn>()
                    .FirstOrDefault(c => c.Name == oldColumn.Name);
                if (seedColumn is not null)
                {
                    seedColumn.Name = newColumnName;
                    seedColumn.HeaderText = newColumnName;
                }
            }
        }
    }
}
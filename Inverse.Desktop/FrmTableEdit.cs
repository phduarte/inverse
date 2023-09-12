using Inverse.Domain;
using System;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class FrmTableEdit : Form
    {
        private Table _table;

        public FrmTableEdit(Table table)
        {
            InitializeComponent();
            _table = table;
        }

        private void FrmTableEdit_Load(object sender, EventArgs e)
        {
            txtName.Text = _table.Name;
            txtNotes.Text = _table.Notes;

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
        }

        private void FrmTableEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            var index = _table.Columns.Count;

            _table.Name = txtName.Text;
            _table.Notes = txtNotes.Text;
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
        }
    }
}

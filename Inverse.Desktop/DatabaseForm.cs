using System;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class DatabaseForm : Form
    {
        private readonly MainForm _parentForm;

        public DatabaseForm(MainForm form)
        {
            _parentForm = form;
            InitializeComponent();
        }

        private void picMssqlServer_Click(object sender, EventArgs e)
        {
            var frm = new DatabaseSqlServerForm(_parentForm);
            Hide();
            frm.ShowDialog();
            Close();
        }

        private void picSqlite_Click(object sender, EventArgs e)
        {
            var frm = new DatabaseSqliteForm(_parentForm);
            Hide();
            frm.ShowDialog();
            Close();
        }
    }
}

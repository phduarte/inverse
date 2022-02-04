using System;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public partial class FrmNewConnection : Form
    {
        private readonly FrmMain _parentForm;

        public FrmNewConnection(FrmMain form)
        {
            _parentForm = form;
            InitializeComponent();
        }

        private void picMssqlServer_Click(object sender, EventArgs e)
        {
            var frm = new FrmNewConnectionSqlServer(_parentForm);
            Hide();
            frm.ShowDialog();
            Close();
        }

        private void picSqlite_Click(object sender, EventArgs e)
        {
            var frm = new FrmNewConnectionSqlite(_parentForm);
            Hide();
            frm.ShowDialog();
            Close();
        }
    }
}

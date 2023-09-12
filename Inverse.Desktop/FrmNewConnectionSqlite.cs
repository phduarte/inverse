using Inverse.Domain;
using System;
using System.IO;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class FrmNewConnectionSqlite : Form
    {
        private readonly FrmMain _parentForm;
        private static string _filename;

        public FrmNewConnectionSqlite(FrmMain parentForm)
        {
            _parentForm = parentForm;
            InitializeComponent();

            txtFilename.Text = _filename;
            btnRevert.Enabled = File.Exists(txtFilename.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Arquivo de Banco de Dados SQLite|*.db";

            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
            {
                txtFilename.Text = _filename = dialog.FileName;
            }

            btnRevert.Enabled = File.Exists(txtFilename.Text);
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            var connectionString = $"Data source={_filename};";

            _parentForm.UseDatabase(Provider.SQLite, connectionString);
            Close();
        }
    }
}

using Inverse.Domain;
using System;
using System.IO;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class DatabaseSqliteForm : Form
    {
        private readonly MainForm _parentForm;
        private static string _filename;

        public DatabaseSqliteForm(MainForm parentForm)
        {
            _parentForm = parentForm;
            InitializeComponent();

            txtFilename.Text = _filename;
            btnRevert.Enabled = File.Exists(txtFilename.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Arquivo de Banco de Dados SQLite|*.db"
            };

            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
            {
                txtFilename.Text = _filename = dialog.FileName;
            }

            btnRevert.Enabled = File.Exists(txtFilename.Text);
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            _parentForm.UseDatabase(Provider.SQLite, $"Data source={_filename};");
            Close();
        }
    }
}
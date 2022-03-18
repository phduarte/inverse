using Inverse.Domain.Databases;
using System;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public partial class FrmNewConnectionSqlServer : Form
    {
        private readonly FrmMain _parentForm;
        private static string _server;
        private static string _database;
        private static string _username;
        private static string _password;

        public FrmNewConnectionSqlServer(FrmMain parentForm)
        {
            _parentForm = parentForm;
            InitializeComponent();
            txtServer.Text = _server;
            txtDatabase.Text = _database;
            txtUsername.Text = _username;
            txtPassword.Text = _password;
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            try
            {
                var connectionString = $"Server={_server = txtServer.Text};Database={_database = txtDatabase.Text};";

                if (!chkWindowsAuth.Checked)
                {
                    connectionString += $"User ID={_username = txtUsername.Text};Password={_password = txtPassword.Text };";
                }
                else
                {
                    connectionString += "Trusted_Connection=True;";
                }

                _parentForm.UseDatabase(Provider.MSSQLServer, connectionString);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtUsername.Enabled = txtPassword.Enabled = !chkWindowsAuth.Checked;
        }
    }
}

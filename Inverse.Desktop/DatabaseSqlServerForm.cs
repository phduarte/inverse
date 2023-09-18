using Inverse.Domain;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Inverse.Desktop
{
    public partial class DatabaseSqlServerForm : Form
    {
        private readonly MainForm _parentForm;
        private static string _server;
        private static string _database;
        private static string _username;
        private static string _password;

        public DatabaseSqlServerForm(MainForm parentForm)
        {
            _parentForm = parentForm;
            InitializeComponent();
            txtServer.Text = _server;
            txtDatabase.Text = _database;
            txtUsername.Text = _username;
            txtPassword.Text = _password;
        }

        private async void btnRevert_Click(object sender, EventArgs e)
        {
            btnRevert.Enabled = false;
            var database = new Database
            {
                Provider = Provider.MSSQLServer
            };

            try
            {
                var connectionString = $"Server={_server = txtServer.Text};Database={_database = txtDatabase.Text};";

                if (!chkWindowsAuth.Checked)
                {
                    connectionString += $"User ID={_username = txtUsername.Text};Password={_password = txtPassword.Text};";
                }
                else
                {
                    connectionString += "Trusted_Connection=True;";
                }

                database.Name = txtDatabase.Text;
                database.ConnectionString = connectionString;
                database.OnTableAdded += (t) =>
                {
                    progressBar1.Value++;
                };

                await Task.Run(() =>
                {
                    _parentForm.UseDatabase(database);
                });

                progressBar1.Value = progressBar1.Maximum;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRevert.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtUsername.Enabled = txtPassword.Enabled = !chkWindowsAuth.Checked;
        }

        private void txtServer_Leave(object sender, EventArgs e)
        {
            // TODO validar se o servidor existe
        }
    }
}

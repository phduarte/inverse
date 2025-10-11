using Inverse.Domain;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inverse.Desktop;

public partial class DatabasePostgreSql : Form
{
    private readonly MainForm _parentForm;
    private static string _server;
    private static string _database;
    private static string _username;
    private static string _password;

    public DatabasePostgreSql(MainForm parentForm)
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
            var connectionString = $"Host={txtServer.Text};Port={txtPort.Text};Database={txtDatabase.Text};Username={txtUsername.Text};Password={txtPassword.Text};";

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

    private void txtServer_Leave(object sender, EventArgs e)
    {
        // TODO validar se o servidor existe
    }
}
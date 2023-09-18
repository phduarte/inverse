using Inverse.Domain;
using System;
using System.Windows.Forms;

namespace Inverse.Desktop
{
    public partial class DatabaseEditForm : Form
    {
        private bool isSaved = true;
        private Database _database;

        public DatabaseEditForm(Database database)
        {
            InitializeComponent();
            _database = database;
        }

        private void DatabaseEditForm_Load(object sender, System.EventArgs e)
        {
            textBoxName.Text = _database.Name;
            comboBoxProvider.DataSource = Enum.GetValues<Provider>();
            comboBoxProvider.SelectedItem = _database.Provider;
            textBoxConnectionString.Text = _database.ConnectionString;
        }

        private void DatabaseEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved
                && MessageBox.Show("Do you want to discard the changes?", "Unsaved changes", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            _database.Name = textBoxName.Text;
            _database.Provider = (Provider)(comboBoxProvider.SelectedValue);
            _database.ConnectionString = textBoxConnectionString.Text;
            isSaved = true;
            Close();
        }

        private void textBoxName_TextChanged(object sender, System.EventArgs e)
        {
            isSaved = false;
        }

        private void comboBoxProvider_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            isSaved = false;
        }
    }
}

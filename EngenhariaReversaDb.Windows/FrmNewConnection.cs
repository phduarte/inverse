using EngenhariaReversaDb.Domain.Model;
using EngenhariaReversaDb.Domain.Services;
using System;
using System.Windows.Forms;

namespace EngenhariaReversaDb.Windows
{
    public partial class FrmNewConnection : Form
    {
        private readonly FrmMain _parentForm;

        public FrmNewConnection(FrmMain form)
        {
            _parentForm = form;
            InitializeComponent();
        }

        private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProvider.SelectedIndex == 0)
            {
                txtConnectionString.Text = @"Server=;Database=;";
            }
            else if (cmbProvider.SelectedIndex == 1)
            {
                txtConnectionString.Text = @"Data source=;";
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            var service = DatabaseGeneratorFactory.Create((Provider)cmbProvider.SelectedIndex);
            var database = service.GetDatabase(txtConnectionString.Text);

            _parentForm.UseDatabase(database);
            Close();
        }
    }
}

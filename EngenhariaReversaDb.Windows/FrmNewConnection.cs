using Inverse.Domain.Model;
using Inverse.Domain.Services;
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
            _parentForm.UseDatabase((Provider)cmbProvider.SelectedIndex, txtConnectionString.Text);
            Close();
        }
    }
}

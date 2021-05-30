using EngenhariaReversaDb.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngenhariaReversaDb.Windows
{
    public partial class NewConnection : Form
    {
        Form1 form;

        public NewConnection(Form1 form)
        {
            this.form = form;

            InitializeComponent();

            textBox1.Text = @"Data source=C:\Users\phdua\source\repos\phduarte\Agenda\Gadz.Agenda.Web\agenda.db";
            comboBox1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                var service = new GenerateModelService(Domain.Provider.SQLite);
                var database = service.GetDatabase(textBox1.Text);

                form.UseDatabase(database);
                Close();
            }
            else
            {
                MessageBox.Show("Provedor não está funcionando.");
            }
        }
    }
}

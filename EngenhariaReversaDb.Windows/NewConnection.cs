using EngenhariaReversaDb.Services;
using System;
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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox1.Text = @"Server=(localdb)\MSSQLLocalDB;Database=PAYMENT_INTEGRATION;";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                textBox1.Text = @"Data source=C:\Users\phdua\source\repos\phduarte\Agenda\Gadz.Agenda.Web\agenda.db";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var service = DatabaseModelFactory.Create((Domain.Provider)comboBox1.SelectedIndex);
            var database = service.GetDatabase(textBox1.Text);

            form.UseDatabase(database);
            Close();
        }
    }
}

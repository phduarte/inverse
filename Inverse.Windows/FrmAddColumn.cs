using Inverse.Domain;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Inverse.Windows
{
    delegate Column MapEventHandler();

    public partial class FrmAddColumn : Form
    {
        private readonly IDictionary<string, MapEventHandler> _mappers = new Dictionary<string, MapEventHandler>();

        public FrmAddColumn()
        {
            InitializeComponent();

            _mappers.Add("Normal", MapToColumn);
            _mappers.Add("Primary Key", MapToPrimaryKey);
            _mappers.Add("Foreign Key", MapToColumn);
            _mappers.Add("Primary Foreign Key", MapToColumn);

            cmbColumnType.Items.Add("Normal");
            cmbColumnType.Items.Add("Primary Key");
            cmbColumnType.Items.Add("Foreign Key");
            cmbColumnType.Items.Add("Primary Foreign Key");

            cmbDataType.Items.Add("INT");
            cmbDataType.Items.Add("VARCHAR");
            cmbDataType.Items.Add("FLOAT");
            cmbDataType.Items.Add("DOUBLE");
        }

        public void AddColumn(Table table)
        {
            txtTableName.Text = table.Name;

            ShowDialog();

            var mapper = _mappers[cmbColumnType.Text];
            var col = mapper();
            table.Add(col);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Column MapToColumn()
        {
            return new Column
            {
                Name = txtColumnName.Text,
                Required = chkRequired.Checked,
                Type = cmbDataType.Text
            };
        }

        private PrimaryKey MapToPrimaryKey()
        {
            return new PrimaryKey
            {
                Name = txtColumnName.Text,
                Required = chkRequired.Checked,
                Type = cmbDataType.Text
            };
        }

        private ForeignKey MapToForeignKey()
        {
            return new ForeignKey
            {
                Name = txtColumnName.Text,
                Required = chkRequired.Checked,
                Type = cmbDataType.Text
            };
        }
    }
}

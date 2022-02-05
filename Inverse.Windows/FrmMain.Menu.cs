using Inverse.Domain.Model;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Inverse.Windows
{
    public partial class FrmMain
    {
        private string _currentFilename = string.Empty;

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FrmNewConnection(this);
            form.ShowDialog();
            ToggleMenuButtons();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Arquivo de Modelo de dados|*.dm";

            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.FileName))
            {
                _currentFilename = dialog.FileName;

                var xml = new XmlDocument();
                xml.Load(dialog.FileName);
                var doc = xml.DocumentElement;

                var dbName = doc.GetAttribute("name");
                var dbGuid = doc.GetAttribute("id");
                var dbProvider = doc.GetAttribute("provider");
                var dbConnectionString = doc.GetAttribute("connectionstring");
                var dbId = Guid.Parse(dbGuid);

                var database = new Database(Enum.Parse<Provider>(dbProvider))
                {
                    Id = dbId,
                    Name = dbName,
                    ConnectionString = dbConnectionString
                };

                var tables = doc.SelectNodes("//table");

                foreach (XmlNode xmlTable in tables)
                {
                    var tbGuid = xmlTable.Attributes["id"]?.Value ?? Guid.NewGuid().ToString();
                    var tbName = xmlTable.Attributes["name"].Value;
                    var tbLeft = xmlTable.Attributes["left"].Value;
                    var tbTop = xmlTable.Attributes["top"].Value;
                    var isHidden = xmlTable.Attributes["isHidden"]?.Value ?? "false";
                    var table = new Table
                    {
                        Id = tbGuid,
                        Name = tbName,
                        Database = database,
                        Left = int.Parse(tbLeft),
                        Top = int.Parse(tbTop),
                        IsHidden = bool.Parse(isHidden)
                    };

                    var xmlColumns = xmlTable.SelectNodes(".//column");

                    foreach (XmlNode xmlColumn in xmlColumns)
                    {
                        var colGuid = xmlColumn.Attributes["id"]?.Value ?? Guid.NewGuid().ToString();
                        var colName = xmlColumn.Attributes["name"].Value;
                        var colIndex = xmlColumn.Attributes["index"].Value;
                        var colType = xmlColumn.Attributes["type"].Value;
                        var colRequired = xmlColumn.Attributes["required"].Value;
                        var colClass = xmlColumn.Attributes["class"].Value;

                        if (colClass.Equals(nameof(Column)))
                        {
                            var column = new Column
                            {
                                Id = colGuid,
                                Name = colName,
                                Type = colType,
                                Table = table,
                                Index = int.Parse(colIndex),
                                Required = bool.Parse(colRequired)
                            };

                            table.Add(column);
                        }
                        else if (colClass.Equals(nameof(ForeignKey)))
                        {
                            var colRelatedTable = xmlColumn.Attributes["relatedTable"].Value;
                            var colRelatedColumn = xmlColumn.Attributes["relatedColumn"].Value;

                            var column = new ForeignKey
                            {
                                Id = colGuid,
                                Name = colName,
                                Type = colType,
                                Table = table,
                                Index = int.Parse(colIndex),
                                Required = bool.Parse(colRequired),
                                RelatedColumn = colRelatedColumn,
                                RelatedTable = colRelatedTable
                            };

                            table.Add(column);
                        }
                        else if (colClass.Equals(nameof(PrimaryKey)))
                        {
                            var column = new PrimaryKey
                            {
                                Id = colGuid,
                                Name = colName,
                                Type = colType,
                                Table = table,
                                Index = int.Parse(colIndex),
                                Required = bool.Parse(colRequired)
                            };

                            table.Add(column);
                        }
                    }

                    database.Tables.Add(table);
                }

                _database = database;
                panel1.Invalidate();
            }

            ToggleMenuButtons();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_database.IsEmpty)
                return;

            if (string.IsNullOrEmpty(_currentFilename))
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Arquivo de Modelo de dados|*.dm",
                    DefaultExt = ".dm"
                };

                dialog.ShowDialog();

                _currentFilename = dialog.FileName;
            }

            if (string.IsNullOrEmpty(_currentFilename))
                return;

            using (var sw = new StreamWriter(_currentFilename))
            {
                sw.WriteLine($"<database name=\"{_database.Name}\" id=\"{_database.Id}\" provider=\"{_database.Provider}\" connectionstring=\"{_database.ConnectionString}\">");
                sw.WriteLine($"    <tables>");
                foreach (var table in _database.Tables)
                {
                    sw.WriteLine($"        <table id=\"{table.Id}\" name=\"{table.Name}\" left=\"{table.Left}\" top=\"{table.Top}\" isHidden=\"{table.IsHidden}\">");
                    sw.WriteLine($"            <columns>");

                    foreach (var column in table.Columns)
                    {
                        if (column is ForeignKey fk)
                        {
                            sw.WriteLine($"                <column id=\"{column.Id}\" name=\"{column.Name}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.Required}\" class=\"{column.GetType().Name}\" relatedTable=\"{fk.RelatedTable}\" relatedColumn=\"{fk.RelatedColumn}\"/>");
                        }
                        else if (column is PrimaryKey pk)
                        {
                            sw.WriteLine($"                <column id=\"{pk.Id}\" name=\"{pk.Name}\" index=\"{pk.Index}\" type=\"{pk.Type}\" required=\"{pk.Required}\" class=\"{pk.GetType().Name}\" />");
                        }
                        else
                        {
                            sw.WriteLine($"                <column id=\"{column.Id}\" name=\"{column.Name}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.Required}\" class=\"{column.GetType().Name}\" />");
                        }
                    }

                    sw.WriteLine($"            </columns>");
                    sw.WriteLine($"        </table>");
                }
                sw.WriteLine($"    </tables>");
                sw.WriteLine($"</database>");
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString))
                return;

            var database = _service.LoadDatabase(_provider, _connectionString);

            UseDatabase(database);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _database = new Database(_provider);
            _currentFilename = null;
            ResetPanelSize();
            ToggleMenuButtons();
        }

        private void scriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_database.IsEmpty)
                return;

            var dialog = new SaveFileDialog
            {
                Filter = "Structured Query Language|*.sql",
                DefaultExt = ".sql"
            };

            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            _service.Export(_database, dialog.FileName);

            MessageBox.Show("Script exportado com sucesso.");
        }

        private void showHiddenTablesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lastFilename = _currentFilename;
            _currentFilename = null;

            saveToolStripMenuItem_Click(sender, e);

            _currentFilename = string.IsNullOrEmpty(_currentFilename) ? lastFilename : _currentFilename;
        }

        private void arrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Arrange();
        }

        private void ToggleMenuButtons()
        {
            saveAsToolStripMenuItem.Enabled
                = saveToolStripMenuItem.Enabled
                = refreshToolStripMenuItem.Enabled
                = scriptToolStripMenuItem.Enabled
                = closeToolStripMenuItem.Enabled
                = exportToolStripMenuItem.Enabled 
                = arrangeToolStripMenuItem.Enabled = !_database.IsEmpty;
        }
    }
}

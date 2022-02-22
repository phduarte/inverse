using Inverse.Domain.Model;
using Inverse.Domain.Services;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Inverse.Plugin.FileManager.EncryptedXml
{
    public sealed class XmlFileManagerStrategy : IFileManagerStrategy
    {
        public string Name => nameof(XmlFileManagerStrategy);

        public string Description => "Arquivo formato xml";

        public string Extension => ".xml";

        public Database OpenFile(string fileName)
        {
            string encodedContent = File.ReadAllText(fileName);
            
            var xml = new XmlDocument();
            xml.LoadXml(encodedContent);
            var doc = xml.DocumentElement;

            string dbName = doc.GetAttribute("name");
            string dbGuid = doc.GetAttribute("id");
            string dbProvider = doc.GetAttribute("provider");
            string dbConnectionString = doc.GetAttribute("connectionstring");
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
                    var colRelatedTable = xmlColumn.Attributes["relatedTable"]?.Value;
                    var colRelatedColumn = xmlColumn.Attributes["relatedColumn"]?.Value;

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
                    else if (colClass.Equals(nameof(ForeignPrimaryKey)))
                    {
                        var column = new ForeignPrimaryKey
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
                }

                database.Add(table);
            }

            return database;
        }

        public void SaveFile(Database database, string fileName)
        {
            using var sw = new StreamWriter(fileName);
            var content = new StringBuilder();

            content.AppendLine($"<database name=\"{database.Name}\" id=\"{database.Id}\" provider=\"{database.Provider}\" connectionstring=\"{database.ConnectionString}\">");
            content.AppendLine($"    <tables>");

            foreach (var table in database.Tables)
            {
                content.AppendLine($"        <table id=\"{table.Id}\" name=\"{table.Name}\" left=\"{table.Left}\" top=\"{table.Top}\" isHidden=\"{table.IsHidden}\">");
                content.AppendLine($"            <columns>");

                foreach (var column in table.Columns)
                {
                    if (column is ForeignKey fk)
                    {
                        content.AppendLine($"                <column id=\"{column.Id}\" name=\"{column.Name}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.Required}\" class=\"{column.GetType().Name}\" relatedTable=\"{fk.RelatedTable}\" relatedColumn=\"{fk.RelatedColumn}\"/>");
                    }
                    else if (column is PrimaryKey pk)
                    {
                        content.AppendLine($"                <column id=\"{pk.Id}\" name=\"{pk.Name}\" index=\"{pk.Index}\" type=\"{pk.Type}\" required=\"{pk.Required}\" class=\"{pk.GetType().Name}\" />");
                    }
                    else
                    {
                        content.AppendLine($"                <column id=\"{column.Id}\" name=\"{column.Name}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.Required}\" class=\"{column.GetType().Name}\" />");
                    }
                }

                content.AppendLine($"            </columns>");
                content.AppendLine($"        </table>");
            }
            content.AppendLine($"    </tables>");
            content.AppendLine($"</database>");

            sw.Write(content);
        }
    }
}

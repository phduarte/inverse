using Inverse.Domain;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Inverse.Plugin.FileManager.EncryptedXml;

public sealed class EncryptedXmlFileManagerStrategy : IFileManagerStrategy
{
    private static readonly byte[] KEY = new byte[] { 105, 110, 118, 101, 114, 115, 101, 95, 100, 98, 95, 118, 49, 48, 48, 33 };
    private static readonly byte[] INIT_VECTOR = new byte[16];

    public string Name => nameof(EncryptedXmlFileManagerStrategy);

    public string Description => "Arquivo de Modelo de dados";

    public string Extension => ".idb";

    public Database OpenFile(string fileName)
    {
        string encodedContent = File.ReadAllText(fileName);
        string decoredContent = DecryptString(encodedContent);

        var xml = new XmlDocument();
        xml.LoadXml(decoredContent);
        var doc = xml.DocumentElement;

        var isbase64 = Convert.ToBoolean(doc.Attributes["isbase64"]?.Value);
        string dbName = doc.GetFromAttribute("name", isbase64);
        string dbGuid = doc.GetFromAttribute("id");
        string dbProvider = doc.GetFromAttribute("provider");
        string dbConnectionString = doc.GetFromAttribute("connectionstring", isbase64);
        var dbId = Guid.Parse(dbGuid);

        if (string.IsNullOrEmpty(dbProvider))
        {
            dbProvider = Provider.MSSQLServer.ToString();
        }

        var database = new Database
        {
            Id = dbId,
            Name = dbName,
            Provider = Enum.Parse<Provider>(dbProvider),
            ConnectionString = dbConnectionString
        };

        var tables = doc.SelectNodes("//table");

        foreach (XmlNode xmlTable in tables)
        {
            var tbGuid = xmlTable.GetFromAttribute("id") ?? Guid.NewGuid().ToString();
            var tbName = xmlTable.GetFromAttribute("name", isbase64);
            var tabNotes = xmlTable.GetFromAttribute("notes", isbase64);
            var tbLeft = xmlTable.GetFromAttribute("left");
            var tbTop = xmlTable.GetFromAttribute("top");
            var isHidden = xmlTable.GetFromAttribute("isHidden") ?? "false";
            var seedData = xmlTable.SelectSingleNode(".//data")?.InnerText.FromBase64();

            var table = new Table
            {
                Id = tbGuid,
                Name = tbName,
                Database = database,
                Left = int.Parse(tbLeft),
                Top = int.Parse(tbTop),
                IsHidden = bool.Parse(isHidden),
                SeedData = seedData
            };

            table.AddComments(Comment.FromNotes(tabNotes));

            var xmlColumns = xmlTable.SelectNodes(".//column");

            foreach (XmlNode xmlColumn in xmlColumns)
            {
                var colGuid = xmlColumn.GetFromAttribute("id") ?? Guid.NewGuid().ToString();
                var colName = xmlColumn.GetFromAttribute("name", isbase64);
                var colDesc = xmlColumn.GetFromAttribute("description", isbase64);
                var colIndex = xmlColumn.GetFromAttribute("index");
                var colType = xmlColumn.GetFromAttribute("type");
                var colRequired = xmlColumn.GetFromAttribute("required");
                var colClass = xmlColumn.GetFromAttribute("class");
                var colRelatedTable = xmlColumn.GetFromAttribute("relatedTable", isbase64);
                var colRelatedColumn = xmlColumn.GetFromAttribute("relatedColumn", isbase64);
                var colDefaultValue = xmlColumn.GetFromAttribute("defaultValue", isbase64);

                if (colClass.Equals(nameof(Column)))
                {
                    var column = new Column
                    {
                        Id = colGuid,
                        Name = colName,
                        Description = colDesc,
                        Type = colType,
                        Table = table,
                        Index = int.Parse(colIndex),
                        IsRequired = bool.Parse(colRequired),
                        DefaultValue = colDefaultValue,
                    };

                    table.AddColumn(column);
                }
                else if (colClass.Equals(nameof(ForeignKey)))
                {
                    var column = new ForeignKey
                    {
                        Id = colGuid,
                        Name = colName,
                        Description = colDesc,
                        Type = colType,
                        Table = table,
                        Index = int.Parse(colIndex),
                        IsRequired = bool.Parse(colRequired),
                        DefaultValue = colDefaultValue,
                        RelatedColumn = colRelatedColumn,
                        RelatedTable = colRelatedTable
                    };

                    table.AddColumn(column);
                }
                else if (colClass.Equals(nameof(PrimaryKey)))
                {
                    var column = new PrimaryKey
                    {
                        Id = colGuid,
                        Name = colName,
                        Description = colDesc,
                        Type = colType,
                        Table = table,
                        Index = int.Parse(colIndex),
                        IsRequired = bool.Parse(colRequired),
                        DefaultValue = colDefaultValue,
                    };

                    table.AddColumn(column);
                }
                else if (colClass.Equals(nameof(ForeignPrimaryKey)))
                {
                    var column = new ForeignPrimaryKey
                    {
                        Id = colGuid,
                        Name = colName,
                        Description = colDesc,
                        Type = colType,
                        Table = table,
                        Index = int.Parse(colIndex),
                        IsRequired = bool.Parse(colRequired),
                        RelatedColumn = colRelatedColumn,
                        RelatedTable = colRelatedTable,
                        DefaultValue = colDefaultValue,
                    };

                    table.AddColumn(column);
                }
            }

            database.AddTable(table);
        }

        return database;
    }

    public void SaveFile(Database database, string fileName)
    {
        using var sw = new StreamWriter(fileName);
        var content = new StringBuilder();

        content.AppendLine($"<database name=\"{database.Name.ToBase64()}\" id=\"{database.Id}\" provider=\"{database.Provider}\" connectionstring=\"{database.ConnectionString.ToBase64()}\" isbase64=\"True\" >");
        content.AppendLine($"    <tables>");

        foreach (var table in database.Tables)
        {
            content.AppendLine($"        <table id=\"{table.Id}\" name=\"{table.Name.ToBase64()}\" notes=\"{table.Notes.ToBase64()}\" left=\"{table.Left}\" top=\"{table.Top}\" isHidden=\"{table.IsHidden}\">");
            content.AppendLine($"            <columns>");

            foreach (var column in table.Columns)
            {
                if (column is ForeignKey fk)
                {
                    content.AppendLine($"                <column id=\"{column.Id}\" name=\"{column.Name.ToBase64()}\" description=\"{column.Description.ToBase64()}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.IsRequired}\" defaultValue=\"{column.DefaultValue}\" class=\"{column.GetType().Name}\" relatedTable=\"{fk.RelatedTable.ToBase64()}\" relatedColumn=\"{fk.RelatedColumn.ToBase64()}\"/>");
                }
                else if (column is PrimaryKey pk)
                {
                    content.AppendLine($"                <column id=\"{pk.Id}\" name=\"{pk.Name.ToBase64()}\" description=\"{column.Description.ToBase64()}\" index=\"{pk.Index}\" type=\"{pk.Type}\" required=\"{pk.IsRequired}\" defaultValue=\"{pk.DefaultValue}\" class=\"{pk.GetType().Name}\" />");
                }
                else
                {
                    content.AppendLine($"                <column id=\"{column.Id}\" name=\"{column.Name.ToBase64()}\" description=\"{column.Description.ToBase64()}\" index=\"{column.Index}\" type=\"{column.Type}\" required=\"{column.IsRequired}\" defaultValue=\"{column.DefaultValue}\" class=\"{column.GetType().Name}\" />");
                }
            }

            content.AppendLine($"            </columns>");
            content.AppendLine($"            <data>{table.SeedData.ToBase64()}</data>");
            content.AppendLine($"        </table>");
        }
        content.AppendLine($"    </tables>");
        content.AppendLine($"</database>");

        string encoded = EncryptString(content.ToString());

        sw.Write(encoded);
    }

    private static string EncryptString(string plainText)
    {
        using RijndaelManaged rijAlg = new();
        rijAlg.Key = KEY;
        rijAlg.IV = INIT_VECTOR;

        ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (StreamWriter swEncrypt = new(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }
        byte[] encrypted = msEncrypt.ToArray();
        return Convert.ToBase64String(encrypted);
    }

    private static string DecryptString(string encodedText)
    {
        try
        {
            byte[] cipherText = Convert.FromBase64String(encodedText);
            using RijndaelManaged rijAlg = new();

            rijAlg.Key = KEY;
            rijAlg.IV = INIT_VECTOR;

            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            using MemoryStream msDecrypt = new(cipherText);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            string plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }
        catch
        {
            return encodedText;
        }
    }
}
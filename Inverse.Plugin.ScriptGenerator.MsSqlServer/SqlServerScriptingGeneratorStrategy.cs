using Inverse.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inverse.Plugin.ScriptGenerator.MsSqlServer;

public sealed class SqlServerScriptingGeneratorStrategy : IScriptingGeneratorStrategy
{
    public string Name => "SQL Server Scripting";

    public string Extension => ".mssql.sql";

    public void ExportToFile(Database database, string filename)
    {
        var exportTables = database.GetAllTablesOrderedByDependency();

        using var sw = new StreamWriter(filename);
        var sql = new System.Text.StringBuilder();

        sql.AppendLine($"/*");
        sql.AppendLine($"Scripting Created By Reversing DB");
        sql.AppendLine($"Created By: {Environment.UserName}");
        sql.AppendLine($"Created At: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sql.AppendLine($"Original: {database.ConnectionString}");
        sql.AppendLine($"*/");
        sql.AppendLine($"if (select count(*) from sys.databases where name = '{database.Name}') = 0");
        sql.AppendLine("BEGIN");
        sql.AppendLine($"\tCREATE DATABASE [{database.Name}];");
        sql.AppendLine("END");
        sql.AppendLine();
        sql.AppendLine($"GO");
        sql.AppendLine();
        sql.AppendLine($"USE [{database.Name}];");
        sql.AppendLine();
        sql.AppendLine("BEGIN TRANSACTION");
        sql.AppendLine();
        sql.AppendLine("-------- CLEAR DB -----------------------------------------------------------");
        sql.AppendLine();

        for (var u = exportTables.Count(); u > 0; u--)
        {
            sql.AppendLine($"DROP TABLE IF EXISTS [{exportTables.ElementAt(u - 1).Name}];");
        }

        sql.AppendLine();
        sql.AppendLine("-------- CREATE TABLES -----------------------------------------------------------");
        sql.AppendLine();

        foreach (var t in exportTables)
        {
            sql.AppendLine($"CREATE TABLE [{t.Name}]");
            sql.AppendLine($"(");
            sql.AppendLine(string.Join(",\r\n", t.Columns.Select(s => GetColumnScript(s))) + ",");

            if (t.PrimaryKeysCount > 1 || t.ForeignKeysCount > 1)
            {
                sql.AppendLine();

                if (t.PrimaryKeysCount > 1)
                {
                    sql.AppendLine($"\tCONSTRAINT PK_{t.Name.ToUpper()} PRIMARY KEY ({string.Join(",", t.PrimaryKeys.Select(s => s.Name))}),");
                }

                if (t.ForeignKeysCount > 1)
                {
                    foreach (var relatedTableName in t.ForeignKeys.Select(f => f.RelatedTable).Distinct())
                    {
                        var chaves = t.ForeignKeys.Where(r => r.RelatedTable.Equals(relatedTableName));
                        var chavesOriginal = database.Tables.First(t => t.Name.Equals(relatedTableName)).Columns.Select(c => new
                        {
                            c.Name,
                            Related = chaves.FirstOrDefault(k => k.RelatedColumn.Equals(c.Name))
                        })
                            .Where(w => w.Related != null);

                        sql.AppendLine($"\tCONSTRAINT FK_{t.Name.ToUpper()}_{relatedTableName.ToUpper()} FOREIGN KEY ({string.Join(",", chavesOriginal.Select(s => s.Name))}) REFERENCES [{relatedTableName}]({string.Join(",", chavesOriginal.Select(s => s.Related.Name))}),");
                    }
                }
            }

            var idx = sql.ToString().LastIndexOf(',');

            sql.Remove(idx, 1); //remove a última vírgula
            sql.AppendLine($")");
            sql.AppendLine();

            // Annotations / descriptions via extended properties
            AppendExtendedProperties(sql, t);
        }

        sql.AppendLine();
        sql.AppendLine("-------- SEED -----------------------------------------------------------");

        // script de seed
        foreach (var t in exportTables)
        {
            var insertSqlStatement = ConvertJsonToInsertStatement(t.Name, t.SeedData);
            if (insertSqlStatement != null)
            {
                sql.AppendLine(insertSqlStatement.ToString());
                sql.AppendLine();
            }
        }

        sql.AppendLine("-------- COMMIT -----------------------------------------------------------");

        sql.AppendLine("COMMIT;");
        sw.Write(sql.ToString());
    }

    /// <summary>
    /// Emits EXEC sp_addextendedproperty statements for table-level and column-level
    /// annotations (Description and Comments).
    /// </summary>
    private static void AppendExtendedProperties(System.Text.StringBuilder sql, Table t)
    {
        // Collect the table-level description: prefer Comments, fall back to Notes.
        var tableDescription = BuildTableDescription(t);

        if (!string.IsNullOrWhiteSpace(tableDescription))
        {
            sql.AppendLine(FormatTableExtendedProperty(t.Name, "MS_Description", tableDescription));
        }

        // Column-level descriptions
        foreach (var col in t.Columns)
        {
            if (!string.IsNullOrWhiteSpace(col.Description))
            {
                sql.AppendLine(FormatColumnExtendedProperty(t.Name, col.Name, "MS_Description", col.Description));
            }
        }
    }

    private static string BuildTableDescription(Table t)
    {
        // If there are structured comments, format them; otherwise use the plain Notes string.
        if (t.Comments.Count > 0)
        {
            var lines = t.Comments.Select(c =>
                $"[{c.Date:yyyy-MM-dd HH:mm:ss}] {c.Author}: {c.Text}");
            return string.Join(" | ", lines);
        }

        return t.Notes?.Trim();
    }

    private static string FormatTableExtendedProperty(string tableName, string propertyName, string value)
    {
        var escaped = value.Replace("'", "''");
        return
            $"EXEC sys.sp_addextendedproperty " +
            $"@name = N'{propertyName}', " +
            $"@value = N'{escaped}', " +
            $"@level0type = N'SCHEMA', @level0name = N'dbo', " +
            $"@level1type = N'TABLE', @level1name = N'{tableName}';";
    }

    private static string FormatColumnExtendedProperty(string tableName, string columnName, string propertyName, string value)
    {
        var escaped = value.Replace("'", "''");
        return
            $"EXEC sys.sp_addextendedproperty " +
            $"@name = N'{propertyName}', " +
            $"@value = N'{escaped}', " +
            $"@level0type = N'SCHEMA', @level0name = N'dbo', " +
            $"@level1type = N'TABLE', @level1name = N'{tableName}', " +
            $"@level2type = N'COLUMN', @level2name = N'{columnName}';";
    }

    private string ConvertJsonToInsertStatement(string tableName, string seedData)
    {
        if (string.IsNullOrEmpty(seedData))
        {
            return null;
        }
        try
        {
            var jsonArray = System.Text.Json.JsonDocument.Parse(seedData).RootElement;
            if (jsonArray.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                throw new InvalidDataException("Seed data is not a valid JSON array.");
            }
            var insertStatements = new List<string>();
            var insertAdded = false;

            foreach (var jsonObject in jsonArray.EnumerateArray())
            {
                if (jsonObject.ValueKind != System.Text.Json.JsonValueKind.Object)
                {
                    throw new InvalidDataException("Seed data array contains non-object elements.");
                }
                var columns = new List<string>();
                var values = new List<string>();
                foreach (var property in jsonObject.EnumerateObject())
                {
                    columns.Add($"[{property.Name}]");
                    switch (property.Value.ValueKind)
                    {
                        case System.Text.Json.JsonValueKind.String:
                            values.Add($"'{property.Value.GetString().Replace("'", "''")}'");
                            break;
                        case System.Text.Json.JsonValueKind.Number:
                            values.Add(property.Value.GetRawText());
                            break;
                        case System.Text.Json.JsonValueKind.True:
                        case System.Text.Json.JsonValueKind.False:
                            values.Add(property.Value.GetBoolean() ? "1" : "0");
                            break;
                        case System.Text.Json.JsonValueKind.Null:
                            values.Add("NULL");
                            break;
                        default:
                            throw new InvalidDataException($"Unsupported JSON value kind: {property.Value.ValueKind}");
                    }
                }

                if (!insertAdded)
                {
                    insertStatements.Add($"INSERT INTO [{tableName}] ({string.Join(", ", columns)})");
                    insertStatements.Add("VALUES");
                    insertAdded = true;
                }

                var insertStatement = $"      ({string.Join(", ", values)}),";
                insertStatements.Add(insertStatement);
            }

            if (insertStatements.Any())
            {
                var ultimo = insertStatements.Last().Remove(insertStatements.Last().Length - 1, 1);

                insertStatements[insertStatements.Count - 1] = ultimo + ";";
            }

            return string.Join(Environment.NewLine, insertStatements);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidDataException("Seed data is not a valid JSON format.", ex);
        }
    }

    private static string GetColumnScript<T>(T column) where T : Column
    {
        var spec = new List<string>
        {
            "\t",
            $"[{column.Name}]",
            column.Type.ToUpper()
        };

        if (column.IsRequired)
        {
            spec.Add("NOT NULL");
        }

        if (column is PrimaryKey && column.Table.PrimaryKeysCount == 1)
        {
            spec.Add("PRIMARY KEY");
        }

        if (!string.IsNullOrEmpty(column.DefaultValue))
        {
            spec.Add($"DEFAULT({column.DefaultValue})");
        }

        if (column is ForeignKey fk && fk.Table.ForeignKeysCount == 1)
        {
            spec.Add($"REFERENCES [{fk.RelatedTable}]({fk.RelatedColumn})");
        }

        return string.Join(" ", spec);
    }
}
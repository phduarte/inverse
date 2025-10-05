using Inverse.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inverse.Plugin.ScriptGenerator.MsSqlServer;

public sealed class SqlServerScriptingGeneratorStrategy : IScriptingGeneratorStrategy
{
    public string Name => "SQL Server Scripting";

    public string Extension => ".sql";

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
        sql.AppendLine("BEGIN TRANSACTION");
        sql.AppendLine();

        for (var u = exportTables.Count(); u > 0; u--)
        {
            sql.AppendLine($"DROP TABLE IF EXISTS [{exportTables.ElementAt(u - 1).Name}];");
        }

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
        }

        sql.AppendLine("COMMIT;");
        sw.Write(sql.ToString());
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
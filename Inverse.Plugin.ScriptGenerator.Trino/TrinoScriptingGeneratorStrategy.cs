using Inverse.Domain;

namespace Inverse.Plugin.ScriptGenerator.Trino;

public sealed class TrinoScriptingGeneratorStrategy : IScriptingGeneratorStrategy
{
    public string Name => "Trino Scripting";

    public string Extension => ".trino.sql";

    public void ExportToFile(Database database, string filename)
    {
        var exportTables = database.GetAllTablesOrderedByDependency();

        using var sw = new StreamWriter(filename);
        var sql = new System.Text.StringBuilder();

        sql.AppendLine($"--");
        sql.AppendLine($"-- Scripting Created By Reversing DB");
        sql.AppendLine($"-- Created By: {Environment.UserName}");
        sql.AppendLine($"-- Created At: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sql.AppendLine($"-- Original: {database.ConnectionString}");
        sql.AppendLine($"--");
        sql.AppendLine();
        sql.AppendLine($"CREATE SCHEMA IF NOT EXISTS \"{database.Name}\";");
        sql.AppendLine();
        sql.AppendLine("-------- CLEAR TABLES -----------------------------------------------------------");
        sql.AppendLine();

        for (var u = exportTables.Count(); u > 0; u--)
        {
            sql.AppendLine($"DROP TABLE IF EXISTS \"{database.Name}\".\"{exportTables.ElementAt(u - 1).Name}\";");
        }

        sql.AppendLine();
        sql.AppendLine("-------- CREATE TABLES -----------------------------------------------------------");
        sql.AppendLine();

        foreach (var t in exportTables)
        {
            sql.AppendLine($"CREATE TABLE \"{database.Name}\".\"{t.Name}\"");
            sql.AppendLine($"(");
            sql.AppendLine(string.Join(",\r\n", t.Columns.Select(s => GetColumnScript(s))) + ",");

            if (t.PrimaryKeysCount > 1 || t.ForeignKeysCount > 1)
            {
                sql.AppendLine();

                if (t.PrimaryKeysCount > 1)
                {
                    sql.AppendLine($"\tCONSTRAINT PK_{t.Name.ToUpper()} PRIMARY KEY ({string.Join(", ", t.PrimaryKeys.Select(s => $"\"{s.Name}\""))}),");
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

                        sql.AppendLine($"\tCONSTRAINT FK_{t.Name.ToUpper()}_{relatedTableName.ToUpper()} FOREIGN KEY ({string.Join(", ", chavesOriginal.Select(s => $"\"{s.Name}\""))}) REFERENCES \"{database.Name}\".\"{relatedTableName}\"({string.Join(", ", chavesOriginal.Select(s => $"\"{s.Related!.Name}\""))}),");
                    }
                }
            }

            var idx = sql.ToString().LastIndexOf(',');
            sql.Remove(idx, 1); // remove a última vírgula
            sql.AppendLine($")");

            // Trino uses WITH for table properties (e.g. connector-specific options)
            sql.AppendLine($"WITH (");
            sql.AppendLine($"    format = 'ORC'");
            sql.AppendLine($");");
            sql.AppendLine();

            // Annotations as comments (Trino does not support extended properties)
            AppendTableComments(sql, t);
        }

        sql.AppendLine();
        sql.AppendLine("-------- SEED -----------------------------------------------------------");

        foreach (var t in exportTables)
        {
            var insertSqlStatement = ConvertJsonToInsertStatement(database.Name, t.Name, t.SeedData);
            if (insertSqlStatement != null)
            {
                sql.AppendLine(insertSqlStatement.ToString());
                sql.AppendLine();
            }
        }

        sw.Write(sql.ToString());
    }

    /// <summary>
    /// Emits SQL comments for table-level and column-level annotations,
    /// since Trino does not support extended properties like SQL Server.
    /// </summary>
    private static void AppendTableComments(System.Text.StringBuilder sql, Table t)
    {
        var tableDescription = BuildTableDescription(t);

        if (!string.IsNullOrWhiteSpace(tableDescription))
        {
            sql.AppendLine($"-- Table \"{t.Name}\": {tableDescription}");
        }

        foreach (var col in t.Columns)
        {
            if (!string.IsNullOrWhiteSpace(col.Description))
            {
                sql.AppendLine($"-- Column \"{t.Name}\".\"{col.Name}\": {col.Description}");
            }
        }

        if (!string.IsNullOrWhiteSpace(tableDescription) || t.Columns.Any(c => !string.IsNullOrWhiteSpace(c.Description)))
        {
            sql.AppendLine();
        }
    }

    private static string BuildTableDescription(Table t)
    {
        if (t.Comments.Count > 0)
        {
            var lines = t.Comments.Select(c =>
                $"[{c.Date:yyyy-MM-dd HH:mm:ss}] {c.Author}: {c.Text}");
            return string.Join(" | ", lines);
        }

        return t.Notes?.Trim() ?? string.Empty;
    }

    private string? ConvertJsonToInsertStatement(string schemaName, string tableName, string seedData)
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
                    columns.Add($"\"{property.Name}\"");
                    switch (property.Value.ValueKind)
                    {
                        case System.Text.Json.JsonValueKind.String:
                            values.Add($"'{property.Value.GetString()!.Replace("'", "''")}'");
                            break;
                        case System.Text.Json.JsonValueKind.Number:
                            values.Add(property.Value.GetRawText());
                            break;
                        case System.Text.Json.JsonValueKind.True:
                        case System.Text.Json.JsonValueKind.False:
                            values.Add(property.Value.GetBoolean() ? "true" : "false");
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
                    insertStatements.Add($"INSERT INTO \"{schemaName}\".\"{tableName}\" ({string.Join(", ", columns)})");
                    insertStatements.Add("VALUES");
                    insertAdded = true;
                }

                insertStatements.Add($"      ({string.Join(", ", values)}),");
            }

            if (insertStatements.Any())
            {
                var ultimo = insertStatements.Last().TrimEnd(',');
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
            $"\"{column.Name}\"",
            MapToTrinoType(column.Type)
        };

        if (column.IsRequired)
        {
            spec.Add("NOT NULL");
        }

        if (column is PrimaryKey && column.Table.PrimaryKeysCount == 1)
        {
            // Trino does not enforce PRIMARY KEY constraints natively (metadata only)
            spec.Add("WITH (primary_key = true)");
        }

        if (!string.IsNullOrEmpty(column.DefaultValue))
        {
            spec.Add($"DEFAULT {column.DefaultValue}");
        }

        // Trino does not support FOREIGN KEY constraints natively; emit as comment
        if (column is ForeignKey fk && fk.Table.ForeignKeysCount == 1)
        {
            spec.Add($"-- REFERENCES \"{fk.RelatedTable}\"(\"{fk.RelatedColumn}\")");
        }

        return string.Join(" ", spec);
    }

    /// <summary>
    /// Maps common SQL/generic type names to Trino-compatible data types.
    /// </summary>
    private static string MapToTrinoType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return "VARCHAR";

        return type.ToUpperInvariant() switch
        {
            "INT" or "INTEGER" or "INT4"                        => "INTEGER",
            "BIGINT" or "INT8" or "LONG"                        => "BIGINT",
            "SMALLINT" or "INT2"                                => "SMALLINT",
            "TINYINT"                                           => "TINYINT",
            "DECIMAL" or "NUMERIC"                              => "DECIMAL",
            "FLOAT" or "FLOAT4" or "REAL"                       => "REAL",
            "FLOAT8" or "DOUBLE" or "DOUBLE PRECISION"          => "DOUBLE",
            "BOOLEAN" or "BIT" or "BOOL"                        => "BOOLEAN",
            "VARCHAR" or "NVARCHAR" or "TEXT" or "STRING"
                or "NTEXT" or "CHAR" or "NCHAR"                 => "VARCHAR",
            "DATE"                                              => "DATE",
            "TIME"                                              => "TIME",
            "DATETIME" or "DATETIME2" or "SMALLDATETIME"        => "TIMESTAMP",
            "TIMESTAMP"                                         => "TIMESTAMP",
            "TIMESTAMPTZ" or "DATETIMEOFFSET"                   => "TIMESTAMP WITH TIME ZONE",
            "UUID" or "UNIQUEIDENTIFIER"                        => "UUID",
            "VARBINARY" or "BINARY" or "IMAGE" or "BYTEA"       => "VARBINARY",
            "JSON" or "JSONB"                                   => "JSON",
            "ARRAY"                                             => "ARRAY(VARCHAR)",
            "MAP"                                               => "MAP(VARCHAR, VARCHAR)",
            "ROW"                                               => "ROW(value VARCHAR)",
            _                                                   => type.ToUpperInvariant()
        };
    }
}

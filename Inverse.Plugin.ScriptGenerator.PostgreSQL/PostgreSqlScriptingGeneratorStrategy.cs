using Inverse.Domain;
using System.Text;

namespace Inverse.Plugin.ScriptGenerator.PostgreSql;

public sealed class PostgreSqlScriptingGeneratorStrategy : IScriptingGeneratorStrategy
{
    public string Name => "PostgreSQL Scripting";
    public string Extension => ".pgsql.sql";

    public void ExportToFile(Database database, string filename)
    {
        var exportTables = database.GetAllTablesOrderedByDependency();

        using var sw = new StreamWriter(filename);
        var sql = new StringBuilder();

        sql.AppendLine("/*");
        sql.AppendLine("Scripting Created By Reversing DB");
        sql.AppendLine($"Created By: {Environment.UserName}");
        sql.AppendLine($"Created At: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sql.AppendLine($"Original: {database.ConnectionString}");
        sql.AppendLine("*/");
        sql.AppendLine();

        sql.AppendLine($"DO $$");
        sql.AppendLine($"BEGIN");
        sql.AppendLine($"   IF NOT EXISTS (SELECT FROM pg_database WHERE datname = '{database.Name}') THEN");
        sql.AppendLine($"      CREATE DATABASE \"{database.Name}\";");
        sql.AppendLine($"   END IF;");
        sql.AppendLine($"END $$;");
        sql.AppendLine();

        sql.AppendLine($"\\c \"{database.Name}\";");
        sql.AppendLine();

        sql.AppendLine("BEGIN;");
        sql.AppendLine();
        sql.AppendLine("-------- CLEAR DB -----------------------------------------------------------");
        sql.AppendLine();

        for (var u = exportTables.Count(); u > 0; u--)
        {
            sql.AppendLine($"DROP TABLE IF EXISTS \"{exportTables.ElementAt(u - 1).Name}\" CASCADE;");
        }

        sql.AppendLine();
        sql.AppendLine("-------- CREATE TABLES -----------------------------------------------------------");
        sql.AppendLine();

        foreach (var t in exportTables)
        {
            sql.AppendLine($"CREATE TABLE \"{t.Name}\" (");
            sql.AppendLine(string.Join(",\n", t.Columns.Select(s => GetColumnScript(s, t))) + ",");

            // Chaves primárias compostas
            if (t.PrimaryKeysCount > 1)
            {
                sql.AppendLine($"\n   CONSTRAINT \"PK_{t.Name.ToLower()}\" PRIMARY KEY ({string.Join(", ", t.PrimaryKeys.Select(s => $"\"{s.Name}\""))}),");
            }

            // Chaves estrangeiras compostas
            if (t.ForeignKeysCount > 1)
            {
                foreach (var relatedTableName in t.ForeignKeys.Select(f => f.RelatedTable).Distinct())
                {
                    var chaves = t.ForeignKeys.Where(r => r.RelatedTable.Equals(relatedTableName));
                    var chavesOriginal = database.Tables.First(tab => tab.Name.Equals(relatedTableName)).Columns.Select(c => new
                    {
                        c.Name,
                        Related = chaves.FirstOrDefault(k => k.RelatedColumn.Equals(c.Name))
                    })
                        .Where(w => w.Related != null);

                    sql.AppendLine(
                        $"   CONSTRAINT \"FK_{t.Name.ToLower()}_{relatedTableName.ToLower()}\" FOREIGN KEY ({string.Join(", ", chavesOriginal.Select(s => $"\"{s.Name}\""))}) REFERENCES \"{relatedTableName}\"({string.Join(", ", chavesOriginal.Select(s => $"\"{s.Related.Name}\""))}),"
                    );
                }
            }

            // Remove última vírgula
            var idx = sql.ToString().LastIndexOf(',');
            if (idx >= 0)
                sql.Remove(idx, 1);

            sql.AppendLine($"\n);");
            sql.AppendLine();
        }

        sql.AppendLine();
        sql.AppendLine("-------- SEED -----------------------------------------------------------");

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

    private static string GetColumnScript(Column column, Table table)
    {
        var spec = new List<string>
        {
            $"   \"{column.Name}\"",
            MapTypeToPostgres(column.Type)
        };

        if (column.IsRequired)
            spec.Add("NOT NULL");

        if (column.IsPrimaryKey && table.PrimaryKeysCount == 1)
            spec.Add("PRIMARY KEY");

        if (!string.IsNullOrEmpty(column.DefaultValue))
            spec.Add($"DEFAULT {FormatDefaultValue(column.DefaultValue, column.Type)}");

        if (column.IsForeignKey && table.ForeignKeysCount == 1)
        {
            spec.Add($"REFERENCES \"{((ForeignKey)column).RelatedTable}\"(\"{((ForeignKey)column).RelatedColumn}\")");
        }

        return string.Join(" ", spec);
    }

    private static string MapTypeToPostgres(string type)
    {
        // Mapeamento básico de tipos SQL Server para PostgreSQL
        return type.ToLower() switch
        {
            "int" or "integer" => "INTEGER",
            "bigint" => "BIGINT",
            "smallint" => "SMALLINT",
            "tinyint" => "SMALLINT",
            "bit" => "BOOLEAN",
            "nvarchar" or "varchar" or "text" or "ntext" => "TEXT",
            "char" or "nchar" => "CHAR(1)",
            "datetime" or "smalldatetime" or "date" => "TIMESTAMP",
            "float" => "DOUBLE PRECISION",
            "real" => "REAL",
            "decimal" or "numeric" or "money" or "smallmoney" => "NUMERIC",
            "uniqueidentifier" => "UUID",
            "binary" or "varbinary" or "image" => "BYTEA",
            _ => type.ToUpper()
        };
    }

    private static string FormatDefaultValue(string defaultValue, string type)
    {
        // Adapta valores padrão para sintaxe PostgreSQL
        if (type.ToLower().Contains("char") || type.ToLower().Contains("text"))
            return $"'{defaultValue.Replace("'", "''")}'";
        if (type.ToLower() == "boolean" || type.ToLower() == "bit")
            return defaultValue == "1" ? "TRUE" : "FALSE";
        return defaultValue;
    }

    private string ConvertJsonToInsertStatement(string tableName, string seedData)
    {
        if (string.IsNullOrEmpty(seedData))
            return null;

        try
        {
            var jsonArray = System.Text.Json.JsonDocument.Parse(seedData).RootElement;
            if (jsonArray.ValueKind != System.Text.Json.JsonValueKind.Array)
                throw new InvalidDataException("Seed data is not a valid JSON array.");

            var insertStatements = new List<string>();
            var insertAdded = false;

            foreach (var jsonObject in jsonArray.EnumerateArray())
            {
                if (jsonObject.ValueKind != System.Text.Json.JsonValueKind.Object)
                    throw new InvalidDataException("Seed data array contains non-object elements.");

                var columns = new List<string>();
                var values = new List<string>();
                foreach (var property in jsonObject.EnumerateObject())
                {
                    columns.Add($"\"{property.Name}\"");
                    switch (property.Value.ValueKind)
                    {
                        case System.Text.Json.JsonValueKind.String:
                            values.Add($"'{property.Value.GetString().Replace("'", "''")}'");
                            break;
                        case System.Text.Json.JsonValueKind.Number:
                            values.Add(property.Value.GetRawText());
                            break;
                        case System.Text.Json.JsonValueKind.True:
                            values.Add("TRUE");
                            break;
                        case System.Text.Json.JsonValueKind.False:
                            values.Add("FALSE");
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
                    insertStatements.Add($"INSERT INTO \"{tableName}\" ({string.Join(", ", columns)})");
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
}
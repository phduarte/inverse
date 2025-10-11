using Inverse.Domain;
using Npgsql;
using System.Data;

namespace Inverse.Plugin.DatabaseGenerator.PostgreSql;

public sealed class PostgreSqlDatabaseGeneratorStrategy : IDatabaseGeneratorStrategy
{
    public Provider Provider { get; } = Provider.PostgreSQL;

    public Database LoadDatabase(string connectionString)
    {
        var database = new Database
        {
            Id = Guid.NewGuid(),
            ConnectionString = connectionString,
            Name = GetDatabaseNameByConnectionString(connectionString),
            Provider = Provider
        };

        var commandText = @"
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public'
              AND table_type = 'BASE TABLE'
            ORDER BY table_name;
        ";

        foreach (var rdr in ExecuteReader(connectionString, commandText))
        {
            var table = new Table
            {
                Id = rdr["table_name"].ToString(),
                Name = rdr["table_name"].ToString(),
                Database = database
            };

            table.AddColumns(GetColumns(connectionString, table));
            table.AddColumns(GetForeignKeys(connectionString, table));

            database.AddTable(table);
        }

        return database;
    }

    private static string GetDatabaseNameByConnectionString(string connectionString)
    {
        using var cnn = new NpgsqlConnection(connectionString);
        cnn.Open();
        return cnn.Database;
    }

    private static IEnumerable<ForeignKey> GetForeignKeys(string connectionString, Table table)
    {
        var commandText = @"
            SELECT
                tc.constraint_name AS foreign_key_name,
                kcu.table_name AS table_name,
                kcu.column_name AS constraint_column_name,
                ccu.table_name AS referenced_table,
                ccu.column_name AS referenced_column_name
            FROM
                information_schema.table_constraints AS tc
                JOIN information_schema.key_column_usage AS kcu
                  ON tc.constraint_name = kcu.constraint_name
                 AND tc.table_schema = kcu.table_schema
                JOIN information_schema.constraint_column_usage AS ccu
                  ON ccu.constraint_name = tc.constraint_name
                 AND ccu.table_schema = tc.table_schema
            WHERE tc.constraint_type = 'FOREIGN KEY'
              AND kcu.table_name = @tableName;
        ";

        var param = new NpgsqlParameter("tableName", table.Name);

        foreach (var rdr in ExecuteReader(connectionString, commandText, param))
        {
            var relatedTable = rdr["referenced_table"].ToString();
            var from = rdr["constraint_column_name"].ToString();
            var to = rdr["referenced_column_name"].ToString();
            var col = table.Columns.FirstOrDefault(x => x.Name.Equals(from));

            if (col == null)
                continue;

            yield return new ForeignKey
            {
                Id = col.Id,
                Name = from,
                Type = col.Type,
                RelatedTable = relatedTable,
                RelatedColumn = to,
                Table = table,
                IsRequired = col.IsRequired
            };
        }
    }

    private static IEnumerable<Column> GetColumns(string connectionString, Table table)
    {
        var commandText = @"
            SELECT
                cols.ordinal_position AS column_id,
                cols.column_name,
                cols.is_nullable,
                cols.data_type,
                cols.character_maximum_length,
                cols.numeric_precision,
                (
                    SELECT tc.constraint_name
                    FROM information_schema.table_constraints tc
                    JOIN information_schema.key_column_usage kcu
                      ON tc.constraint_name = kcu.constraint_name
                     AND tc.table_schema = kcu.table_schema
                    WHERE tc.table_name = cols.table_name
                      AND kcu.column_name = cols.column_name
                      AND tc.constraint_type = 'PRIMARY KEY'
                    LIMIT 1
                ) AS primary_key_name
            FROM information_schema.columns cols
            WHERE cols.table_name = @tableName
              AND cols.table_schema = 'public'
            ORDER BY cols.ordinal_position;
        ";

        var param = new NpgsqlParameter("tableName", table.Name);

        foreach (var rdr in ExecuteReader(connectionString, commandText, param))
        {
            var id = rdr["column_id"].ToString();
            var name = rdr["column_name"].ToString();
            var type = rdr["data_type"].ToString();
            var required = rdr["is_nullable"].ToString()?.Equals("NO", StringComparison.OrdinalIgnoreCase) ?? false;
            var pk = rdr["primary_key_name"].ToString();
            var size = rdr["character_maximum_length"]?.ToString();
            var precision = rdr["numeric_precision"]?.ToString();

            if ((type == "character varying" || type == "varchar" || type == "character") && !string.IsNullOrEmpty(size))
            {
                type = $"{type}({size})";
            }
            else if ((type == "numeric" || type == "decimal") && !string.IsNullOrEmpty(precision) && !string.IsNullOrEmpty(size))
            {
                type = $"{type}({precision},{size})";
            }

            if (!string.IsNullOrEmpty(pk))
            {
                yield return new PrimaryKey
                {
                    Id = id,
                    Name = name,
                    Type = type,
                    Table = table,
                    IsRequired = required,
                };
            }
            else
            {
                yield return new Column
                {
                    Id = id,
                    Name = name,
                    Type = type,
                    Table = table,
                    IsRequired = required,
                };
            }
        }
    }

    private static IEnumerable<IDataReader> ExecuteReader(string connectionString, string commandText, params NpgsqlParameter[] parameters)
    {
        using var cnn = new NpgsqlConnection(connectionString);
        using var cmd = cnn.CreateCommand();
        cmd.CommandText = commandText;
        if (parameters != null && parameters.Length > 0)
            cmd.Parameters.AddRange(parameters);
        cnn.Open();

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            yield return reader;
        }
    }

    public static IDatabaseGeneratorStrategy Create()
    {
        return new PostgreSqlDatabaseGeneratorStrategy();
    }
}
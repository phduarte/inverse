using Inverse.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Inverse.Domain.Services
{
    internal class SqliteDatabaseGeneratorStrategy : IDatabaseGeneratorStrategy
    {
        public Provider Provider => Provider.SQLite;

        public Database LoadDatabase(string connectionString)
        {
            var database = new Database(Provider)
            {
                Id = Guid.NewGuid(),
                ConnectionString = connectionString
            };

            using (var cnn = new SQLiteConnection(connectionString))
            {
                database.Name = cnn.Database;

                using var cmd = cnn.CreateCommand();
                cmd.CommandText = @"SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";
                cnn.Open();

                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var table = new Table
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = rdr.GetString(0),
                        Database = database
                    };

                    table.AddRange(GetColumns(cnn, table));
                    table.AddRange(GetForeignKeys(cnn, table));

                    database.Add(table);
                }
            }

            return database;
        }

        private static IEnumerable<ForeignKey> GetForeignKeys(IDbConnection connection, Table table)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $@"SELECT * FROM pragma_foreign_key_list('{table.Name}')";
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var relatedTable = rdr.GetString(2);
                var from = rdr.GetString(3);
                var to = rdr.GetString(4);
                var col = table.Columns.FirstOrDefault(x => x.Name.Equals(from));

                yield return new ForeignKey
                {
                    Id = col.Id,
                    Name = col.Name,
                    //Name = $"FK_{table.Name}_{rdr.GetString(2)}",
                    Type = col.Type,
                    RelatedTable = relatedTable,
                    RelatedColumn = to,
                    Table = table,
                    Required = col.Required
                };
            }
        }

        private static IEnumerable<Column> GetColumns(IDbConnection connection, Table table)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $@"SELECT * FROM pragma_table_info('{table.Name}')";

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.NewGuid().ToString();
                var name = rdr.GetString(1);
                var type = rdr.GetString(2);
                var required = rdr.GetBoolean(3);
                var pk = rdr.GetBoolean(5);

                if (pk)
                {
                    yield return new PrimaryKey
                    {
                        Id = id,
                        Name = name,
                        Type = type,
                        Table = table,
                        Required = required,
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
                        Required = required,
                    };
                }
            }
        }

        public static IDatabaseGeneratorStrategy Create()
        {
            return new SqliteDatabaseGeneratorStrategy();
        }
    }
}

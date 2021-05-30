using EngenhariaReversaDb.Domain;
using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace EngenhariaReversaDb.Services
{
    class GenerateModelService : IGenerateModelService
    {
        public Provider Provider { get; }

        public GenerateModelService(Provider provider)
        {
            Provider = provider;
        }

        public Database GetDatabase(string connectionString)
        {
            var database = new Database
            {
                Id = Guid.NewGuid(),
                Name = "Database 1",
                ConnectionString = connectionString
            };

            using (var cnn = new SQLiteConnection(connectionString))
            {
                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";
                    cnn.Open();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var table = new Table
                            {
                                Id = Guid.NewGuid(),
                                Name = rdr.GetString(0),
                                Database = database
                            };

                            AddColumns(cnn, table);
                            AddForeignKeys(cnn, table);

                            database.Tables.Add(table);
                        }
                    }
                }
            }

            return database;
        }

        private void AddForeignKeys(IDbConnection connection, Table table)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $@"SELECT * FROM pragma_foreign_key_list('{table.Name}')";
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var relatedTable = rdr.GetString(2);
                        var from = rdr.GetString(3);
                        var to = rdr.GetString(4);
                        var col = table.Columns.FirstOrDefault(x => x.Name.Equals(from));

                        table.Add(new ForeignKey
                        {
                            Id = col.Id,
                            Name = col.Name,
                            //Name = $"FK_{table.Name}_{rdr.GetString(2)}",
                            Type = col.Type,
                            RelatedTable = relatedTable,
                            RelatedColumn = to,
                            Table = table,
                            Required = col.Required
                        });
                    }
                }
            }
        }

        private void AddColumns(IDbConnection connection, Table table)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $@"SELECT * FROM pragma_table_info('{table.Name}')";

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var pk = rdr.GetBoolean(5);

                        if (pk)
                        {
                            table.Add(new PrimaryKey
                            {
                                Id = Guid.NewGuid(),
                                Name = rdr.GetString(1),
                                Type = rdr.GetString(2),
                                Table = table,
                                Required = rdr.GetBoolean(3),
                            });
                        }
                        else
                        {
                            table.Add(new Column
                            {
                                Id = Guid.NewGuid(),
                                Name = rdr.GetString(1),
                                Type = rdr.GetString(2),
                                Table = table,
                                Required = rdr.GetBoolean(3),
                            });
                        }
                    }
                }
            }
        }
    }
}

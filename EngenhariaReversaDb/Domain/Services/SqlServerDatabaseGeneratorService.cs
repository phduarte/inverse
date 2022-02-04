using EngenhariaReversaDb.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EngenhariaReversaDb.Domain.Services
{
    internal class SqlServerDatabaseGeneratorService : IDatabaseGeneratorStrategy
    {
        public Provider Provider { get; }

        public SqlServerDatabaseGeneratorService(Provider provider)
        {
            Provider = provider;
        }

        public Database GetDatabase(string connectionString)
        {
            var database = new Database(Provider)
            {
                Id = Guid.NewGuid(),
                ConnectionString = connectionString,
                Name = GetDatabaseNameByConnectionString(connectionString)
            };

            var commandText = @"select object_id, name
                                from sys.all_objects 
                                where type = 'U' 
                                  and name not in ('sysdiagrams','trace_xe_action_map','trace_xe_event_map')
                                order by name";

            foreach (var rdr in ExecuteReader(connectionString, commandText))
            {
                var table = new Table
                {
                    Id = rdr["object_id"].ToString(),
                    Name = rdr["name"].ToString(),
                    Database = database
                };

                table.AddRange(GetColumns(connectionString, table));
                table.AddRange(GetForeignKeys(connectionString, table));

                database.Tables.Add(table);
            }

            return database;
        }

        private static string GetDatabaseNameByConnectionString(string connectionString)
        {
            using var cnn = new SqlConnection(connectionString);

            return cnn.Database;
        }

        private IEnumerable<ForeignKey> GetForeignKeys(string connectionString, Table table)
        {
            var tableId = new SqlParameter("tableId", table.Id);
            var commandText = $@"SELECT   
                                    f.name AS foreign_key_name,
                                    OBJECT_NAME(f.parent_object_id) AS table_name,
                                    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS constraint_column_name,
                                    OBJECT_NAME (f.referenced_object_id) AS referenced_object,
                                    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS referenced_column_name,
                                    is_disabled,
                                    delete_referential_action_desc,
                                    update_referential_action_desc  
                                    FROM sys.foreign_keys AS f  
                                INNER JOIN sys.foreign_key_columns AS fc   
                                    ON f.object_id = fc.constraint_object_id   
                                WHERE f.parent_object_id = @tableId";

            foreach (var rdr in ExecuteReader(connectionString, commandText, tableId))
            {
                var relatedTable = rdr["referenced_object"].ToString();
                var from = rdr["constraint_column_name"].ToString();
                var to = rdr["referenced_column_name"].ToString();
                var col = table.Columns.FirstOrDefault(x => x.Name.Equals(from));

                yield return new ForeignKey
                {
                    Id = col.Id,
                    Name = from,
                    Type = col.Type,
                    RelatedTable = relatedTable,
                    RelatedColumn = to,
                    Table = table,
                    Required = col.Required
                };
            }
        }

        private IEnumerable<Column> GetColumns(string connectionString, Table table)
        {
            var tableId = new SqlParameter("tableId", table.Id);
            var commandText = $@"select	c.column_id,
		                                c.name as column_name,
		                                c.is_nullable,
		                                t.name as type_name,
		                                c.max_length,
                                        c.precision,
		                                (
			                                select distinct i.name as primarykey_name
			                                from sys.indexes i 
			                                join sys.index_columns ic on i.object_id = ic.object_id
			                                where i.object_id = c.object_id and ic.column_id = c.column_id and i.is_primary_key = 1
		                                ) as primary_key_name
                                from sys.all_columns c
                                join sys.types t on c.user_type_id = t.user_type_id
                                where object_id = @tableId
                                order by c.column_id";

            foreach (var rdr in ExecuteReader(connectionString, commandText, tableId))
            {
                var id = rdr["column_id"].ToString();
                var name = rdr["column_name"].ToString();
                var type = rdr["type_name"].ToString();
                var required = rdr["is_nullable"].ToString().Equals("False");
                var pk = rdr["primary_key_name"].ToString();
                var size = rdr["max_length"].ToString();
                var precision = rdr["precision"].ToString();

                if (type == "varchar" || type == "char")
                {
                    type = $"{type}({size})";
                }
                else if (type == "numeric" || type == "decimal")
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

        private IEnumerable<IDataReader> ExecuteReader(string connectionString, string commandText, params SqlParameter[] parameters)
        {
            using var cnn = new SqlConnection(connectionString);
            using var cmd = cnn.CreateCommand();
            cmd.CommandText = commandText;
            cmd.Parameters.AddRange(parameters);
            cnn.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                yield return reader;
            }
        }
    }
}

using Inverse.Domain.Model;
using Inverse.Domain.Services;
using System;
using System.IO;
using Xunit;

namespace Inverse.Tests
{
    public class DatabaseServiceTests
    {
        [Fact]
        public void ShouldExportDatabaseWithOneTableToFile()
        {
            var svc = new DatabaseService();
            var db = CreateDatabaseWithOneTable();
            const string temp_file = "test.sql";

            if (File.Exists(temp_file))
            {
                File.Delete(temp_file);
            }

            svc.Export(db, temp_file);

            try
            {
                using (var sr = new StreamReader(temp_file))
                {
                    var text = sr.ReadToEnd();

                    //Assert.Contains("CREATE DATABASE security_users_db;\r\n", text);
                    //Assert.Contains("USE security_users_db", text);
                    //Assert.Contains("CREATE TABLE [users]", text);
                    //Assert.Contains("user_id STRING NOT NULL PRIMARY KEY,", text);
                    //Assert.Contains("username STRING NOT NULL", text);
                    Assert.Contains("CREATE DATABASE security_users_db;\r\n\r\nGO\r\n\r\nUSE security_users_db;\r\n\r\nCREATE TABLE [users]\r\n(\r\n\tuser_id STRING NOT NULL PRIMARY KEY,\r\n\tusername STRING NOT NULL\r\n)\r\n\r\n", text);
                }
            }
            finally
            {
                File.Delete(temp_file);
            }
        }

        private Database CreateDatabaseWithOneTable()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.NewGuid(),
                ConnectionString = "Data source=file.db",
                Name = "security_users_db",
            };

            var table = new Table
            {
                Id = "1",
                Name = "users",
                Database = db
            };

            var userId = new PrimaryKey
            {
                Id = "1",
                Name = "user_id",
                Type = "string",
                Index = 0,
                Required = true
            };

            var username = new Column
            {
                Id = "2",
                Name = "username",
                Type = "string",
                Index = 1,
                Required = true
            };

            table.Add(userId);
            table.Add(username);

            db.Add(table);

            return db;
        }
    }
}

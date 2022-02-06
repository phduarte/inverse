using Inverse.Domain.Model;
using Inverse.Domain.Services;
using System;
using System.IO;
using Xunit;
using FakeItEasy;

namespace Inverse.Tests
{
    public class DatabaseServiceTests
    {
        [Fact]
        public void ShouldExportDatabaseWithOneTableToFile()
        {
            var db = CreateDatabaseWithOneTable();
            var expected = "CREATE DATABASE security_users_db;\r\n\r\nGO\r\n\r\nUSE security_users_db;\r\n\r\nCREATE TABLE [users]\r\n(\r\n\tuser_id STRING NOT NULL PRIMARY KEY,\r\n\tusername STRING NOT NULL\r\n)\r\n\r\n";
            Assert.EndsWith(expected, ExportAndReadScript(db, "test1.sql"));
        }

        [Fact]
        public void ShouldExportDatabaseWithRelations()
        {
            var db = CreateDatabaseWithTwoTables();
            var expected = "CREATE TABLE [users]\r\n(\r\n\tuser_id INT NOT NULL PRIMARY KEY,\r\n\tusername STRING NOT NULL\r\n)\r\n\r\nCREATE TABLE [permissions]\r\n(\r\n\tpermission_id INT NOT NULL PRIMARY KEY,\r\n\tpermission STRING NOT NULL,\r\n\tuser_id INT NOT NULL REFERENCES users(user_id)\r\n)\r\n\r\n";
            Assert.EndsWith(expected, ExportAndReadScript(db, "test2.sql"));
        }

        [Fact]
        public void ShouldExportDatabaseWithCompositePrimaryKey()
        {
            var db = CreateDatabaseWithCompositePrimaryKey();
            var expected = "CREATE TABLE [grant]\r\n(\r\n\tuser_id INT NOT NULL,\r\n\tpermission_id INT NOT NULL,\r\n\tcreated_dt DATETIME,\r\n\r\n\tCONSTRAINT PK_GRANT PRIMARY KEY (user_id,permission_id)\r\n)\r\n\r\n";
            Assert.EndsWith(expected, ExportAndReadScript(db, "test3.sql"));
        }

        [Fact]
        public void ShouldExportDatabaseWithCompositeForeignKey()
        {
            var db = CreateDatabaseWithCompositeForeignKey();
            var expected = "CREATE TABLE [cadastro]\r\n(\r\n\talmope INT NOT NULL,\r\n\tdata DATE NOT NULL,\r\n\r\n\tCONSTRAINT PK_CADASTRO PRIMARY KEY (almope,data)\r\n)\r\n\r\nCREATE TABLE [headcount]\r\n(\r\n\talmope INT NOT NULL,\r\n\tdata DATE NOT NULL,\r\n\tpresente INT,\r\n\r\n\tCONSTRAINT FK_HEADCOUNT_CADASTRO FOREIGN KEY (almope,data) REFERENCES cadastro(almope,data)\r\n)\r\n\r\n";
            Assert.EndsWith(expected, ExportAndReadScript(db, "test4.sql"));
        }

        [Fact]
        public void ShouldExportDatabaseWithThreeTables()
        {
            var db = CreateDatabaseWithOptionalRelationship();
            var expected = "CREATE TABLE [people]\r\n(\r\n\tperson_id INT NOT NULL PRIMARY KEY,\r\n\tperson_name STRING NOT NULL\r\n)\r\n\r\nCREATE TABLE [schedule]\r\n(\r\n\tschedule_id INT NOT NULL PRIMARY KEY,\r\n\tstarttime DATETIME NOT NULL,\r\n\tendtime DATETIME,\r\n\tperson_id INT REFERENCES people(person_id)\r\n)\r\n\r\n";
            Assert.EndsWith(expected, ExportAndReadScript(db, "test5.sql"));
        }

        [Fact]
        public void ShouldAcceptDatabaseGenetorStrategyInjection()
        {
            var fake = A.Fake<IDatabaseGeneratorStrategy>();
            var db = CreateDatabaseWithOneTable();
            A.CallTo(() => fake.LoadDatabase(A<string>.Ignored)).Returns(db);

            var svc = new DatabaseService(databaseGenerator: fake);

            var ret = svc.LoadDatabase(Provider.SQLite, "connectionString");

            A.CallTo(() => fake.LoadDatabase(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ShouldAcceptScriptingGenetorStrategyInjection()
        {
            var fake = A.Fake<IScriptingGeneratorStrategy>();
            var db = CreateDatabaseWithOneTable();
            A.CallTo(() => fake.ExportToFile(A<Database>.Ignored, A<string>.Ignored)).DoesNothing();

            var svc = new DatabaseService(scriptingGenerator: fake);

            svc.Export(db, "test6.sql");

            A.CallTo(() => fake.ExportToFile(A<Database>.Ignored, A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        private string ExportAndReadScript(Database db, string temp_file)
        {
            var svc = new DatabaseService();

            if (File.Exists(temp_file))
            {
                File.Delete(temp_file);
            }

            svc.Export(db, temp_file);

            try
            {
                using (var sr = new StreamReader(temp_file))
                {
                    var txt = sr.ReadToEnd(); // this line to make copyng easier
                    return txt;
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

        private Database CreateDatabaseWithTwoTables()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.NewGuid(),
                ConnectionString = "Data source=file.db",
                Name = "security_users_db",
            };

            var users = new Table
            {
                Id = "1",
                Name = "users",
                Database = db
            };

            var userId = new PrimaryKey
            {
                Id = "1",
                Name = "user_id",
                Type = "int",
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

            users.Add(userId);
            users.Add(username);

            db.Add(users);

            // permissions table
            var permissions = new Table
            {
                Id = "2",
                Name = "permissions",
                Database = db
            };

            var permissionId = new PrimaryKey
            {
                Index = 0,
                Id = "1",
                Name = "permission_id",
                Required = true,
                Type = "int",
            };

            var permission = new Column
            {
                Index = 1,
                Id = "2",
                Name = "permission",
                Required = true,
                Type = "string",
            };

            var userIdFk = new ForeignKey
            {
                Index = 2,
                Id = "3",
                Name = "user_id",
                RelatedTable = "users",
                RelatedColumn = "user_id",
                Required = true,
                Type = "int",
            };

            permissions.Add(permissionId);
            permissions.Add(permission);
            permissions.Add(userIdFk);

            db.Add(permissions);

            return db;
        }

        private Database CreateDatabaseWithOptionalRelationship()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.NewGuid(),
                ConnectionString = "Data source=file.db",
                Name = "security_users_db",
            };

            var people = new Table
            {
                Id = "2",
                Name = "people",
            };
            var schedule = new Table
            {
                Id = "3",
                Name = "schedule"
            };

            people.AddRange(
                new PrimaryKey
                {
                    Index = 0,
                    Id = "1",
                    Name = "person_id",
                    Required = true,
                    Type = "int"
                },
                new Column
                {
                    Index = 1,
                    Id = "2",
                    Name = "person_name",
                    Required = true,
                    Type = "string"
                }
                );

            schedule.AddRange(
                new PrimaryKey
                {
                    Index = 0,
                    Id = "1",
                    Name = "schedule_id",
                    Type = "int",
                    Required = true
                },
                new Column
                {
                    Index = 1,
                    Id = "2",
                    Name = "starttime",
                    Type = "datetime",
                    Required = true
                },
                new Column
                {
                    Index = 2,
                    Id = "3",
                    Name = "endtime",
                    Type = "datetime",
                    Required = false
                },
                new ForeignKey
                {
                    Index = 3,
                    Id = "4",
                    Name = "person_id",
                    RelatedTable = "people",
                    RelatedColumn = "person_id",
                    Required = false,
                    Type = "int",
                }
                );

            db.AddRange(schedule, people);

            return db;
        }

        private Database CreateDatabaseWithCompositePrimaryKey()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.NewGuid(),
                ConnectionString = "Data source=file.db",
                Name = "security_users_db",
            };

            var grant = new Table
            {
                Id = "1",
                Name = "grant",
                Database = db
            };

            var userId = new PrimaryKey
            {
                Index = 0,
                Id = "1",
                Name = "user_id",
                Required = true,
                Type = "int"
            };

            var permissionId = new PrimaryKey
            {
                Index = 1,
                Id = "2",
                Name = "permission_id",
                Required = true,
                Type = "int"
            };

            var createdDate = new Column
            {
                Index = 2,
                Id = "3",
                Name = "created_dt",
                Required = false,
                Type = "datetime"
            };

            grant.Add(userId);
            grant.Add(permissionId);
            grant.Add(createdDate);

            db.Add(grant);

            return db;
        }

        private Database CreateDatabaseWithCompositeForeignKey()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.NewGuid(),
                ConnectionString = "Data source=file.db",
                Name = "security_users_db",
            };

            var cadastro = new Table
            {
                Id = "1",
                Name = "cadastro",
            };

            var almopePk = new PrimaryKey
            {
                Index = 0,
                Id = "1",
                Name = "almope",
                Required = true,
                Type = "int",
            };

            var dataPk = new PrimaryKey
            {
                Index = 1,
                Id = "2",
                Name = "data",
                Required = true,
                Type = "date",
            };

            cadastro.Add(almopePk);
            cadastro.Add(dataPk);

            var headcount = new Table
            {
                Id = "2",
                Name = "headcount",
            };

            var almopeFk = new ForeignKey
            {
                Index = 0,
                Id = "1",
                Name = "almope",
                RelatedColumn = "almope",
                RelatedTable = "cadastro",
                Required = true,
                Type = "int",
            };

            var dataFk = new ForeignKey
            {
                Index = 1,
                Id = "2",
                Name = "data",
                RelatedColumn = "data",
                RelatedTable = "cadastro",
                Required = true,
                Type = "date",
            };

            var presente = new Column
            {
                Index = 2,
                Id = "3",
                Name = "presente",
                Required = false,
                Type = "int",
            };

            headcount.Add(almopeFk);
            headcount.Add(dataFk);
            headcount.Add(presente);

            db.Add(cadastro);
            db.Add(headcount);

            return db;
        }
    }
}

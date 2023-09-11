using FakeItEasy;
using Inverse.Application;
using Inverse.Domain;
using System;
using System.IO;
using Xunit;

namespace Inverse.Tests
{
    public class DatabaseServiceTests
    {
        private readonly string _encodedContent = "Grv5n8ThhgvbafwmLC0o6VCIJuYz0Wc2RuWV18EzizhazAcz4D2V+7HhyvXfbTmluHSoMw4OcsG3BdTyYVVYzduKM8EpYCVUalAu3chTgIMFk6Uy9kEPUzbkYDT2YFhIOfL2b7tmsYyZ7fM3N9oFg3Cc3m4U76lrV3NrUcTocR4/5YH2XfBeqtBpFVCDG5Uu9BwE+wQFTdX5+J5oF93HSyVqtN0vqxYVFfRdK4sb2cV8Qy69EI4p78YU770r1fBWFqzTQtQPhpMZNJqpoJKU6d3MLdxTvzXYMhm+W37JF8f0zrY/oBtKepuQkJZYBL45GDiFwjaJPbYVuphnnyMhwYVzWJ14v161vqpBxOF3lqoeLqaNKKvcFesAmfVpuoThRL6S0sQP1Q0LVHEhyCrJQ7EXaGV3bp6laJnf7u3WqhTbkEIJirMh4mijzl65GTfkWi6l1JzH1M03t4XPsGBocnbBniZb9PmK6c0w1c8v5ead96DdIJlVE38peJhedM/y9B44Sy6iaJEPzIp1X1034Dc3P1fTmwXu5AyJt5NH6FW+Q1NjompGsfejCRoHAhFpcgkkn4GMGf7l6JQlgnFG21oW7FhNg/VhOiwTAIEFWF1ZvgIlWNv4LQPHQ2EY6t1MGE40uFh4qxuL8N7ZL6ZJMg4hAW3CaHecWv6TtH1jyJYwP7Cgnzu2Wa7hkqtLdUA1QMHPtV1E3QqFoBHvONajAQR5CaBnA9iyia00zakYVBmc9GH6la47kgwW/AnqETu8sqpZWfLOAzkeRyxBT48ho1XOPfNu/+jcAyfArrsa94DJ+VCOBeOt3+o9hGQtwBoqPEJ4eOFOoiLuWcz3rP2O0r1xXkCcJzSm8bhJf4y/kqtfBquFPrLgmdSwCf7V85ZJY7xH9EGe1BFpLCAw51v7jkfb2cLXks+IvAxXVHD3PvdsOpHOOA8N4Fs2xYZiEINhROHsUlXkXx/HAd9Okhc84jQ0ejPWvv/yO8zuLIjY05Cv3/kueBpGbpY04wA9xXW9cZPRqXhDpXPLjFX4tJakX+kuPc2X7tiGu47qX9fpt0LSg+OJjLJTn5qAb7ohR7x8cByIA54QdyncnR9Xio/XZaSKaL7M1k62BkcUzqCL39BjX+pOmXGuyvcZJit402EjrJ05Z2le7EmW/352fbMZ4UySbXXBTKXmPUsQUGAuD+Wg/nY+6soPFg9zM+jmHlUdVXKyoNyrHZXolDedTVIYUwTKM5VQhadenh2Cg2cgzk2o3r/2acz7sg40JUvclFaX+GcS27vTS/FToZKu5J4yTFMPWnxTb8625kM3FZMD5F/Ye5PH1mnppdeOXEm4PcFkL95ifyl+Aq2WU6GGEWJs5pvNysvHh+TINcgvj3AubxQ=";

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

            A.CallTo(() => fake.Provider).Returns(Provider.SQLite);
            A.CallTo(() => fake.LoadDatabase(A<string>.Ignored)).Returns(db);

            var svc = new DatabaseService().With(fake);

            svc.LoadDatabase(Provider.SQLite, "connectionString");

            A.CallTo(() => fake.LoadDatabase(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ShouldAcceptScriptingGenetorStrategyInjection()
        {
            var fake = A.Fake<IScriptingGeneratorStrategy>();
            var db = CreateDatabaseWithOneTable();

            A.CallTo(() => fake.Extension).Returns(".sql");
            A.CallTo(() => fake.ExportToFile(A<Database>.Ignored, A<string>.Ignored)).DoesNothing();

            var svc = new DatabaseService().With(fake);

            svc.Export(db, "test6.sql");

            A.CallTo(() => fake.ExportToFile(A<Database>.Ignored, A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ShouldUseStrategyToSaveFile()
        {
            var fake = A.Fake<IFileManagerStrategy>();
            var db = CreateDatabaseWithOneTable();

            A.CallTo(() => fake.Extension).Returns(".idb");
            A.CallTo(() => fake.SaveFile(A<Database>.Ignored, A<string>.Ignored)).DoesNothing();

            var svc = new DatabaseService().With(fake);

            svc.SaveFile(db, "test1.idb");

            A.CallTo(() => fake.SaveFile(A<Database>.Ignored, A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ShouldUseStrategyToOpenFile()
        {
            var fake = A.Fake<IFileManagerStrategy>();
            var db = CreateDatabaseWithOneTable();

            A.CallTo(() => fake.Extension).Returns(".idb");
            A.CallTo(() => fake.OpenFile(A<string>.Ignored)).Returns(db);

            var svc = new DatabaseService()
                .With(fake);

            svc.OpenFile("test1.idb");

            A.CallTo(() => fake.OpenFile(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ShouldSaveToFile()
        {
            var fileName = "test3.idb";
            var db = CreateDatabaseWithTwoTables();
            var svc = new DatabaseService();

            svc.InstallPlugins();

            svc.SaveFile(db, fileName);

            var expected = _encodedContent;
            var actual = ReadContent(fileName, true);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldOpenFile()
        {
            var fileName = "test4.idb";
            var expected = CreateDatabaseWithTwoTables();
            var actual = new Database(Provider.MSSQLServer);
            var svc = new DatabaseService();

            svc.InstallPlugins();

            try
            {
                File.WriteAllText(fileName, _encodedContent);

                actual = svc.OpenFile(fileName);
            }
            finally
            {
                if (File.Exists(fileName)) File.Delete(fileName);
            }

            Assert.Equal(expected, actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Tables, actual.Tables);
            Assert.Equal(expected.ConnectionString, actual.ConnectionString);
            Assert.Equal(expected.Provider, actual.Provider);
        }

        private static string ExportAndReadScript(Database db, string temp_file)
        {
            var svc = new DatabaseService()
                .With(new Inverse.Plugin.ScriptGenerator.MsSqlServer.SqlServerScriptingGeneratorStrategy());

            if (File.Exists(temp_file))
            {
                File.Delete(temp_file);
            }

            svc.Export(db, temp_file);

            return ReadContent(temp_file);
        }

        private static string ReadContent(string fileName, bool deleteAfterReaded = true)
        {
            try
            {
                using var sr = new StreamReader(fileName);
                return sr.ReadToEnd();
            }
            finally
            {
                if (deleteAfterReaded) File.Delete(fileName);
            }
        }

        private static Database CreateDatabaseWithOneTable()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
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

        private static Database CreateDatabaseWithTwoTables()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
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

        private static Database CreateDatabaseWithOptionalRelationship()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
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

        private static Database CreateDatabaseWithCompositePrimaryKey()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
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

        private static Database CreateDatabaseWithCompositeForeignKey()
        {
            var db = new Database(Provider.SQLite)
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
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

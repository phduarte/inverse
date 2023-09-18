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
        private readonly string _encodedContent = "Grv5n8ThhgvbafwmLC0o6YFhCfgk7wU4zcAHs9BKxmRzJmnVJmqTQifkNEHsrkTL8cZsatQSTg8Vzlp1vnZTcOYqQZqSOX0QTiXR4I/oGJ+8ypMle00VH0c6XvE86ir4CgI5URttdTv4UYU1mtTn1MqF4iChy4fKPe33/R0vjmAK6BD3XAnWvWxsZ/Ym27vNC7w9f318RvimJOnIAuc0fnvzegdz7XPdWLb+YjyV1d3/mP7D23FmUteHW0W6x/dJ4QaSeWClsd+VhAxaPgIke7+ccqrZWIdsrwn1ylGEY6mJMrAXZhdCcryLl7eHvIE5n7Am4fQeYvVePAE9MafEpU9KyIFQI4XKvzYn+he2JSa6RgHeRYsy+XYq8dboRAuFVqt1Zl8w6gxnkh1CwBxmWmqGtlfS0xDQ88pMZyq+r9NJal/LxJN7UOpa4+0g57wObq1TM/yORDOhbar4Cm2PHir1Vl0dMsPFK3fo2zulT5FCqSxNf3B4ckERQWDzsYmgxGuiHbp+E1Ec8diaIWlNursQ2McvhhJWEfUqjJGWdWmQxavfFOtSpenl5a4H509DEDk/Sh3u69QZrPxKb9U6iDdwAMqtcsBD4j6TwSit2L1/XW8ZUp3OC+MtnkLICjh7+5Sp1C/VQNnsQQFMwtzFCwRBG4mCZITjcxt6hsRhyn2DnRGy2UhMjzvhmAt+XSw/Sh+tNV/AuKPR/Sz3pO2yUVRHTyy0yE4SH78CuAOYOMJmY7xc5N/uj/XAxXoKtxqSDcRhN5JeNBz/0xU7VRiSdDZwsxamSfK2xZrIdAygggVrIN8vr7xeuJvnUnKMcFFSHl6P2nqzEzZfHet8hrThkUOIyLHcCG7ryZKFUN7JNQrK1oS1k4scGeNJYGxSh5www0z+BW37T7v5LHIYPR35aC2s9CX/xxQeKo0vFdBxtsh/ZnpaNaA5n7Mea+porpDdXe0ae5pe3ScqTrqmH/LI67RMc+ut420/M90XZNIGQd9tU+SsXI5xoAqG4jUEWzrSSnmptXXFz7r2QfSR5XuucBJ4sTs2n+jOxZcB0GPMhgnc3s6YkT5a8g+ae7JRsIwIWkR5bqARo9b/HiiKVgVKuBjFtEZMNVrwbHjBpC4boWgYUs8FPm87E+SDY497/YGCrzFKwH8uf9bo+JYTWUmSX5xuAstc/XGGFmAAM8B8dq52o942UE+NPrKI9rKimRV9EtQtdd9SHr18Tl2IZybK/uDIbnBigBTbiIPTX7KPAM4kH+V9x79cXRHkI3zEYSVJt7RBtR0Kx45RgIIh1gFHvDHumNdQA1KmowzNplijZIIjn5bgRhkCEIR5v66kqJH5mK581Cs4uORJ1wUjJl5vOSBjlfgBF2uoNtbRWy3s7d6cXEHBHg7Zawf8t7whKNVr1MewK3hGcC1u2i5WTdmLQR3xpdqdFyRJEKyTwoS5lBSG7GI5F4OLhhJBPl8nOFt9403NG6N4xkaOIoQXrznsHYW0K17LBFFT4iGSxTGqh34gSkn9yqVxaYhV3/FJ7ASvSyBGSkdAJr+qCwN0LpakSrBRZizTLchTEjuhMpqbSGCh1liWI38mLfXcjdHgAdjqEGKQ9XF5dU4nUeS98Tpgfw==";

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
            var actual = new Database { Provider = expected.Provider };
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
            var db = new Database
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
                Name = "security_users_db",
                Provider= Provider.SQLite,
                ConnectionString = "Data source=file.db",
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
                IsRequired = true
            };

            var username = new Column
            {
                Id = "2",
                Name = "username",
                Type = "string",
                Index = 1,
                IsRequired = true
            };

            table.Add(userId);
            table.Add(username);

            db.Add(table);

            return db;
        }

        private static Database CreateDatabaseWithTwoTables()
        {
            var db = new Database
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
                Name = "security_users_db",
                Provider = Provider.SQLite,
                ConnectionString = "Data source=file.db",
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
                IsRequired = true
            };

            var username = new Column
            {
                Id = "2",
                Name = "username",
                Type = "string",
                Index = 1,
                IsRequired = true
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
                IsRequired = true,
                Type = "int",
            };

            var permission = new Column
            {
                Index = 1,
                Id = "2",
                Name = "permission",
                IsRequired = true,
                Type = "string",
            };

            var userIdFk = new ForeignKey
            {
                Index = 2,
                Id = "3",
                Name = "user_id",
                RelatedTable = "users",
                RelatedColumn = "user_id",
                IsRequired = true,
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
            var db = new Database
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
                Name = "security_users_db",
                Provider= Provider.SQLite,
                ConnectionString = "Data source=file.db",
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
                    IsRequired = true,
                    Type = "int"
                },
                new Column
                {
                    Index = 1,
                    Id = "2",
                    Name = "person_name",
                    IsRequired = true,
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
                    IsRequired = true
                },
                new Column
                {
                    Index = 1,
                    Id = "2",
                    Name = "starttime",
                    Type = "datetime",
                    IsRequired = true
                },
                new Column
                {
                    Index = 2,
                    Id = "3",
                    Name = "endtime",
                    Type = "datetime",
                    IsRequired = false
                },
                new ForeignKey
                {
                    Index = 3,
                    Id = "4",
                    Name = "person_id",
                    RelatedTable = "people",
                    RelatedColumn = "person_id",
                    IsRequired = false,
                    Type = "int",
                }
                );

            db.AddRange(schedule, people);

            return db;
        }

        private static Database CreateDatabaseWithCompositePrimaryKey()
        {
            var db = new Database
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
                Name = "security_users_db",
                Provider= Provider.SQLite,
                ConnectionString = "Data source=file.db",
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
                IsRequired = true,
                Type = "int"
            };

            var permissionId = new PrimaryKey
            {
                Index = 1,
                Id = "2",
                Name = "permission_id",
                IsRequired = true,
                Type = "int"
            };

            var createdDate = new Column
            {
                Index = 2,
                Id = "3",
                Name = "created_dt",
                IsRequired = false,
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
            var db = new Database
            {
                Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
                Name = "security_users_db",
                Provider = Provider.SQLite,
                ConnectionString = "Data source=file.db",
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
                IsRequired = true,
                Type = "int",
            };

            var dataPk = new PrimaryKey
            {
                Index = 1,
                Id = "2",
                Name = "data",
                IsRequired = true,
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
                IsRequired = true,
                Type = "int",
            };

            var dataFk = new ForeignKey
            {
                Index = 1,
                Id = "2",
                Name = "data",
                RelatedColumn = "data",
                RelatedTable = "cadastro",
                IsRequired = true,
                Type = "date",
            };

            var presente = new Column
            {
                Index = 2,
                Id = "3",
                Name = "presente",
                IsRequired = false,
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

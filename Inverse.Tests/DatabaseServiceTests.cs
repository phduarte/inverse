using FakeItEasy;
using Inverse.Application;
using Inverse.Domain;
using System;
using System.IO;
using Xunit;

namespace Inverse.Tests;

public class DatabaseServiceTests
{
    private readonly string _encodedContent = "Grv5n8ThhgvbafwmLC0o6YFhCfgk7wU4zcAHs9BKxmRzJmnVJmqTQifkNEHsrkTL8cZsatQSTg8Vzlp1vnZTcOYqQZqSOX0QTiXR4I/oGJ+8ypMle00VH0c6XvE86ir4CgI5URttdTv4UYU1mtTn1MqF4iChy4fKPe33/R0vjmAK6BD3XAnWvWxsZ/Ym27vNC7w9f318RvimJOnIAuc0fnvzegdz7XPdWLb+YjyV1d3/mP7D23FmUteHW0W6x/dJ4QaSeWClsd+VhAxaPgIke7+ccqrZWIdsrwn1ylGEY6mJMrAXZhdCcryLl7eHvIE5n7Am4fQeYvVePAE9MafEpU9KyIFQI4XKvzYn+he2JSa6RgHeRYsy+XYq8dboRAuFVqt1Zl8w6gxnkh1CwBxmWmqGtlfS0xDQ88pMZyq+r9NJal/LxJN7UOpa4+0g57wObq1TM/yORDOhbar4Cm2PHir1Vl0dMsPFK3fo2zulT5FCqSxNf3B4ckERQWDzsYmg7bMuCrJ1dWKb9CtMowATOwFwnJBUuBmujgCerhv/k3SbGh3odlO7nKLHT7nZHTLNoB77kqsacaWbEShNEeB9pt9gZiNKtXb1cZZoXpGHhF93ZUro58u3V2n2Uz8aeBI5lm9id4A2gzepc0Ub0ofG5XUexfVPfbi7QUhcSoGBVFJclq0v9wQRhrVhcOKgiKBLEFv0S1mhRkcGyGjQDL+ojcTTV/aV1xWTATqIK374LjjY0yexHUNYZVFe4mf2hp1Uv9FodT1rIsKXptIcnPwKFkkxMmTk+yqqHiaAsNgXUiLzRLvdDaj6v7EAYIRNyLMUmcIZLSvz60fhSQ/zNRKveducGCoCg+BdY0qHpCXMkeDbnDKG9PxMPqtItHHay5QQjFVip66Du12K5EaVLCvRj+up8Gxr+W8dqelcxn21qlZXU4mT875F0L/tKh+vfjDt52dHVynXJ1t69uGs583ZoeEylt+0y35GTmdlRsUBuKI8nIrrlwtuuDDwQiAba1Wbjsk5wOt0OpxJMkJ1tLZworC0yuflt/hTASHhxyAB4nh2frXTIs/F6u1UvZ3XGxsxZYkWh2DpZm+nBl/e9+g2VVVSccibo1ckk6utZsUYQimoMUp/y2cO9l2NaPz4F8WVgENgCzYoXCRB5Rdp8yyEB9d9lgsJuXj633MVZcRYY28ppKPjnXFlHC4PMnQmZ3EzaW8cjdEQj0reLRjz1q+PYsKMD747e5JYZA46+9O51W+2Fi0dbRl9I+AtuiZfrT6f/RM6OALVL+MFYoVTctbfi3eaxq+GI+KfqHzHPZKoOed6x4m1DyEMUdowd4QivWg9JpDEbkdbJu9OUcNfae6oVRqDDoRX7oWRFdq7/dAxP7NqUC4zgcxuUcfFCA7LH2JgmzZl3vlzd/8sOC3gE90izyAsmaR821UYIueU6VlCRai9uTKRI0sX03vqXtOtpdYKy3DTlVbjPkhGyxVA4hfT2DoNiagpwdV+DS7ho2reQyL3KxlpiVPYQpxVU4GRuP/OFXMNgWlzWRs22oJVbAIIz/R+5xTDFaNkZvV8RlMzZkBHW3P538BlBcIx3IusUv/V7YeynnDB3II2oGRs+QI1KLW1VYK8bKvK0anEeaBGo0IuYdhhN2hURRA0QNFM8OI167Za3KIp7YjpdXXgWXjKCDZKvrTVG1UNMW7dhkyHWkluMTYlAG5FGDAbhXUWnBX8";

    [Fact]
    public void ShouldExportDatabaseWithOneTableToFile()
    {
        var db = CreateDatabaseWithOneTable();
        var expected = "CREATE DATABASE [security_users_db];\r\n\r\nGO\r\n\r\nUSE [security_users_db];\r\n\r\nCREATE TABLE [users]\r\n(\r\n\t [user_id] STRING NOT NULL PRIMARY KEY,\r\n\t [username] STRING NOT NULL\r\n)\r\n\r\n";
        var actual = ExportAndReadScript(db, "test1.sql");

        Assert.EndsWith(expected, actual);
    }

    [Fact]
    public void ShouldExportDatabaseWithRelations()
    {
        var db = CreateDatabaseWithTwoTables();
        var expected = "CREATE TABLE [users]\r\n(\r\n\t [user_id] INT NOT NULL PRIMARY KEY,\r\n\t [username] STRING NOT NULL\r\n)\r\n\r\nCREATE TABLE [permissions]\r\n(\r\n\t [permission_id] INT NOT NULL PRIMARY KEY,\r\n\t [permission] STRING NOT NULL,\r\n\t [user_id] INT NOT NULL REFERENCES [users](user_id)\r\n)\r\n\r\n";
        var actual = ExportAndReadScript(db, "test2.sql");

        Assert.EndsWith(expected, actual);
    }

    [Fact]
    public void ShouldExportDatabaseWithCompositePrimaryKey()
    {
        var db = CreateDatabaseWithCompositePrimaryKey();
        var expected = "CREATE TABLE [grant]\r\n(\r\n\t [user_id] INT NOT NULL,\r\n\t [permission_id] INT NOT NULL,\r\n\t [created_dt] DATETIME,\r\n\r\n\tCONSTRAINT PK_GRANT PRIMARY KEY (user_id,permission_id)\r\n)\r\n\r\n";
        var actual = ExportAndReadScript(db, "test3.sql");

        Assert.EndsWith(expected, actual);
    }

    [Fact]
    public void ShouldExportDatabaseWithCompositeForeignKey()
    {
        var db = CreateDatabaseWithCompositeForeignKey();
        var expected = "CREATE TABLE [cadastro]\r\n(\r\n\t [almope] INT NOT NULL,\r\n\t [data] DATE NOT NULL,\r\n\r\n\tCONSTRAINT PK_CADASTRO PRIMARY KEY (almope,data)\r\n)\r\n\r\nCREATE TABLE [headcount]\r\n(\r\n\t [almope] INT NOT NULL,\r\n\t [data] DATE NOT NULL,\r\n\t [presente] INT,\r\n\r\n\tCONSTRAINT FK_HEADCOUNT_CADASTRO FOREIGN KEY (almope,data) REFERENCES [cadastro](almope,data)\r\n)\r\n\r\n";
        var actual = ExportAndReadScript(db, "test4.sql");

        Assert.EndsWith(expected, actual);
    }

    [Fact]
    public void ShouldExportDatabaseWithThreeTables()
    {
        var db = CreateDatabaseWithOptionalRelationship();
        var expected = "CREATE TABLE [people]\r\n(\r\n\t [person_id] INT NOT NULL PRIMARY KEY,\r\n\t [person_name] STRING NOT NULL\r\n)\r\n\r\nCREATE TABLE [schedule]\r\n(\r\n\t [schedule_id] INT NOT NULL PRIMARY KEY,\r\n\t [starttime] DATETIME NOT NULL,\r\n\t [endtime] DATETIME,\r\n\t [person_id] INT REFERENCES [people](person_id)\r\n)\r\n\r\n";
        var actual = ExportAndReadScript(db, "test5.sql");

        Assert.EndsWith(expected, actual);
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
            Provider = Provider.SQLite,
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

        table.AddColumn(userId);
        table.AddColumn(username);

        db.AddTable(table);

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

        users.AddColumn(userId);
        users.AddColumn(username);

        db.AddTable(users);

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

        permissions.AddColumn(permissionId);
        permissions.AddColumn(permission);
        permissions.AddColumn(userIdFk);

        db.AddTable(permissions);

        return db;
    }

    private static Database CreateDatabaseWithOptionalRelationship()
    {
        var db = new Database
        {
            Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
            Name = "security_users_db",
            Provider = Provider.SQLite,
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

        people.AddColumns(
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

        schedule.AddColumns(
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

        db.AddTables(schedule, people);

        return db;
    }

    private static Database CreateDatabaseWithCompositePrimaryKey()
    {
        var db = new Database
        {
            Id = Guid.Parse("4E174BFC-70B8-493D-872E-D098512442CB"),
            Name = "security_users_db",
            Provider = Provider.SQLite,
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

        grant.AddColumn(userId);
        grant.AddColumn(permissionId);
        grant.AddColumn(createdDate);

        db.AddTable(grant);

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

        cadastro.AddColumn(almopePk);
        cadastro.AddColumn(dataPk);

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

        headcount.AddColumn(almopeFk);
        headcount.AddColumn(dataFk);
        headcount.AddColumn(presente);

        db.AddTable(cadastro);
        db.AddTable(headcount);

        return db;
    }
}
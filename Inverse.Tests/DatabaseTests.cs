using Inverse.Domain.Models;
using Xunit;

namespace Inverse.Tests
{
    public class DatabaseTests
    {
        private readonly Database db;

        public DatabaseTests()
        {
            db = new Database(Provider.MSSQLServer)
            {
                Name = "test_db",
                ConnectionString = "foobar"
            };
        }

        [Fact]
        public void ShouldDisplayConnectionStringOnToStringMethod()
        {
            Assert.Equal("foobar", db.ToString());
        }

        [Fact]
        public void ShouldAddRangeOfTableParam()
        {
            var table1 = new Table
            {
                Id = "1",
                Name = "table1"
            };

            var table2 = new Table
            {
                Id = "2",
                Name = "table2"
            };

            db.AddRange(table1, table2);

            Assert.Equal(2, db.Tables.Count);
        }

        [Fact]
        public void ShouldStartAsEmptyDatabase()
        {
            Assert.True(db.IsEmpty);
        }

        [Fact]
        public void ShouldFlagAsNotEmptyDatabase()
        {
            var table1 = new Table
            {
                Id = "1",
                Name = "table1"
            };

            var table2 = new Table
            {
                Id = "2",
                Name = "table2"
            };

            db.AddRange(table1, table2);

            Assert.False(db.IsEmpty);
        }

        [Fact]
        public void ShouldFlagAsEmptyDatabase()
        {
            var table1 = new Table
            {
                Id = "1",
                Name = "table1"
            };

            var table2 = new Table
            {
                Id = "2",
                Name = "table2"
            };

            db.AddRange(table1, table2);

            Assert.False(db.IsEmpty);

            db.ConnectionString = null;

            Assert.True(db.IsEmpty);
        }
    }
}

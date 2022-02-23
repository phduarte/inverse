using Inverse.Domain.Models;
using Xunit;

namespace Inverse.Tests
{
    public class ColumnTests
    {
        private readonly Column column = new()
        {
            Name = "NOME_DA_COLUNA",
            Table = new Table
            {
                Name = "NOME_DA_TABELA"
            }
        };

        [Fact]
        public void ShouldRecognizeWhenItsHover()
        {
            Assert.True(column.IsHover(LayoutDefinition.Tables.WIDTH / 2, LayoutDefinition.Columns.HEIGHT / 2));
        }

        [Fact]
        public void ShouldRecognizeWhenItsNotHover()
        {
            Assert.False(column.IsHover(LayoutDefinition.Tables.WIDTH + 1, LayoutDefinition.Columns.HEIGHT / 2));
        }

        [Fact]
        public void ShouldDisplayNameOnToStringMethod()
        {
            Assert.Equal("NOME_DA_COLUNA", column.ToString());
        }

        [Fact]
        public void ShouldRecognizePrimaryKey()
        {
            var pk = new PrimaryKey();
            Assert.True(pk.IsPrimaryKey);
        }

        [Fact]
        public void ShouldRecognizeWhenItsNotPrimaryKey()
        {
            var pk = new Column();
            Assert.False(pk.IsPrimaryKey);
        }

        [Fact]
        public void ShouldRecognizeForeignKey()
        {
            var pk = new ForeignKey();
            Assert.True(pk.IsForeignKey);
        }

        [Fact]
        public void ShouldRecognizeWhenItsNotForeignKey()
        {
            var pk = new Column();
            Assert.False(pk.IsForeignKey);
        }

        [Fact]
        public void ShouldRecognizePrimaryKeyPrefix()
        {
            var pk = new PrimaryKey();
            Assert.Equal(LayoutDefinition.Columns.PRIMARY_KEY_PREFIX, pk.Prefix);
        }

        [Fact]
        public void ShouldRecognizeForeignKeyPrefix()
        {
            var fk = new ForeignKey();
            Assert.Equal(LayoutDefinition.Columns.FOREIGN_KEY_PREFIX, fk.Prefix);
        }
    }
}

using Inverse.Domain;
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
            Assert.True(column.IsHover(Table.WIDTH / 2, Table.HEIGHT / 2));
        }

        [Fact]
        public void ShouldRecognizeWhenItsNotHover()
        {
            Assert.False(column.IsHover(Table.WIDTH + 1, Table.HEIGHT / 2));
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
            Assert.Equal(Column.PRIMARY_KEY_PREFIX, pk.Prefix);
        }

        [Fact]
        public void ShouldRecognizeForeignKeyPrefix()
        {
            var fk = new ForeignKey();
            Assert.Equal(Column.FOREIGN_KEY_PREFIX, fk.Prefix);
        }

        [Fact]
        public void ShouldRecognizeForeignPrimaryKey()
        {
            var fpk = new ForeignPrimaryKey();
            Assert.True(fpk.IsForeignKey);
            Assert.True(fpk.IsPrimaryKey);
        }

        [Fact]
        public void ShouldRecognizeForeignPrimaryKeyPrefix()
        {
            var fpk = new ForeignPrimaryKey();
            Assert.Equal(Column.FOREIGN_PRIMART_KEY_PREFIX, fpk.Prefix);
        }

        [Fact]
        public void ShouldParseForeignPrimaryKeyFromForeignKey()
        {
            var fk = new ForeignKey
            {
                Id = "1",
                Name = "CustomerId",
                Type = "INT",
                Index = 1,
                RelatedTable = "Customers",
                RelatedColumn = "CustomerId",
                IsRequired = true,
                Table = new Table
                {
                    Name = "Sales"
                }
            };

            var fpk = ForeignPrimaryKey.Parse(fk);

            Assert.Equal(fk.Id, fpk.Id);
            Assert.Equal(fk.Name, fpk.Name);
            Assert.Equal(fk.Type, fpk.Type);
            Assert.Equal(fk.Index, fpk.Index);
            Assert.Equal(fk.RelatedTable, fpk.RelatedTable);
            Assert.Equal(fk.RelatedColumn, fpk.RelatedColumn);
            Assert.Equal(fk.IsRequired, fpk.IsRequired);
            Assert.Equal(fk.Table.Name, fpk.Table.Name);
            Assert.Equal(Column.FOREIGN_PRIMART_KEY_PREFIX, fpk.Prefix);
        }
    }
}
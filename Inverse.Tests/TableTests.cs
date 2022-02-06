using Inverse.Domain.Model;
using System;
using Xunit;

namespace Inverse.Tests
{
    public class TableTests
    {
        private Table table;

        public TableTests()
        {
            table = new Table
            {
                Name = "NOME_DA_TABELA"
            };
        }

        [Fact]
        public void ShouldCalculateWidth()
        {
            Assert.Equal(LayoutDefinition.Tables.WIDTH, table.Width);

            table.Add(new Column { Name = "NOME_DA_COLUNA" });
            Assert.NotEqual(LayoutDefinition.Tables.WIDTH, table.Width);
        }

        [Fact]
        public void ShouldCalculateHeight()
        {
            Assert.Equal(LayoutDefinition.Columns.HEIGHT, table.Height);

            table.Add(new Column { Name = "NOME_DA_COLUNA" });

            var expectedHeight = LayoutDefinition.Columns.HEIGHT * (table.Columns.Count + 1);

            Assert.Equal(expectedHeight, table.Height);
        }

        [Fact]
        public void ShouldCalculateCenter()
        {
            Assert.Equal(LayoutDefinition.Tables.WIDTH / 2, table.Center);
        }

        [Fact]
        public void ShouldCalculateMiddle()
        {
            Assert.Equal(LayoutDefinition.Columns.HEIGHT / 2, table.Middle);
        }

        [Fact]
        public void ShouldMoveToLeft()
        {
            Assert.Equal(0, table.Left);
            table.MoveOffset(1, 0);
            Assert.Equal(1, table.Left);
            table.MoveOffset(-1, 0);
            Assert.Equal(0, table.Left);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(100)]
        [InlineData(1000)]
        public void ShouldMoveToRight(int pxToRight)
        {
            Assert.Equal(LayoutDefinition.Tables.WIDTH, table.Right);
            table.MoveOffset(pxToRight, 0);
            Assert.Equal(LayoutDefinition.Tables.WIDTH + pxToRight, table.Right);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(100)]
        [InlineData(1000)]
        public void ShouldMoveDown(int pxToDown)
        {
            Assert.Equal(LayoutDefinition.Columns.HEIGHT, table.Bottom);
            table.MoveOffset(0, pxToDown);
            Assert.Equal(LayoutDefinition.Columns.HEIGHT + pxToDown, table.Bottom);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(100)]
        [InlineData(1000)]
        public void ShouldMoveUp(int pxToUp)
        {
            Assert.Equal(0, table.Top);
            table.MoveOffset(0, pxToUp);
            Assert.Equal(pxToUp, table.Top);
            table.MoveOffset(0, -pxToUp);
            Assert.Equal(0, table.Top);
        }

        [Theory]
        [InlineData(100, 0)]
        [InlineData(0, 100)]
        [InlineData(100, 100)]
        [InlineData(50, 50)]
        public void ShouldMoveToDefinedPosition(int x, int y)
        {
            Assert.Equal(0, table.Left);
            table.MoveTo(x, y);
            Assert.Equal(x, table.Left);
            Assert.Equal(y, table.Top);
        }

        [Fact]
        public void ShouldNotAllowMoveOutOfTheBoard()
        {
            // is starting at 0 left position
            Assert.Equal(0, table.Left);

            // when it tries to move 1px to left out of board
            table.MoveTo(-1, 0);

            // should keep in same place
            Assert.Equal(0, table.Left);

            // even to up
            table.MoveTo(0, -1);

            // nothing happens
            Assert.Equal(0, table.Top);

            // the samething when using offset to left
            table.MoveOffset(-1, 0);
            Assert.Equal(0, table.Left);

            // and the samething when using offset to up
            table.MoveOffset(0, -1);
            Assert.Equal(0, table.Top);
        }

        [Fact]
        public void ShouldRecognizeWhenItsHover()
        {
            Assert.True(table.IsHover(LayoutDefinition.Tables.WIDTH / 2, LayoutDefinition.Columns.HEIGHT / 2));
        }

        [Fact]
        public void ShouldRecognizeWhenItsNotHover()
        {
            Assert.False(table.IsHover(LayoutDefinition.Tables.WIDTH + 1, LayoutDefinition.Columns.HEIGHT / 2));
        }

        [Fact]
        public void ShouldCalculateForeignKeysCount()
        {
            Assert.Equal(0, table.ForeignKeysCount);
            table.Add(new ForeignKey { Name = "FK_NAME" });
            Assert.Equal(1, table.ForeignKeysCount);
        }

        [Fact]
        public void ShouldCalculatePrimaryKeysCount()
        {
            Assert.Equal(0, table.PrimaryKeysCount);
            table.Add(new PrimaryKey { Name = "FK_NAME" });
            Assert.Equal(1, table.PrimaryKeysCount);
        }

        [Fact]
        public void ShouldNotAllowColumnsWithoutName()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                table.Add(new Column { Id = "1" });
            });
        }

        [Fact]
        public void ShouldAddColumns()
        {
            table.Add(new Column { Id = "1", Name = "ID" });
            table.Add(new Column { Id = "2", Name = "NAME" });
            Assert.Equal(2, table.Columns.Count);
        }

        [Fact]
        public void ShouldAddRangeColumns()
        {
            table.AddRange((System.Collections.Generic.IEnumerable<Column>)(new[]
            {
                new Column { Id = "1", Name = "ID" },
                new Column { Id = "2", Name = "NAME" }
            }));

            Assert.Equal(2, table.Columns.Count);
        }

        [Fact]
        public void ShouldAddRangeColumnsWithDifferentTypes()
        {
            table.AddRange((System.Collections.Generic.IEnumerable<Column>)(new[]
            {
                new PrimaryKey { Id = "1", Name = "ID" },
                new Column { Id = "2", Name = "NAME" },
                new ForeignKey { Id = "3", Name = "ADDRESS_ID" }
            }));

            Assert.Equal(3, table.Columns.Count);
            Assert.Equal(1, table.PrimaryKeysCount);
            Assert.Equal(1, table.ForeignKeysCount);
        }

        [Fact]
        public void ShouldNotAllowDuplicatedColumns()
        {
            table.Add(new ForeignKey { Id = "1", Name = "ID" });
            table.Add(new ForeignKey { Id = "1", Name = "ID" });
            Assert.Equal(1, table.Columns.Count);
        }

        [Fact]
        public void ShouldReplaceColumnInfoWhenColumnAlreadyExists()
        {
            table.Add(new ForeignKey { Id = "1", Name = "ID" });
            table.Add(new ForeignKey { Id = "1", Name = "NAME" });
            Assert.Equal("NAME", table.Columns[0].Name);
        }

        [Fact]
        public void ShouldBeHidden()
        {
            Assert.False(table.IsHidden);
            table.Hide();
            Assert.True(table.IsHidden);
        }

        [Fact]
        public void ShouldBeShowed()
        {
            Assert.False(table.IsHidden);
            table.Hide();
            Assert.True(table.IsHidden);
            table.Show();
            Assert.False(table.IsHidden);
        }

        [Fact]
        public void ShouldDisplayNameOnToStringMethod()
        {
            Assert.Equal("NOME_DA_TABELA", table.ToString());
        }
    }
}

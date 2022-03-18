using Inverse.Domain;
using Inverse.Domain.Columns;
using Inverse.Domain.Tables;
using System;
using Xunit;

namespace Inverse.Tests
{
    public class TableTests
    {
        private readonly Table _table;

        public TableTests()
        {
            _table = new Table
            {
                Name = "NOME_DA_TABELA"
            };
        }

        [Fact]
        public void ShouldCalculateWidth()
        {
            Assert.Equal(LayoutDefinition.Tables.WIDTH, _table.Width);

            _table.Add(new Column { Name = "NOME_DA_COLUNA" });
            Assert.NotEqual(LayoutDefinition.Tables.WIDTH, _table.Width);
        }

        [Fact]
        public void ShouldCalculateHeight()
        {
            Assert.Equal(LayoutDefinition.Columns.HEIGHT, _table.Height);

            _table.Add(new Column { Name = "NOME_DA_COLUNA" });

            var expectedHeight = LayoutDefinition.Columns.HEIGHT * (_table.Columns.Count + 1);

            Assert.Equal(expectedHeight, _table.Height);
        }

        [Fact]
        public void ShouldCalculateCenter()
        {
            Assert.Equal(LayoutDefinition.Tables.WIDTH / 2, _table.Center);
        }

        [Fact]
        public void ShouldCalculateMiddle()
        {
            Assert.Equal(LayoutDefinition.Columns.HEIGHT / 2, _table.Middle);
        }

        [Fact]
        public void ShouldMoveToLeft()
        {
            Assert.Equal(0, _table.Left);
            _table.MoveOffset(1, 0);
            Assert.Equal(1, _table.Left);
            _table.MoveOffset(-1, 0);
            Assert.Equal(0, _table.Left);
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
            Assert.Equal(LayoutDefinition.Tables.WIDTH, _table.Right);
            _table.MoveOffset(pxToRight, 0);
            Assert.Equal(LayoutDefinition.Tables.WIDTH + pxToRight, _table.Right);
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
            Assert.Equal(LayoutDefinition.Columns.HEIGHT, _table.Bottom);
            _table.MoveOffset(0, pxToDown);
            Assert.Equal(LayoutDefinition.Columns.HEIGHT + pxToDown, _table.Bottom);
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
            Assert.Equal(0, _table.Top);
            _table.MoveOffset(0, pxToUp);
            Assert.Equal(pxToUp, _table.Top);
            _table.MoveOffset(0, -pxToUp);
            Assert.Equal(0, _table.Top);
        }

        [Theory]
        [InlineData(100, 0)]
        [InlineData(0, 100)]
        [InlineData(100, 100)]
        [InlineData(50, 50)]
        public void ShouldMoveToDefinedPosition(int x, int y)
        {
            Assert.Equal(0, _table.Left);
            _table.MoveTo(x, y);
            Assert.Equal(x, _table.Left);
            Assert.Equal(y, _table.Top);
        }

        [Fact]
        public void ShouldNotAllowMoveOutOfTheBoard()
        {
            // is starting at 0 left position
            Assert.Equal(0, _table.Left);

            // when it tries to move 1px to left out of board
            _table.MoveTo(-1, 0);

            // should keep in same place
            Assert.Equal(0, _table.Left);

            // even to up
            _table.MoveTo(0, -1);

            // nothing happens
            Assert.Equal(0, _table.Top);

            // the samething when using offset to left
            _table.MoveOffset(-1, 0);
            Assert.Equal(0, _table.Left);

            // and the samething when using offset to up
            _table.MoveOffset(0, -1);
            Assert.Equal(0, _table.Top);
        }

        [Fact]
        public void ShouldRecognizeWhenItsHover()
        {
            Assert.True(_table.IsHover(LayoutDefinition.Tables.WIDTH / 2, LayoutDefinition.Columns.HEIGHT / 2));
        }

        [Fact]
        public void ShouldRecognizeWhenItsNotHover()
        {
            Assert.False(_table.IsHover(LayoutDefinition.Tables.WIDTH + 1, LayoutDefinition.Columns.HEIGHT / 2));
        }

        [Fact]
        public void ShouldCalculateForeignKeysCount()
        {
            Assert.Equal(0, _table.ForeignKeysCount);
            _table.Add(new ForeignKey { Name = "FK_NAME" });
            Assert.Equal(1, _table.ForeignKeysCount);
        }

        [Fact]
        public void ShouldCalculatePrimaryKeysCount()
        {
            Assert.Equal(0, _table.PrimaryKeysCount);
            _table.Add(new PrimaryKey { Name = "FK_NAME" });
            Assert.Equal(1, _table.PrimaryKeysCount);
        }

        [Fact]
        public void ShouldNotAllowColumnsWithoutName()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                _table.Add(new Column { Id = "1" });
            });
        }

        [Fact]
        public void ShouldAddColumns()
        {
            _table.Add(new Column { Id = "1", Name = "ID" });
            _table.Add(new Column { Id = "2", Name = "NAME" });
            Assert.Equal(2, _table.Columns.Count);
        }

        [Fact]
        public void ShouldAddRangeColumns()
        {
            _table.AddRange((System.Collections.Generic.IEnumerable<Column>)(new[]
            {
                new Column { Id = "1", Name = "ID" },
                new Column { Id = "2", Name = "NAME" }
            }));

            Assert.Equal(2, _table.Columns.Count);
        }

        [Fact]
        public void ShouldAddRangeColumnsWithDifferentTypes()
        {
            _table.AddRange((System.Collections.Generic.IEnumerable<Column>)(new[]
            {
                new PrimaryKey { Id = "1", Name = "ID" },
                new Column { Id = "2", Name = "NAME" },
                new ForeignKey { Id = "3", Name = "ADDRESS_ID" }
            }));

            Assert.Equal(3, _table.Columns.Count);
            Assert.Equal(1, _table.PrimaryKeysCount);
            Assert.Equal(1, _table.ForeignKeysCount);
        }

        [Fact]
        public void ShouldNotAllowDuplicatedColumns()
        {
            _table.Add(new ForeignKey { Id = "1", Name = "ID" });
            _table.Add(new ForeignKey { Id = "1", Name = "ID" });
            Assert.Equal(1, _table.Columns.Count);
        }

        [Fact]
        public void ShouldReplaceColumnInfoWhenColumnAlreadyExists()
        {
            _table.Add(new ForeignKey { Id = "1", Name = "ID" });
            _table.Add(new ForeignKey { Id = "1", Name = "NAME" });
            Assert.Equal("NAME", _table.Columns[0].Name);
        }

        [Fact]
        public void ShouldBeHidden()
        {
            Assert.False(_table.IsHidden);
            _table.Hide();
            Assert.True(_table.IsHidden);
        }

        [Fact]
        public void ShouldBeShowed()
        {
            Assert.False(_table.IsHidden);
            _table.Hide();
            Assert.True(_table.IsHidden);
            _table.Show();
            Assert.False(_table.IsHidden);
        }

        [Fact]
        public void ShouldDisplayNameOnToStringMethod()
        {
            Assert.Equal("NOME_DA_TABELA", _table.ToString());
        }
    }
}

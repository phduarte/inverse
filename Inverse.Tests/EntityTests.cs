using Inverse.Domain;
using Xunit;

namespace Inverse.Tests
{
    public class EntityTests
    {
        [Fact]
        public void ShouldRecognizeSameEntities()
        {
            var entity1 = new Table { Id = "1" };
            var entity2 = new Table { Id = "1" };

            Assert.True(entity1.Equals(entity2));
        }

        [Fact]
        public void ShouldRecognizeDifferentEntitiesOfSameType()
        {
            var entity1 = new Table { Id = "1" };
            var entity2 = new Table { Id = "2" };

            Assert.False(entity1.Equals(entity2));
        }

        [Fact]
        public void ShouldRecognizeDifferentEntitiesOfDifferentTypes()
        {
            var entity1 = new Table { Id = "1" };
            var entity2 = new Column { Id = "1" };

            Assert.False(entity1.Equals(entity2));
        }
    }
}
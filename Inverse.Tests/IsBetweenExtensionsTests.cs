using Xunit;

namespace Inverse.Tests
{
    public class IsBetweenExtensionsTests
    {
        [Theory(DisplayName = "Comparação de Valores Inteiros")]
        [InlineData(1, 2, 3)]
        [InlineData(10, 20, 30)]
        [InlineData(100, 200, 300)]
        [InlineData(5, 6, 7)]
        public void ShouldCompareInt16Numbers(int a, int b, int c)
        {
            Assert.True(b.IsBetween(a, c));
        }

        [Theory(DisplayName = "Comparação de Valores Inteiros longos")]
        [InlineData(10000000000, 20000000000, 30000000000)]
        [InlineData(1232135465, 1232135475, 1232135488)]
        [InlineData(100, 200, 300)]
        [InlineData(5, 6, 7)]
        public void ShouldCompareInt32Numbers(long a, long b, long c)
        {
            Assert.True(b.IsBetween(a, c));
        }

        [Theory(DisplayName = "Comparação de Valores decimais")]
        [InlineData(1.5, 1.6, 1.7)]
        [InlineData(5, 6, 7)]
        public void ShouldCompareDecimalNumbers(decimal a, decimal b, decimal c)
        {
            Assert.True(b.IsBetween(a, c));
        }

        [Fact]
        public void ShouldCompareLetters()
        {
            var a = "A";
            var b = "B";
            var c = "C";

            Assert.True(b.IsBetween(a, c));
        }

        [Fact]
        public void ShouldCompareTexts()
        {
            string a = "A";
            string b = "AB";
            string c = "ABC";

            Assert.True(b.IsBetween(a, c));
        }
    }
}

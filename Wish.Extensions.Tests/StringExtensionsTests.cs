using NUnit.Framework;

namespace Wish.Extensions.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void TestSurroundSingleQuote()
        {
            Assert.AreEqual("'test'", "test".Surround("'"));
        }

        [Test]
        public void TestSurroundDoubleQuote()
        {
            Assert.AreEqual("\"test\"", "test".Surround("\""));
        }
    }
}

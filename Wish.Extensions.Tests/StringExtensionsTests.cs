using System;
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

        [Test]
        public void TestContainsIgnoreCase()
        {
            Assert.True("test".Contains("T", StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void TestContainsSpecifyingCaseSensitivity()
        {
            Assert.False("test".Contains("T", StringComparison.InvariantCulture));
        }
    }
}
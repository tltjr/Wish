using System;
using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class PartialPathTests
    {
        [Test]
        public void BasePath()
        {
            var partialPath = new PartialPath(@"dir1\subd");
            Assert.AreEqual(@"dir1\", partialPath.Base);
        }

        [Test]
        public void BasePathTwoDeep()
        {
            var partialPath = new PartialPath(@"dir1\subdir1\Tes");
            Assert.AreEqual(@"dir1\subdir1\", partialPath.Base);
        }

        [Test]
        public void BasePathNone()
        {
            var partialPath = new PartialPath(@"di");
            Assert.AreEqual(String.Empty, partialPath.Base);
        }

        [Test]
        public void Pattern()
        {
            var partialPath = new PartialPath(@"di");
            Assert.AreEqual("di*", partialPath.Pattern);
        }

        [Test]
        public void PatternPathDeep()
        {
            var partialPath = new PartialPath(@"dir1\subdir");
            Assert.AreEqual(@"dir1\subdir*", partialPath.Pattern);
        }
    }
}

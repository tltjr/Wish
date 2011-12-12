using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class MatchStrategyTests
    {
        [Test]
        public void RelativePath()
        {
            var strategy = new RelativePathMatchStrategy(@"..\D*");
            var args = strategy.GetArgs(@"C:\Users\tlthorn1.AMR\Documents");
            Assert.AreEqual(@"C:\Users\tlthorn1.AMR", args.Path);
            Assert.AreEqual(@"D*", args.SearchPattern);
        }

        [Test]
        public void RelativePathTwoDeep()
        {
            var strategy = new RelativePathMatchStrategy(@"..\..\D*");
            var args = strategy.GetArgs(@"C:\Users\tlthorn1.AMR\Documents\Snagit");
            Assert.AreEqual(@"C:\Users\tlthorn1.AMR", args.Path);
            Assert.AreEqual(@"D*", args.SearchPattern);
        }

        [Test]
        public void NonExistingWorkingDirectoryReturnsNull()
        {
            var strategy = new RelativePathMatchStrategy(@"..\..\D*");
            var args = strategy.GetArgs(@"C:\doesnotexist");
            Assert.IsNull(args);
        }

        [Test]
        public void DotsOnly()
        {
            var strategy = new RelativePathMatchStrategy(@"..");
            var args = strategy.GetArgs(@"C:\Users\tlthorn1.AMR");
            Assert.IsNull(args);
        }
    }
}
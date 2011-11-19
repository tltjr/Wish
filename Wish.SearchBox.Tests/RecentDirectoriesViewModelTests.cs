using NUnit.Framework;
using Wish.SearchBox.ViewModels;

namespace Wish.SearchBox.Tests
{
    [TestFixture]
    public class RecentDirectoriesViewModelTests
    {
        private RecentDirectoriesViewModel _rdvm;

        [SetUp]
        public void Init()
        {
            _rdvm = new RecentDirectoriesViewModel();
        }

        [Test]
        public void Color()
        {
            Assert.AreEqual("#ffc853", _rdvm.Color);
        }

        [Test]
        public void Text()
        {
            Assert.AreEqual("Search Recent Directories: ", _rdvm.Text);
        }
    }
}

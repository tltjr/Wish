using NUnit.Framework;
using Wish.SearchBox.ViewModels;

namespace Wish.SearchBox.Tests
{
    [TestFixture]
    public class RecentArgumentsViewModelTests
    {
        private RecentArgumentsViewModel _ahvm;

        [SetUp]
        public void Init()
        {
            _ahvm = new RecentArgumentsViewModel();
        }

        [Test]
        public void Color()
        {
            Assert.AreEqual("#51fd62", _ahvm.Color);
        }

        [Test]
        public void Text()
        {
            Assert.AreEqual("Search Argument History: ", _ahvm.Text);
        }
    }
}

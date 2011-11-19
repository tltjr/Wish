using NUnit.Framework;
using Wish.SearchBox.ViewModels;

namespace Wish.SearchBox.Tests
{
    [TestFixture]
    public class CommandHistoryViewModelTests
    {
        private CommandHistoryViewModel _chvm;

        [SetUp]
        public void Init()
        {
            _chvm = new CommandHistoryViewModel();
        }

        [Test]
        public void Color()
        {
            Assert.AreEqual("#7E51FD", _chvm.Color);
        }

        [Test]
        public void Text()
        {
            Assert.AreEqual("Search Command History: ", _chvm.Text);
        }
    }
}

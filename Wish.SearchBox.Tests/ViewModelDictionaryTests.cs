using NUnit.Framework;
using Wish.SearchBox.ViewModels;

namespace Wish.SearchBox.Tests
{
    [TestFixture]
    public class ViewModelDictionaryTests
    {
        private ViewModelDictionary _vmDict;

        [SetUp]
        public void Init()
        {
            _vmDict = new ViewModelDictionary();
        }

        [Test]
        public void CommandHistory()
        {
            var vm = _vmDict[SearchType.CommandHistory];
            Assert.IsInstanceOf<CommandHistoryViewModel>(vm);
        }

        [Test]
        public void RecentDirectories()
        {
            var vm = _vmDict[SearchType.RecentDirectories];
            Assert.IsInstanceOf<RecentDirectoriesViewModel>(vm);
        }

        [Test]
        public void RecentArguments()
        {
            var vm = _vmDict[SearchType.RecentArguments];
            Assert.IsInstanceOf<RecentArgumentsViewModel>(vm);
        }
    }
}

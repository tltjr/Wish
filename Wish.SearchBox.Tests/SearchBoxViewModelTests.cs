using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using NUnit.Framework;
using Wish.SearchBox.ViewModels;

namespace Wish.SearchBox.Tests
{
    [TestFixture]
    public class SearchBoxViewModelTests
    {
        private TestViewModel _vm;

        [SetUp]
        public void Init()
        {
            _vm = new TestViewModel {BaseCollection = new List<string> {"one", "two"}};
        }

        [Test]
        public void QueryTextNullReturnsCorrectCount()
        {
            var list = (List<string>) _vm.QueryCollection;
            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void QueryTextNullReturnsBaseCollectionOne()
        {
            var list = (List<string>) _vm.QueryCollection;
            Assert.True(list.Contains("one"));
        }

        [Test]
        public void QueryTextNullReturnsBaseCollectionTwo()
        {
            var list = (List<string>) _vm.QueryCollection;
            Assert.True(list.Contains("two"));
        }

        [Test]
        public void QueryTextReturnsAllMatches()
        {
            _vm.QueryText = "o";
            var list = _vm.QueryCollection.Cast<string>().ToList();
            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void QueryTextReturnsStartsWith()
        {
            _vm.QueryText = "o";
            var list = _vm.QueryCollection.Cast<string>().ToList();
            Assert.True(list.Contains("one"));
        }

        [Test]
        public void QueryTextReturnsContains()
        {
            _vm.QueryText = "o";
            var list = _vm.QueryCollection.Cast<string>().ToList();
            Assert.True(list.Contains("two"));
        }

        [Test]
        public void QueryTextExcludesNonMatches()
        {
            _vm.QueryText = "n";
            var list = _vm.QueryCollection.Cast<string>().ToList();
            Assert.False(list.Contains("two"));
        }
    }

    public class TestViewModel : SearchBoxViewModel
    {
        public override string Color
        {
            get { throw new NotImplementedException(); }
        }

        public override string Text
        {
            get { throw new NotImplementedException(); }
        }

        public override void HandleEnter(KeyEventArgs e, Action<string> onSelection, string selected)
        {
            throw new NotImplementedException();
        }
    }
}

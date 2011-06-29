using NUnit.Framework;
using Wish.Core;

namespace Wish.Testing
{
    [TestFixture]
    public class CommandHistoryTester
    {
        private CommandHistory _history;

        [SetUp]
        public void Init()
        {
            _history = new CommandHistory();
            _history.Add(new Command("one arg1 arg2", "one", new[] { "arg1", "arg2"}));
            _history.Add(new Command("two arg1 arg2", "two", new[] { "arg1", "arg2"}));
        }

        [Test]
        public void One()
        {
            Assert.AreEqual("one", _history.GetNext().Name);
        }

        [Test]
        public void Two()
        {
            Assert.AreEqual("one", _history.GetNext().Name);
            Assert.AreEqual("two", _history.GetNext().Name);
            Assert.AreEqual("one", _history.GetPrevious().Name);
            Assert.AreEqual("", _history.GetPrevious().Name);
        }

        [Test]
        public void Wrap()
        {
            Assert.AreEqual("one", _history.GetNext().Name);
            Assert.AreEqual("two", _history.GetNext().Name);
            Assert.AreEqual("", _history.GetNext().Name);
            Assert.AreEqual("one", _history.GetNext().Name);
        }

        [Test]
        public void WrapPrevious()
        {
            Assert.AreEqual("one", _history.GetNext().Name);
            Assert.AreEqual("", _history.GetPrevious().Name);
            Assert.AreEqual("two", _history.GetPrevious().Name);
            Assert.AreEqual("one", _history.GetPrevious().Name);
        }

    }
}

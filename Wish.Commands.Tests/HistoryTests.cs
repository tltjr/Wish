using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class HistoryTests
    {
        private History _history;

        [SetUp]
        public void Init()
        {
            _history = new History();
        }

        [Test]
        public void InitiallyEmpty()
        {
            var result = _history.Up();
            Assert.IsNull(result);
        }

        private Command AddLs()
        {
            var command = new Command("ls"); 
            _history.Add(command);
            return command;
        }

        [Test]
        public void Add()
        {
            var command = AddLs();
            var result = _history.Up();
            Assert.AreEqual(command, result);
        }

        [Test]
        public void AddTwo()
        {
            var command = AddLs();
            var command2 = new Command("command 2");
            _history.Add(command2);
            var result = _history.Up();
            Assert.AreEqual(command2, result);
            var result2 = _history.Up();
            Assert.AreEqual(command, result2);
        }

        [Test]
        public void BlankPlaceholder()
        {
            AddLs();
            _history.Up();
            var result = _history.Down();
            Assert.AreEqual(new Command(string.Empty), result);
        }

        [Test]
        public void OneDownSameAsUpIfOneEntry()
        {
            var command = AddLs();
            var result = _history.Down();
            Assert.AreEqual(command, result);
        }

        [Test]
        public void UpWrapping()
        {
            AddLs();
            var expected = new Command("command 2");
            _history.Add(expected);
            _history.Up();
            _history.Up();
            _history.Up();
            var result = _history.Up();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ResetSetsIndexToNegOne()
        {
            _history.Add(new Command(""));
            _history.Reset();
            Assert.AreEqual(-1, _history.Index);
        }

        [Test]
        public void DownHandlesEmptyHistory()
        {
            var result = _history.Down();
            Assert.IsNull(result);
        }

        [Test]
        public void NoDuplicates()
        {
            AddLs();
            _history.Add(new Command("ls"));
            Assert.AreEqual(1, _history.Count);
        }
    }
}

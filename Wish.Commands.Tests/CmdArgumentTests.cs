using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class CmdArgumentTests
    {
        private Argument _argument;

        private IEnumerable<string> GetResult(string text)
        {
            var mock = new Mock<IRunner>();
            mock.Setup(o => o.WorkingDirectory).Returns(Path.Combine(Environment.CurrentDirectory, "Test"));
            _argument = ArgumentFactory.Create(mock.Object, text);
            return _argument.Complete();
        }

        [Test]
        public void Complete()
        {
            var result = GetResult("di");
            Assert.True(result.Contains("dir1"));
        }

        [Test]
        public void CompleteIgnoresCase()
        {
            var result = GetResult("di");
            Assert.True(result.Contains("Dir2"));
        }

        [Test]
        public void CompleteDoesntContainNonMatches()
        {
            var result = GetResult("di");
            Assert.False(result.Contains("sample"));
        }

        [Test]
        public void CompleteCorrectCount()
        {
            var result = GetResult("di");
            Assert.AreEqual(2, result.Count());
        }


        [Test]
        public void CompleteReturnsDirectoriesAndFiles()
        {
            var result = GetResult("sa").ToList();
            Assert.True(result.Contains("sample"));
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void CompleteReturnsFiles()
        {
            var result = GetResult("sa");
            Assert.True(result.Contains("safiletest.txt"));
        }

        [Test]
        public void PathSpacesCompleted()
        {
            var result = GetResult("wi");
            Assert.True(result.Contains("\"with space\""));
        }

        [Test]
        [Ignore]
        public void CompleteSupportsVariousDepths()
        {
            //Directory.GetFiles() has an overload which accepts a SearchOption
            //which could be used to search all directories or possible to a certain depth
        }
    }
}

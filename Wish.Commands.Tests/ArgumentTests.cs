using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class ArgumentTests
    {
        private Argument _argument;

        private IEnumerable<string> GetResult(string text)
        {
            _argument = new TestArgument(text);
            return _argument.Complete();
        }

        [Test]
        public void CompleteWithSlashesReturns2()
        {
            var results = GetResult(@"dir1\subd");
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public void CompleteWithSlashesContainSubdir1()
        {
            var results = GetResult(@"dir1\subd");
            Assert.True(results.Contains(@"dir1\subdir1"));
        }

        [Test]
        public void CompleteWithSlashesSubdir2()
        {
            var results = GetResult(@"dir1\subd");
            Assert.True(results.Contains(@"dir1\subdir2"));
        }

        [Test]
        public void CompleteFullPathReturnsSelf()
        {
            var results = GetResult(@"dir1");
            Assert.AreEqual(@"dir1", results.First());
        }

        [Test]
        [Ignore]
        public void CompleteQuotedArg()
        {
        }


    }

    public class TestArgument : Argument 
    {
        public TestArgument(string text) : base(text) { }

        public override IEnumerable<string> Complete()
        {
            var list = GetDirectories(Path.Combine(Environment.CurrentDirectory, "Test"));
            return QuoteSpaces(list, "'");
        }
    }


}

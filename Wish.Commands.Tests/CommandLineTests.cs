using System.Linq;
using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class CommandLineTests
    {
        [Test]
        public void Function()
        {
            var commandLine = new CommandLine(@"cd somedir");
            var function = commandLine.Function;
            Assert.AreEqual("cd", function);
        }

        [Test]
        public void ArgumentsZero()
        {
            var commandLine = new CommandLine(@"popd");
            var args = commandLine.Arguments;
            Assert.AreEqual(0, args.Count);
        }

        [Test]
        public void ArgumentsOne()
        {
            var commandLine = new CommandLine(@"cd somedir");
            var args = commandLine.Arguments;
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(@"somedir", args[0]);
        }

        [Test]
        public void ArgumentsTwo()
        {
            var commandLine = new CommandLine(@"cd somefile somedir");
            var args = commandLine.Arguments;
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual(@"somefile", args[0]);
            Assert.AreEqual(@"somedir", args[1]);
        }

        [Test]
        public void EmptyArgs()
        {
            var commandLine = new CommandLine(string.Empty);
            Assert.False(commandLine.Arguments.Any());
        }

        [Test]
        public void QuotedArgsNoSpaceAfter()
        {
            var commandLine = new CommandLine(@"cd '.\with space'\blah");
            var args = commandLine.Arguments;
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(@"'.\with space'\blah", args[0]);
        }

        [Test]
        public void QuotedArgsSpaceAfter()
        {
            var commandLine = new CommandLine(@"cd '.\with space' blah");
            var args = commandLine.Arguments;
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual(@"'.\with space'", args[0]);
            Assert.AreEqual(@"blah", args[1]);
        }

        [Test]
        public void MultipleQuotedSegments()
        {
            var commandLine = new CommandLine(@"cd '.\with space' '.\blah again'");
            var args = commandLine.Arguments;
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual(@"'.\with space'", args[0]);
            Assert.AreEqual(@"'.\blah again'", args[1]);
        }

        [Test]
        public void MultipleQuotedSegmentsStuffAfterQuotes()
        {
            var commandLine = new CommandLine(@"cd '.\with space'\uhoh '.\blah again'\canyouhandleit?");
            var args = commandLine.Arguments;
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual(@"'.\with space'\uhoh", args[0]);
            Assert.AreEqual(@"'.\blah again'\canyouhandleit?", args[1]);
        }

        [Test]
        public void MultipleQuotedSegmentsNonQuotesStuffInBetween()
        {
            var commandLine = new CommandLine(@"cd '.\with space'\uhoh ohman tricky '.\blah again'\canyouhandleit?");
            var args = commandLine.Arguments;
            Assert.AreEqual(4, args.Count);
            Assert.AreEqual(@"'.\with space'\uhoh", args[0]);
            Assert.AreEqual(@"ohman", args[1]);
            Assert.AreEqual(@"tricky", args[2]);
            Assert.AreEqual(@"'.\blah again'\canyouhandleit?", args[3]);
        }
    }
}

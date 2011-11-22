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
    }
}

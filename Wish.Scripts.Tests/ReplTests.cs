using System.Linq;
using Moq;
using NUnit.Framework;
using Wish.Commands;

namespace Wish.Scripts.Tests
{
    [TestFixture]
    public class ReplTests
    {
        private Repl _repl;

        [SetUp]
        public void Init()
        {
            _repl = new Repl();
        }

        [Test]
        public void NewReplPrompt()
        {
            Assert.AreEqual("> ", _repl.Prompt.Current);
        }

        [Test]
        public void NewReplPromptIndex()
        {
            Assert.AreEqual(2, _repl.LastPromptIndex);
        }

        [Test]
        public void ReadFunction()
        {
            var command = _repl.Read("> cp ./blah.txt ./temp/blahdir");
            Assert.AreEqual("cp", command.Function.Name);
        }

        [Test]
        public void ReadArgs()
        {
            var command = _repl.Read("> cp ./blah.txt ./temp/blahdir");
            var args = command.Arguments.Select(o => o.PartialPath.Text).ToList();
            Assert.True(args.Contains("./blah.txt"));
            Assert.True(args.Contains("./temp/blahdir"));
        }

        [Test]
        public void EvalLsContainsDirectoryHeading()
        {
            // actual execution - relative to Wish.Scripts.Tests\bin\Debug
            var result = ExecuteLs();
            Assert.True(result.Contains(@"Directory: T:\src\dotnet\"), "Result was: {0}", result);
        }

        [Test]
        public void EvalLsContainsDirectories()
        {
            // actual execution - relative to Wish.Scripts.Tests\bin\Debug
            var result = ExecuteLs();
            Assert.True(result.Contains(@"Test"), "Result was: {0}", result);
        }

        [Test]
        public void EvalLsContainsModeHeader()
        {
            // actual execution - relative to Wish.Scripts.Tests\bin\Debug
            var command = _repl.Read("> ls");
            _repl.Eval(command);
            var result = _repl.Print();
            Assert.True(result.Contains(@"Mode"), "Result was: {0}", result);
        }

        [Test]
        public void EvalLsContainsLastWriteTimeHeader()
        {
            // actual execution - relative to Wish.Scripts.Tests\bin\Debug
            var result = ExecuteLs();
            Assert.True(result.Contains(@"LastWriteTime"), "Result was: {0}", result);
        }

        [Test]
        public void EvalLsContainsArchiveTypes()
        {
            // actual execution - relative to Wish.Scripts.Tests\bin\Debug
            var result = ExecuteLs();
            Assert.True(result.Contains(@"-a--"), "Result was: {0}", result);
        }

        [Test]
        public void EvalLsContainsDirectoryType()
        {
            // actual execution - relative to Wish.Scripts.Tests\bin\Debug
            var result = ExecuteLs();
            Assert.True(result.Contains(@"d---"), "Result was: {0}", result);
        }

        [Test]
        public void EvalLsContainsFiles()
        {
            // actual execution - relative to Wish.Scripts.Tests\bin\Debug
            var result = ExecuteLs();
            Assert.True(result.Contains(@"Wish.Scripts.Tests.dll"), "Result was: {0}", result);
        }

        [Test]
        public void LastPromptIndexAfterLs()
        {
            // super fragile, will break with any change to file structure
            ExecuteLs();
            //Assert.AreEqual(1566, _repl.LastPromptIndex);
        }

        [Test]
        public void SecondRead()
        {
            // super fragile, will break with any change to file structure
            var text = ExecuteLs();
            text += "ls";
            var comm = _repl.Read(text);
            Assert.AreEqual("ls", comm.Function.Name);
        }

        [Test]
        public void SecondReadWithArguments()
        {
            // super fragile, will break with any change to file structure
            var text = ExecuteLs();
            text += @"cp .\blah.txt .\targetdir";
            var comm = _repl.Read(text);
            Assert.AreEqual("cp", comm.Function.Name);
            var args = comm.Arguments.Select(o => o.PartialPath.Text).ToList();
            Assert.True(args.Contains(@".\blah.txt"));
            Assert.True(args.Contains(@".\targetdir"));
        }

        [Test]
        public void LoopNonExit()
        {
            var mock = new Mock<IRunner>();
            mock.Setup(o => o.Execute("> ls")).Returns("some ls output");
            var result = _repl.Loop("> ls");
            Assert.False(result.IsExit);
        }

        [Test]
        public void LoopNonExitHandled()
        {
            var mock = new Mock<IRunner>();
            mock.Setup(o => o.Execute("> ls")).Returns("some ls output");
            var result = _repl.Loop("> ls");
            Assert.True(result.Handled);
        }

        [Test]
        public void LoopExitHandled()
        {
            var result = _repl.Loop("> exit");
            Assert.True(result.Handled);
        }

        [Test]
        public void LoopExit()
        {
            var result = _repl.Loop("> exit");
            Assert.True(result.IsExit);
        }

        private string ExecuteLs()
        {
            var command = _repl.Read("> ls");
            _repl.Eval(command);
            var text = _repl.Print();
            return text;
        }
    }
}

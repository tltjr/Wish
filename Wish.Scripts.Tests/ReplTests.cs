using System;
using System.IO;
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

        private void StartAndOverrideDefaultPrompt()
        {
            _repl.Start();
            _repl.Prompt = new Prompt { Current = "> " };
            _repl.Eval(new Command("cd " + Directory.GetCurrentDirectory()));
        }

        [Test]
        public void StartReplPromptIndex()
        {
            StartAndOverrideDefaultPrompt();
            Assert.AreEqual(2, _repl.LastPromptIndex);
        }

        [Test]
        public void ReadFunction()
        {
            StartAndOverrideDefaultPrompt();
            var command = _repl.Read("> cp ./blah.txt ./temp/blahdir");
            Assert.AreEqual("cp", command.Function.Name);
        }

        [Test]
        public void ReadArgs()
        {
            StartAndOverrideDefaultPrompt();
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
            StartAndOverrideDefaultPrompt();
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
            mock.Setup(o => o.Execute("ls")).Returns("some ls output");
            StartAndOverrideDefaultPrompt();
            var result = _repl.Loop(mock.Object, "> ls");
            Assert.False(result.IsExit);
        }

        [Test]
        public void LoopNonExitNotHandled()
        {
            var mock = new Mock<IRunner>();
            mock.Setup(o => o.Execute("ls")).Returns("some ls output");
            StartAndOverrideDefaultPrompt();
            var result = _repl.Loop(mock.Object, "> ls");
            Assert.False(result.FullyProcessed);
        }

        [Test]
        public void LoopExitHandled()
        {
            StartAndOverrideDefaultPrompt();
            var result = _repl.Loop("> exit");
            Assert.True(result.FullyProcessed);
        }

        [Test]
        public void LoopExit()
        {
            StartAndOverrideDefaultPrompt();
            var result = _repl.Loop("> exit");
            Assert.True(result.IsExit);
        }

        [Test]
        public void WorkingDirectoryChangesReturnedInResult()
        {
            var mock = new Mock<IRunner>();
            mock.Setup(o => o.Execute("cd ..")).Returns("test");
            mock.Setup(o => o.WorkingDirectory).Returns("testdir");
            StartAndOverrideDefaultPrompt();
            var result = _repl.Loop(mock.Object, "> cd ..");
            Assert.AreEqual("testdir", result.WorkingDirectory);
        }

        [Test]
        public void PromptDefaultsToHomeDirectory()
        {
            _repl.Start();
            var expected = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + " >> ";
            Assert.AreEqual(expected, _repl.Text);
        }

        [Test]
        public void DirectoryChangesReflectedInPrompt()
        {
            StartAndOverrideDefaultPrompt();
            _repl.Loop("> cd ..");
            Assert.AreEqual(@"T:\src\dotnet\wish-all\Wish\Wish.Scripts.Tests\bin >> ", _repl.Prompt.Current);
        }

        private const string BaseText = @"C:\Users\tlthorn1.AMR >> ";
        private const string UpdatedText = @"C:\Users\tlthorn1.AMR >> "  + "ls";

        [Test]
        public void HistoryRequest()
        {
            _repl.Start();
            Assert.AreEqual(BaseText, _repl.Text);
            _repl.Eval(new Command("ls"));
            var result = _repl.Up(BaseText);
            Assert.AreEqual(UpdatedText, result.Text);
            Assert.AreEqual(UpdatedText, _repl.Text);
        }

        [Test]
        public void HistoryRequestBlank()
        {
            _repl.Start();
            Assert.AreEqual(BaseText, _repl.Text);
            _repl.Eval(new Command("ls"));
            var result = _repl.Up(BaseText);
            Assert.AreEqual(UpdatedText, result.Text);
            Assert.AreEqual(UpdatedText, _repl.Text);
            var result2 = _repl.Up(UpdatedText);
            Assert.AreEqual(BaseText, result2.Text);
            Assert.AreEqual(BaseText, _repl.Text);
        }

        [Test]
        public void HistoryRequestWithExistingTypingReplacesTyping()
        {
            _repl.Start();
            Assert.AreEqual(BaseText, _repl.Text);
            _repl.Eval(new Command("ls"));
            var result = _repl.Up(BaseText + "some gibberish a user typed in");
            Assert.AreEqual(UpdatedText, result.Text);
            Assert.AreEqual(UpdatedText, _repl.Text);
        }

        [Test]
        public void Down()
        {
            _repl.Start();
            Assert.AreEqual(BaseText, _repl.Text);
            _repl.Eval(new Command("ls"));
            var result = _repl.Down(UpdatedText);
            Assert.AreEqual(UpdatedText, result.Text);
            Assert.AreEqual(UpdatedText, _repl.Text);
        }

        private string ExecuteLs()
        {
            StartAndOverrideDefaultPrompt();
            var command = _repl.Read("> ls");
            _repl.Eval(command);
            var text = _repl.Print();
            return text;
        }
    }
}

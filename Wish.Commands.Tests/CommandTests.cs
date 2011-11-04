using System.Collections;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private Command _command;

        [SetUp]
        public void Init()
        {
            _command = new Command(@"cd somedir");
        }

        [Test]
        public void FunctionName()
        {
            Assert.AreEqual("cd", _command.Function.Name);
        }

        [Test]
        public void ArgumentText()
        {
            Assert.AreEqual("somedir", _command.Arguments.First().PartialPath.Text);
        }

        [Test]
        public void CompleteNoArgumentReturnsFalse()
        {
            _command = new Command(@"func");
            Assert.IsEmpty((ICollection) _command.Complete());
        }

        private void CreateCommandWithRunner()
        {
            const string command = @"cd somedir";
            var mock = new Mock<IRunner>();
            mock.Setup(o => o.Execute(command)).Returns("testing");
            _command = new Command(mock.Object, command);
        }

        [Test]
        public void ConstructorWithRunnerFunctionName()
        {
            CreateCommandWithRunner();
            Assert.AreEqual("cd", _command.Function.Name);
        }

        [Test]
        public void ConstructorWithRunnerArgumentText()
        {
            CreateCommandWithRunner();
            Assert.AreEqual("somedir", _command.Arguments.First().PartialPath.Text);
        }

        [Test]
        public void ConstructorWithRunner()
        {
            CreateCommandWithRunner();
            Assert.AreEqual("testing", _command.Execute());
        }

        [Test]
        [Ignore]
        public void SetAStandardForUnityContainerAndMeetItOrRemoveIt()
        {
        }

        [Test]
        [Ignore]
        public void WhatHappenWhenIRunnerIsNotSpecifiedShouldNotBeAnIssue()
        {
        }
    }
}

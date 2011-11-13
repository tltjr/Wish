﻿using System;
using System.Windows.Input;
using Moq;
using NUnit.Framework;
using Wish.Commands;
using Wish.Common;
using Wish.Scripts;

namespace Wish.Module.Tests
{
    [TestFixture]
    public class WishModelTests
    {
        private WishModel _wishModel;
        private static TestRunner _testRunner;

        [SetUp]
        public void Init()
        {
            var mock = CreateMockRepl("stub", true, false, string.Empty);
            _testRunner = new TestRunner();
            _wishModel = new WishModel(mock.Object, _testRunner);
        }

        private static Mock<IRepl> CreateMockRepl(string input, bool handled, bool isExit, string text)
        {
            var mock = new Mock<IRepl>();
            mock.Setup(o => o.Start()).Returns(new CommandResult { Text = "test" });
            mock.Setup(o => o.Loop(_testRunner, input))
                .Returns(new CommandResult { Handled = handled, IsExit = isExit, Text = text });
            return mock;
        }

        [Test]
        public void RaiseKeyPressUnregisteredKeyNotHandled()
        {
            var result = _wishModel.Raise(Key.A, "stub");
            Assert.False(result.Handled);
        }

        [Test]
        public void RaiseKeyPressUnregisteredNotExit()
        {
            var result = _wishModel.Raise(Key.A, "stub");
            Assert.False(result.IsExit);
        }

        [Test]
        public void RaiseEnterOnExitReturnsIsExitTrue()
        {
            var mock = CreateMockRepl(@"> exit", true, true, string.Empty);
            _wishModel = new WishModel(mock.Object, _testRunner);
            var result = _wishModel.Raise(Key.Enter, @"> exit");
            Assert.True(result.IsExit);
        }

        [Test]
        public void RaiseEnterOnExitReturnsHandledTrue()
        {
            var mock = CreateMockRepl(@"> exit", true, true, string.Empty);
            _wishModel = new WishModel(mock.Object, _testRunner);
            var result = _wishModel.Raise(Key.Enter, @"> exit");
            Assert.True(result.Handled);
        }

        [Test]
        public void RaiseEnterOnExitReturnsTextEmpty()
        {
            var mock = CreateMockRepl(@"> exit", true, true, string.Empty);
            _wishModel = new WishModel(mock.Object, _testRunner);
            var result = _wishModel.Raise(Key.Enter, @"> exit");
            Assert.AreEqual(result.Text, string.Empty);
        }

        [Test]
        public void RaiseEnterOnExitReturnsNullError()
        {
            var mock = CreateMockRepl(@"> exit", true, true, string.Empty);
            _wishModel = new WishModel(mock.Object, _testRunner);
            var result = _wishModel.Raise(Key.Enter, @"> exit");
            Assert.IsNull(result.Error);
        }

        [Test]
        public void RaiseUp()
        {
            var mock = CreateMockRepl(@"blah", true, true, string.Empty);
            _wishModel = new WishModel(mock.Object, _testRunner);
            _wishModel.Raise(Key.Up, "doesnt matter");
            mock.Verify(o => o.Up("doesnt matter"));
        }

        [Test]
        public void RaiseDown()
        {
            var mock = CreateMockRepl(@"blah", true, true, string.Empty);
            _wishModel = new WishModel(mock.Object, _testRunner);
            _wishModel.Raise(Key.Down, "doesnt matter");
            mock.Verify(o => o.Down("doesnt matter"));
        }
    }

    public class TestRunner : IRunner
    {
        public string Execute(string line)
        {
            throw new NotImplementedException();
        }

        public string WorkingDirectory
        {
            get { throw new NotImplementedException(); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class FunctionTests
    {
        private IEnumerable<string> _result;

        [SetUp]
        public void Init()
        {
            var abcdDir = Path.Combine(Environment.CurrentDirectory, "Test");
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + abcdDir, EnvironmentVariableTarget.Process);
            var function = new Function("abc");
            _result = function.Complete();
        }

        [Test]
        public void CompleteContainsExe()
        {
            Assert.True(_result.Contains("abcd.exe"));
        }

        [Test]
        public void CompleteContainsCmd()
        {
            Assert.True(_result.Contains("abcd.cmd"));
        }

        [Test]
        public void CompleteContainsCom()
        {
            Assert.True(_result.Contains("abcd.com"));
        }

        [Test]
        public void CompleteContainsJs()
        {
            Assert.True(_result.Contains("abcd.js"));
        }

        [Test]
        public void CompleteContainsPs1()
        {
            Assert.True(_result.Contains("abcd.ps1"));
        }

        [Test]
        public void CompleteDoesNotContainDll()
        {
            Assert.False(_result.Contains("abcd.dll"));
        }

        [Test]
        public void CompleteDoesNotContainMsi()
        {
            Assert.False(_result.Contains("abcd.msi"));
        }

        [Test]
        public void CompleteDoesNotContainSys()
        {
            Assert.False(_result.Contains("abcd.sys"));
        }

        [Test]
        public void CompleteDoesNotContainTxt()
        {
            Assert.False(_result.Contains("abcd.txt"));
        }

        [Test]
        public void NoPathVariable()
        {
            Environment.SetEnvironmentVariable("PATH", String.Empty, EnvironmentVariableTarget.Process);
            var function = new Function("abc");
            var result = function.Complete();
            Assert.False(result.Any());
        }

        [Test]
        public void EmptyDirectoryInPath()
        {
            //var abcdDir = Path.Combine(Environment.CurrentDirectory, "Test");
            Environment.SetEnvironmentVariable("PATH", ";;", EnvironmentVariableTarget.Process);
            var function = new Function("abc");
            var result = function.Complete();
            Assert.False(result.Any());
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Wish.Extensions.Tests
{
    [TestFixture]
    public class FileSystemInfoExtensionsTests
    {
        private static IEnumerable<string> GetExes()
        {
            var dirInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Test"));
            return dirInfo.Executables();
        }

        [Test]
        public void ExecutablesContainsExe()
        {
            var exes = GetExes();
            Assert.True(exes.Contains("abcd.exe"));
        }

        [Test]
        public void ExecutablesContainsBats()
        {
            var exes = GetExes();
            Assert.True(exes.Contains("abcd.bat"));
        }

        [Test]
        public void ExecutablesContainsCmd()
        {
            var exes = GetExes();
            Assert.True(exes.Contains("abcd.cmd"));
        }

        [Test]
        public void ExecutablesContainsCom()
        {
            var exes = GetExes();
            Assert.True(exes.Contains("abcd.com"));
        }

        [Test]
        public void ExecutablesContainsJs()
        {
            var exes = GetExes();
            Assert.True(exes.Contains("abcd.js"));
        }

        [Test]
        public void ExecutablesContainsPs1()
        {
            var exes = GetExes();
            Assert.True(exes.Contains("abcd.ps1"));
        }

        [Test]
        public void ExecutablesDoesNotContainDll()
        {
            var exes = GetExes();
            Assert.False(exes.Contains("abcd.dll"));
        }

        [Test]
        public void ExecutablesDoesNotContainMsi()
        {
            var exes = GetExes();
            Assert.False(exes.Contains("abcd.msi"));
        }

        [Test]
        public void ExecutablesDoesNotContainSys()
        {
            var exes = GetExes();
            Assert.False(exes.Contains("abcd.sys"));
        }
        
        [Test]
        public void ExecutablesDoesNotContainTxt()
        {
            var exes = GetExes();
            Assert.False(exes.Contains("abcd.txt"));
        }

        [Test]
        public void IsExecutableExeIsTrue()
        {
            Check("abcd.exe", true);  
        }

        [Test]
        public void IsExecutableBatIsTrue()
        {
            Check("abcd.bat", true);
        }

        [Test]
        public void IsExecutableCmdIsTrue()
        {
            Check("abcd.cmd", true);
        }

        [Test]
        public void IsExecutableComIsTrue()
        {
            Check("abcd.com", true);
        }

        [Test]
        public void IsExecutableJsIsTrue()
        {
            Check("abcd.bat", true);
        }

        [Test]
        public void IsExecutablePs1IsTrue()
        {
            Check("abcd.ps1", true);
        }

        [Test]
        public void IsExecutableDllIsFalse()
        {
            Check("abcd.dll", false);
        }

        [Test]
        public void IsExecutableMsiIsFalse()
        {
            Check("abcd.msi", false);
        }

        [Test]
        public void IsExecutableSysIsFalse()
        {
            Check("abcd.sys", false);
        }

        [Test]
        public void IsExecutableTxtIsFalse()
        {
            Check("abcd.txt", false);
        }

        private static void Check(string file, bool b)
        {
            var fileInfo = new FileInfo(file);
            Assert.AreEqual(b, fileInfo.IsExecutable());
        }
    }
}

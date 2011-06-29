using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Wish.Core;

namespace Wish.Testing
{
    [TestFixture]
    public class CompletionHelperTester
    {
        private readonly CompletionHelper _completionHelper = new CompletionHelper();

        [Test]
        public void SearchStringBasic()
        {
            var result = _completionHelper.GetSearchString("a", "b");
            Assert.AreEqual(@"b\a", result);
        }

        [Test]
        public void SearchStringPrompt()
        {
            var result = _completionHelper.GetSearchString("a", @"C:\");
            Assert.AreEqual(@"C:\a", result);
        }

        [Test]
        public void SearchStringPromptOther()
        {
            var result = _completionHelper.GetSearchString("a", @"Z:\");
            Assert.AreEqual(@"Z:\a", result);
        }
        
        [Test]
        public void GetDirectoriesBasic()
        {
            var result = _completionHelper.GetDirectories("", @"C:\temp");
            Assert.IsTrue(result.Contains(@"C:\temp\Wish"));
            Assert.IsTrue(result.Contains(@"C:\temp\toolbox"));
        }

        [Test]
        public void GetDirectoriesPrompt()
        {
            var result = _completionHelper.GetDirectories("", @"C:\");
            Assert.IsTrue(result.Contains(@"C:\Users"));
            Assert.IsTrue(result.Contains(@"C:\ubuntu"));
        }

        [Test]
        public void GetDirectoriesPromptWithArg()
        {
            var result = _completionHelper.GetDirectories(@"temp\", @"C:\");
            Assert.IsTrue(result.Contains(@"C:\temp\Wish"));
            Assert.IsTrue(result.Contains(@"C:\temp\toolbox"));
        }

        [Test]
        public void GetDirectoriesWithArg()
        {
            var result = _completionHelper.GetDirectories(@"Wish\", @"C:\temp");
            Assert.IsTrue(result.Contains(@"C:\temp\Wish\AvalonDock"));
            Assert.IsTrue(result.Contains(@"C:\temp\Wish\depreciated"));
        }

        [Test]
        public void GetDirectoriesWithNestedArg()
        {
            var result = _completionHelper.GetDirectories(@"Wish\bin\", @"C:\temp");
            Assert.IsTrue(result.Contains(@"C:\temp\Wish\bin\Debug"));
        }

        [Test]
        public void GetDirectoriesNamesPrompt()
        {
            var dirs = _completionHelper.GetDirectories("", @"C:\");
            var result = _completionHelper.GetDirectoryNames(@"C:\", dirs);
            Assert.IsTrue(result.Contains("temp"));
            Assert.IsTrue(result.Contains("Users"));
            Assert.IsTrue(result.Contains("ubuntu"));
        }

        [Test]
        public void GetDirectoriesNamesNonPrompt()
        {
            var dirs = _completionHelper.GetDirectories("", @"C:\temp");
            var result = _completionHelper.GetDirectoryNames(@"C:\temp", dirs);
            Assert.IsTrue(result.Contains("Wish"));
            Assert.IsTrue(result.Contains("Werminal"));
            Assert.IsTrue(result.Contains("toolbox"));
        }
    }
}

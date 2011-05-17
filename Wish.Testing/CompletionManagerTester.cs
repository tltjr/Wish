using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Terminal;
using WishModule;

namespace Wish.Testing
{
    [TestFixture]
    public class CompletionManagerTester
    {
        [Test]
        public void PromptTests()
        {
            string result;
            var compManager = new CompletionManager();
            var command = new Command("cd p", "cd", new[] {"p"});
            var flag = compManager.Complete(out result, command, false, "c:\\", "c:\\>cd p");
            Assert.AreEqual("c:\\>cd PerfLogs\\", result);
            flag = compManager.Complete(out result, command, true, "c:\\", "c:\\>cd p");
            Assert.AreEqual("c:\\>cd PRO\\", result);
        }

        [Test]
        public void PTest2()
        {
            string result;
            var compManager = new CompletionManager();
            var command2 = new Command("cd PRO\\S", "cd", new[] {"PRO\\S"});
            var flag = compManager.Complete(out result, command2, false, "c:\\", "c:\\>cd PRO\\S");
            Assert.AreEqual("c:\\>cd PRO\\Source\\", result);
        }
    }
}

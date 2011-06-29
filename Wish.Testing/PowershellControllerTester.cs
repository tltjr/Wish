using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Wish.Core;

namespace Wish.Testing
{
    [TestFixture]
    public class PowershellControllerTester
    {
        [Test]
        public void TestErrorStream()
        {
            var controller = new PowershellController();
            controller.RunScript(@"cd C:\temp\blah");
            var results = controller.RunScriptForFormattedResult(@"git clone https://tltjr@github.com/tltjr/closing-the-loop.git");
            Assert.AreEqual(@"fatal: destination path 'closing-the-loop' already exists and is not an empty directory.", results);
        }
    }
}
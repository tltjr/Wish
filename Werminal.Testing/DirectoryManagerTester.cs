using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WerminalModule;

namespace Werminal.Testing
{
    [TestFixture]
    public class DirectoryManagerTester
    {
        [Test]
        public void ChangeViaFullName()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            Assert.AreEqual(@"C:\Users\tlthorn1", directoryManager.WorkingDirectory);
            directoryManager.ChangeDirectory(@"C:\temp");
            Assert.AreEqual(@"C:\temp", directoryManager.WorkingDirectory);
        }

    }
}

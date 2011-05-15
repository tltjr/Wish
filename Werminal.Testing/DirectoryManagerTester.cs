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
            var changed = directoryManager.ChangeDirectory(@"C:\temp");
            Assert.True(changed);
            Assert.AreEqual(@"C:\temp", directoryManager.WorkingDirectory);
        }

        [Test]
        public void ChangeViaFullNameInvalidDirectory()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            var changed = directoryManager.ChangeDirectory(@"C:\nonsense");
            Assert.False(changed);
            Assert.AreEqual(@"C:\Users\tlthorn1", directoryManager.WorkingDirectory);
        }

    }
}

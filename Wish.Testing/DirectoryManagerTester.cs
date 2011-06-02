using NUnit.Framework;
using WishModule;

namespace Wish.Testing
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

        [Test]
        public void ChangeViaRelativeNameChildDirectory()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            var changed = directoryManager.ChangeDirectory(@"Links");
            Assert.True(changed);
            Assert.AreEqual(@"C:\Users\tlthorn1\Links", directoryManager.WorkingDirectory);
        }

        [Test]
        public void ChangeViaRelativeNameInvalid()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            var changed = directoryManager.ChangeDirectory(@"Invalid");
            Assert.False(changed);
            Assert.AreEqual(@"C:\Users\tlthorn1", directoryManager.WorkingDirectory);
        }

        [Test]
        public void ChangeViaRelativeNameMultipleLevelChildDirectory()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            var changed = directoryManager.ChangeDirectory(@"Documents\EntLib50Src");
            Assert.True(changed);
            Assert.AreEqual(@"C:\Users\tlthorn1\Documents\EntLib50Src", directoryManager.WorkingDirectory);
        }

        [Test]
        public void ChangesAreCaseInsensitive()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            var changed = directoryManager.ChangeDirectory(@"links");
            Assert.True(changed);
            Assert.AreEqual(@"C:\Users\tlthorn1\links", directoryManager.WorkingDirectory);
        }

        [Test]
        public void ChangingToHigherDirectoriesWorks()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            var changed = directoryManager.ChangeDirectory(@"..");
            Assert.True(changed);
            Assert.AreEqual(@"C:\Users", directoryManager.WorkingDirectory);
        }

        [Test]
        public void ChangingToHigherHigherDirectoriesWorks()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            var changed = directoryManager.ChangeDirectory(@"..\..");
            Assert.True(changed);
            Assert.AreEqual(@"C:\", directoryManager.WorkingDirectory);
        }

        [Test]
        public void TrailingSlashesAreTrimmed()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1\"};
            Assert.AreEqual(@"C:\Users\tlthorn1", directoryManager.WorkingDirectory);
        }

        [Test]
        public void TrailingUnixSlashesAreTrimmed()
        {
            var directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1/"};
            Assert.AreEqual(@"C:\Users\tlthorn1", directoryManager.WorkingDirectory);
        }
    }
}

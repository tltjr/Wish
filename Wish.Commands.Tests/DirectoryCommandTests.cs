using NUnit.Framework;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class DirectoryCommandTests
    {
        // would throw exception if types not resolved
        [Test]
        public void Resolve()
        {
            new DirectoryCommand(string.Empty);
        }
    }
}

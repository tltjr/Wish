using NUnit.Framework;
using Wish.Commands.Runner;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class ArgumentFactoryTests
    {
        [Test]
        public void PowershellArgumentType()
        {
            var result = ArgumentFactory.Create(new Powershell(1), "arg");
            Assert.IsInstanceOf(typeof(PowershellArgument), result);
        }

        [Test]
        public void CmdArgument()
        {
            // this works because anything not of type powershell defaults to CmdArg  
            var result = ArgumentFactory.Create(new TestRunner(), "arg");
            Assert.IsInstanceOf(typeof(CmdArgument), result);
        }
    }

    public class TestRunner : IRunner
    {
        public string Execute(RunnerArgs args)
        {
            throw new System.NotImplementedException();
        }

        public string WorkingDirectory
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}

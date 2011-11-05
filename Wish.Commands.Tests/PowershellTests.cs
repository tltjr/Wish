using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Wish.Commands.Runner;

namespace Wish.Commands.Tests
{
    [TestFixture]
    public class PowershellTests
    {
        [Test]
        public void Io()
        {
            var pshell = new Powershell();
            //pshell.Execute(@"git clone https://tltjr@github.com/tltjr/rc.git");
        }
    }
}

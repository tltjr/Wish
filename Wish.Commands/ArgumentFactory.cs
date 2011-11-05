using System;
using Wish.Commands.Runner;

namespace Wish.Commands
{
    public class ArgumentFactory
    {
        public static Argument Create(IRunner runner, string argument)
        {
            var pshellType = runner.GetType();
            if(pshellType.Equals(typeof(Powershell)))
            {
                return new PowershellArgument(runner, argument); 
            }
            return new CmdArgument(runner, argument);
        }
    }
}

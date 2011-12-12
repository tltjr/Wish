using Wish.Commands.Runner;

namespace Wish.Commands
{
    public class ArgumentFactory
    {
        private static PartialPath _partialPath;

        public static Argument Create(IRunner runner, string argument)
        {
            _partialPath = new PartialPath(argument);
            var pshellType = runner.GetType();
            Argument arg;
            if (pshellType.Equals(typeof(Powershell)))
            {
                arg = new PowershellArgument(runner);
            }
            else
            {
                arg = new CmdArgument(runner);
            }
            arg.PartialPath = _partialPath;
            arg.MatchStrategy = SetMatchStrategy();
            return arg;
        }

        private static IMatchStrategy SetMatchStrategy()
        {
            var pattern = _partialPath.Pattern;
            if(pattern.Contains(":"))
            {
                return new FullPathMatchStrategy(pattern);
            }
            if(pattern.Contains(".."))
            {
                return new RelativePathMatchStrategy(pattern);
            }
            return new StandardMatchStrategy(pattern);
        }
    }
}

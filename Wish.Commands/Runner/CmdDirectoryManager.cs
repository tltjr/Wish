using System.Linq;
using System.Text.RegularExpressions;

namespace Wish.Commands.Runner
{
    public class CmdDirectoryManager
    {
        public void UpdateWorkingDirectory(string script)
        {
            var commandLine = new CommandLine(script);
            switch (CommandType(script))
            {
                case CdCommand.Prompt:
                    WorkingDirectory = commandLine.Function;
                    break;
                case CdCommand.Slash:
                    WorkingDirectory = WorkingDirectory.Substring(0, 3);
                    break;
                case CdCommand.Regular:
                    var args = commandLine.Arguments;
                    if (args != null && args.Count > 0)
                    {
                        var arg = args.First();
                        if(arg.Contains(":"))
                        {
                            WorkingDirectory = arg;
                        }
                        else
                        {
                            if (WorkingDirectory.EndsWith("\\"))
                            {
                                WorkingDirectory = WorkingDirectory + arg;
                            }
                            else
                            {
                                WorkingDirectory = WorkingDirectory + "\\" + arg;
                            }
                        }
                    }
                    break;
            }
        }

        public CdCommand CommandType(string script)
        {
            var commandLine = new CommandLine(script);
            var function = commandLine.Function;
            if (Regex.IsMatch(function, @"^[\w]:$"))
            {
                return CdCommand.Prompt;
            }
            if (Regex.IsMatch(function, @"^cd\\$"))
            {
                return CdCommand.Slash;
            }
            return Regex.IsMatch(function, @"^cd$") ? CdCommand.Regular : CdCommand.None;
        }

        public enum CdCommand
        {
            Prompt, Slash, Regular, None
        }

        public string WorkingDirectory { get; set; }
    }
}

using System;
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
                    _workingDirectory = commandLine.Function.ToUpper() + "\\";
                    break;
                case CdCommand.Slash:
                    _workingDirectory = WorkingDirectory.Substring(0, 3);
                    break;
                case CdCommand.Regular:
                    var args = commandLine.Arguments;
                    if (args != null && args.Count > 0)
                    {
                        var arg = args.First();
                        if(arg.Contains(":"))
                        {
                            _workingDirectory = arg;
                        }
                        else
                        {
                            if (arg.Contains(".."))
                            {
                                var levels = Regex.Matches(arg, @"\.\.").Count;
                                var segments = WorkingDirectory.Split('\\');
                                var count = segments.Count();
                                var newLevels = count - levels;
                                if (newLevels < 2)
                                {
                                    _workingDirectory = segments.First() + "\\";
                                }
                                else
                                {
                                    var newSegs = segments.Take(newLevels);
                                    _workingDirectory = string.Join("\\", newSegs);
                                }
                            }
                            else
                            {
                                if (WorkingDirectory.EndsWith("\\"))
                                {
                                    _workingDirectory = WorkingDirectory + arg;
                                }
                                else
                                {
                                    _workingDirectory = WorkingDirectory + "\\" + arg;
                                }
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

        private string _workingDirectory;
        public string WorkingDirectory
        {
            get
            {
                if (_workingDirectory != null) return _workingDirectory;
                var homedrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
                var homepath = Environment.GetEnvironmentVariable("HOMEPATH");
                if (null != homedrive && null != homepath)
                {
                    _workingDirectory = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
                }
                else
                {
                    _workingDirectory = @"C:\";
                }
                return _workingDirectory;
            } 
        }
    }
}

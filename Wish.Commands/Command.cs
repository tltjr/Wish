using System;
using System.Collections.Generic;
using System.Linq;
using Wish.Commands.Runner;
using Wish.Common;

namespace Wish.Commands
{
    public class Command : ICommand
    {
        private readonly IRunner _runner;
        private readonly CommandLine _commandLine;
        public Function Function { get; set; }
        public IEnumerable<Argument> Arguments { get; set; }
        public bool IsExit { get; set; }

        public string Text { get; set; }

        public Command(IRunner runner, string command) : this(command)
        {
            _runner = runner;
        }

        public Command(string command)
        {
            _commandLine = new CommandLine(command);
            Function = new Function(_commandLine.Function);
            IsExit = Function.Name.Equals("exit", StringComparison.InvariantCultureIgnoreCase);
            Arguments = CreateArguments(_commandLine.Arguments);
            Text = _commandLine.Text;
            _runner = new Powershell();
        }

        public CommandResult Execute()
        {
            var text = _runner.Execute(Text);
            var result = new CommandResult { Text = text };
            var dirChange = _commandLine.IsDirectoryCommand();
            if(dirChange)
            {
                result.WorkingDirectory = _runner.WorkingDirectory;
            }
            return result;
        }

        public IEnumerable<string> Complete()
        {
            var lastArgument = Arguments.LastOrDefault();
            return null == lastArgument ? Enumerable.Empty<string>() : lastArgument.Complete();
        }

        protected IEnumerable<Argument> CreateArguments(IEnumerable<string> arguments)
        {
            return arguments.Select(argument => ArgumentFactory.Create(_runner, argument));
        }

        public override string ToString()
        {
            return _commandLine.Text;
        }

        public override bool Equals(object obj)
        {
            return ((Command)obj)._commandLine.Text.Equals(_commandLine.Text);
        }

        public override int GetHashCode()
        {
            return _commandLine.Text.GetHashCode();
        }

    }
}

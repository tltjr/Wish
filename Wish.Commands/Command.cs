using System;
using System.Collections.Generic;
using System.Linq;
using Wish.Commands.Runner;

namespace Wish.Commands
{
    public class Command : ICommand
    {
        private readonly IRunner _runner;
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
            var commandLine = new CommandLine(command);
            Function = new Function(commandLine.Function);
            IsExit = Function.Name.Equals("exit", StringComparison.InvariantCultureIgnoreCase) ? true : false;
            Arguments = CreateArguments(commandLine.Arguments);
            Text = commandLine.Text;
            _runner = new Powershell();
        }

        public string Execute()
        {
            return _runner.Execute(Text);
        }

        public IEnumerable<string> Complete()
        {
            var lastArgument = Arguments.LastOrDefault();
            return null == lastArgument ? Enumerable.Empty<string>() : lastArgument.Complete();
        }

        protected IEnumerable<Argument> CreateArguments(IEnumerable<string> arguments)
        {
            foreach (var argument in arguments)
            {
                yield return ArgumentFactory.Create(_runner, argument);
            }
        }
    }
}

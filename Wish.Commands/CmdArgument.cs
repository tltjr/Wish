using System.Collections.Generic;

namespace Wish.Commands
{
    public class CmdArgument : Argument
    {
        private readonly IRunner _runner;

        public CmdArgument(IRunner runner, string text) : base(text)
        {
            _runner = runner;
        }

        public override IEnumerable<string> Complete()
        {
            var list = GetDirectories(_runner.WorkingDirectory);
            return QuoteSpaces(list, "\"");
        }
    }
}

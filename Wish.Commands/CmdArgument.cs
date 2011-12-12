using System.Collections.Generic;
using System.Linq;

namespace Wish.Commands
{
    public class CmdArgument : Argument
    {
        private readonly IRunner _runner;

        public CmdArgument(IRunner runner)
        {
            _runner = runner;
        }

        public override IEnumerable<string> Complete()
        {
            var list = GetDirectories(_runner.WorkingDirectory);
            if (list.Count == 0) return Enumerable.Empty<string>();
            return QuoteSpaces(list, "\"");
        }
    }
}

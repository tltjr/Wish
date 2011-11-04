using System.Collections.Generic;
using System.Linq;

namespace Wish.Commands
{
    public class PowershellArgument : Argument
    {
        private readonly IRunner _runner;
        
        public PowershellArgument(IRunner runner, string text) : base(text)
        {
            _runner = runner;
        }
        
        public override IEnumerable<string> Complete()
        {
            var list = GetDirectories(_runner.WorkingDirectory);
            var dotSlashed = list.Select(o => @".\" + o).ToList();
            return QuoteSpaces(dotSlashed, "'");
        }
    }
}

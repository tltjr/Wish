using System.Collections.Generic;

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
            var dotSlashed = new List<string>();
            foreach (var dir in list)
            {
                if(dir.Contains(@".\") || dir.Contains(@":"))
                {
                    dotSlashed.Add(dir);
                }
                else
                {
                    dotSlashed.Add(@".\" + dir);
                }
            }
            return QuoteSpaces(dotSlashed, "'");
        }
    }
}

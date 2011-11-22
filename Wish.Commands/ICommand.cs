using System.Collections.Generic;
using Wish.Common;

namespace Wish.Commands
{
    public interface ICommand
    {
        CommandResult Execute();
        IEnumerable<string> Complete();
        CommandLine CommandLine { get; set; }
        Function Function { get; set; }
        IEnumerable<Argument> Arguments { get; set; }
        bool IsExit { get; set; }
    }
}

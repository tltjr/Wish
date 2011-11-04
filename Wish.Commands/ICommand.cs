using System.Collections.Generic;

namespace Wish.Commands
{
    public interface ICommand
    {
        string Execute();
        IEnumerable<string> Complete();
        Function Function { get; set; }
        IEnumerable<Argument> Arguments { get; set; }
    }
}

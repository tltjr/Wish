using System.Collections.Generic;

namespace Wish.Core
{
    public interface ICommandHistory
    {
        List<Command> Commands { get; }
        void Add(Command command);
        Command GetNext();
        Command GetPrevious();
        void Reset();
    }
}
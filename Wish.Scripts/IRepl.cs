using Wish.Commands;
using Wish.Common;

namespace Wish.Scripts
{
    public interface IRepl
    {
        ICommand Read(IRunner runner, string text);
        ICommand Read(string text);
        void Eval(ICommand command);
        string Print();
        CommandResult Loop(IRunner runner, string text);
        CommandResult Loop(string text);
        CommandResult Start();
        CommandResult Up(string text);
        CommandResult Down(string text);
        History History { get; set; }
    }
}
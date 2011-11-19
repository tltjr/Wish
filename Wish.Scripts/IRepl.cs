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
        /// <summary>
        /// Initilizes the repl, inserts a prompt and
        /// changes to the appropriate directory.
        /// </summary>
        /// <returns>A CommandResult with the text and starting working directory.</returns>
        CommandResult Start();
        CommandResult Up(string text);
        CommandResult Down(string text);
        History History { get; set; }
        UniqueList<string> RecentDirectories { get; set; }
        UniqueList<string> RecentArguments { get; set; }
    }
}
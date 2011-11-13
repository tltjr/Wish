using System.Windows.Input;
using Wish.Commands;
using Wish.Commands.Runner;
using Wish.Common;
using Wish.Scripts;

namespace Wish
{
    public class WishModel
    {
        private readonly IRepl _repl;
        private readonly IRunner _runner;

        public WishModel(IRepl repl)
        {
            _repl = repl;
            _runner = new Powershell();
        }
        
        public WishModel(IRepl repl, IRunner runner)
        {
            _repl = repl;
            _runner = runner;
        }

        public CommandResult Raise(Key key, string text)
        {
            switch (key)
            {
                case Key.Enter: return _repl.Loop(_runner, text);
                case Key.Up: return _repl.Up(text);
                case Key.Down: return _repl.Down(text);
            }
            return new CommandResult { Handled = false, IsExit = false, Text = string.Empty, Error = "massive fail" };
        }

        public CommandResult Start()
        {
            return _repl.Start();
        }
    }
}
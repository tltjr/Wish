using System;
using System.Linq;
using System.Windows.Controls.Primitives;
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
        private bool _started;

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
                case Key.Enter: return Execute(text);
                case Key.Up: return _repl.Up(text);
                case Key.Down: return _repl.Down(text);
            }
            return new CommandResult { FullyProcessed = true };
        }

        public CommandResult Start()
        {
            if (!_started)
            {
                _started = true;
                return _repl.Start();
            }
            return new CommandResult
                       {
                           FullyProcessed = true
                       };
        }

        public CommandResult Execute(string text)
        {
            return _repl.Loop(_runner, text);
        }

        public void RequestHistorySearch(Popup popup, Action<string> callback)
        {
            var history = _repl.History.Select(x => x.ToString());
            var searchBox = new SearchBox.Views.SearchBox(history, callback);
            popup.Opened += searchBox.Opened;
            popup.Child = searchBox;
            popup.IsOpen = true;
            popup.StaysOpen = false;
        }
    }
}
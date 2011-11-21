using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Wish.Commands;
using Wish.Commands.Runner;
using Wish.Common;
using Wish.Scripts;
using Wish.SearchBox;

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

        public CommandResult Complete(TextEditor textEditor, Action onClosed)
        {
            var command = _repl.Read(textEditor.Text);
            var args = command.Arguments.ToList();
            string completionTarget;
            List<string> completions;
            if(args.Any())
            {
                var arg = args.Last();
                completionTarget = arg.PartialPath.Text;
                completions = command.Complete().ToList();
            }
            else
            {
                completionTarget = command.Function.Name;
                completions = command.Function.Complete().ToList();
            }
            if(completions.Count() == 0) return new CommandResult { FullyProcessed = true, Handled = true };
            var completionWindow = new CompletionWindow(textEditor.TextArea)
                                {
                                    SizeToContent = SizeToContent.WidthAndHeight,
                                    MinWidth = 150
                                };
            var completionData = completionWindow.CompletionList.CompletionData;
            foreach (var completion in completions)
            {
                completionData.Add(new CompletionData(completionTarget, completion));
            }
            if (completionData.Count == 0) return new CommandResult { FullyProcessed = true, Handled = true };
            completionWindow.Show();
            completionWindow.Closed += delegate
                                            {
                                                onClosed.Invoke();
                                                completionWindow = null;
                                            };
            //completionWindow.CompletionList.SelectedItem = completionData[0];
            return new CommandResult{ FullyProcessed = true, Handled = false, State = State.Tabbing };
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
            CreatePopup(popup, callback, history, SearchType.CommandHistory);
        }

        public void RequestRecentDirectory(Popup popup, Action<string> callback)
        {
            var dirs = _repl.RecentDirectories;
            CreatePopup(popup, callback, dirs, SearchType.RecentDirectories);
        }

        public void RequestRecentArgument(Popup popup, Action<string> callback)
        {
            var args = _repl.RecentArguments;
            CreatePopup(popup, callback, args, SearchType.RecentArguments);
        }

        private static void CreatePopup(Popup popup, Action<string> callback, IEnumerable<string> args, SearchType type)
        {
            var searchBox = new SearchBox.Views.SearchBox(type, args, callback);
            popup.Opened += searchBox.Opened;
            popup.Child = searchBox;
            popup.IsOpen = true;
            popup.StaysOpen = false;
        }
    }
}
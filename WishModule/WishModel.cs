using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Wish.Commands;
using Wish.Commands.Runner;
using Wish.Common;
using Wish.Input;
using Wish.Scripts;
using Wish.SearchBox;
using Wish.State;

namespace Wish
{
    public class WishModel
    {
        public IRepl Repl { get; set; }
        private IRunner _runner;
        private bool _started;

        public WishModel(int next)
        {
            Repl = new Repl();
            _runner = new Powershell(next);
        }

        public void SetRunner(IRunner runner, string workingDirectory)
        {
            _runner = runner;
            _runner.Execute(new RunnerArgs { Script = "cd " + workingDirectory});
            Repl.Prompt.Runner = runner;
        }
        
        public CommandResult Raise(WishArgs wishArgs)
        {
            var text = wishArgs.TextEditor.Text;
            switch (wishArgs.Key)
            {
                case Key.Enter: return Execute(text, wishArgs.WorkingDirectory);
                case Key.Up: return Repl.Up(text);
                case Key.Down: return Repl.Down(text);
            }
            return new CommandResult { FullyProcessed = true };
        }

        private CompletionWindow _completionWindow;
        public CommandResult Complete(WishArgs wishArgs)
        {
            var command = Repl.Read(_runner, wishArgs.TextEditor.Text);
            var args = command.Arguments.ToList();
            string completionTarget;
            List<string> completions;
            if(args.Any())
            {
                var arg = args.Last();
                completionTarget = arg.PartialPath.CompletionTarget;
                completions = command.Complete().ToList();
            }
            else
            {
                completionTarget = command.Function.Name;
                completions = command.Function.Complete().ToList();
            }
            if(completions.Count() == 0) return new CommandResult { FullyProcessed = true, Handled = true };
            _completionWindow = new CompletionWindow(wishArgs.TextEditor.TextArea)
                                             {
                                                 SizeToContent = SizeToContent.WidthAndHeight,
                                                 MinWidth = 150
                                             };
            var completionData = _completionWindow.CompletionList.CompletionData;
            foreach (var completion in completions)
            {
                completionData.Add(new CompletionData(completionTarget, completion));
            }
            if (completionData.Count == 0) return new CommandResult { FullyProcessed = true, Handled = true };
            _completionWindow.Show();
            _completionWindow.Closed += delegate
                                            {
                                                wishArgs.OnClosed.Invoke();
                                                _completionWindow = null;
                                            };
            return new CommandResult{ FullyProcessed = true, Handled = false, State = Common.State.Tabbing  };
        }

        public CommandResult Start()
        {
            if (!_started)
            {
                _started = true;
                return Repl.Start(_runner);
            }
            return new CommandResult
                       {
                           FullyProcessed = true
                       };
        }

        public CommandResult Execute(string text, string workingDirectory)
        {
            var reserved = new ReservedCommands();
            var commandLine = Repl.Read(_runner, text).CommandLine.Text;
            var isReserved = reserved.IsReservedCommand(commandLine);
            if (isReserved)
            {
                reserved.Execute(commandLine, workingDirectory);
                return Repl.ExecuteReserved(text);
            }
            return Repl.Loop(_runner, text);
        }

        public void RequestHistorySearch(Popup popup, Action<string> callback)
        {
            var history = Repl.History.Select(x => x.ToString());
            CreatePopup(popup, callback, history, SearchType.CommandHistory);
        }

        public void RequestRecentDirectory(Popup popup, Action<string> callback)
        {
            var dirs = Repl.RecentDirectories;
            CreatePopup(popup, callback, dirs, SearchType.RecentDirectories);
        }

        public void RequestRecentArgument(Popup popup, Action<string> callback)
        {
            var args = Repl.RecentArguments;
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

        public void CloseCompletionWindow()
        {
            _completionWindow.Close();
        }
    }
}
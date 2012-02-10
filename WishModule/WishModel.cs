using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
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
using System.ComponentModel.Composition;

namespace Wish
{
    [Export]
    public class WishModel
    {
        private readonly IRepl _repl;
        private IRunner _runner;
        private bool _started;
        private readonly Runspace _runspace;

        public WishModel()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
            _runner = new Powershell(_runspace);
            _repl = new Repl(_runner);
        }

        public void SetRunner(IRunner runner, string workingDirectory)
        {
            _runner = runner;
            _runner.Runspace = _runspace;
            _runner.Execute(new RunnerArgs { Script = "cd " + workingDirectory});
            _repl.Runner = runner;
        }
        
        public CommandResult Raise(WishArgs wishArgs)
        {
            var text = wishArgs.TextEditor.Text;
            switch (wishArgs.Key)
            {
                case Key.Enter: return Execute(text, wishArgs.WorkingDirectory);
                case Key.Up: return _repl.Up(text);
                case Key.Down: return _repl.Down(text);
            }
            return new CommandResult { FullyProcessed = true };
        }

        private CompletionWindow _completionWindow;
        public CommandResult Complete(WishArgs wishArgs)
        {
            var command = _repl.Read(wishArgs.TextEditor.Text);
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
                return _repl.Start();
            }
            return new CommandResult
                       {
                           FullyProcessed = true
                       };
        }

        public CommandResult Execute(string text, string workingDirectory)
        {
            var reserved = new ReservedCommands();
            var commandLine = _repl.Read(text).CommandLine.Text;
            var isReserved = reserved.IsReservedCommand(commandLine);
            if (isReserved)
            {
                reserved.Execute(commandLine, workingDirectory);
                return _repl.ExecuteReserved(text);
            }
            return _repl.Loop(text);
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

        public void CloseCompletionWindow()
        {
            _completionWindow.Close();
        }
    }
}
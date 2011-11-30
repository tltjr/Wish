using System;
using System.Linq;
using Wish.Commands;
using Wish.Commands.Runner;
using Wish.Common;

namespace Wish.Scripts
{
    public class Repl : IRepl
    {
        private Prompt _prompt;
        // for testing purposes
        public Prompt Prompt
        {
            get { return _prompt; }
            set
            {
                _prompt = value;
                LastPromptIndex = _prompt.Current.Length;
            }
        }
        public int LastPromptIndex { get; set; }
        public string Text { get; set; }
        private CommandResult _result;
        public History History { get; set; }
        public UniqueList<string> RecentDirectories { get; set; }
        public UniqueList<string> RecentArguments { get; set; }

        public CommandResult ExecuteReserved(string text)
        {
            var command = Read(text);
            History.Add(command);
            RecentArguments.AddRange(command.Arguments.Select(o => o.PartialPath.Text));
            InsertNewPrompt();
            InsertLineBeforePrompt();
            InsertLineBeforePrompt();
            return new CommandResult {Text = Text, Handled = true};
        }

        public CommandResult Start(IRunner runner)
        {
            History = new History();
            RecentDirectories = new UniqueList<string>();
            RecentArguments = new UniqueList<string>();
            _prompt = new Prompt();
            LastPromptIndex = _prompt.Current.Length;
            Text = _prompt.Current;
            var command = new Command(runner, "cd " + _prompt.WorkingDirectory);
            command.Execute();
            if (runner.GetType() == typeof(Powershell))
            {
                var profile = new Profile();
                if (profile.Exists)
                {
                    var profileInfo = profile.Load(Text);
                    if (!string.IsNullOrWhiteSpace(profileInfo))
                    {
                        Text = profileInfo + "\n\n" + Text;
                    }
                }
            }
            return new CommandResult
                       {
                           Text = Text,
                           WorkingDirectory = _prompt.WorkingDirectory,
                           PromptLength = _prompt.Current.Length + 1,
                           FullyProcessed = false
                       };
        }

        public ICommand Read(IRunner runner, string text)
        {
            Text = text;
            var line = GetLine(text);
            return new Command(runner, line, _prompt.WorkingDirectory);
        }

        public ICommand Read(string text)
        {
            Text = text;
            var line = GetLine(text);
            return new Command(line);
        }

        public void Eval(ICommand command)
        {
            History.Add(command);
            RecentArguments.AddRange(command.Arguments.Select(o => o.PartialPath.Text));
            _result = command.Execute();
            var workingDirectory = _result.WorkingDirectory;
            RecentDirectories.Add(workingDirectory);
            _prompt.Update(workingDirectory);
            _result.PromptLength = _prompt.Current.Length + 1;
        }

        public string Print()
        {
            InsertNewPrompt();
            InsertLineBeforePrompt();
            return Text;
        }

        public CommandResult Loop(IRunner runner, string text)
        {
            var command = Read(runner, text);
            ProcessCommand(command);
            _result.WorkingDirectory = _prompt.WorkingDirectory;
            return _result;
        }

        public CommandResult Loop(string text)
        {
            var command = Read(text);
            ProcessCommand(command);
            _result.WorkingDirectory = _prompt.WorkingDirectory;
            return _result;
        }

        public CommandResult Up(string text)
        {
            Text = text;
            var command = History.Up();
            return AppendToBaseText(command);
        }

        public CommandResult Down(string text)
        {
            Text = text;
            var command = History.Down();
            return AppendToBaseText(command);
        }

        private CommandResult AppendToBaseText(ICommand command)
        {
            var baseText = Text.Substring(0, LastPromptIndex);
            Text = baseText + command;
            return new CommandResult { Text = Text, FullyProcessed = false, Handled = true};
        }

        private void ProcessCommand(ICommand command)
        {
            if (command.IsExit)
            {
                _result = new CommandResult { IsExit = true, FullyProcessed = true };
            }
            else
            {
                Eval(command);
                _result.Text = Print();
                _result.IsExit = false;
                _result.FullyProcessed = false;
            }
        }

        private string GetLine(string text)
        {
            var line = text.Substring(LastPromptIndex);
            return line.Trim();
        }

        private void InsertNewPrompt()
		{
			if (Text.Length > 0)
				Text += Text.EndsWith("\n") ? String.Empty : "\n";
		    Text += _prompt.Current;
			LastPromptIndex = Text.Length;
		}

        private void InsertLineBeforePrompt() 
		{
			var startIndex = LastPromptIndex - _prompt.Current.Length;
			var oldPromptIndex = LastPromptIndex;
            var temp = _result.Text;
            //temp += temp.EndsWith("\n") ? String.Empty : "\n";
            temp += "\n";
            Text = Text.Insert(startIndex, temp);
			LastPromptIndex = oldPromptIndex + temp.Length;
            _result.Text = temp;
		}
    }
}
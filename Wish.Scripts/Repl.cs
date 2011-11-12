using System;
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
    }

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

        public CommandResult Start()
        {
            _prompt = new Prompt();
            LastPromptIndex = _prompt.Current.Length;
            Text = _prompt.Current;
            var command = new Command("cd " + _prompt.WorkingDirectory);
            command.Execute();
            return new CommandResult
                       {
                           Text = Text,
                           WorkingDirectory = _prompt.WorkingDirectory,
                           PromptLength = _prompt.Current.Length + 1
                       };
        }

        public ICommand Read(IRunner runner, string text)
        {
            Text = text;
            var line = GetLine(text);
            return new Command(runner, line);
        }

        public ICommand Read(string text)
        {
            Text = text;
            var line = GetLine(text);
            return new Command(line);
        }

        public void Eval(ICommand command)
        {
            _result = command.Execute();
            _prompt.Update(_result.WorkingDirectory);
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

        private void ProcessCommand(ICommand command)
        {
            if (command.IsExit)
            {
                _result = new CommandResult { IsExit = true, Handled = true };
            }
            else
            {
                Eval(command);
                _result.Text = Print();
                _result.IsExit = false;
                _result.Handled = true;
            }
        }

        private string GetLine(string text)
        {
            return text.Substring(LastPromptIndex);
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
            temp += temp.EndsWith("\n") ? String.Empty : "\n";
            Text = Text.Insert(startIndex, temp);
			LastPromptIndex = oldPromptIndex + temp.Length;
            _result.Text = temp;
		}
    }

}

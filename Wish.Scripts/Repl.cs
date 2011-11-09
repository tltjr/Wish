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
        public Prompt Prompt { get; set; }
        public int LastPromptIndex { get; set; }
        private string _text;
        private CommandResult _result;

        public CommandResult Start()
        {
            Prompt = new Prompt {Current = @"> "};
            // needs to be adjusted by one for some ungodly reason
            Global.PromptLength = Prompt.Current.Length + 1;
            LastPromptIndex = Prompt.Current.Length;
            _text = Prompt.Current;
            return new CommandResult {Text = _text};
        }

        public ICommand Read(IRunner runner, string text)
        {
            _text = text;
            var line = GetLine(text);
            return new Command(runner, line);
        }

        public ICommand Read(string text)
        {
            _text = text;
            var line = GetLine(text);
            return new Command(line);
        }

        public void Eval(ICommand command)
        {
            _result = command.Execute();
        }

        public string Print()
        {
            InsertNewPrompt();
            InsertLineBeforePrompt();
            return _text;
        }

        public CommandResult Loop(IRunner runner, string text)
        {
            var command = Read(runner, text);
            ProcessCommand(command);
            return _result;
        }

        public CommandResult Loop(string text)
        {
            var command = Read(text);
            ProcessCommand(command);
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
			if (_text.Length > 0)
				_text += _text.EndsWith("\n") ? String.Empty : "\n";
		    _text += Prompt.Current;
			LastPromptIndex = _text.Length;
		}

        private void InsertLineBeforePrompt() 
		{
			var startIndex = LastPromptIndex - Prompt.Current.Length;
			var oldPromptIndex = LastPromptIndex;
            var temp = _result.Text;
            temp += temp.EndsWith("\n") ? String.Empty : "\n";
            _text = _text.Insert(startIndex, temp);
			LastPromptIndex = oldPromptIndex + temp.Length;
            _result.Text = temp;
		}
    }

}

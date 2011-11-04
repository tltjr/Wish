using System;
using Wish.Commands;

namespace Wish.Scripts
{
    public interface IRepl
    {
        ICommand Read(IRunner runner, string text);
        ICommand Read(string text);
        void Eval(ICommand command);
        string Print();
        string Loop(IRunner runner, string text);
        string Loop(string text);
    }

    public class Repl : IRepl
    {
        public Prompt Prompt { get; set; }
        public int LastPromptIndex { get; set; }
        private string _text;
        private string _result;

        public Repl()
        {
            Prompt = new Prompt {Current = @"> "};
            LastPromptIndex = Prompt.Current.Length;
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

        public string Loop(IRunner runner, string text)
        {
            var command = Read(runner, text);
            Eval(command);
            return Print();
        }

        public string Loop(string text)
        {
            var command = Read(text);
            Eval(command);
            return Print();
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
            _result += _result.EndsWith("\n") ? String.Empty : "\n";
            _text = _text.Insert(startIndex, _result);
			LastPromptIndex = oldPromptIndex + _result.Length;
		}
    }

}

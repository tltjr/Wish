using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Terminal;
using Wish.Core;

namespace Wish.Models
{
    public class Terminal : INotifyPropertyChanged
    {
        private string _text = String.Empty;
        private string _prompt;
        private const string StartDirectory = @"C:\Users\tlthorn1";
        private readonly PowershellController _powershellController = new PowershellController();

		public int LastPromptIndex { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Terminal()
        {
			LastPromptIndex = -1;
            ChangeDirectory("cd " + StartDirectory);
            InsertNewPrompt();
        }

        public string Prompt
        {
            get { return _prompt; }
            set { _prompt = value; }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        public string FontFamily
        {
            get { return "Consolas"; }
        }

        public void ProcessCommand()
        {
            var command = ParseScript();
            var isDirChange = IsDirectoryChange(command.Name);
            if(isDirChange)
            {
                var comm = command.Raw;
                if(IsPromptBasedChange(command.Name))
                {
                    comm = comm.Insert(0, "cd ");
                    comm += "\\";
                }
                ChangeDirectory(comm);
                InsertNewPrompt();
                InsertLineBeforePrompt("\n");
            }
            else
            {
                var output = _powershellController.RunScriptForFormattedResult(command.Raw);
                InsertNewPrompt();
                InsertLineBeforePrompt(output);
            }
        }

        private bool IsPromptBasedChange(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z]:$");
        }

        private void ChangeDirectory(string target)
        {
            _powershellController.RunScript(target);
            var results = _powershellController.RunScriptForResult("pwd");
            if (results.Count == 0) return;
            var pwd = results[0];
            Prompt = pwd + ">";
        }

		private void InsertNewPrompt()
		{
			if (Text.Length > 0)
				Text += Text.EndsWith("\n") ? "" : "\n";
		    Text += _prompt;
			LastPromptIndex = Text.Length;
		}

        private void InsertOutput(string text)
        {
            
        }

		private void InsertLineBeforePrompt(string str) 
		{
			var startIndex = LastPromptIndex - Prompt.Length;
			var oldPromptIndex = LastPromptIndex;
			if (!str.EndsWith("\n"))
				str += "\n";
			Text = Text.Insert(startIndex, str);
			LastPromptIndex = oldPromptIndex + str.Length;
            PropertyChanged(this, new PropertyChangedEventArgs("Text"));
		}

        private Command ParseScript()
        {
			var line = Text.Substring(LastPromptIndex);
			Text += "\n";
			return TerminalUtils.ParseCommandLine(line);
        }

        private bool IsDirectoryChange(string name)
        {
            return Regex.IsMatch(name, @"^(c|pop|push)d$")
                   || Regex.IsMatch(name, @"^cd\\$")
                   || Regex.IsMatch(name, @"^[A-Za-z]:$");
        }
    }
}

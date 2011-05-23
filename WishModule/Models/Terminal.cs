using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Media;
using CodeBoxControl;
using CodeBoxControl.Decorations;
using Microsoft.Practices.Prism.Regions;
using Terminal;
using Wish.Core;
using Wish.Views;

namespace Wish.Models
{
    public class Terminal : INotifyPropertyChanged
    {
        private string _text = String.Empty;
        private string _prompt;
        private string _workingDirectory;
        private readonly PowershellController _powershellController = new PowershellController();
        private IRegion _region;
        private WishView _view;

        //private readonly SyntaxHighlighter _syntaxHighlighter;
        private readonly RegexDecoration _user;

		public int LastPromptIndex { get; private set; }
        public string WorkingDirectory
        {
            get { return _workingDirectory; }
            set { _workingDirectory = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Terminal(IRegion region, WishView view, CodeBox textBox, string workingDirectory)
        {
            _region = region;
            _view = view;
            _workingDirectory = workingDirectory;
            textBox.Decorations.Clear();
            var reg = WindowsIdentity.GetCurrent().Name;
            _user = new RegexDecoration
                        {
                            DecorationType = EDecorationType.TextColor,
                            Brush = new SolidColorBrush(Colors.Green),
                            RegexString = reg
                        };
            textBox.Decorations.Add(_user);
            //_syntaxHighlighter = new SyntaxHighlighter();
            //_syntaxHighlighter.Highlight();
			LastPromptIndex = -1;
            ChangeDirectory("cd " + _workingDirectory);
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

        public int FontSize
        {
            get { return 16; }
        }

        public string Background
        {
            get { return "Black"; }
        }

        public string Foreground
        {
            get { return "White"; }
        }

        public void ProcessCommand()
        {
            var command = ParseScript();
            if(command.Name.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                var i = _region.Views.Count();
                if(i > 1)
                {
                    _region.Remove(_view);
                }
                else
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }
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
            _workingDirectory = pwd.ToString();
            Prompt = WindowsIdentity.GetCurrent().Name;
            Prompt += "@";
            Prompt += Environment.MachineName;
            Prompt += " ";
            Prompt += pwd;
            Prompt += " ";
            Prompt += ">>";
        }

        public void InsertNewPrompt()
		{
			if (Text.Length > 0)
				Text += Text.EndsWith("\n") ? "" : "\n";
		    Text += _prompt;
			LastPromptIndex = Text.Length;
		}

        private void InsertOutput(string text)
        {
            
        }

        public void InsertLineBeforePrompt(string str) 
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

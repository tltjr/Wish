﻿using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Terminal;

namespace Wish.Models
{
    public class Terminal : INotifyPropertyChanged
    {
        private string _text = String.Empty;
        private string _prompt = @"C:\Users\tlthorn1>";

		public int LastPromptIndex { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Terminal()
        {
			LastPromptIndex = -1;
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

		public void InsertNewPrompt()
		{
			if (Text.Length > 0)
				Text += Text.EndsWith("\n") ? "" : "\n";
		    Text += _prompt;
			LastPromptIndex = Text.Length;
		}

        public void InsertOutput(string text)
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

        public Command ParseScript()
        {
			var line = Text.Substring(LastPromptIndex);
			Text += "\n";
			return TerminalUtils.ParseCommandLine(line);
        }

        public bool GetCommandType(string name)
        {
            return Regex.IsMatch(name, @"^(c|pop|push)d$");
        }

    }
}

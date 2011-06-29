using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit;
using Wish.Core;

namespace GuiHelpers
{
    public class TextTransformations
    {
        private string _prompt;

        public Command ParseScript(string text, int lastPromptIndex)
        {
			var line = text.Substring(lastPromptIndex);
			return ParseCommandLine(line);
        }

		public Command ParseCommandLine(string line) {
			var command = "";
			var args = new List<string>();
			var m = Regex.Match(line.Trim() + " ", @"^(.+?)(?:\s+|$)(.*)");
			if (m.Success) {
				command = m.Groups[1].Value.Trim();
				var argsLine = m.Groups[2].Value.Trim();
				var m2 = Regex.Match(argsLine + " ", @"(?<!\\)"".*?(?<!\\)""|[\S]+");
				while (m2.Success) {
					var arg = Regex.Replace(m2.Value.Trim(), @"^""(.*?)""$", "$1");
					args.Add(arg);
					m2 = m2.NextMatch();
				}
			}
			return new Command(line, command, args.ToArray());
		}

        public string Prompt
        {
            get { return _prompt; }
            set { _prompt = value; }
        }

        // returns LastPromptIndex
        public int InsertNewPrompt(TextEditor textEditor)
		{
			if (textEditor.Text.Length > 0)
				textEditor.Text += textEditor.Text.EndsWith("\n") ? "" : "\n";
		    textEditor.Text += _prompt;
			return textEditor.Text.Length;
		}

        // returns LastPromptIndex
        public int InsertLineBeforePrompt(TextEditor textEditor, string str, int lastPromptIndex) 
		{
			var startIndex = lastPromptIndex - Prompt.Length;
			var oldPromptIndex = lastPromptIndex;
            if (!str.EndsWith("\n"))
                str += "\n";
			textEditor.Text = textEditor.Text.Insert(startIndex, str);
			return oldPromptIndex + str.Length;
		}

        public void CreatePrompt(string workingDirectory)
        {
            Prompt = WindowsIdentity.GetCurrent().Name;
            Prompt += "@";
            Prompt += Environment.MachineName;
            Prompt += " ";
            Prompt += workingDirectory;
            Prompt += " ";
            Prompt += ">> ";
        }
    }
}

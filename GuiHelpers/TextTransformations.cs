using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using ICSharpCode.AvalonEdit;
using Terminal;

namespace GuiHelpers
{
    public class TextTransformations
    {
        private string _prompt;

        public Command ParseScript(string text, int lastPromptIndex)
        {
			var line = text.Substring(lastPromptIndex);
			return TerminalUtils.ParseCommandLine(line);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CodeBoxControl;
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
        public int InsertNewPrompt(CodeBox codeBox)
		{
			if (codeBox.Text.Length > 0)
				codeBox.Text += codeBox.Text.EndsWith("\n") ? "" : "\n";
		    codeBox.Text += _prompt;
			return codeBox.Text.Length;
		}

        // returns LastPromptIndex
        public int InsertLineBeforePrompt(CodeBox codeBox, string str, int lastPromptIndex) 
		{
			var startIndex = lastPromptIndex - Prompt.Length;
			var oldPromptIndex = lastPromptIndex;
			if (!str.EndsWith("\n"))
				str += "\n";
			codeBox.Text = codeBox.Text.Insert(startIndex, str);
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

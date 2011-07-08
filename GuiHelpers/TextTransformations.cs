using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit;
using Tltjr.Core;
using Wish.Core;

namespace GuiHelpers
{
    public class TextTransformations
    {
        private string _prompt;
        private int _lastPromptIndex = -1;

        public Command ParseScript(string text)
        {
			var line = text.Substring(_lastPromptIndex);
			return line.ParseCommandLine();
        }

        public string Prompt
        {
            get { return _prompt; }
            set { _prompt = value; }
        }

        public void InsertNewPrompt(TextEditor textEditor)
		{
			if (textEditor.Text.Length > 0)
				textEditor.Text += textEditor.Text.EndsWith("\n") ? "" : "\n";
		    textEditor.Text += _prompt;
			_lastPromptIndex = textEditor.Text.Length;
		}

        public void InsertLineBeforePrompt(TextEditor textEditor, string str) 
		{
			var startIndex = _lastPromptIndex - Prompt.Length;
			var oldPromptIndex = _lastPromptIndex;
            if (!str.EndsWith("\n"))
                str += "\n";
			textEditor.Text = textEditor.Text.Insert(startIndex, str);
			_lastPromptIndex = oldPromptIndex + str.Length;
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

        public void HandleResults(TextEditor editor, string output, string workingDirectory)
        {
            CreatePrompt(workingDirectory);
            InsertNewPrompt(editor);
            InsertLineBeforePrompt(editor, output);
        }

        public void ReplaceLine(TextEditor editor, Command command)
        {
            if (null == command) return;
            var raw = command.Raw;
            var text = editor.Text;
            var line = editor.Text.Substring(_lastPromptIndex);
            if (!String.IsNullOrEmpty(line))
            {
                var baseText = text.Remove(text.Length - line.Length);
                editor.Text = baseText + raw;
            }
            else
            {
                editor.Text += raw;
            }
        }
    }
}

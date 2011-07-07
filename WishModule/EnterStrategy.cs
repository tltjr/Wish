using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GuiHelpers;
using Wish.Core;

namespace Wish
{
    public class EnterStrategy : IKeyStrategy
    {
        private readonly WishModel _wishModel;
        private readonly TextTransformations _textTransformations = new TextTransformations();

        public EnterStrategy(WishModel wishModel)
        {
            _wishModel = wishModel;
        }

        public void Handle(KeyEventArgs e)
        {
            if(!_wishModel.ActivelyTabbing)
            {
                var command = _textTransformations.ParseScript(_wishModel.TextEditor.Text, _wishModel.LastPromptIndex);
                CommandHistory.Add(command);
                _wishModel.TextEditor.Text += "\n";
                if (!_wishModel.IsExit(command))
                {
                    var output = _wishModel.CommandEngine.ProcessCommand(command);
                    HandleResults(output);
                }
                EnsureCorrectCaretPosition();
                e.Handled = true;
            }
            CommandHistory.Reset();
        }

        private void EnsureCorrectCaretPosition()
        {
            var editor = _wishModel.TextEditor;
            var line = editor.Document.GetLineByNumber(editor.TextArea.Caret.Line);
            if (0 == line.Length)
            {
                editor.TextArea.Caret.Line = editor.TextArea.Caret.Line - 1;
            }
        }

        public void HandleResults(string output)
        {
            var workingDirectory = _wishModel.CommandEngine.WorkingDirectory;
            _wishModel.WorkingDirectory = workingDirectory;
            _wishModel.View.Title = workingDirectory;
            _textTransformations.CreatePrompt(workingDirectory);
            _wishModel.LastPromptIndex = _textTransformations.InsertNewPrompt(_wishModel.TextEditor);
            _wishModel.LastPromptIndex = _textTransformations.InsertLineBeforePrompt(_wishModel.TextEditor, output, _wishModel.LastPromptIndex);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GuiHelpers;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Wish.Core;

namespace Wish
{
    public class StandardState : IState
    {
        private readonly WishModel _wish;

        public StandardState(WishModel wish)
        {
            _wish = wish;
        }

        public void KeyPress(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    var next = _wish.CommandHistory.GetNext();
                    ReplaceLine(next);
                    break;

                case Key.Down:
                    var previous = _wish.CommandHistory.GetPrevious();
                    ReplaceLine(previous);
                    break;

                case Key.Tab:

                    var line2 = _wish.TextEditor.Text.Substring(_wish.LastPromptIndex);
                    var command2 = _wish.TextTransformations.ParseCommandLine(line2);
                    if(command2.Args.Count() > 0)
                    {
                        new CompletionManager().CreateWindow(_wish.TextEditor.TextArea, command2.Args, _wish.WorkingDirectory);
                        _wish.State = _wish.Complete;
                    }
                    e.Handled = true;
                    break;

                case Key.Enter:
            
                    var command = _wish.TextTransformations.ParseScript(_wish.TextEditor.Text, _wish.LastPromptIndex);
                    _wish.CommandHistory.Add(command);
                    _wish.TextEditor.Text += "\n";
                    if (!IsExit(command))
                    {
                        var output = _wish.CommandEngine.ProcessCommand(command);
                        _wish.WorkingDirectory = _wish.CommandEngine.WorkingDirectory;
                        _wish.View.Title = _wish.WorkingDirectory;
                        _wish.TextTransformations.CreatePrompt(_wish.WorkingDirectory);
                        _wish.LastPromptIndex = _wish.TextTransformations.InsertNewPrompt(_wish.TextEditor);
                        _wish.LastPromptIndex = _wish.TextTransformations.InsertLineBeforePrompt(_wish.TextEditor, output, _wish.LastPromptIndex);
                    }
                    var line = _wish.TextEditor.Document.GetLineByNumber(_wish.TextEditor.TextArea.Caret.Line);
                    if (0 == line.Length)
                    {
                        _wish.TextEditor.TextArea.Caret.Line = _wish.TextEditor.TextArea.Caret.Line - 1;
                    }
                    e.Handled = true;
                    break;

                default:
                    break;
            }
        }

        public void RequestHistorySearch()
        {
            var history = _wish.CommandHistory.Commands;
        }

        private bool IsExit(Command command)
        {
            if (command.Name.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                var i = _wish.Region.Views.Count();
                if (i > 1)
                {
                    _wish.Region.Remove(_wish.View);
                }
                else
                {
                    System.Windows.Application.Current.Shutdown();
                }
                return true;
            }
            return false;
        }

        private void ReplaceLine(Command command)
        {
            if (null == command) return;
            var raw = command.Raw;
            var text = _wish.TextEditor.Text;
            var line = _wish.TextEditor.Text.Substring(_wish.LastPromptIndex);
            if (!String.IsNullOrEmpty(line))
            {
                var baseText = text.Remove(text.Length - line.Length);
                _wish.TextEditor.Text = baseText + raw;
            }
            else
            {
                _wish.TextEditor.Text += raw;
            }
        }
    }
}

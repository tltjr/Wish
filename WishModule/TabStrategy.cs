using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GuiHelpers;
using ICSharpCode.AvalonEdit;
using Wish.Core;

namespace Wish
{
    public class TabStrategy : IKeyStrategy
    {
        private readonly WishModel _wishModel;
        private readonly TextTransformations _textTransformations = new TextTransformations();

        public TabStrategy(WishModel wishModel)
        {
            _wishModel = wishModel;
        }

        public void OnCompletionWindowClosed()
        {
            _wishModel.ActivelyTabbing = false;
        }

        public void Handle(KeyEventArgs e)
        {
            if (_wishModel.ActivelyTabbing) return;
            bool result = false;
            //var line2 = _wishModel.TextEditor.Text.Substring(_wishModel.LastPromptIndex);
            //var command2 = _textTransformations.ParseCommandLine(line2);
            var command = _textTransformations.ParseScript(_wishModel.TextEditor.Text, _wishModel.LastPromptIndex);
            if (command.Args.Length > 0)
            {
                result = new CompletionManager().CreateWindow(_wishModel.TextEditor.TextArea, command.Args, _wishModel.WorkingDirectory, OnCompletionWindowClosed);
            }
            e.Handled = true;
            CommandHistory.Reset();
            _wishModel.ActivelyTabbing = result;
        }
    }
}

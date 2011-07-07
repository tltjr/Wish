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
            var line2 = _wishModel.TextEditor.Text.Substring(_wishModel.LastPromptIndex);
            var command2 = _textTransformations.ParseCommandLine(line2);
            if (command2.Args.Length > 0)
            {
                new CompletionManager().CreateWindow(_wishModel.TextEditor.TextArea, command2.Args, _wishModel.WorkingDirectory, OnCompletionWindowClosed);
            }
            e.Handled = true;
            CommandHistory.Reset();
            _wishModel.ActivelyTabbing = true;
        }
    }
}

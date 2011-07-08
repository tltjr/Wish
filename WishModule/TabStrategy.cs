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

        public TabStrategy(WishModel wishModel)
        {
            _wishModel = wishModel;
        }

        public void OnCompletionWindowClosed()
        {
            _wishModel.ActivelyTabbing = false;
        }

        public string Handle(KeyEventArgs e)
        {
            if (_wishModel.ActivelyTabbing) return String.Empty;
            bool result = false;
            var command = _wishModel.TextTransformations.ParseScript(_wishModel.TextEditor.Text);
            if (command.Args.Length > 0)
            {
                result = new CompletionManager().CreateWindow(_wishModel.TextEditor.TextArea, command.Args, _wishModel.WorkingDirectory, OnCompletionWindowClosed);
            }
            e.Handled = true;
            CommandHistory.Reset();
            _wishModel.ActivelyTabbing = result;
            return null;
        }
    }
}

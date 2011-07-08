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
        private readonly TextTransformations _textTransformations;

        public EnterStrategy(WishModel wishModel)
        {
            _wishModel = wishModel;
            _textTransformations = _wishModel.TextTransformations;
        }

        public string Handle(KeyEventArgs e)
        {
            string workingDirectory = null;
            if(!_wishModel.ActivelyTabbing)
            {
                var command = _textTransformations.ParseScript(_wishModel.TextEditor.Text);
                workingDirectory = _wishModel.Execute(command);
                e.Handled = true;
            }
            CommandHistory.Reset();
            return workingDirectory;
        }

    }
}

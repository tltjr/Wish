using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ICSharpCode.AvalonEdit;
using Wish.Common;

namespace Wish.State
{
    public class Completion : IState
    {
        private readonly WishModel _wishModel;

        public Completion(WishModel wishModel)
        {
            _wishModel = wishModel;
        }

        public CommandResult OnPreviewKeyDown(WishArgs args)
        {

            switch (args.Key)
            {
                case Key.Enter:
                    return new CommandResult { State = Common.State.Tabbing, FullyProcessed = true};
                case Key.Tab:
                    return new CommandResult { State = Common.State.Tabbing, FullyProcessed = true};
                case Key.Escape:
                    _wishModel.CloseCompletionWindow();
                    break;
            }
            return new CommandResult {Handled = true, FullyProcessed = true};
        }

        public Popup RequestPopup(Action<Popup, Action<string>> requestRecentArgument, Action<string> append, TextEditor textEditor)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows.Controls.Primitives;
using ICSharpCode.AvalonEdit;
using Wish.Common;

namespace Wish.State
{
    public interface IState
    {
        CommandResult OnPreviewKeyDown(WishArgs args);
        Popup RequestPopup(Action<Popup, Action<string>> requestRecentArgument, Action<string> append, TextEditor textEditor);
        //KeyDownResult ProcessCommandResult(CommandResult result);
    }
}

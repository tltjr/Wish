using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ICSharpCode.AvalonEdit;
using Wish.Common;

namespace Wish.State
{
    public class Normal : IState
    {
        private readonly WishModel _wishModel;

        public Normal(WishModel wishModel)
        {
            _wishModel = wishModel;
        }

        public CommandResult OnPreviewKeyDown(WishArgs args)
        {
            return args.Key.Equals(Key.Tab)
                             ? _wishModel.Complete(args)
                             : _wishModel.Raise(args);
        }

        public Popup RequestPopup(Action<Popup, Action<string>> fn, Action<string> action, TextEditor textEditor)
        {
            var popup = new Popup { IsOpen = false, PlacementTarget = textEditor, Placement = PlacementMode.Center, Width = 285 };
            fn(popup, action);
            return popup;
        }
    }
}

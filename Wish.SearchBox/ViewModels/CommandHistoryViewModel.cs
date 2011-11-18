using System;
using System.Windows.Input;

namespace Wish.SearchBox.ViewModels
{
    public class CommandHistoryViewModel : SearchBoxViewModel
    {
        public override string Color
        {
            get { return "#7E51FD"; }
        }

        public override string Text
        {
            get { return "Search Command History: "; }
        }

        public override void HandleEnter(KeyEventArgs e, Action<string> onSelection, string selected)
        {
            onSelection(selected);
            e.Handled = true;
        }
    }
}

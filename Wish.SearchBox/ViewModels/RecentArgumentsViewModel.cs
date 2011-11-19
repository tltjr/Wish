using System;
using System.Windows.Input;

namespace Wish.SearchBox.ViewModels
{
    public class RecentArgumentsViewModel : SearchBoxViewModel
    {
        public override string Color
        {
            get { return "#51fd62"; }
        }

        public override string Text
        {
            get { return "Search Argument History: "; }
        }

        public override void HandleEnter(KeyEventArgs e, Action<string> onSelection, string selected)
        {
            onSelection(selected);
            e.Handled = true;
        }
    }
}

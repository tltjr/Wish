using System;
using System.Windows.Input;

namespace Wish.SearchBox.ViewModels
{
    public class RecentDirectoriesViewModel : SearchBoxViewModel
    {
        public override string Color
        {
            get { return "#ffc853"; }
        }

        public override void HandleEnter(KeyEventArgs e, Action<string> onSelection, string selected)
        {
            onSelection("cd " + selected);
            e.Handled = true;
        }
    }
}

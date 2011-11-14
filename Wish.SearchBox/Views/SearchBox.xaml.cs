using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wish.SearchBox.Views
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private readonly OnSelection _onSelection;

        public delegate string OnSelection(string command);

        public SearchBox(OnSelection onSelection)
        {
            InitializeComponent();
            _onSelection = onSelection;
        }

        public void Opened(object sender, EventArgs e)
        {
            searchTb.Focus();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            //_viewModel = Resources["ViewModel"] as TabCompletionViewModel;
            if (Key.Enter != e.Key) return;
            {
                var selectedItem = searchTb.ListBox.SelectedItem as string;
                _onSelection(selectedItem);
                e.Handled = true;
            }
        }
    }
}

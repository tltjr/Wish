using System;
using System.Collections.Generic;
using System.Windows.Input;
using Wish.SearchBox.ViewModels;

namespace Wish.SearchBox.Views
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox
    {
        private readonly Action<string> _onSelectionCallback;

        public SearchBox(IEnumerable<string> history, Action<string> callback)
        {
            InitializeComponent();
            _onSelectionCallback = callback;
            var viewModel = Resources["ViewModel"] as TabCompletionViewModel;
            if (viewModel != null) viewModel.BaseCollection = history;
        }

        public void Opened(object sender, EventArgs e)
        {
            searchTb.Focus();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Key.Enter != e.Key) return;
            {
                var selectedItem = searchTb.ListBox.SelectedItem as string;
                _onSelectionCallback(selectedItem);
                e.Handled = true;
            }
        }
    }
}

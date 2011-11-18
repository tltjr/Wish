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
        private readonly ViewModelDictionary _viewModelDictionary = new ViewModelDictionary();
        private readonly SearchBoxViewModel _viewModel;
        private readonly Action<string> _onSelected;

        public SearchBox(SearchType type, IEnumerable<string> uniqueList, Action<string> callback)
        {
            InitializeComponent();
            _onSelected = callback;
            _viewModel = _viewModelDictionary[type];
            _viewModel.BaseCollection = uniqueList;
            DataContext = _viewModel;
        }

        public void Opened(object sender, EventArgs e)
        {
            searchTb.Focus();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Key.Enter != e.Key) return;
            var selected = searchTb.ListBox.SelectedItem as string;
            _viewModel.HandleEnter(e, _onSelected, selected);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tltjr.Core;
using Wish.Core;

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private readonly OnSelection _onSelection;

        public delegate string OnSelection(Command command);

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
            //_viewModel = Resources["ViewModel"] as ActbViewModel;
            if (Key.Enter != e.Key) return;
            {
                var selectedItem = searchTb.ListBox.SelectedItem as string;
                _onSelection(selectedItem.ParseCommandLine());
                e.Handled = true;
            }
        }
    }
}

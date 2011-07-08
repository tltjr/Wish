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
using Wish.Core;

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private ActbViewModel _viewModel;
        private CommandEngine _commandEngine;

        public SearchBox(CommandEngine commandEngine)
        {
            InitializeComponent();
            _commandEngine = commandEngine;
        }

        public void Opened(object sender, EventArgs e)
        {
            searchTb.Focus();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            _viewModel = Resources["ViewModel"] as ActbViewModel;
            if (null == _viewModel) return;
            _viewModel.HandleKeyDown(e, searchTb.ListBox.SelectedItem);
        }
    }
}

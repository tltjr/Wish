using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Wish.ViewModels;

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for WishView.xaml
    /// </summary>
    public partial class WishView : UserControl
    {
        private IEventAggregator _eventAggregator;
        private IRegion _mainRegion;

        public WishView(IRegion mainRegion, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            DataContext = new TerminalViewModel();
            _mainRegion = mainRegion;
            _eventAggregator = eventAggregator;
        }

        private void ScrollToEnd(object sender, TextChangedEventArgs e)
        {
            textBox.ScrollToEnd();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textBox);
        }
    }
}

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

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private Popup _popup;

        public SearchBox(Popup popup)
        {
            InitializeComponent();
            _popup = popup;
        }

        public new void LostFocus(object sender, EventArgs e)
        {
            //if (searchTb.IsFocused || searchTb.ListBox.IsKeyboardFocused) return;
            //_popup.IsOpen = false;
        }

        public void Opened(object sender, EventArgs e)
        {
            searchTb.Focus();
        }
    }
}

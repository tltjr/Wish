using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Regions;

namespace Wish.Menu.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        private readonly IRegion _menuRegion;

        public MenuView(IRegion region)
        {
            InitializeComponent();
            _menuRegion = region;
        }

        private void CmdSelected(object sender, RoutedEventArgs e)
        {
            //pshell.IsChecked = false;
            //vsPrompt.IsChecked = false;
            //cmd.IsChecked = true;
            //_wishModel.SetRunner(new Cmd(), Title);
            //Execute();
        }

        private void PowershellSelected(object sender, RoutedEventArgs e)
        {
            //cmd.IsChecked = false;
            //vsPrompt.IsChecked = false;
            //pshell.IsChecked = true;
            //_wishModel.SetRunner(new Powershell(), Title);
            //Execute();
        }

        private void VsSelected(object sender, RoutedEventArgs e)
        {
            //cmd.IsChecked = false;
            //pshell.IsChecked = false;
            //vsPrompt.IsChecked = true;
            //_wishModel.SetRunner(new Powershell(), Title);
            throw new NotImplementedException();
        }

        private void NewTab(object sender, RoutedEventArgs e)
        {
            //ExecuteNewTab(sender, null);
        }
    }
}

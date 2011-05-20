using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Wish.Core;
using Wish.Models;
using Wish.Views;

namespace Wish.ViewModels
{
    public class TerminalViewModel
    {
        private readonly Models.Terminal _terminal;

        public Models.Terminal Terminal
        {
            get { return _terminal; }
        }

        public ICommand CommandEntered { get; set; }

        public TerminalViewModel(IRegion region, WishView view, string workingDirectory)
        {
            _terminal = new Models.Terminal(region, view, workingDirectory);
            CommandEntered = new DelegateCommand(ProcessCommand);
        }

        private void ProcessCommand()
        {
            _terminal.ProcessCommand();
        }

    }

}

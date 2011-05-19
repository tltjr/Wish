using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
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

        public TerminalViewModel()
        {
            _terminal = new Models.Terminal();
            CommandEntered = new DelegateCommand(ProcessCommand);
        }

        private void ProcessCommand()
        {
            _terminal.ProcessCommand();
        }

    }

}

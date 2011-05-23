using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CodeBoxControl;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
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

        public TerminalViewModel(IRegion mainRegion, WishView wishView, CodeBox textBox, string workingDirectory)
        {
            _terminal = new Models.Terminal(mainRegion, wishView, textBox, workingDirectory);
            CommandEntered = new DelegateCommand(ProcessCommand);
        }

        private void ProcessCommand()
        {
            _terminal.ProcessCommand();
        }
    }
}

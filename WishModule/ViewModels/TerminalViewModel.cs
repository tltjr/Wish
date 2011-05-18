using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Wish.Models;

namespace Wish.ViewModels
{
    public class TerminalViewModel
    {
        private readonly Terminal _terminal;

        public Terminal Terminal
        {
            get { return _terminal; }
        }

        public ICommand CommandEntered { get; set; }

        public string Name { get { return _terminal.Name ; } }

        public string FontFamily { get { return _terminal.FontFamily; } }

        public string Text { get { return _terminal.Text; } }

        public TerminalViewModel()
        {
            _terminal = new Terminal();
            CommandEntered = new DelegateCommand<object>(ProcessCommand);
        }

        private void ProcessCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Wish.Core;
using Wish.Models;

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
            CommandEntered = new DelegateCommand<object>(ProcessCommand);
        }

        private void ProcessCommand(object obj)
        {
            var powershellController = new PowershellController();
            var script = _terminal.ParseScript();
            var output = powershellController.RunScriptForFormattedResult(script);
            _terminal.InsertNewPrompt();
            _terminal.InsertLineBeforePrompt(output);
        }
    }

}

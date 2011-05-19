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
            CommandEntered = new DelegateCommand<object>(ProcessCommand);
        }

        private void ProcessCommand(object obj)
        {
            var powershellController = new PowershellController();
            var command = _terminal.ParseScript();
            var isDirChange = _terminal.GetCommandType(command.Name);
            if(isDirChange)
            {
                powershellController.RunScript(command.Raw);
                var results = powershellController.RunScriptForResult("pwd");
                if(results.Count == 0) return;
                var pwd = results[0];
                _terminal.Prompt = pwd + ">";
                _terminal.InsertNewPrompt();
                _terminal.InsertLineBeforePrompt("\n");
            }
            else
            {
                var output = powershellController.RunScriptForFormattedResult(command.Raw);
                _terminal.InsertNewPrompt();
                _terminal.InsertLineBeforePrompt(output);
            }
        }

    }

}

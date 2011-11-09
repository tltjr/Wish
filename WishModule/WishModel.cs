using System;
using System.Configuration;
using System.Windows.Input;
using Wish.Commands;
using Wish.Commands.Runner;
using Wish.Common;
using Wish.Scripts;

namespace Wish
{
    public class WishModel
    {
        private readonly IRepl _repl;
        private readonly IRunner _runner;
        private string _workingDirectory;

        public WishModel(IRepl repl)
        {
            _repl = repl;
            _runner = new Powershell();
        }
        
        public WishModel(IRepl repl, IRunner runner)
        {
            _repl = repl;
            _runner = runner;
        }

        public CommandResult Raise(Key key, string text)
        {
            if (key.Equals(Key.Enter))
            {
                var result = _repl.Loop(_runner, text);
                if (result.WorkingDirectory != null && _workingDirectory != result.WorkingDirectory)
                {
                    _workingDirectory = result.WorkingDirectory;
                }
                else
                {
                    result.WorkingDirectory = _workingDirectory;
                }
                return result;
            }
            return new CommandResult { Handled = false, IsExit = false, Text = string.Empty, Error = "massive fail" };
        }

        public CommandResult Start()
        {
            _workingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
            ChangeDirectory();
            var result = _repl.Start();
            result.WorkingDirectory = _workingDirectory ?? Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            return result;
        }

        private void ChangeDirectory()
        {
            if(null != _workingDirectory)
            {
                _repl.Eval(new Command("cd " + _workingDirectory));
            }
        }
    }
}
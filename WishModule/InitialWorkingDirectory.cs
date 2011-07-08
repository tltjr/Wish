using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wish.Core;

namespace Wish
{
    public class InitialWorkingDirectory
    {
        private readonly CommandEngine _commandEngine;

        public InitialWorkingDirectory(CommandEngine commandEngine)
        {
            _commandEngine = commandEngine;
        }

        public void Set(string workingDirectory)
        {
            try
            {
                _commandEngine.ProcessCommand(new Command("cd " + workingDirectory, "cd", new[] {workingDirectory}));
            }
            catch (Exception e)
            {
                throw new Exception("Invalid working directory:\t" + e.StackTrace);
            }
            
        }
    }
}

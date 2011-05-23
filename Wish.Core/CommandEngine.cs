using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using Terminal;

namespace Wish.Core
{
    public class CommandEngine
    {
        private readonly PowershellController _powershellController = new PowershellController();

        public string ChangeDirectory(string target)
        {
            _powershellController.RunScript(target);
            var results = _powershellController.RunScriptForResult("pwd");
            if (results.Count == 0) throw new Exception();
            return results[0].ToString();
        }

        public string ProcessCommand(Command command)
        {
            var isDirChange = IsDirectoryChange(command.Name);
            if(isDirChange)
            {
                var comm = command.Raw;
                if(IsPromptBasedChange(command.Name))
                {
                    comm = comm.Insert(0, "cd ");
                    comm += "\\";
                }
                ChangeDirectory(comm);
                return "\n";
            }
            return _powershellController.RunScriptForFormattedResult(command.Raw);
        }

        private bool IsDirectoryChange(string name)
        {
            return Regex.IsMatch(name, @"^(c|pop|push)d$")
                   || Regex.IsMatch(name, @"^cd\\$")
                   || Regex.IsMatch(name, @"^[A-Za-z]:$");
        }

        private bool IsPromptBasedChange(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z]:$");
        }

    }
}

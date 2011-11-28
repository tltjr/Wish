using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wish.Input
{
    public class ReservedCommands
    {
        private static readonly List<string> ReservedCommandList = new List<string> { @"^git push*", @"mongo*" };
       
        public bool IsReservedCommand(string command)
        {
            return ReservedCommandList.Any(pattern => Regex.IsMatch(command, pattern, RegexOptions.IgnoreCase));
        }

        public void Execute(string text, string workingDirectory)
        {
            var psi = new ProcessStartInfo("cmd.exe")
                          {
                              Arguments = string.Format("/k \"{0}\"", text),
                              CreateNoWindow = false,
                              UseShellExecute = true,
                              WorkingDirectory = workingDirectory
                          };
            Process.Start(psi);
        }
    }
}

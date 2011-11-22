using System.Collections.Generic;
using System.Text.RegularExpressions;
using Wish.Common;

namespace Wish.Input
{
    public class ReservedCommands
    {
        private static readonly List<string> ReservedCommandList = new List<string> { @"^git push*" };
       
        public bool IsReservedCommand(string command)
        {
            foreach (var pattern in ReservedCommandList)
            {
                if (Regex.IsMatch(command, pattern, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public CommandResult Execute(string text)
        {
            return null;
        }
    }
}

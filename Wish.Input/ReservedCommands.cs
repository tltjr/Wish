﻿using System.Collections.Generic;
using System.Diagnostics;
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

        public void Execute(string text, string workingDirectory)
        {
            var psi = new ProcessStartInfo("cmd.exe");
            psi.Arguments = string.Format("/k \"{0}\"", text);
            psi.CreateNoWindow = false;
            psi.UseShellExecute = true;
            psi.WorkingDirectory = workingDirectory;
            Process.Start(psi);
        }
    }
}
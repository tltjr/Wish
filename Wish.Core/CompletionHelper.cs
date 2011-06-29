using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wish.Core
{
    public class CompletionHelper
    {
        public string GetSearchString(string arg, string workingDirectory)
        {
            if (Regex.IsMatch(workingDirectory, @"\w:\\$")) 
                return workingDirectory + arg;
            return workingDirectory + "\\" + arg;
        }

        public IEnumerable<string> GetDirectories(string arg, string workingDirectory)
        {
            if (arg.Contains("\\"))
            {
                var dirsInArg = arg.Split('\\');
                var dirsToJoin = dirsInArg.Take(dirsInArg.Count() - 1);
                var dirString = String.Join("\\", dirsToJoin);
                if(Regex.IsMatch(workingDirectory, @"\w:\\$"))
                {
                    return Directory.GetDirectories(workingDirectory + dirString);
                }
                return Directory.GetDirectories(workingDirectory + "\\" + dirString);
            }
            return Directory.GetDirectories(workingDirectory);
        }

        public IEnumerable<string> GetDirectoryNames(string workingDirectory, IEnumerable<string> matches)
        {
            if(Regex.IsMatch(workingDirectory, @"\w:\\$"))
            {
                return matches.Select(match => match.Replace(workingDirectory, ""));
            }
            return matches.Select(match => match.Replace(workingDirectory + "\\", ""));
        }
    }
}

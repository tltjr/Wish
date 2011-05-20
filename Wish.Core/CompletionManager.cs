using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Terminal;

namespace Wish.Core
{
    public class CompletionManager
    {
        private IList<string> _currentTabDirs;
        private int _currentTabIndex;
        private string _currentTabOption;

        public bool Complete(out string result, Command command, bool activelyTabbing, string workingDirectory, string text)
        {
            return activelyTabbing ? GetNextInCurrentTabList(out result, text) 
                : PopulateTabListAndReturnFirst(out result, command, workingDirectory, text);
        }

        private bool PopulateTabListAndReturnFirst(out string result, Command command, string workingDirectory, string text)
        {
            result = String.Empty;
            _currentTabIndex = 0;
            if (command.Args.Count() == 0) return false;
            var arg = command.Args[0];
            var directories = GetDirectories(arg, workingDirectory);
            var searchString = GetSearchString(arg, workingDirectory);
            var matches =
                directories.Where(o => o.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
            _currentTabDirs = GetDirectoryNames(workingDirectory, matches).ToList();
            if (!TryGetCurrentTabOption()) return false;
            var baseText = text.Remove(text.Length - arg.Length);
            result = baseText + _currentTabOption + "\\";
            return true;
        }

        private bool GetNextInCurrentTabList(out string result, string text)
        {
            var length = _currentTabDirs[_currentTabIndex].Length + 1;
            _currentTabIndex++;
            if(_currentTabIndex >= _currentTabDirs.Count)
            {
                _currentTabIndex = 0;
            }
            result =  _currentTabDirs[_currentTabIndex];
            var baseText = text.Remove(text.Length - length);
            result = baseText + result + "\\";
            return true;
        }

        private bool TryGetCurrentTabOption()
        {
            if(_currentTabIndex < _currentTabDirs.Count)
            {
                _currentTabOption = _currentTabDirs[_currentTabIndex];
                return true;
            }
            return false;
        }

        private string GetSearchString(string arg, string workingDirectory)
        {
            if (Regex.IsMatch(workingDirectory, @"\w:\\$")) 
                return workingDirectory + arg;
            return workingDirectory + "\\" + arg;
        }

        private IEnumerable<string> GetDirectories(string arg, string workingDirectory)
        {
            if (arg.Contains("\\"))
            {
                var dirsInArg = arg.Split('\\');
                var dirsToJoin = dirsInArg.Take(dirsInArg.Count() - 1);
                var dirString = String.Join("\\", dirsToJoin);
                return Directory.GetDirectories(workingDirectory + "\\" + dirString);
            }
            return Regex.IsMatch(workingDirectory, @"\w:$") ? Directory.GetDirectories(workingDirectory + "\\") : Directory.GetDirectories(workingDirectory);
        }

        private IEnumerable<string> GetDirectoryNames(string workingDirectory, IEnumerable<string> matches)
        {
            return matches.Select(match => match.Replace(workingDirectory + "\\", ""));
        }

    }
}

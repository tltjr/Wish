using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Terminal;

namespace WishModule
{
    public class CompletionManager
    {
        private IList<string> _currentTabDirs;
        private int _currentTabIndex;
        private string _baseTabText;
        private string _currentTabOption;

        public bool Complete(out string result, Command command, bool activelyTabbing, string workingDirectory, string currentText)
        {
            result = String.Empty;
            if (!activelyTabbing)
            {
                if (command.Args.Count() == 0) return false;
                var arg = command.Args[0];
                _currentTabIndex = 0;
                var trimmedWorkingDir = ParseEndingSlash(workingDirectory);
                var directories = GetDirectories(arg, trimmedWorkingDir);
                var searchString = GetSearchString(arg, trimmedWorkingDir);
                var matches = directories.Where(o => o.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
                _currentTabDirs = GetDirectoryNames(trimmedWorkingDir, matches).ToList();
                if (!TryGetCurrentTabOption()) return false;
                SetTabIndex();
                _baseTabText = currentText.Remove(currentText.Length - arg.Length);
                result = _baseTabText + _currentTabOption + "\\";
                return true;
            }
            if (!TryGetCurrentTabOption()) return false;
            SetTabIndex();
            result = _baseTabText + _currentTabOption + "\\";
            return true;
        }

        private string ParseEndingSlash(string workingDirectory)
        {
            if(workingDirectory.EndsWith("\\") || workingDirectory.EndsWith("/"))
            {
                return workingDirectory.Substring(0, workingDirectory.Length - 1);
            }
            return workingDirectory;
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

        private void SetTabIndex()
        {
            if (_currentTabIndex < _currentTabDirs.Count - 1)
            {
                _currentTabIndex++;
            }
            else
            {
                _currentTabIndex = 0;
            }
        }

        private IEnumerable<string> GetDirectoryNames(string workingDirectory, IEnumerable<string> matches)
        {
 //           if (Regex.IsMatch(workingDirectory, @"\w:\\$"))
  //              return matches.Select(match => match.Replace(workingDirectory, ""));
            return matches.Select(match => match.Replace(workingDirectory + "\\", ""));
        }
    }
}

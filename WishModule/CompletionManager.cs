using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terminal;

namespace WishModule
{
    public class CompletionManager
    {
        private IList<string> _currentTabDirs;
        private int _currentTabIndex;
        private string _baseTabText;
        private string _currentTabOption;

        public string Complete(Command command, bool activelyTabbing, string workingDirectory, string currentText)
        {
            if (!activelyTabbing)
            {
                if (command.Args.Count() == 0) return "";
                var arg = command.Args[0];
                string[] directories;
                if (arg.Contains("\\"))
                {
                    var dirsInArg = arg.Split('\\');
                    var dirsToJoin = dirsInArg.Take(dirsInArg.Count() - 1);
                    var dirString = String.Join("\\", dirsToJoin);
                    directories = Directory.GetDirectories(workingDirectory + "\\" + dirString);
                }
                else
                {
                    directories = Directory.GetDirectories(workingDirectory);
                }
                var searchString = workingDirectory == "C:\\" 
                    ? workingDirectory + arg : 
                    workingDirectory + "\\" + arg;
                var matches = directories.Where(o => o.StartsWith(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
                _currentTabDirs = GetDirectoryNames(workingDirectory, matches).ToList();
                var length = arg.Length;
                _baseTabText = currentText.Remove(currentText.Length - length);
                _currentTabOption = _currentTabDirs[_currentTabIndex];
                if (_currentTabIndex < _currentTabDirs.Count - 1)
                {
                    _currentTabIndex++;
                }
                else
                {
                    _currentTabIndex = 0;
                }
                return _baseTabText + _currentTabOption + "\\";
            }
            _currentTabOption = _currentTabDirs[_currentTabIndex];
            if (_currentTabIndex < _currentTabDirs.Count - 1)
            {
                _currentTabIndex++;
            }
            else
            {
                _currentTabIndex = 0;
            }
            return _baseTabText + _currentTabOption + "\\";
        }

        private IEnumerable<string> GetDirectoryNames(string workingDirectory, IEnumerable<string> matches)
        {
            return matches.Select(match => match.Replace(workingDirectory + "\\", ""));
        }
    }
}

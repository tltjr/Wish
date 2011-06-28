using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Wish.Core
{
    public class CompletionManager
    {
        private CompletionWindow _completionWindow;
        private IList<ICompletionData> _completionData;

        public void CreateWindow(TextArea textArea, string arg, string workingDirectory)
        {
            _completionWindow = new CompletionWindow(textArea);
            PopulateCompletionList(arg, workingDirectory);
            _completionWindow.Show();
            _completionWindow.Closed += delegate
            {
                _completionWindow = null;
            };
            if(_completionData != null && _completionData.Count > 0)
            {
                _completionWindow.CompletionList.SelectedItem = _completionData[0];
            }
        }

        private void PopulateCompletionList(string arg, string workingDirectory)
        {
            var directories = GetDirectories(arg, workingDirectory);
            var searchString = GetSearchString(arg, workingDirectory);
            var matches =
                directories.Where(o => o.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
            var names = GetDirectoryNames(workingDirectory, matches).ToList();
            _completionData = _completionWindow.CompletionList.CompletionData;
            foreach (var name in names)
            {
                _completionData.Add(new TabCompletionData(name, arg));
            }
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
                if(Regex.IsMatch(workingDirectory, @"\w:\\$"))
                {
                    return Directory.GetDirectories(workingDirectory + dirString);
                }
                return Directory.GetDirectories(workingDirectory + "\\" + dirString);
            }
            return Directory.GetDirectories(workingDirectory);
        }

        private IEnumerable<string> GetDirectoryNames(string workingDirectory, IEnumerable<string> matches)
        {
            if(Regex.IsMatch(workingDirectory, @"\w:\\$"))
            {
                return matches.Select(match => match.Replace(workingDirectory, ""));
            }
            return matches.Select(match => match.Replace(workingDirectory + "\\", ""));
        }

    }

    internal class TabCompletionData : ICompletionData
    {
        public TabCompletionData(string text, string textToReplace)
        {
            Text = text;
            Replace = textToReplace;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; private set; }
        public string Replace { get; private set; }

        // Use this property if you want to show a fancy UIElement in the drop down list.
        public object Content
        {
            get { return Text; }
        }

        public object Description
        {
            get { return "Description for " + Text; }
        }

        public double Priority { get { return 0; } }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
			//Replace(segment.Offset, segment.Length, text, null);
            var offset = completionSegment.Offset - Replace.Length; // offset - length of the string to be replaced
            var length = Replace.Length;
            textArea.Document.Replace(offset, length, Text);
            //textArea.Document.Replace(completionSegment, Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using Wish.Core;

namespace GuiHelpers
{
    public class CompletionManager
    {
        private CompletionWindow _completionWindow;
        private IList<ICompletionData> _completionData;
        private readonly CompletionHelper _completionHelper = new CompletionHelper();
        private List<string> _matches;

        public delegate void OnCloseDelegate();

        public bool CreateWindow(TextArea textArea, string[] args, string workingDirectory, OnCloseDelegate onCloseDelegate)
        {
            _completionWindow = new CompletionWindow(textArea)
                                    {
                                        SizeToContent = SizeToContent.WidthAndHeight,
                                        MinWidth = 150
                                    };
            PopulateCompletionList(args.Last(), workingDirectory);
            if (_matches.Count == 0) return false;
            _completionWindow.Show();
            _completionWindow.Closed += delegate
                                            {
                                                _completionWindow = null;
                                                onCloseDelegate.Invoke();
                                            };
            if (_completionData != null && _completionData.Count > 0)
            {
                _completionWindow.CompletionList.SelectedItem = _completionData[0];
            }
            return true;
        }

        private void PopulateCompletionList(string arg, string workingDirectory)
        {
            var directories = _completionHelper.GetDirectories(arg, workingDirectory);
            var searchString = _completionHelper.GetSearchString(arg, workingDirectory);
            _matches =
                directories.Where(o => o.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (0 == _matches.Count) return;
            var names = _completionHelper.GetDirectoryNames(workingDirectory, _matches).ToList();
            _completionData = _completionWindow.CompletionList.CompletionData;
            foreach (var name in names)
            {
                _completionData.Add(new TabCompletionData(name, arg));
            }
        }
    }
}
using System;
using System.Windows.Input;
using GuiHelpers;
using ICSharpCode.AvalonEdit;
using Microsoft.Practices.Prism.Regions;
using Wish.Core;
using Wish.Views;

namespace Wish
{
    public class WishModel
    {
        public TextEditor TextEditor;
        public IRegion Region;
        public WishView View;
        public string WorkingDirectory { get; set; }

        public IState Standard { get; set; }
        public IState Complete { get; set; }
        private IState _state;

        public IState State { 
            get { return _state; } 
            set
            {
                if(value != Complete)
                {
                    CommandHistory.Reset();
                }
                _state = value;
            } 
        }

        public int LastPromptIndex { get; set; }

        public readonly CommandEngine CommandEngine = new CommandEngine();
        public readonly TextTransformations TextTransformations = new TextTransformations();
        public readonly CommandHistory CommandHistory = new CommandHistory();
        public readonly SyntaxHighlighting SyntaxHighlighting = new SyntaxHighlighting();

        public WishModel(TextEditor textEditor, IRegion region, WishView view, string workingDirectory)
        {
            TextEditor = textEditor;
            Region = region;
            View = view;
            WorkingDirectory = workingDirectory;
            LastPromptIndex = -1;
            Standard = new StandardState(this);
            Complete = new CompleteState(this);
            _state = Standard;
            SyntaxHighlighting.SetSyntaxHighlighting(typeof(WishView), textEditor);
            TextTransformations.CreatePrompt(WorkingDirectory);
            LastPromptIndex = TextTransformations.InsertNewPrompt(textEditor);
            SetInitialWorkingDirectory();
        }

        private void SetInitialWorkingDirectory()
        {
            try
            {
                CommandEngine.ProcessCommand(new Command("cd " + WorkingDirectory, "cd", new[] {WorkingDirectory}));
                WorkingDirectory = CommandEngine.WorkingDirectory;
            }
            catch (Exception e)
            {
                throw new Exception("Invalid working directory:\t" + e.StackTrace);
            }
        }

        public void KeyPress(KeyEventArgs e)
        {
            _state.KeyPress(e);
        }

        public void RequestHistorySearch()
        {
            _state.RequestHistorySearch();
        }
    }
}

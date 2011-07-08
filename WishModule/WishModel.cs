using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
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
        public string WorkingDirectory { get; set; }

        public TextEditor TextEditor { get; set; }
        public bool ActivelyTabbing { get; set; }

        public TextTransformations TextTransformations { get; set; }
        public CommandEngine CommandEngine { get; set; }
        private readonly SyntaxHighlighting _syntaxHighlighting = new SyntaxHighlighting();
        private readonly InitialWorkingDirectory _iwd;

        private readonly IDictionary<Key, IKeyStrategy> _strategies = new Dictionary<Key, IKeyStrategy>();
        private Popup _popup;

        public WishModel(TextEditor textEditor, TextTransformations textTransformations, string workingDirectory)
        {
            TextEditor = textEditor;
            TextTransformations = textTransformations;
            WorkingDirectory = workingDirectory;
            _syntaxHighlighting.SetSyntaxHighlighting(typeof(WishView), textEditor);
            TextTransformations.CreatePrompt(workingDirectory);
            TextTransformations.InsertNewPrompt(textEditor);
            CommandEngine = new CommandEngine();
            _iwd = new InitialWorkingDirectory(CommandEngine);
            _iwd.Set(workingDirectory);
            BootstrapKeyStrategies();
        }

        private void BootstrapKeyStrategies()
        {
            _strategies.Add(Key.Up, new UpStrategy(this));
            _strategies.Add(Key.Down, new DownStrategy(this));
            _strategies.Add(Key.Tab, new TabStrategy(this));
            _strategies.Add(Key.Enter, new EnterStrategy(this));
        }

        public string KeyPress(KeyEventArgs e)
        {
            IKeyStrategy strategy;
            if(_strategies.TryGetValue(e.Key, out strategy))
            {
                WorkingDirectory = strategy.Handle(e) ?? WorkingDirectory;
                return WorkingDirectory;
            }
            CommandHistory.Reset();
            return null;
        }

        public string RequestHistorySearch()
        {
            _popup = new Popup {IsOpen = false, PlacementTarget = TextEditor, Placement = PlacementMode.Center};
            var searchBox = new SearchBox(Execute);
            _popup.Opened += searchBox.Opened;
            _popup.Child = searchBox;
            _popup.IsOpen = true;
            _popup.StaysOpen = false;
            return WorkingDirectory;
        }

        private void EnsureCorrectCaretPosition()
        {
            var line = TextEditor.Document.GetLineByNumber(TextEditor.TextArea.Caret.Line);
            if (0 == line.Length)
            {
                TextEditor.TextArea.Caret.Line = TextEditor.TextArea.Caret.Line - 1;
            }
        }

        public string Execute(Command command)
        {
            CheckOpenPopup();
            CommandHistory.Add(command);
            TextEditor.Text += "\n";
            var output = CommandEngine.ProcessCommand(command);
            WorkingDirectory = CommandEngine.WorkingDirectory;
            TextTransformations.HandleResults(TextEditor, output, WorkingDirectory);
            Keyboard.Focus(TextEditor);
            EnsureCorrectCaretPosition();
            return WorkingDirectory;
        }

        private void CheckOpenPopup()
        {
            if(null != _popup)
            {
                _popup.IsOpen = false;
            }
        }
    }
}

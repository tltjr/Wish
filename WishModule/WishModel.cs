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
        public WishView View;

        private readonly IRegion _region;

        public int LastPromptIndex { get; set; }
        public bool ActivelyTabbing { get; set; }

        private readonly TextTransformations _textTransformations = new TextTransformations();
        public CommandEngine CommandEngine { get; set; }
        private readonly SyntaxHighlighting _syntaxHighlighting = new SyntaxHighlighting();
        private readonly InitialWorkingDirectory _iwd;

        private readonly IDictionary<Key, IKeyStrategy> _strategies = new Dictionary<Key, IKeyStrategy>();

        public WishModel(TextEditor textEditor, IRegion region, WishView view, string workingDirectory)
        {
            TextEditor = textEditor;
            _region = region;
            View = view;
            WorkingDirectory = workingDirectory;
            LastPromptIndex = -1;
            _syntaxHighlighting.SetSyntaxHighlighting(typeof(WishView), textEditor);
            _textTransformations.CreatePrompt(WorkingDirectory);
            LastPromptIndex = _textTransformations.InsertNewPrompt(textEditor);
            CommandEngine = new CommandEngine();
            _iwd = new InitialWorkingDirectory(CommandEngine);
            WorkingDirectory = _iwd.Set(WorkingDirectory);
            BootstrapKeyStrategies();
        }

        private void BootstrapKeyStrategies()
        {
            _strategies.Add(Key.Up, new UpStrategy(this));
            _strategies.Add(Key.Down, new DownStrategy(this));
            _strategies.Add(Key.Tab, new TabStrategy(this));
            _strategies.Add(Key.Enter, new EnterStrategy(this));
        }

        public void KeyPress(KeyEventArgs e)
        {
            IKeyStrategy strategy;
            if(_strategies.TryGetValue(e.Key, out strategy))
            {
                strategy.Handle(e);
            }
            else
            {
                CommandHistory.Reset();
            }
        }

        public void RequestHistorySearch()
        {
            var popup = new Popup {IsOpen = false, PlacementTarget = TextEditor, Placement = PlacementMode.Center};
            var searchBox = new SearchBox();
            popup.Opened += searchBox.Opened;
            popup.Child = searchBox;
            popup.IsOpen = true;
            popup.StaysOpen = false;
        }


        public bool IsExit(Command command)
        {
            if (command.Name.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                var i = _region.Views.Count();
                if (i > 1)
                {
                    _region.Remove(View);
                }
                else
                {
                    System.Windows.Application.Current.Shutdown();
                }
                return true;
            }
            return false;
        }

        public void ReplaceLine(Command command)
        {
            if (null == command) return;
            var raw = command.Raw;
            var text = TextEditor.Text;
            var line = TextEditor.Text.Substring(LastPromptIndex);
            if (!String.IsNullOrEmpty(line))
            {
                var baseText = text.Remove(text.Length - line.Length);
                TextEditor.Text = baseText + raw;
            }
            else
            {
                TextEditor.Text += raw;
            }
        }
    }
}

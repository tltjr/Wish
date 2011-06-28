using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using GuiHelpers;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Terminal;
using Wish.Core;

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for WishView.xaml
    /// </summary>
    public partial class WishView
    {

        private readonly IRegion _mainRegion;
        private readonly IEventAggregator _eventAggregator;
        public static RoutedCommand TabNew = new RoutedCommand();
        public static RoutedCommand TabNext = new RoutedCommand();
        public static RoutedCommand TabPrevious = new RoutedCommand();
        private bool _activelyTabbing;
        private readonly CommandEngine _commandEngine = new CommandEngine();
        private readonly TextTransformations _textTransformations = new TextTransformations();
        private readonly CommandHistory _commandHistory = new CommandHistory();
        private CompletionManager _completionManager;
        private readonly TabManager _tabManager = TabManager.Instance();

        private string _workingDirectory;

        public int LastPromptIndex { get; private set; }

        public WishView(IRegion mainRegion, IEventAggregator eventAggregator, string workingDirectory)
        {
            InitializeComponent();
            _mainRegion = mainRegion;
            _eventAggregator = eventAggregator;
            _workingDirectory = workingDirectory;
            LastPromptIndex = -1;

            SetInputGestures();
            SetInitialWorkingDirectory();
            SetSyntaxHighlighting();

            _textTransformations.CreatePrompt(_workingDirectory);
            LastPromptIndex = _textTransformations.InsertNewPrompt(textEditor);
            _tabManager.Add(this);
            //_mainRegion.Activate(this);

            textEditor.TextArea.TextEntering += TextEditorTextAreaTextEntering;
            //textEditor.TextArea.TextEntered += TextEditorTextAreaTextEntered;
        }

        private void SetSyntaxHighlighting()
        {
            IHighlightingDefinition customHighlighting;
            using (Stream s = typeof(WishView).Assembly.GetManifestResourceStream("Wish.Views.CustomHighlighting.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", null, customHighlighting);
            textEditor.SyntaxHighlighting = customHighlighting;
        }

        private void SetInitialWorkingDirectory()
        {
            try
            {
                _commandEngine.ChangeDirectory("cd " + _workingDirectory);
                _workingDirectory = _commandEngine.WorkingDirectory;
            }
            catch (Exception e)
            {
                throw new Exception("Invalid working directory:\t" + e.StackTrace);
            }
        }

        private void SetInputGestures()
        {
            var cntrlShftT = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
            TabNew.InputGestures.Add(cntrlShftT);

            var controlT = new KeyGesture(Key.T, ModifierKeys.Control);
            TabNew.InputGestures.Add(controlT);

            //var controlPageDown = new KeyGesture(Key.PageDown, ModifierKeys.Control);
            //TabNext.InputGestures.Add(controlPageDown);

            //var controlTab = new KeyGesture(Key.Tab, ModifierKeys.Control);
            //TabNext.InputGestures.Add(controlTab);

            //var cntrlTab = new KeyGesture(Key.Tab, ModifierKeys.Control);
            //TabNext.InputGestures.Add(cntrlTab);

            //var cntrlPageUp = new KeyGesture(Key.PageUp, ModifierKeys.Control);
            //TabPrevious.InputGestures.Add(cntrlPageUp);

            //KeyGesture keyGesture = new KeyGesture(Key.PageDown, ModifierKeys.Control);

            //KeyBinding keyBinding = new KeyBinding(TabNext, keyGesture);

            //InputBindings.Add(keyBinding);

        }


        private void ScrollToEnd(object sender, EventArgs eventArgs)
        {
            textEditor.ScrollToEnd();
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textEditor);
            Title = _workingDirectory;
        }


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            foreach (InputBinding inputBinding in this.InputBindings)
            {
                KeyGesture keyGesture = inputBinding.Gesture as KeyGesture;
                if (keyGesture != null && keyGesture.Key == e.Key && keyGesture.Modifiers == Keyboard.Modifiers)
                {
                    if (inputBinding.Command != null)
                    {
                        inputBinding.Command.Execute(0);
                        e.Handled = true;
                    }
                }
            }

            foreach (CommandBinding cb in this.CommandBindings)
            {
                RoutedCommand command = cb.Command as RoutedCommand;
                if (command != null)
                {
                    foreach (InputGesture inputGesture in command.InputGestures)
                    {
                        KeyGesture keyGesture = inputGesture as KeyGesture;
                        if (keyGesture != null && keyGesture.Key == e.Key && keyGesture.Modifiers == Keyboard.Modifiers)
                        {
                            command.Execute(0, this);
                            e.Handled = true;
                            break;
                        }
                    }
                }
            }
            switch (e.Key)
            {
                case Key.Up:
                    {
                        var command = _commandHistory.GetNext();
                        ReplaceLine(command);
                    }
                    break;
                case Key.Down:
                    {
                        var command = _commandHistory.GetPrevious();
                        ReplaceLine(command);
                    }
                    break;
                case Key.Tab:
                    {
                        //if (textEditor.CaretIndex != textEditor.Text.Length || textEditor.CaretIndex == LastPromptIndex)
                        //    return;
                        if (!_activelyTabbing)
                        {
                            var line = textEditor.Text.Substring(LastPromptIndex);
                            var command = TerminalUtils.ParseCommandLine(line);
                            var arg = command.Args[0];
                            _completionManager = new CompletionManager();
                            _completionManager.CreateWindow(textEditor.TextArea, arg, _workingDirectory);
                            _activelyTabbing = true;
                            e.Handled = true;
                        }
                        _commandHistory.Reset();
                    }
                    break;
                case Key.Enter:
                    {
                        if(!_activelyTabbing)
                        {
                            var command = _textTransformations.ParseScript(textEditor.Text, LastPromptIndex);
                            _commandHistory.Add(command);
                            textEditor.Text += "\n";
                            if (!IsExit(command))
                            {
                                var output = _commandEngine.ProcessCommand(command);
                                _workingDirectory = _commandEngine.WorkingDirectory;
                                Title = _workingDirectory;
                                _textTransformations.CreatePrompt(_workingDirectory);
                                LastPromptIndex = _textTransformations.InsertNewPrompt(textEditor);
                                LastPromptIndex = _textTransformations.InsertLineBeforePrompt(textEditor, output, LastPromptIndex);
                            }
                            var line = textEditor.Document.GetLineByNumber(textEditor.TextArea.Caret.Line);
                            if (0 == line.Length)
                            {
                                textEditor.TextArea.Caret.Line = textEditor.TextArea.Caret.Line - 1;
                            }
                            e.Handled = true;
                        }
                        _activelyTabbing = false;
                        _commandHistory.Reset();
                    }
                    break;
                default:
                    base.OnPreviewKeyDown(e);
                    _commandHistory.Reset();
                    break;
            }
        }

        private void ReplaceLine(Command command)
        {
            if (null != command)
            {
                var raw = command.Raw;
                var text = textEditor.Text;
                var line = textEditor.Text.Substring(LastPromptIndex);
                if (!String.IsNullOrEmpty(line))
                {
                    var baseText = text.Remove(text.Length - line.Length);
                    textEditor.Text = baseText + raw;
                }
                else
                {
                    textEditor.Text += raw;
                }
            }
        }

        private bool IsExit(Command command)
        {
            if (command.Name.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                var i = _mainRegion.Views.Count();
                if (i > 1)
                {
                    _mainRegion.Remove(this);
                }
                else
                {
                    Application.Current.Shutdown();
                }
                return true;
            }
            return false;
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(WishView),
                new PropertyMetadata(@"amr\tlthorn1")
                );


        public string Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }

        private void ExecuteNewTab(object sender, ExecutedRoutedEventArgs e)
        {
            var view = new WishView(_mainRegion, _eventAggregator, _workingDirectory);
            _mainRegion.Add(view);
        }

        void TextEditorTextAreaTextEntering(object sender, TextCompositionEventArgs e)
        {
        }
    }

}

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using GuiHelpers;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
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
        private bool _activelyTabbing;
        private readonly CommandEngine _commandEngine = new CommandEngine();
        private readonly TextTransformations _textTransformations = new TextTransformations();
        private readonly CommandHistory _commandHistory = new CommandHistory();
        private readonly SyntaxHighlighting _syntaxHighlighting = new SyntaxHighlighting();
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
            _syntaxHighlighting.SetSyntaxHighlighting(typeof(WishView), textEditor);

            _textTransformations.CreatePrompt(_workingDirectory);
            LastPromptIndex = _textTransformations.InsertNewPrompt(textEditor);
            _tabManager.Add(this);
        }

        private void SetInitialWorkingDirectory()
        {
            try
            {
                _commandEngine.ProcessCommand(new Command("cd " + _workingDirectory, "cd", new[] {_workingDirectory}));
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
            foreach (var inputBinding in from InputBinding inputBinding in InputBindings
                                                  let keyGesture = inputBinding.Gesture as KeyGesture
                                                  where (keyGesture != null && keyGesture.Key == e.Key) && keyGesture.Modifiers == Keyboard.Modifiers
                                                  where inputBinding.Command != null
                                                  select inputBinding)
            {
                inputBinding.Command.Execute(0);
                e.Handled = true;
            }

            foreach (var command in
                (from CommandBinding cb in CommandBindings select cb.Command).OfType<RoutedCommand>().Where(command => (from InputGesture inputGesture in command.InputGestures select inputGesture as KeyGesture).Any(keyGesture => (keyGesture != null && keyGesture.Key == e.Key) && keyGesture.Modifiers == Keyboard.Modifiers)))
            {
                command.Execute(0, this);
                e.Handled = true;
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
                            var command = _textTransformations.ParseCommandLine(line);
                            _completionManager = new CompletionManager();
                            _completionManager.CreateWindow(textEditor.TextArea, command.Args, _workingDirectory);
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

    }
}

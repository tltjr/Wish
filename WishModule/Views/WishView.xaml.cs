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
        private bool _activelyTabbing;
        private readonly CompletionManager _completionManager = new CompletionManager();
        private readonly CommandEngine _commandEngine = new CommandEngine();
        private readonly TextTransformations _textTransformations = new TextTransformations();
        private readonly CommandHistory _commandHistory = new CommandHistory();

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
        }

        private void SetSyntaxHighlighting()
        {
            IHighlightingDefinition customHighlighting;
            using (Stream s = typeof(WishView).Assembly.GetManifestResourceStream("Wish.Views.CustomHighlighting.xshd")) {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s)) {
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
            catch(Exception e)
            {
                throw new Exception("Invalid working directory:\t" + e.StackTrace);
            }
        }

        private void SetInputGestures()
        {
            var keyGesture = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
            TabNew.InputGestures.Add(keyGesture);
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
            switch (e.Key)
            {
                case Key.Up:
                    {
                        var command = _commandHistory.GetNext();
                        ReplaceLine(command);
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    {
                        var command = _commandHistory.GetPrevious();
                        ReplaceLine(command);
                        e.Handled = true;
                    }
                    break;
                case Key.Tab:
                    {
                        //if (textEditor.CaretIndex != textEditor.Text.Length || textEditor.CaretIndex == LastPromptIndex)
                        //    return;
                        var line = textEditor.Text.Substring(LastPromptIndex);
                        var command = TerminalUtils.ParseCommandLine(line);
                        string result;
                        var flag = _completionManager.Complete(out result, command, _activelyTabbing, _workingDirectory, textEditor.Text);
                        if (flag)
                        {
                            _activelyTabbing = true;
                            textEditor.Text = result;
                        }
                        e.Handled = true;
                    }
                    break;
                case Key.Enter:
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
                        if(0 == line.Length)
                        {
                            textEditor.TextArea.Caret.Line = textEditor.TextArea.Caret.Line - 1;
                        }
                        e.Handled = true;
                    }
                    break;
                default:
                    base.OnPreviewKeyDown(e);
                    _activelyTabbing = false;
                    break;
            }
        }

        private void ReplaceLine(Command command)
        {
            if(null != command)
            {
                var raw = command.Raw;
                var text = textEditor.Text;
                var line = textEditor.Text.Substring(LastPromptIndex);
                if(!String.IsNullOrEmpty(line))
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
            if(command.Name.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                var i = _mainRegion.Views.Count();
                if(i > 1)
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

        private void NewTabRequest(object sender, ExecutedRoutedEventArgs e)
        {
            var view = new WishView(_mainRegion, _eventAggregator, _workingDirectory);
            _mainRegion.Add(view);
        }
    }
}

using System;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CodeBoxControl.Decorations;
using GuiHelpers;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Terminal;
using Wish.Core;

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for WishView.xaml
    /// </summary>
    public partial class WishView : UserControl
    {
        private IEventAggregator _eventAggregator;
        private IRegion _mainRegion;
        public static RoutedCommand TabNew = new RoutedCommand();
        private bool _activelyTabbing;
        private readonly CompletionManager _completionManager = new CompletionManager();
        private readonly CommandEngine _commandEngine = new CommandEngine();
        private readonly SyntaxHighlighter _syntaxHighlighter = new SyntaxHighlighter();
        private readonly TextTransformations _textTransformations = new TextTransformations();


        private string _workingDirectory;


        public int LastPromptIndex { get; private set; }

        public WishView(IRegion mainRegion, IEventAggregator eventAggregator, string workingDirectory)
        {
            InitializeComponent();
            _mainRegion = mainRegion;
            _eventAggregator = eventAggregator;
            var keyGesture = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
            TabNew.InputGestures.Add(keyGesture);


            _workingDirectory = workingDirectory;


            _syntaxHighlighter.SetSyntaxHighlighting(textBox);

			LastPromptIndex = -1;
            try
            {
                _workingDirectory = _commandEngine.ChangeDirectory("cd " + _workingDirectory);
            }
            catch(Exception e)
            {
                throw new Exception("Invalid working directory:\t" + e.StackTrace);
            }
            _textTransformations.CreatePrompt(_workingDirectory);

            LastPromptIndex = _textTransformations.InsertNewPrompt(textBox);
        }

            
        private void ScrollToEnd(object sender, TextChangedEventArgs e)
        {
            textBox.ScrollToEnd();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textBox);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    {
                        if (textBox.CaretIndex != textBox.Text.Length || textBox.CaretIndex == LastPromptIndex)
                            return;
                        var line = textBox.Text.Substring(LastPromptIndex);
                        var command = TerminalUtils.ParseCommandLine(line);
                        string result;
                        var flag = _completionManager.Complete(out result, command, _activelyTabbing, _workingDirectory, textBox.Text);
                        if (flag)
                        {
                            _activelyTabbing = true;
                            textBox.Text = result;
                        }
                        e.Handled = true;
                    }
                    break;
                case Key.Enter:
                    {
                        var command = _textTransformations.ParseScript(textBox.Text, LastPromptIndex);
                        textBox.Text += "\n";
                        if (!IsExit(command))
                        {
                            var output = _commandEngine.ProcessCommand(command);
                            LastPromptIndex = _textTransformations.InsertNewPrompt(textBox);
                            LastPromptIndex = _textTransformations.InsertLineBeforePrompt(textBox, output, LastPromptIndex);
                        }
                    }
                    break;
                default:
                    base.OnPreviewKeyDown(e);
                    _activelyTabbing = false;
                    break;
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

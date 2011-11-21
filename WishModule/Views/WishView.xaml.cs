using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Practices.Prism.Regions;
using Wish.Common;
using Wish.Scripts;

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for WishView.xaml
    /// </summary>
    public partial class WishView
    {
        private readonly IRegion _mainRegion;
        private readonly WishModel _wishModel;
        public static RoutedCommand TabNew = new RoutedCommand();
        public static RoutedCommand ControlR = new RoutedCommand();
        public static RoutedCommand ControlP = new RoutedCommand();
        public static RoutedCommand ControlN = new RoutedCommand();
        public static RoutedCommand ControlD = new RoutedCommand();
        public static RoutedCommand ControlA = new RoutedCommand();
        private int _promptLength;
        private State _state;

        public WishView(IRegion mainRegion)
        {
            InitializeComponent();
            SetInputGestures();
            SetSyntaxHighlighting();
            _mainRegion = mainRegion;
            _wishModel = new WishModel(new Repl());
            _state = State.Normal;
        }

        private static void SetInputGestures()
        {
            var cntrlShftT = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
            TabNew.InputGestures.Add(cntrlShftT);

            var controlT = new KeyGesture(Key.T, ModifierKeys.Control);
            TabNew.InputGestures.Add(controlT);

            var controlR = new KeyGesture(Key.R, ModifierKeys.Control);
            ControlR.InputGestures.Add(controlR);

            var controlP = new KeyGesture(Key.P, ModifierKeys.Control);
            ControlP.InputGestures.Add(controlP);

            var controlN = new KeyGesture(Key.N, ModifierKeys.Control);
            ControlN.InputGestures.Add(controlN);

            var controlD = new KeyGesture(Key.D, ModifierKeys.Control);
            ControlD.InputGestures.Add(controlD);

            var controlA = new KeyGesture(Key.A, ModifierKeys.Control);
            ControlA.InputGestures.Add(controlA);
        }

        private void ScrollToEnd(object sender, EventArgs eventArgs)
        {
            EnsureCorrectCaretPosition();
            textEditor.ScrollToEnd();
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textEditor);
            var result = _wishModel.Start();
            if (result.FullyProcessed) return;
            textEditor.Text = result.Text;
            Title = result.WorkingDirectory;
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
                (from CommandBinding cb in CommandBindings select cb.Command)
                    .OfType<RoutedCommand>()
                    .Where(command => (from InputGesture inputGesture in command.InputGestures select inputGesture as KeyGesture)
                                        .Any(keyGesture => (keyGesture != null && keyGesture.Key == e.Key) 
                                            && keyGesture.Modifiers == Keyboard.Modifiers)))
            {
                command.Execute(0, this);
                e.Handled = true;
            }
            if (_state.Equals(State.Tabbing))
            {
                if (e.Key.Equals(Key.Enter) || e.Key.Equals(Key.Tab)) return;
                if (e.Key.Equals(Key.Escape))
                {
                    _wishModel.CloseCompletionWindow();
                }
                e.Handled = true;
                return;
            }
            var result = e.Key.Equals(Key.Tab) ? _wishModel.Complete(textEditor, StateNormal) : _wishModel.Raise(e.Key, textEditor.Text);
            if (result.IsExit)
            {
                Exit();
                return;
            }
            _state = result.State;
            if (result.FullyProcessed) return;
            ProcessCommandResult(result, false);
            e.Handled = result.Handled;
        }

        private void StateNormal()
        {
            _state = State.Normal;
        }

        private void EnsureCorrectCaretPosition()
        {
            var line = textEditor.Document.GetLineByNumber(textEditor.TextArea.Caret.Line);
            if (0 != line.Length) return;
            textEditor.TextArea.Caret.Line = textEditor.TextArea.Caret.Line - 1;
            textEditor.TextArea.Caret.Column = _promptLength;
        }

        private void Exit()
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
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(WishView),
                new PropertyMetadata(@"Wish")
                );

        private Popup _popup;

        public string Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }

        private void ExecuteNewTab(object sender, ExecutedRoutedEventArgs e)
        {
            var view = new WishView(_mainRegion);
            _mainRegion.Add(view);
        }

        private void ExecuteControlR(object sender, ExecutedRoutedEventArgs e)
        {
            _popup = new Popup {IsOpen = false, PlacementTarget = textEditor, Placement = PlacementMode.Center, Width = 285};
            _wishModel.RequestHistorySearch(_popup, Process);
        }

        private void Process(string text)
        {
            var result = _wishModel.Raise(Key.Enter, textEditor.Text + text);
            _state = result.State;
            ProcessCommandResult(result, true);
        }

        private void ProcessCommandResult(CommandResult result, bool clearPopups)
        {
            if (clearPopups)
            {
                ClearPopups();
                textEditor.Focus();
            }
            if (result.IsExit)
            {
                Exit();
                return;
            }
            textEditor.Text = result.Text;
            var wdir = result.WorkingDirectory;
            if (null != wdir)
            {
                Title = result.WorkingDirectory;
            }
            _promptLength = result.PromptLength;
            textEditor.ScrollToEnd();
        }

        private void ClearPopups()
        {
            if(null != _popup)
            {
                _popup.IsOpen = false;
            }
        }

        private void SetSyntaxHighlighting()
        {
            IHighlightingDefinition customHighlighting;
            var type = typeof(WishView);
            using (var s = type.Assembly.GetManifestResourceStream("Wish.Views.CustomHighlighting.xshd"))
            {
                if (s == null)
                {
                    throw new InvalidOperationException("Could not find embedded resource");
                }
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", null, customHighlighting);
            textEditor.SyntaxHighlighting = customHighlighting;
        }

        private void ExecuteControlP(object sender, ExecutedRoutedEventArgs e)
        {
            var result = _wishModel.Raise(Key.Up, textEditor.Text);
            ProcessCommandResult(result, true);
        }

        private void ExecuteControlN(object sender, ExecutedRoutedEventArgs e)
        {
            var result = _wishModel.Raise(Key.Down, textEditor.Text);
            ProcessCommandResult(result, true);
        }

        private void ExecuteControlD(object sender, ExecutedRoutedEventArgs e)
        {
            _popup = new Popup {IsOpen = false, PlacementTarget = textEditor, Placement = PlacementMode.Center, Width = 285};
            _wishModel.RequestRecentDirectory(_popup, Process);
        }

        private void ExecuteControlA(object sender, ExecutedRoutedEventArgs e)
        {
            _popup = new Popup {IsOpen = false, PlacementTarget = textEditor, Placement = PlacementMode.Center, Width = 285};
            _wishModel.RequestRecentArgument(_popup, Append);
        }

        private void Append(string obj)
        {
            textEditor.Text += obj;
            ClearPopups();
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Practices.Prism.Regions;
using Wish.Commands.Runner;
using Wish.Common;
using Wish.State;

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
        public static RoutedCommand ControlShiftP = new RoutedCommand();
        public static RoutedCommand ControlShiftC = new RoutedCommand();
        public static RoutedCommand ControlShiftV = new RoutedCommand();
        private int _promptLength;
        private IState _state;

        public WishView(IRegion mainRegion)
        {
            InitializeComponent();
            SetInputGestures();
            SetSyntaxHighlighting();
            _mainRegion = mainRegion;
            _wishModel = new WishModel();
            _state = new Normal(_wishModel);
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

            var controlShiftP = new KeyGesture(Key.P, ModifierKeys.Control | ModifierKeys.Shift);
            ControlShiftP.InputGestures.Add(controlShiftP);

            var controlShiftC = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
            ControlShiftC.InputGestures.Add(controlShiftC);

            var controlShiftV = new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift);
            ControlShiftV.InputGestures.Add(controlShiftV);
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
            var args = new WishArgs
                       {
                           TextEditor = textEditor,
                           OnClosed = StateNormal,
                           Execute = Execute,
                           Key = e.Key,
                           WorkingDirectory = Title
                       };
            var result = _state.OnPreviewKeyDown(args);
            if(result.IsExit)
            {
                Exit();
                return;
            }
            e.Handled = result.Handled;
            _state = EnumToState(result.State);
            if (result.FullyProcessed) return;
            ProcessCommandResult(result);
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

        private IState EnumToState(Common.State state)
        {
            switch (state)
            {
                case Common.State.Normal:
                    return new Normal(_wishModel);
                default:
                    return new Completion(_wishModel);
            }
        }

        void Execute()
        {
            var args = new WishArgs {Key = Key.Enter, WorkingDirectory = Title, TextEditor = textEditor};
            var result = _wishModel.Raise(args);
            ClearPopups();
            textEditor.Focus();
            ProcessCommandResult(result);
        }

        private void StateNormal()
        {
            _state = new Normal(_wishModel);
        }

        private void EnsureCorrectCaretPosition()
        {
            var line = textEditor.Document.GetLineByNumber(textEditor.TextArea.Caret.Line);
            if (0 != line.Length) return;
            textEditor.TextArea.Caret.Line = textEditor.TextArea.Caret.Line - 1;
            textEditor.TextArea.Caret.Column = _promptLength;
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

        private void Process(string text)
        {
            textEditor.Text += text;
            var args = new WishArgs {Key = Key.Enter, WorkingDirectory = Title, TextEditor = textEditor};
            var result = _wishModel.Raise(args);
            var state = result.State;
            switch (state)
            {
                case Common.State.Normal:
                    _state = new Normal(_wishModel);
                    break;
                case Common.State.Tabbing:
                    _state = new Completion(_wishModel);
                    break;
            }
            ClearPopups();
            textEditor.Focus();
            ProcessCommandResult(result);
        }


        private void ClearPopups()
        {
            if (null == _popup) return;
            _popup.IsOpen = false;
            _popup = null;
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
            var args = new WishArgs {Key = Key.Up, WorkingDirectory = Title, TextEditor = textEditor};
            var result = _wishModel.Raise(args);
            ClearPopups();
            textEditor.Focus();
            ProcessCommandResult(result);
        }

        private void ExecuteControlN(object sender, ExecutedRoutedEventArgs e)
        {
            var args = new WishArgs {Key = Key.Down, WorkingDirectory = Title, TextEditor = textEditor};
            var result = _wishModel.Raise(args);
            ClearPopups();
            textEditor.Focus();
            ProcessCommandResult(result);
        }

        private void ProcessCommandResult(CommandResult result)
        {
            textEditor.Text = result.Text;
            var wdir = result.WorkingDirectory;
            if (null != wdir)
            {
                Title = result.WorkingDirectory;
            }
            _promptLength = result.PromptLength;
            textEditor.ScrollToEnd();
        }

        private void ExecuteControlR(object sender, ExecutedRoutedEventArgs e)
        {
            _popup = _state.RequestPopup(_wishModel.RequestHistorySearch, Process, textEditor);
        }

        private void ExecuteControlD(object sender, ExecutedRoutedEventArgs e)
        {
            _popup = _state.RequestPopup(_wishModel.RequestRecentDirectory, Process, textEditor);
        }

        private void ExecuteControlA(object sender, ExecutedRoutedEventArgs e)
        {
            _popup = _state.RequestPopup(_wishModel.RequestRecentArgument, Append, textEditor);
        }

        private void Append(string obj)
        {
            textEditor.Text = textEditor.Text.TrimEnd();
            textEditor.Text += " " + obj;
            ClearPopups();
            textEditor.Focus();
        }

        private void CmdSelected(object sender, RoutedEventArgs e)
        {
            pshell.IsChecked = false;
            vsPrompt.IsChecked = false;
            cmd.IsChecked = true;
            _wishModel.SetRunner(new Cmd(), Title);
        }

        private void PowershellSelected(object sender, RoutedEventArgs e)
        {
            cmd.IsChecked = false;
            vsPrompt.IsChecked = false;
            pshell.IsChecked = true;
            _wishModel.SetRunner(new Powershell(), Title);
        }

        private void VsSelected(object sender, RoutedEventArgs e)
        {
            //cmd.IsChecked = false;
            //pshell.IsChecked = false;
            //vsPrompt.IsChecked = true;
            //_wishModel.SetRunner(new Powershell(), Title);
            throw new NotImplementedException();
        }
    }
}

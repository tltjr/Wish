using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Practices.Prism.Regions;
using Wish.Common;
using Wish.State;
using Wish.ViewModels;

namespace Wish.Views
{
    /// <summary>
    /// Interaction logic for WishView.xaml
    /// </summary>
    public partial class WishView
    {
        private readonly IRegion _mainRegion;
        private readonly WishModel _wishModel;
        private int _promptLength;
        private IState _state;
        private readonly WishViewModel _viewModel;

        public WishView(IRegion mainRegion, WishViewModel viewModel)
        {
            InitializeComponent();
            _mainRegion = mainRegion;
            _wishModel = viewModel.Model;
            _state = new Normal(_wishModel);
            _viewModel = viewModel;
            DataContext = viewModel;
        }

        private void ScrollToEnd(object sender, EventArgs eventArgs)
        {
            EnsureCorrectCaretPosition();
            var line = textEditor.TextArea.Caret.Line;
            textEditor.ScrollTo(line, textEditor.TextArea.Caret.Column);
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textEditor);
            var result = _wishModel.Start();
            if (result.FullyProcessed) return;
            textEditor.Text = result.Text;
            _viewModel.Title = result.WorkingDirectory;
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
            if (e.KeyboardDevice.Modifiers.Any()) return;
            var args = new WishArgs
                       {
                           TextEditor = textEditor,
                           OnClosed = StateNormal,
                           Execute = Execute,
                           Key = e.Key,
                           WorkingDirectory = _viewModel.Title
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
            var args = new WishArgs {Key = Key.Enter, WorkingDirectory = _viewModel.Title, TextEditor = textEditor};
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

        private void ExecuteNewTab(object sender, ExecutedRoutedEventArgs e)
        {
            var view = new WishView(_mainRegion, new WishViewModel(new WishModel()));
            _mainRegion.Add(view);
        }

        private void Process(string text)
        {
            textEditor.Text += text;
            var args = new WishArgs {Key = Key.Enter, WorkingDirectory = _viewModel.Title, TextEditor = textEditor};
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


        private void ExecuteControlP(object sender, ExecutedRoutedEventArgs e)
        {
            var args = new WishArgs {Key = Key.Up, WorkingDirectory = _viewModel.Title, TextEditor = textEditor};
            var result = _wishModel.Raise(args);
            ClearPopups();
            textEditor.Focus();
            ProcessCommandResult(result);
        }

        private void ExecuteControlN(object sender, ExecutedRoutedEventArgs e)
        {
            var args = new WishArgs {Key = Key.Down, WorkingDirectory = _viewModel.Title, TextEditor = textEditor};
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
                _viewModel.Title = result.WorkingDirectory;
            }
            _promptLength = result.PromptLength;
            textEditor.ScrollToEnd();
        }


        private void Append(string obj)
        {
            textEditor.Text = textEditor.Text.TrimEnd();
            textEditor.Text += " " + obj;
            ClearPopups();
            textEditor.Focus();
        }
    }
}
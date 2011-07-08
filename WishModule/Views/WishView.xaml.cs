using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GuiHelpers;
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
        public static RoutedCommand ControlR = new RoutedCommand();
        private readonly WishModel _wish;
        private readonly TextTransformations _textTransformations = new TextTransformations();
        //public readonly SyntaxHighlighting SyntaxHighlighting = new SyntaxHighlighting();

        public WishView(IRegion mainRegion, IEventAggregator eventAggregator, string workingDirectory)
        {
            InitializeComponent();
            _mainRegion = mainRegion;
            _eventAggregator = eventAggregator;
            _wish = new WishModel(textEditor, _textTransformations, workingDirectory);
            SetInputGestures();
        }

        private void SetInputGestures()
        {
            var cntrlShftT = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
            TabNew.InputGestures.Add(cntrlShftT);

            var controlT = new KeyGesture(Key.T, ModifierKeys.Control);
            TabNew.InputGestures.Add(controlT);

            var controlR = new KeyGesture(Key.R, ModifierKeys.Control);
            ControlR.InputGestures.Add(controlR);
        }

        private void ScrollToEnd(object sender, EventArgs eventArgs)
        {
            textEditor.ScrollToEnd();
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textEditor);
            Title = _wish.WorkingDirectory;
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
            var comm = _textTransformations.ParseScript(textEditor.Text);
            if (IsExit(comm)) return;
            var workingDir = _wish.KeyPress(e);
            if (String.IsNullOrEmpty(workingDir)) return;
            Title = workingDir;
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(WishView),
                new PropertyMetadata(@"Wish")
                );

        public string Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }

        private void ExecuteNewTab(object sender, ExecutedRoutedEventArgs e)
        {
            var view = new WishView(_mainRegion, _eventAggregator, _wish.WorkingDirectory);
            _mainRegion.Add(view);
        }

        private void ExecuteControlR(object sender, ExecutedRoutedEventArgs e)
        {
            var workingDir = _wish.RequestHistorySearch();
            if (String.IsNullOrEmpty(workingDir)) return;
            Title = workingDir;
        }

        public bool IsExit(Command command)
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
    }
}

﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Terminal;
using Wish.Core;
using Wish.ViewModels;

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
        private readonly TerminalViewModel _terminalViewModel;
        private bool _activelyTabbing;
        private readonly CompletionManager _completionManager = new CompletionManager();

        public WishView(IRegion mainRegion, IEventAggregator eventAggregator, string workingDirectory)
        {
            InitializeComponent();
            _mainRegion = mainRegion;
            _terminalViewModel = new TerminalViewModel(_mainRegion, this, workingDirectory);
            _eventAggregator = eventAggregator;
            var keyGesture = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
            TabNew.InputGestures.Add(keyGesture);
        }

        private void ScrollToEnd(object sender, TextChangedEventArgs e)
        {
            textBox.ScrollToEnd();
            textBox.CaretIndex = textBox.Text.Length;
            Title = _terminalViewModel.Terminal.WorkingDirectory;
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textBox);
            // need to bind late to get the title bound
            DataContext = _terminalViewModel;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var terminal = _terminalViewModel.Terminal;
            if(e.Key == Key.Tab)
            {
                if(textBox.CaretIndex != textBox.Text.Length || textBox.CaretIndex == terminal.LastPromptIndex)
                    return;
                var line = textBox.Text.Substring(terminal.LastPromptIndex);
                var command = TerminalUtils.ParseCommandLine(line);
                string result;
                var flag = _completionManager.Complete(out result, command, _activelyTabbing, terminal.WorkingDirectory, textBox.Text);
                if(flag)
                {
                    _activelyTabbing = true;
                    textBox.Text = result;
                }
                e.Handled = true;
            }
            else
            {
                base.OnPreviewKeyDown(e);
                _activelyTabbing = false;
            }
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
            var view = new WishView(_mainRegion, _eventAggregator, _terminalViewModel.Terminal.WorkingDirectory);
            _mainRegion.Add(view);
        }
    }
}

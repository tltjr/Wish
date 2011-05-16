using System;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Terminal;

namespace WishModule.Views
{
    /// <summary>
    /// Interaction logic for WishView.xaml
    /// </summary>
    public partial class WishView
    {
        private string _result;
        private readonly DirectoryManager _directoryManager;
        public static RoutedCommand TabNew = new RoutedCommand();
        private readonly IRegion _mainRegion;
        private readonly IEventAggregator _eventAggregator;

        public WishView(IRegion mainRegion, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            Keyboard.Focus(Output);
            _directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            Output.InsertNewPrompt(_directoryManager.WorkingDirectory);
            SetKeybindings();
            _mainRegion = mainRegion;
            _eventAggregator = eventAggregator;
        }

        protected virtual void HandleTabKey()
        {
            // Command completion works only if caret is at last character
            // and if the user already typed something.
            if (Output.CaretIndex != Output.Text.Length || Output.CaretIndex == Output.LastPromptIndex)
                return;

            // Get command name and associated commands
            string line = Output.Text.Substring(Output.LastPromptIndex);
            return;
        }

        protected virtual void HandleEnterKey()
        {
            var line = Output.Text.Substring(Output.LastPromptIndex);
            Output.Text += "\n";
            Output.IsInputEnabled = false;
            var cmd = TerminalUtils.ParseCommandLine(line);
            Output.CommandLog.Add(cmd);
            Output.IndexInLog = Output.CommandLog.Count;
            Output.RaiseCommandEntered(cmd);
        }

        private void SetKeybindings()
        {
            var keyGesture = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
            TabNew.InputGestures.Add(keyGesture);
        }

        private void CommandEntered(object sender, Terminal.Terminal.CommandEventArgs e)
        {
            ExecuteCommandSync(e);
            Output.InsertNewPrompt(_directoryManager.WorkingDirectory);
            Output.InsertLineBeforePrompt(_result);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(Output);
        }

        public void ExecuteCommandSync(object obj)
        {
            var e = (Terminal.Terminal.CommandEventArgs) obj;
            var command = e.Command;
            if(DirectoryChange(command))
            {
                var changed = _directoryManager.ChangeDirectory(command.Args.FirstOrDefault());
                _result = changed ? "" : "The system cannot find the path specified\n";
            }
            else
            {
                try
                {
                    var procStartInfo =
                        new System.Diagnostics.ProcessStartInfo("cmd", "/c" + command.Raw)
                            {
                                RedirectStandardOutput = true,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                WorkingDirectory = _directoryManager.WorkingDirectory
                            };
                    var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
                    proc.Start();
                    _result = proc.StandardOutput.ReadToEnd();
                }
                catch (Exception objException)
                {
                    _result = objException.Message;
                    
                }
            }
        }

        private bool DirectoryChange(Command command)
        {
            return command.Name.Equals("cd");
        }

        public void ExecuteCommandAsync(string command)
        {
            try
            {
                var objThread = new Thread(ExecuteCommandSync)
                                    {
                                        IsBackground = true,
                                        Priority = ThreadPriority.AboveNormal
                                    };
                objThread.Start(command);
            }
            catch (ThreadStartException)
            {
                // Log the exception
            }
            catch (ThreadAbortException)
            {
                // Log the exception
            }
            catch (Exception)
            {
                // Log the exception
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
            var newTab = new WishView(_mainRegion, _eventAggregator);
            _mainRegion.Add(newTab);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            // If Ctrl+C is entered, raise an abortrequested event !
            if (e.Key == Key.C)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    //RaiseAbortRequested();
                    e.Handled = true;
                    return;
                }
            }

            // If input is not allowed, warn the user and discard its input.
            //if (!IsInputEnabled)
            //{
            //    if (IsSystemBeepEnabled)
            //        SystemSounds.Beep.Play();
            //    e.Handled = true;
            //    return;
            //}

            // Test the caret position.
            //
            // 1. If located before the last prompt index
            //    ==> Warn, set the caret at the end of input text, add text, discard the input
            //        if user tries to erase text, else process it.
            //
            // 2. If located at the last prompt index and user tries to erase text
            //    ==> Warn, discard the input.
            //
            // 3. If located at the last prompt index and user tries to move backward
            //    ==> Warn, discard the input.
            //
            // 4. If located after (>=) the last prompt index and user presses the UP key
            //    ==> Launch command history backward, discard the input.
            //
            // 5. If located after (>=) the last prompt index and user presses the UP key
            //    ==> Launch command history forward, discard the input.
            //
            //if (CaretIndex < LastPromptIndex)
            //{
            //    if (IsSystemBeepEnabled)
            //        SystemSounds.Beep.Play();
            //    CaretIndex = Text.Length;
            //    e.Handled = false;
            //    if (e.Key == Key.Back || e.Key == Key.Delete)
            //        e.Handled = true;
            //}
            //else if (CaretIndex == LastPromptIndex && e.Key == Key.Back)
            //{
            //    if (IsSystemBeepEnabled)
            //        SystemSounds.Beep.Play();
            //    e.Handled = true;
            //}
            //else if (CaretIndex == LastPromptIndex && e.Key == Key.Left)
            //{
            //    if (IsSystemBeepEnabled)
            //        SystemSounds.Beep.Play();
            //    e.Handled = true;
            //}
            //else if (CaretIndex >= LastPromptIndex && e.Key == Key.Up)
            //{
            //    HandleCommandHistoryRequest(CommandHistoryDirection.Backward);
            //    e.Handled = true;
            //}
            //else if (CaretIndex >= LastPromptIndex && e.Key == Key.Down)
            //{
            //    HandleCommandHistoryRequest(CommandHistoryDirection.Forward);
            //    e.Handled = true;
            //}

            // If input has not yet been discarded, test the key for special inputs.
            // ENTER   => validates the input
            // TAB     => launches command completion with registered commands
            // CTRL+C  => raises an abort request event
            if (!e.Handled)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        HandleEnterKey();
                        e.Handled = true;
                        break;
                    case Key.Tab:
                        HandleTabKey();
                        e.Handled = true;
                        break;
                }
            }

            base.OnPreviewKeyDown(e);
        }

    }
}

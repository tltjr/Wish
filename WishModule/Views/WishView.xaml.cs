using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Terminal;

namespace WishModule.Views
{
    /// <summary>
    /// Interaction logic for WishView.xaml
    /// </summary>
    public partial class WishView : UserControl
    {
        private string _result;
        private readonly DirectoryManager _directoryManager;
        public static RoutedCommand TabNew = new RoutedCommand();
        private IRegion _mainRegion;
        private IEventAggregator _eventAggregator;

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
            catch (ThreadStartException objException)
            {
                // Log the exception
            }
            catch (ThreadAbortException objException)
            {
                // Log the exception
            }
            catch (Exception objException)
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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Terminal;
using Command = Terminal.Command;

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
        private CompletionManager _completionManager = new CompletionManager();

        private readonly Runspace _runspace;
        private Pipeline _pipeline;

        private bool _activelyTabbing;

        public WishView(IRegion mainRegion, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            Output.InsertNewPrompt(_directoryManager.WorkingDirectory);
            SetKeybindings();
            _mainRegion = mainRegion;
            _eventAggregator = eventAggregator;
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
            Output.ScrollToEnd();
            Keyboard.Focus(Output);
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Tab)
            {
    			if (Output.CaretIndex != Output.Text.Length || Output.CaretIndex == Output.LastPromptIndex)
    				return;

    			// Get command name and associated commands
    			string line = Output.Text.Substring(Output.LastPromptIndex);
                TabComplete(TerminalUtils.ParseCommandLine(line));
                e.Handled = true;
                _activelyTabbing = true;
            }
            else
            {
                Output.PreviewKeyDown(e);
                _activelyTabbing = false;
            }
        }

        private void TabComplete(Command command)
        {
            string result;
            if (!_completionManager.Complete(out result, command, _activelyTabbing,
                                             _directoryManager.WorkingDirectory, Output.Text)) return;
            Output.Text = result;
            Output.CaretIndex = result.Length;
        }

        public void ExecuteCommandSync(object obj)
        {
           var e = (Terminal.Terminal.CommandEventArgs) obj;
            var command = e.Command;
            _pipeline = _runspace.CreatePipeline();
            _pipeline.Commands.AddScript(command.Raw);
            _pipeline.Commands.Add("Out-String");
            var results = _pipeline.Invoke();

            var sb = new StringBuilder();
            foreach (var psObject in results)
            {
                sb.AppendLine(psObject.ToString());
            }
            _result = sb.ToString();
            //var e = (Terminal.Terminal.CommandEventArgs) obj;
            //var command = e.Command;
            //if(DirectoryChange(command))
            //{
            //    var changed = _directoryManager.ChangeDirectory(command.Args.FirstOrDefault());
            //    _result = changed ? "" : "The system cannot find the path specified\n";
            //}
            //else
            //{
            //    try
            //    {
            //        var procStartInfo =
            //            new System.Diagnostics.ProcessStartInfo("cmd", "/c" + command.Raw)
            //                {
            //                    RedirectStandardOutput = true,
            //                    UseShellExecute = false,
            //                    CreateNoWindow = true,
            //                    WorkingDirectory = _directoryManager.WorkingDirectory
            //                };
            //        var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
            //        proc.Start();
            //        _result = proc.StandardOutput.ReadToEnd();
            //    }
            //    catch (Exception objException)
            //    {
            //        _result = objException.Message;
                    
            //    }
            //}
        }

        private bool DirectoryChange(Command command)
        {
            //equals cd or eventually popd, pushd, etc..
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

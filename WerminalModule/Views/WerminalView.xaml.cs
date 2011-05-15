using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Terminal;

namespace WerminalModule.Views
{
    /// <summary>
    /// Interaction logic for WerminalView.xaml
    /// </summary>
    public partial class WerminalView : UserControl
    {
        private string _result;
        private readonly DirectoryManager _directoryManager;

        public WerminalView()
        {
            InitializeComponent();
            Keyboard.Focus(Output);
            _directoryManager = new DirectoryManager {WorkingDirectory = @"C:\Users\tlthorn1"};
            Output.InsertNewPrompt(_directoryManager.WorkingDirectory);
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
                if(changed)
                {
                    _result = "";
                }
                else
                {
                    _result = "The system cannot find the path specified\n";
                }
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
    }
}

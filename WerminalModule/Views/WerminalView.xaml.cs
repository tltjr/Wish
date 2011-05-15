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
        private string _workingDirectory = @"C:\Users\tlthorn1";

        public WerminalView()
        {
            InitializeComponent();
            Keyboard.Focus(Output);
        }

        private void CommandEntered(object sender, Terminal.Terminal.CommandEventArgs e)
        {
            var s = e.Command.Raw;
            ExecuteCommandSync(s);
            Output.InsertNewPrompt();
            Output.InsertLineBeforePrompt(_result);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(Output);
        }

        public void ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.

                var procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command)
                        {
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                procStartInfo.WorkingDirectory = _workingDirectory;

                var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
                proc.Start();
                _result = proc.StandardOutput.ReadToEnd();
                var blah = proc.StandardOutput;
            }
            catch (Exception objException)
            {
                _result = objException.Message;
            }
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

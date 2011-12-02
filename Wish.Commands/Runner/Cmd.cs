using System.Diagnostics;
using System.Management.Automation.Runspaces;

namespace Wish.Commands.Runner
{
    public class Cmd : IRunner
    {
        private readonly CmdDirectoryManager _cmdDirectoryManager = new CmdDirectoryManager();

        public string Execute(RunnerArgs args)
        {
            var process = new Process
                              {
                                  StartInfo = new ProcessStartInfo("cmd", "/c " + args.Script)
                                                  {
                                                      RedirectStandardOutput = true,
                                                      RedirectStandardError = true,
                                                      CreateNoWindow = true,
                                                      UseShellExecute = false,
                                                      WorkingDirectory = WorkingDirectory
                                                  }
                              };
            process.Start();
            /* can't read both error and output
             * see: http://msdn.microsoft.com/en-us/library/system.diagnostics.process.standarderror(v=vs.71).aspx
             * ctrl + f -> A similar problem
             * Both error and output would need thier own threads
             */
            //var error = process.StandardError.ReadToEnd();
            //if (!string.IsNullOrEmpty(error)) return error;
            var output = process.StandardOutput.ReadToEnd();
            if(!process.WaitForExit(1000))
            {
                process.Kill();
                return "An error has occurred";
            }
            _cmdDirectoryManager.UpdateWorkingDirectory(args.Script);
            return output;
        }

        public string WorkingDirectory
        {
            get
            {
                return _cmdDirectoryManager.WorkingDirectory;
            }
        }

        // implements interface but isn't used
        public Runspace Runspace { get; set; }

        public override string ToString()
        {
            return "[C]";
        }
    }
}

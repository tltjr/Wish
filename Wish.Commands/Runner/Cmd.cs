using System.Diagnostics;

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
            var error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error)) return error;
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            _cmdDirectoryManager.UpdateWorkingDirectory(args.Script);
            return output;
        }

        public string WorkingDirectory
        {
            get { return _cmdDirectoryManager.WorkingDirectory; }
        }

        public override string ToString()
        {
            return "[C]";
        }
    }
}

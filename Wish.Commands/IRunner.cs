using System.Management.Automation.Runspaces;
using Wish.Commands.Runner;

namespace Wish.Commands
{
    public interface IRunner
    {
        string Execute(RunnerArgs line);
        string WorkingDirectory { get; }
        Runspace Runspace { get; set; }
    }
}

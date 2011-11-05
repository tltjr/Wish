using System.Management.Automation.Runspaces;

namespace Wish.Commands.Runner
{
    public class RunspaceSingleton
    {
        private static Runspace _runspace;

        private static void CreateRunspace()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
        }

        public static Runspace Instance
        {
            get
            {
                if (_runspace == null)
                {
                    CreateRunspace();
                }
                return _runspace;
            }
        }
    }
}

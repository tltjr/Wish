using System.Management.Automation.Runspaces;

namespace Wish.Core
{
    public class RunspaceSingleton
    {
        private static RunspaceSingleton _instance;
        private static Runspace _runspace;

        private RunspaceSingleton()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
        }
        
        public static Runspace RunspaceInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RunspaceSingleton();
                }
                return _runspace;
            }
        }
    }
}

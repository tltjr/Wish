using System.Collections.Generic;
using System.Management.Automation.Runspaces;

namespace Wish.Commands.Runner
{
    public class RunspaceSingleton
    {
        private static readonly IDictionary<int, Runspace> Runspaces = new Dictionary<int, Runspace>();

        private static Runspace CreateRunspace(int id)
        {
            var rspace = RunspaceFactory.CreateRunspace();
            rspace.Open();
            Runspaces.Add(id, rspace);
            return rspace;
        }

        public static Runspace GetInstance(int id)
        {
            Runspace runspace;
            return Runspaces.TryGetValue(id, out runspace) ? runspace : CreateRunspace(id);
        }
    }
}

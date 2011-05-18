using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Wish.Core
{
    public class PowershellController : IDisposable
    {
        private readonly Runspace _runspace;
        private Pipeline _pipeline;

        public PowershellController()
        {
            _runspace = RunspaceSingleton.RunspaceInstance;
        }

        public string RunScriptForFormattedResult(string script)
        {
            _pipeline = _runspace.CreatePipeline();
            _pipeline.Commands.AddScript(script);
            _pipeline.Commands.Add("Out-String");

            var psObjects = _pipeline.Invoke();
            var sb = new StringBuilder();
            foreach (var psObject in psObjects)
            {
                sb.AppendLine(psObject.ToString());
            }
            return sb.ToString();
        }

        public void RunScript(string script)
        {
            var pipeline = _runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            pipeline.Invoke();
        }

        public Collection<PSObject> RunScriptForResult(string script)
        {
            var pipeline = _runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            return pipeline.Invoke();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Wish.Core
{
    public class PowershellController
    {
        private readonly Runspace _runspace;
        private Pipeline _pipeline;

        public PowershellController()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
        }

        public string RunScriptForFormattedResult(string script)
        {
            _pipeline = _runspace.CreatePipeline();
            _pipeline.Commands.AddScript(script);
            _pipeline.Commands.Add("Out-String");
            Collection<PSObject> psObjects;
            try
            {
                psObjects = _pipeline.Invoke();
            } catch(CommandNotFoundException e)
            {
                return e.Message;
            }
            //check other streams for content
            if(_pipeline.Error.Count > 0)
            {
                return _pipeline.Error.ReadToEnd().FirstOrDefault().ToString();
            }
            if(_pipeline.Output.Count > 0)
            {
                return _pipeline.Output.ReadToEnd().FirstOrDefault().ToString();
            }
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

        private Collection<PSObject> RunScriptForResult(string script)
        {
            var pipeline = _runspace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            return pipeline.Invoke();
        }

        public string GetWorkingDirectory()
        {
            var pipeline = _runspace.CreatePipeline();
            pipeline.Commands.AddScript("pwd");
            var results = pipeline.Invoke();
            return results.FirstOrDefault().ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Wish.Commands.Runner
{
    public class Powershell : IRunner
    {
        private readonly Runspace _runspace;
        private Pipeline _pipeline;

        public Powershell()
        {
            _runspace = RunspaceSingleton.Instance;
        }

        public string Execute(string line)
        {
            var pshell = PowerShell.Create();
            pshell.AddCommand(line);
            var output = new List<string>();
            pshell.Invoke(line, output);



            _pipeline = _runspace.CreatePipeline();
            _pipeline.Commands.AddScript(line);
            _pipeline.Commands.Add("Out-String");
            Collection<PSObject> psObjects;
            try
            {
                psObjects = _pipeline.Invoke();
                //if you wanted to catch unfound parameter exceptions here, you could try
                // and expanded shortened versions, i.e. -rf => -recurse -force !!
                // of course, there is probably a smarter way to do this without expensive exceptions?
            } catch(Exception e)
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

        public string WorkingDirectory
        {
            get
            {
                var pipeline = _runspace.CreatePipeline();
                pipeline.Commands.AddScript("pwd");
                var results = pipeline.Invoke();
                return results.FirstOrDefault().ToString();
            }
        }
    }
}

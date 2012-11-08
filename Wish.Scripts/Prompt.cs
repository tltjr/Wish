using System;
using System.Security.Principal;
using System.Text;
using Wish.Commands;

namespace Wish.Scripts
{
    public class Prompt
    {
        public string Current { get; set; }
        public string WorkingDirectory { get; set; }
        public IRunner Runner { get; set; }

        public Prompt(IRunner runner)
        {
            Runner = runner;
			//var homedrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
			//var homepath = Environment.GetEnvironmentVariable("HOMEPATH");
			//if (null != homedrive && null != homepath)
			//{
			//    WorkingDirectory = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
			//}
			//else
			//{
			//    WorkingDirectory = @"C:\";
			//}
	        WorkingDirectory = @"C:\demo";
            Update(WorkingDirectory);
        }

        public void Update(string workingDirectory)
        {
            if (workingDirectory == null) return;
            var sb = new StringBuilder();
            sb.Append(Runner + " ");
			WorkingDirectory = workingDirectory;
			//sb.Append(WindowsIdentity.GetCurrent().Name);
			//sb.Append("@");
			//sb.Append(Environment.MachineName);
	        sb.Append("tltjr");
            sb.Append(" ");
            sb.Append(workingDirectory);
            sb.Append(" ");
            sb.Append(">> ");
            Current = sb.ToString();
        }
    }
}

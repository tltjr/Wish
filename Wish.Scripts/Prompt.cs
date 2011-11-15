using System;
using System.Security.Principal;
using System.Text;

namespace Wish.Scripts
{
    public class Prompt
    {
        public string Current { get; set; }
        public string WorkingDirectory { get; set; }

        public Prompt()
        {
            var homedrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
            var homepath = Environment.GetEnvironmentVariable("HOMEPATH");
            if (null != homedrive && null != homepath)
            {
                WorkingDirectory = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
            else
            {
                WorkingDirectory = @"C:\";
            }
            Update(WorkingDirectory);
        }

        public void Update(string workingDirectory)
        {
            if (workingDirectory == null) return;
            var sb = new StringBuilder();
            WorkingDirectory = workingDirectory;
            sb.Append(WindowsIdentity.GetCurrent().Name);
            sb.Append("@");
            sb.Append(Environment.MachineName);
            sb.Append(" ");
            sb.Append(workingDirectory);
            sb.Append(" ");
            sb.Append(">> ");
            Current = sb.ToString();
        }
    }
}

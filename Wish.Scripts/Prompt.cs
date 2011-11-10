using System;
using Wish.Common;

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
            WorkingDirectory = workingDirectory;
            Current = workingDirectory + " >> ";
            Global.PromptLength = Current.Length + 1;
        }
    }
}

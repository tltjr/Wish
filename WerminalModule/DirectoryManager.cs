using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal;

namespace WerminalModule
{
    public class DirectoryManager
    {
        private string _workingDirectory;

        public string WorkingDirectory { 
            get { return _workingDirectory; }
            set { _workingDirectory = value; }
        }

        public void ChangeDirectory(string dir)
        {
            WorkingDirectory = dir;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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

        public bool ChangeDirectory(string dir)
        {
            var directoryInfo = new DirectoryInfo(dir);
            if (!directoryInfo.Exists)
            {
                return false;
            }
            WorkingDirectory = dir;
            return true;
        }
    }
}

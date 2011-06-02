using System;
using System.IO;
using System.Linq;

namespace WishModule
{
    public class DirectoryManager
    {
        private string _workingDirectory;

        public string WorkingDirectory { 
            get
            {
                return _workingDirectory.Equals("C:", StringComparison.CurrentCultureIgnoreCase) 
                    ? "C:\\" : ParseEndingSlashes(_workingDirectory);
            }
            set { _workingDirectory = ParseEndingSlashes(value); }
        }

        private string ParseEndingSlashes(string workingDirectory)
        {
            if(workingDirectory.EndsWith("\\") || workingDirectory.EndsWith("/"))
            {
                return workingDirectory.Substring(0, workingDirectory.Length - 1);
            }
            return workingDirectory;
        }

        public bool ChangeDirectory(string dir)
        {
            return dir.Contains(":") ? TrySetWorkingDirectory(dir) : 
                   dir.Contains("..") ? NavigateUp(dir) :
                   TrySetWorkingDirectory(_workingDirectory + "\\" + dir);
        }

        private bool NavigateUp(string dir)
        {
            try
            {
                if (dir.Equals(".."))
                {
                    return TrySetWorkingDirectory(GetParentDirectory());
                }
                if (dir.Equals(@"..\.."))
                {
                    if(TrySetWorkingDirectory(GetParentDirectory()))
                    {
                        return TrySetWorkingDirectory(GetParentDirectory());
                    }
                }
                if (dir.Equals(@"..\..\.."))
                {
                    if(TrySetWorkingDirectory(GetParentDirectory()))
                    {
                        if(TrySetWorkingDirectory(GetParentDirectory()))
                        {
                            return TrySetWorkingDirectory(GetParentDirectory());
                        }
                    }
                }
                if (dir.Equals(@"..\..\..\.."))
                {
                    if(TrySetWorkingDirectory(GetParentDirectory()))
                    {
                        if(TrySetWorkingDirectory(GetParentDirectory()))
                        {
                            if(TrySetWorkingDirectory(GetParentDirectory()))
                            {
                                return TrySetWorkingDirectory(GetParentDirectory());
                            }
                        }
                    }
                    
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GetParentDirectory()
        {
            var dirs = _workingDirectory.Split('\\');
            var withoutCurrent = dirs.Take(dirs.Count() - 1);
            return String.Join("\\", withoutCurrent);
        }

        private bool TrySetWorkingDirectory(string dir)
        {
            var directoryInfo = new DirectoryInfo(dir);
            if (!directoryInfo.Exists)
            {
                return false;
            }
            _workingDirectory = ParseEndingSlashes(dir);
            return true;
        }
    }
}

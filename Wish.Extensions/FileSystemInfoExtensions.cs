using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wish.Extensions
{
    public static class FileSystemInfoExtensions
    {
        private static readonly List<string> ExecutableExtensions = new List<string> { @"^.bat$", @"^.cmd$", @"^.com$", @"^.exe$", @"^.js$", @"^.ps1$" };

        public static IEnumerable<string> Executables(this DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists) return Enumerable.Empty<string>();
            var files = Directory.GetFiles(directoryInfo.FullName);
            var result = new List<string>();
            result.AddRange(Executables(files));
            return result;
        }

        public static bool IsExecutable(this FileSystemInfo file)
        {
            foreach (var pattern in ExecutableExtensions)
            {
                if (Regex.IsMatch(file.Extension, pattern, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<string> Executables(IEnumerable<string> files)
        {
            var result = new List<string>();
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.IsExecutable())
                {
                    result.Add(fileInfo.Name);
                }
            }
            return result;
        }

    }
}

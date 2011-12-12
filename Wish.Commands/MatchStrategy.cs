using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wish.Commands
{
    public interface IMatchStrategy
    {
        FileSystemArgs GetArgs(string workingDirectory);
    }

    public class FileSystemArgs
    {
        public string Path { get; set; }
        public string SearchPattern { get; set; }
    }

    public class FullPathMatchStrategy : IMatchStrategy
    {
        private readonly string _pattern;

        public FullPathMatchStrategy(string pattern)
        {
            _pattern = pattern;
        }

        public FileSystemArgs GetArgs(string workingDirectory)
        {
            var split = _pattern.Split('\\');
            var segments = split.Take(split.Count() - 1).ToList();
            var path = GetBasePath(segments);
            var searchPattern = split.Last();
            return new FileSystemArgs {Path = path, SearchPattern = searchPattern};
        }

        private static string GetBasePath(List<string> segments)
        {
            return (segments.Count() > 1) ? string.Join("\\", segments) : segments.First() + "\\";
        }
    }

    public class RelativePathMatchStrategy : IMatchStrategy
    {
        private readonly string _pattern;

        public RelativePathMatchStrategy(string pattern)
        {
            _pattern = pattern;
        }

        public FileSystemArgs GetArgs(string workingDirectory)
        {
            var split = _pattern.Split('\\');
            var dots = split.TakeWhile(o => o.Equals("..")).ToList();
            //var numSegments = split.Count() - dots.Count();
            //if (numSegments < 1) return null;
            var segments = split.SkipWhile(o => o.Equals(".."));
            var directoryInfo = new DirectoryInfo(workingDirectory);
            if (!directoryInfo.Exists) return null;
            var result = directoryInfo;
            for (var i = 0; i < dots.Count(); i++)
            {
                if (result != null) result = result.Parent;
            }
            return result != null ? new FileSystemArgs {Path = result.FullName, SearchPattern = string.Join("\\", segments)} : null;
        }
    }

    public class StandardMatchStrategy : IMatchStrategy
    {
        private readonly string _pattern;

        public StandardMatchStrategy(string pattern)
        {
            _pattern = pattern;
        }

        public FileSystemArgs GetArgs(string workingDirectory)
        {
            var path = workingDirectory;
            return new FileSystemArgs {Path = path, SearchPattern = _pattern};
        }
    }
}

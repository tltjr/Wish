using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wish.Extensions;

namespace Wish.Commands
{
    public abstract class Argument : Completable
    {
        public PartialPath PartialPath { get; set; }

        protected Argument(string text)
        {
            PartialPath = new PartialPath(text);
        }

        protected List<string> GetDirectories(string workingDirectory)
        {
            var directories = Directory.GetFileSystemEntries(workingDirectory, PartialPath.Pattern);
            return directories.Select(directory => PartialPath.Base + new DirectoryInfo(directory).Name).ToList();
        }

        protected static IEnumerable<string> QuoteSpaces(IEnumerable<string> list, string quote)
        {
            var lst = list.ToList();
            var surrounded = lst.Where(o => o.Contains(" "))
                       .Select(o => o.Surround(quote));
            var result = lst.Where(o => !o.Contains(" ")).ToList();
            result.AddRange(surrounded);
            return result;
        }
    }
}
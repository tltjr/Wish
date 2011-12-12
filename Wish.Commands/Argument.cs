using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wish.Extensions;

namespace Wish.Commands
{
    public abstract class Argument : Completable
    {
        public PartialPath PartialPath { get; set; }
        public IMatchStrategy MatchStrategy { get; set; }

        protected List<string> GetDirectories(string workingDirectory)
        {
            try
            {
                var directories = GetStringDirectories(workingDirectory);
                return directories.Select(directory => PartialPath.Base + new DirectoryInfo(directory).Name).ToList();
            }
            catch(Exception)
            {
                return new List<string>();
            }
        }

        private IEnumerable<string> GetStringDirectories(string workingDirectory)
        {
            var args = MatchStrategy.GetArgs(workingDirectory);
            return Directory.GetFileSystemEntries(args.Path, args.SearchPattern);
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
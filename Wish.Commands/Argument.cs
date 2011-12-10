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

        protected Argument(string text)
        {
            PartialPath = new PartialPath(text);
        }

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
            var pattern = PartialPath.Pattern;
            string path;
            string searchPattern;
            if (pattern.Contains(":"))
            {
                var split = pattern.Split('\\');
                var segments = split.Take(split.Count() - 1).ToList();
                path = GetBasePath(segments);
                searchPattern = split.Last();
            }
            else
            {
                path = workingDirectory;
                searchPattern = PartialPath.Pattern;
            }
            return Directory.GetFileSystemEntries(path, searchPattern);
        }

        private static string GetBasePath(List<string> segments)
        {
            return (segments.Count() > 1) ? string.Join("\\", segments) : segments.First() + "\\";
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
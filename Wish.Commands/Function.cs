using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wish.Extensions;

namespace Wish.Commands
{
    public class Function : Completable
    {
        public string Name { get; set; }

        public Function(string name)
        {
            Name = name;
        }

        public override IEnumerable<string> Complete()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            if (path == null) return Enumerable.Empty<string>();
            var directories = path.Split(';');
            var result = new List<string>();
            foreach (var directory in directories)
            {
                if(String.IsNullOrEmpty(directory)) continue;
                var directoryInfo = new DirectoryInfo(directory);
                result.AddRange(directoryInfo.Executables());
            }
            return result;
        }
    }
}

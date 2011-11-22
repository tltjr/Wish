using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Wish.Commands
{
    public class CommandLine
    {
        public string Text { get; set; }

        public CommandLine(string line)
        {
            Text = line;
        }

        public string Function
        {
            get
            {
                var m = Regex.Match(Text.Trim() + " ", @"^(.+?)(?:\s+|$)(.*)");
                return m.Success ? m.Groups[1].Value.Trim() : String.Empty;
            }
        }

        public List<string> Arguments
        {
            get
            {
                var args = new List<string>();
                var m = Regex.Match(Text.Trim() + " ", @"^(.+?)(?:\s+|$)(.*)");
                var argsLine = m.Groups[2].Value.Trim();
                var m2 = Regex.Match(argsLine + " ", @"(?<!\\)"".*?(?<!\\)""|[\S]+");
                while (m2.Success)
                {
                    var arg = Regex.Replace(m2.Value.Trim(), @"^""(.*?)""$", "$1");
                    args.Add(arg);
                    m2 = m2.NextMatch();
                }
                return args;
            }
        }
    }
}
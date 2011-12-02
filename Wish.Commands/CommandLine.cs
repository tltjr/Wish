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
                var match = Regex.Match(Text.Trim() + " ", @"^(.+?)(?:\s+|$)(.*)");
                var argsLine = match.Groups[2].Value.Trim();
                string pattern; 
                if(argsLine.Contains("'"))
                {
                    pattern = @"[^\s']+|'[^']*'[^\s]+|'[^']*'";
                }
                else
                {
                    pattern = argsLine.Contains("\"") ? @"[^\s""]+|""[^""]*""[^\s]+|""[^""]*""" : @"(?<!\\)"".*?(?<!\\)""|[\S]+";
                }

                var argMatches = Regex.Match(argsLine + " ", pattern);
                while (argMatches.Success)
                {
                    var arg = Regex.Replace(argMatches.Value.Trim(), @"^""(.*?)""$", "$1");
                    args.Add(arg);
                    argMatches = argMatches.NextMatch();
                }
                return args;
            }
        }
    }
}
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Terminal {
	public static class TerminalUtils {
		/// <summary>
		/// Parses a full command line and returns a Command object
		/// containing the command name as well as the different arguments.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static Command ParseCommandLine(string line) {
			var command = "";
			var args = new List<string>();

			var m = Regex.Match(line.Trim() + " ", @"^(.+?)(?:\s+|$)(.*)");
			if (m.Success) {
				command = m.Groups[1].Value.Trim();
				var argsLine = m.Groups[2].Value.Trim();
				var m2 = Regex.Match(argsLine + " ", @"(?<!\\)"".*?(?<!\\)""|[\S]+");
				while (m2.Success) {
					var arg = Regex.Replace(m2.Value.Trim(), @"^""(.*?)""$", "$1");
					args.Add(arg);
					m2 = m2.NextMatch();
				}
			}

			return new Command(line, command, args.ToArray());
		}
	}
}

using System;
using System.IO;
using Wish.Commands;
using Wish.Commands.Runner;

namespace Wish.Scripts
{
    public class Profile
    {
        private readonly IRunner _runner;
        private readonly string _home;
        private const string RcFile = "_wishrc";
        private readonly string _profile;
        public bool Exists { get; set; }

        public Profile(IRunner runner)
        {
            _runner = runner;
            _home = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            if (string.IsNullOrEmpty(_home)) return;
            _profile = Path.Combine(_home, RcFile);
            var fi = new FileInfo(_profile);
            Exists = fi.Exists;
        }

        public string Load(string text)
        {
            if (!Exists) return string.Empty;
            return _runner.Execute(new RunnerArgs {Script = File.ReadAllText(_profile)});
        }
    }
}

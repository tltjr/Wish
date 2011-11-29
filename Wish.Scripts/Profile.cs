using System;
using System.IO;
using Wish.Commands.Runner;

namespace Wish.Scripts
{
    public class Profile
    {
        private readonly Powershell _powershell;
        private readonly string _home;
        private const string RcFile = "_wishrc";
        private readonly string _profile;
        public bool Exists { get; set; }

        public Profile()
        {
            _powershell = new Powershell();
            _home = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            if (string.IsNullOrEmpty(_home)) return;
            _profile = Path.Combine(_home, RcFile);
            var fi = new FileInfo(_profile);
            Exists = fi.Exists;
        }

        public string Load(string text)
        {
            if (!Exists) return string.Empty;
            return _powershell.Execute(File.ReadAllText(_profile));
        }
    }
}

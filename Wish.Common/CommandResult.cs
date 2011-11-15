using System;

namespace Wish.Common
{
    public class CommandResult
    {
        public bool IsExit { get; set; }
        public bool Handled { get; set; }
        public string Text { get; set; }
        public string Error { get; set; }
        public string WorkingDirectory { get; set; }
        public int PromptLength { get; set; }
    }
}

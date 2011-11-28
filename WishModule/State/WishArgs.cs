using System;
using System.Windows.Input;
using ICSharpCode.AvalonEdit;

namespace Wish.State
{
    public class WishArgs
    {
        public TextEditor TextEditor { get; set; }
        public Action OnClosed { get; set; }
        public Action Execute { get; set; }
        public Key Key { get; set; }
        public string WorkingDirectory { get; set; }
    }
}

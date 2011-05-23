using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Media;
using CodeBoxControl;
using CodeBoxControl.Decorations;

namespace Wish.Core
{
    public class SyntaxHighlighter
    {
        private RegexDecoration _user;

        public SyntaxHighlighter()
        {
            var reg = WindowsIdentity.GetCurrent().Name;
            _user = new RegexDecoration
                        {
                            DecorationType = EDecorationType.TextColor,
                            Brush = new SolidColorBrush(Colors.Green),
                            RegexString = reg
                        };
        }

        public void Highlight(CodeBox textBox)
        {
            textBox.Decorations.Clear();
            textBox.Decorations.Add(_user);
        }
    }
}

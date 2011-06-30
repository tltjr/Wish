using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Highlighting;

namespace Wish.ViewModel
{
    public class WishViewModel : ViewModelBase
    {
        private WishModel _wish;
        private string _title;
        private string _text;
        private IHighlightingDefinition _syntaxHighlighting;

        public WishViewModel()
        {
            _wish = new WishModel();
            _syntaxHighlighting = _wish.SyntaxHighlighting
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public IHighlightingDefinition SyntaxHighlighting
        {
            get { return _syntaxHighlighting; }
            set
            {
                if (_syntaxHighlighting == value) return;
                _syntaxHighlighting = value;
                OnPropertyChanged("SyntaxHighlighting");
            }
        }

        public void KeyPress(KeyEventArgs keyEventArgs)
        {
            _wish.KeyPress(keyEventArgs);
        }
    }
}

using System;
using System.ComponentModel;

namespace Wish.Models
{
    public class Terminal : INotifyPropertyChanged
    {
        private string _text;
        private string _prompt = @"C:\Users\tlthorn1>";

        public event PropertyChangedEventHandler PropertyChanged;

        public Terminal()
        {
            _text = _prompt;
        }

        public string Prompt
        {
            get { return _prompt; }
            set { _prompt = value; }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if(value != _text)
                {
                    _text = value;
                    if(PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                    }
                }
            }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string FontFamily
        {
            get { return "Consolas"; }
        }
    }
}

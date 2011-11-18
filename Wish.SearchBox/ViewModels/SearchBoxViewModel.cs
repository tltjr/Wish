using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Wish.Extensions;

namespace Wish.SearchBox.ViewModels
{
    public abstract class SearchBoxViewModel : INotifyPropertyChanged
    {
        public IEnumerable<string> BaseCollection { get; set; }
        public abstract string Color { get; }
        public abstract string Text { get; }
        public abstract void HandleEnter(KeyEventArgs e, Action<string> onSelection, string selected);
        private string _queryText;

        public string QueryText
        {
            get { return _queryText; }
            set
            {
                if (_queryText == value) return;
                _queryText = value;
                OnPropertyChanged("QueryText");
                OnPropertyChanged("QueryCollection");
            }
        }

        public IEnumerable QueryCollection
        {
            get
            {
                var matches = QueryText != null ? BaseCollection.Where(o => o.Contains(QueryText, StringComparison.InvariantCultureIgnoreCase)) : BaseCollection;
                return matches;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}

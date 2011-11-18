using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Wish.SearchBox.ViewModels
{
    public class TabCompletionViewModel : INotifyPropertyChanged
    {
        private readonly List<string> _waitMessage = new List<string> { "Please Wait..." };
        public IEnumerable WaitMessage { get { return _waitMessage; } }

        private string _queryText;

        public TabCompletionViewModel(IEnumerable<string> history)
        {
            BaseCollection = history;
        }

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
                var matches = QueryText != null ? BaseCollection.Where(o => o.Contains(QueryText)) : BaseCollection;
                return matches;
            }
        }

        public IEnumerable<string> BaseCollection { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wish
{
    public class ActbViewModel : INotifyPropertyChanged
    {
        private readonly List<string> _waitMessage = new List<string>() { "Please Wait..." };
        public IEnumerable WaitMessage { get { return _waitMessage; } }

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
                //var result = CommandHistory.Commands.Select(o => o.Raw);
                //return QueryText != null ? result.Where(o => o.Contains(QueryText)) : result;
                return null;
            }
        }

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

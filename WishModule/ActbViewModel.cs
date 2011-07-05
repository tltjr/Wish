using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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
                if (_queryText != value)
                {
                    _queryText = value;
                    OnPropertyChanged("QueryText");
                    _queryCollection = null;
                    OnPropertyChanged("QueryCollection");
                }
            }
        }

        public IEnumerable _queryCollection = null;
        public IEnumerable QueryCollection
        {
            get
            {
                QueryGoogle(QueryText);
                return _queryCollection;
            }
        }

        private void QueryGoogle(string searchTerm)
        {
            var result = new List<string>();
            result.Add("ls");
            result.Add("dir");
            result.Add("cd elsewhere");
            _queryCollection = searchTerm != null ? result.Where(o => o.Contains(searchTerm)) : result;
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

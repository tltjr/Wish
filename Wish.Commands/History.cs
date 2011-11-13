using System.Collections.Generic;

namespace Wish.Commands
{
    public class History
    {
        private IList<ICommand> _history = new List<ICommand>();
        private int _index = -1;

        public ICommand Up()
        {
            if(_history.Count > 0)
            {
                _index++;
                if(_index > _history.Count - 1)
                {
                    _index = -1;
                }
                if (_index == -1)
                {
                    return new Command(string.Empty);
                }
                return _history[_index];
            }
            return null;
        }

        public void Add(ICommand command)
        {
            _history.Insert(0, command);
        }

        public ICommand Down()
        {
            _index--;
            if (_index == -1)
            {
                return new Command(string.Empty);
            }
            if (_index < -1)
            {
                _index = _history.Count - 1;
            }
            return _history[_index];
        }
    }
}

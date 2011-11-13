using System;
using System.Collections.Generic;

namespace Wish.Commands
{
    public class History
    {
        private readonly IList<ICommand> _history = new List<ICommand>();
        // public for testing purposes only
        public int Index = -1;

        public ICommand Up()
        {
            if(_history.Count > 0)
            {
                Index++;
                if(Index > _history.Count - 1)
                {
                    Index = -1;
                }
                if (Index == -1)
                {
                    return new Command(string.Empty);
                }
                return _history[Index];
            }
            return null;
        }

        public void Add(ICommand command)
        {
            _history.Insert(0, command);
        }

        public ICommand Down()
        {
            Index--;
            if (Index == -1)
            {
                return new Command(string.Empty);
            }
            if (Index < -1)
            {
                Index = _history.Count - 1;
            }
            return _history[Index];
        }

        public void Reset()
        {
            Index = -1;
        }
    }
}

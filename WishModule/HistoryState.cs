using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Wish
{
    public class HistoryState : IState
    {
        private Wish _wish;

        public HistoryState(Wish wish)
        {
            _wish = wish;
        }

        public void KeyPress(KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

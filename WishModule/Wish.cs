using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Wish
{
    public class Wish
    {
        public IState Standard { get; set; }
        public IState Complete { get; set; }
        public IState History { get; set; }

        public IState State { get; set; }

        public Wish()
        {
            Standard = new StandardState(this);
            Complete = new CompleteState(this);
            History = new HistoryState(this);
            State = Standard;
        }

        public void KeyPress(KeyEventArgs e)
        {
            State.KeyPress(e);
        }

    }
}

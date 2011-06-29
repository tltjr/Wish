using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Wish
{
    public class StandardState : IState
    {
        private readonly Wish _wish;

        public StandardState(Wish wish)
        {
            _wish = wish;
        }

        public void KeyPress(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    _wish.State = _wish.Complete;
                    break;
            }
        }
    }
}

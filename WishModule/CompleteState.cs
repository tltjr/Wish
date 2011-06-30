using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Wish
{
    public class CompleteState : IState
    {
        private readonly WishModel _wish;

        public CompleteState(WishModel wish)
        {
            _wish = wish;
        }

        public void KeyPress(KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                _wish.State = _wish.Standard;
            }
        }
    }
}

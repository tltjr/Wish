using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Wish
{
    public class CompleteState : IState
    {
        private Wish _wish;

        public CompleteState(Wish wish)
        {
            _wish = wish;
        }

        public void KeyPress(KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wish.Core;

namespace Wish
{
    public class DefaultStrategy : IKeyStrategy
    {
        public void Handle(KeyEventArgs e)
        {
            CommandHistory.Reset();
        }
    }
}

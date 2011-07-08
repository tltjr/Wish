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
        public string Handle(KeyEventArgs e)
        {
            CommandHistory.Reset();
            return String.Empty;
        }
    }
}

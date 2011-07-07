using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Wish
{
    public interface IKeyStrategy
    {
        void Handle(KeyEventArgs e);
    }
}

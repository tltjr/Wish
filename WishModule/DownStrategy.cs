using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wish.Core;

namespace Wish
{
    public class DownStrategy : IKeyStrategy
    {
        private readonly WishModel _wishModel;

        public DownStrategy(WishModel wishModel)
        {
            _wishModel = wishModel;
        }

        public void Handle(KeyEventArgs e)
        {
            if (_wishModel.ActivelyTabbing) return;
            var previous = CommandHistory.GetPrevious();
            _wishModel.ReplaceLine(previous);
        }
    }
}

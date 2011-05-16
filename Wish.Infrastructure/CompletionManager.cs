using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace Wish.Infrastructure
{
    public class CompletionManager
    {
        private string _input;

        public CompletionManager(string input)
        {
            _input = input;
        }

        public void DoStuff()
        {
        }
    }
}

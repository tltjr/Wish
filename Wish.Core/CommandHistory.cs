using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wish.Core
{
    public static class CommandHistory
    {
        private static List<Command> _commands = new List<Command>();
        private static int _index;

        public static List<Command> Commands
        {
            get { return _commands; }
        }

        public static void Add(Command command)
        {
            _commands.Insert(0, command);
        }

        public static Command GetNext()
        {
            if (_commands.Count == 0) return null;
            if(0 == _index)
            {
                _commands.Insert(0, new Command("", "", null));
            }
            _index++;
            if(_index >= _commands.Count)
            {
                _index = 0;
            }
            return _commands[_index];
        }

        public static Command GetPrevious()
        {
            if (_commands.Count == 0) return null;
            _index--;
            if(_index < 0)
            {
                _index = _commands.Count - 1;
            }
            return _commands[_index];
        }

        public static void Reset()
        {
            _commands = _commands.Where(o => !(String.IsNullOrEmpty(o.Name))).ToList();
            _index = 0;
        }
    }
}

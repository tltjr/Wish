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
        private static bool _dirty;

        public static List<Command> Commands
        {
            get { return _commands; }
        }

        public static void Add(Command command)
        {
            _dirty = true;
            if(_commands.Any(o => o.Raw.Equals(command.Raw))) return;
            _commands.Insert(0, command);
        }

        public static Command GetNext()
        {
            _dirty = true;
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
            _dirty = true;
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
            if (!_dirty) return;
            _commands = _commands.Where(o => !(String.IsNullOrEmpty(o.Name))).ToList();
            _index = 0;
            _dirty = false;
        }
    }
}

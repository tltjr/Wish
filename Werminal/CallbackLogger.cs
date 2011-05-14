using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Logging;

namespace Werminal
{
    public class CallbackLogger : ILoggerFacade
    {
        private readonly Queue<Tuple<string, Category, Priority>> _savedLogs =
            new Queue<Tuple<string, Category, Priority>>();

        public Action<string, Category, Priority> Callback { get; set; }

        #region ILoggerFacade Members

        public void Log(string message, Category category, Priority priority)
        {
            if (Callback != null)
            {
                Callback(message, category, priority);
            }
            else
            {
                _savedLogs.Enqueue(new Tuple<string, Category, Priority>(message, category, priority));
            }
        }

        #endregion

        public void ReplaySavedLogs()
        {
            if (Callback == null) return;
            while (_savedLogs.Count > 0)
            {
                var log = _savedLogs.Dequeue();
                Callback(log.Item1, log.Item2, log.Item3);
            }
        }
    }
}
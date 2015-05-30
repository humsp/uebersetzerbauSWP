using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.Console
{
    public class ClassLogger : Logger
    {

        private string _className;

        public ClassLogger(string className)
        {
            _className = className;
        }

        public void Log(string text, LogEvent logEvent)
        {
            if (Logger.ActiveLogEvents.Contains(logEvent))
            {
                foreach (LogWriter writer in Logger.LogWriter)
                {
                    writer.Log(logEvent + " - " + _className + ": " + text);
                }
            }
        }
    }
}

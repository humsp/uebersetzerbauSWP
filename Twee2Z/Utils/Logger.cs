using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.Utils
{
    public class Logger
    {
        public enum LogEvent
        {
            Analyzer,
            ObjectTree,
            Validation,
            CodeGen,
            Warning,
            Error,
            Debug,
            UserOutput
        }

        public static void LogAnalyzer(string text)
        {
            Log(text, LogEvent.Analyzer);
        }

        public static void LogObjectTree(string text)
        {
            Log(text, LogEvent.ObjectTree);
        }

        public static void LogValidation(string text)
        {
            Log(text, LogEvent.Validation);
        }

        public static void LogCodeGen(string text)
        {
            Log(text, LogEvent.CodeGen);
        }

        public static void LogWarning(string text)
        {
            Log(text, LogEvent.Warning);
        }

        public static void LogError(string text)
        {
            Log( text, LogEvent.Error);
        }

        public static void LogError(string text, Exception exception)
        {
            Log(text, LogEvent.Error, exception);
        }

        public static void LogDebug(string text)
        {
            Log(text, LogEvent.Debug);
        }

        public static void LogUserOutput(string text)
        {
            Log(text, LogEvent.UserOutput);
        }

        static HashSet<LogEvent> _activeLogEvents = new HashSet<LogEvent>();
        static List<LogWriter> _logWriter = new List<LogWriter>();

        public static void LogAll(){
            Logger.AddLogEvent(Logger.LogEvent.Analyzer);
            Logger.AddLogEvent(Logger.LogEvent.ObjectTree);
            Logger.AddLogEvent(Logger.LogEvent.Validation);
            Logger.AddLogEvent(Logger.LogEvent.CodeGen);
            Logger.AddLogEvent(Logger.LogEvent.Warning);
            Logger.AddLogEvent(Logger.LogEvent.Error);
            Logger.AddLogEvent(Logger.LogEvent.Debug); 
            Logger.AddLogEvent(Logger.LogEvent.UserOutput);
        }

        public static void UseConsoleLogWriter()
        {
            AddLogWriter(new LogWriter());
        }

        public static void AddLogEvent(LogEvent logEvent)
        {
            if (!_activeLogEvents.Contains(logEvent))
            {
                _activeLogEvents.Add(logEvent);
            }
        }

        public static void RemoveLogEvent(LogEvent logEvent)
        {
            if (_activeLogEvents.Contains(logEvent))
            {
                _activeLogEvents.Remove(logEvent);
            }
        }

        public static void AddLogWriter(LogWriter logWriter)
        {
            if (!_logWriter.Contains(logWriter))
            {
                _logWriter.Add(logWriter);
            }
        }

        public static void RemoveLogWriter(LogWriter logWriter)
        {
            if (_logWriter.Contains(logWriter))
            {
                _logWriter.Remove(logWriter);
            }
        }

        public static void Log(string text, LogEvent logEvent)
        {
            if (_activeLogEvents.Contains(logEvent))
            {
                foreach (LogWriter writer in _logWriter)
                {
                    writer.Log(logEvent + ": " + text);
                }
            }
        }

        public static void Log(string text, LogEvent logEvent, Exception exception)
        {
            if (_activeLogEvents.Contains(logEvent))
            {
                foreach (LogWriter writer in _logWriter)
                {
                    writer.Log(logEvent + ": " + text + " - Exception: " + exception.StackTrace);
                }
            }
        }

        public static HashSet<LogEvent> ActiveLogEvents
        {
            get
            {
                return _activeLogEvents;
            }
        }

        public static List<LogWriter> LogWriter
        {
            get
            {
                return _logWriter;
            }
        }
    }
}

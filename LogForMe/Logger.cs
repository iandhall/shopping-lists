using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LogForMe
{
    public enum Levels
    {
        None,
        Error,
        Warning,
        Info,
        Debug
    }

    public static class Logger
    {
        private static readonly object locker = new object();
        public static string FileName { get; set; }
        public static bool WriteToTrace { get; set; }
        public static Levels Level { get; set; }

        static Logger()
        {
            string levelSetting = ConfigurationManager.AppSettings["Logger.Log.Level"];
            string fileNameSetting = ConfigurationManager.AppSettings["Logger.Log.FileName"];
            string writeToTraceSetting = ConfigurationManager.AppSettings["Logger.Log.WriteToTrace"];
            Levels level;
            if (!string.IsNullOrWhiteSpace(levelSetting))
            {
                if (Enum.TryParse<Levels>(levelSetting, out level))
                {
                    Level = level;
                }
            }
            if (!string.IsNullOrWhiteSpace(fileNameSetting))
            {
                FileName = fileNameSetting;
            }
            if (!string.IsNullOrWhiteSpace(writeToTraceSetting))
            {
                WriteToTrace = bool.Parse(writeToTraceSetting);
            }
        }

        public static void Debug(string entry, params object[] args)
        {
            if (Level < Levels.Debug) return;
            LogEntry(true, Levels.Debug, entry, args);
        }

        public static void Debug(bool outputMethodDetails, string entry, params object[] args)
        {
            if (Level < Levels.Debug) return;
            LogEntry(outputMethodDetails, Levels.Debug, entry, args);
        }

        public static void Info(string entry, params object[] args)
        {
            if (Level < Levels.Info) return;
            LogEntry(true, Levels.Info, entry, args);
        }

        public static void Warning(string entry, params object[] args)
        {
            if (Level < Levels.Warning) return;
            LogEntry(true, Levels.Warning, entry, args);

        }
        public static void Error(Exception ex)
        {
            if (Level < Levels.Error) return;
            LogEntry(true, Levels.Error, ex.ToString());
        }

        private static void LogEntry(bool outputMethodDetails, Levels level, string entry, params object[] args)
        {
            lock (locker)
            {
                string logDir = Path.GetDirectoryName(FileName);
                if (logDir.Length != 0)
                {
                    if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                }
                string methodDetails = "";
                if (outputMethodDetails)
                {
                    var methodBase = new StackTrace().GetFrame(2).GetMethod();
                    methodDetails = " " + methodBase.ReflectedType.FullName + "." + methodBase.Name;
                }
                if (args.Length == 0)
                {
                    entry = entry.Replace("{", "{{").Replace("}", "}}");
                }
                using (StreamWriter writer = File.AppendText(FileName))
                {
                    var expandedEntry = string.Format(
                        "{0} {1}{2} {3}",
                        DateTime.Now.ToString("yyyy-MM-dd H:mm:ss fff"),
                        level.ToString(),
                        methodDetails,
                        string.Format(entry, args)
                    );
                    writer.WriteLine(expandedEntry);
                    writer.Flush();
                    if (WriteToTrace) Trace.WriteLine(expandedEntry);
                }
            }
        }
    }
}

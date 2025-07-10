//Reference: http://www.michaelhamrah.com/blog/2010/02/performance-tracing-for-your-applications-via-enterprise-library/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace YMCAObjects
{
    public interface IPerformanceTracer
    {
        void WriteElapsedTime(string title, string message);
        void WriteInfo(string title, string message);
        string EndTrace();
    }

    public class PerformanceTracer : IDisposable, IPerformanceTracer
    {
        private Stopwatch _sw;
        private List<string> _perfMessages;
        private List<string> _infoMessages;
        private string _title;
        private LogWriter _writer;
        private long _lastTime;
        private bool _eventLogEnabled = false;

        /// <summary>
        /// Creates a new instance of a performance tracer. An EventLog entry is automatically written to the event log when this object is disposed. An instance should usually be declared in a using() statement.
        /// </summary>
        /// <param name="stName">The name of this performance trace</param>
        public PerformanceTracer(string stName)
        {
            if (string.IsNullOrEmpty(stName))
                throw new ArgumentNullException("Operation name can't be null or empty when instantiating the Performance Tracer");

            _sw = new Stopwatch();
            _perfMessages = new List<string>();
            _infoMessages = new List<string>();
            _title = stName;
            try
            {
                _writer = Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Writer;
	           	_sw.Start();
                _eventLogEnabled = true;	//if everything initialized properly then set the eventLogEnabled property to true
            }
            catch { } // ignore exception if no logging available.
        }
        /// <summary>
        /// Writes a message to the log with the elapsed time since the performance instance was created.
        /// </summary>
        /// <param name="stTitle">The module name or the function name from where the message is being generated</param>
        /// <param name="stMessage">The message to be used in the trace</param>
        public void WriteElapsedTime(string stTitle, string stMessage)
        {
            var current = _sw.ElapsedMilliseconds;
            var diff = current - _lastTime;

            _perfMessages.Add(string.Format("{0}:{1}: {2}: {3}", current.ToString().PadLeft(4), diff.ToString().PadLeft(4), stTitle, stMessage));
            _lastTime = current;
        }
        /// <summary>
        /// Writes information without a timestamp; useful for auxilary information.
        /// </summary>
        /// <param name="stTitle">The module name or the function name from where the message is being generated</param>
        /// <param name="stMessage">The message to be used in the trace</param>
        public void WriteInfo(string stTitle, string stMessage)
        {
            _infoMessages.Add(string.Format("{0}: {1}", stTitle, stMessage));
        }
        public void Dispose()
        {
            EndTrace();
        }

        public string EndTrace()
        {
            _sw.Stop();

            this.WriteElapsedTime(_title, "Complete");
            var sb = new StringBuilder();

            sb.AppendLine("Timers:");
            sb.AppendLine(string.Join(Environment.NewLine, _perfMessages.ToArray()));
            sb.AppendLine("Info:");
            sb.AppendLine(string.Join(Environment.NewLine, _infoMessages.ToArray()));

            if (IsTracingEnabled())
            {
                var logEntry = new LogEntry();
                logEntry.Title = _title;
                logEntry.Message = sb.ToString();
                logEntry.Categories = new List<string>();
                logEntry.Categories.Add("Performance");
                logEntry.Severity = TraceEventType.Verbose;

                _writer.Write(logEntry);
            }

            return sb.ToString();
        }

        public bool IsTracingEnabled()
        {
            return _eventLogEnabled && _writer != null && _writer.IsTracingEnabled();
        }
    }

	/// <summary>
	/// Provides helper methods for a performance tracer to be used in a web application. Each request instantiates a new tracer object which is stored in the Current 
	/// </summary>
    public static class WebPerformanceTracer
    {
        /// <summary>
        /// Writes a message to the log with the elapsed time since the performance instance was created.
        /// </summary>
        /// <param name="stTitle">The module name or the function name from where the message is being generated</param>
        /// <param name="stMessage">The message to be used in the trace</param>
        public static void LogPerformanceTrace(string stTitle, string stMessage)
        {
            IPerformanceTracer value;
            if (System.Web.HttpContext.Current == null) return;
            lock (System.Web.HttpContext.Current)
            {
                value = (IPerformanceTracer)System.Web.HttpContext.Current.Items["PerformanceTracer"];
                if (value == null)
                {
                    value = new PerformanceTracer("SessionID=" + System.Web.HttpContext.Current.Session.SessionID + ", URL=" + System.Web.HttpContext.Current.Request.RawUrl);
                    System.Web.HttpContext.Current.Items["PerformanceTracer"] = value;
                }
            }
            value.WriteElapsedTime(stTitle, stMessage);
        }
        /// <summary>
        /// Writes information without a timestamp; useful for auxilary information.
        /// </summary>
        /// <param name="stTitle">The module name or the function name from where the message is being generated</param>
        /// <param name="stMessage">The message to be used in the trace</param>
        public static void LogInfoTrace(string stTitle, string stMessage)
        {
            IPerformanceTracer value;
            if (System.Web.HttpContext.Current == null) return;
            lock (System.Web.HttpContext.Current)
            {
                value = (IPerformanceTracer)System.Web.HttpContext.Current.Items["PerformanceTracer"];
                if (value == null)
                {
                    value = new PerformanceTracer("SessionID=" + System.Web.HttpContext.Current.Session.SessionID + ", URL=" + System.Web.HttpContext.Current.Request.RawUrl);
                    System.Web.HttpContext.Current.Items["PerformanceTracer"] = value;
                }
            }
            value.WriteInfo(stTitle, stMessage);
        }
        /// <summary>
        /// Writes all the information from the trace into the log file and also returns the same to the caller. 
        /// This method has to be called at the end of the request. If not called then the log will be written 
        /// when the object is destroyed by the GC which is a non-deterministic event. Calling the LogInfoTrace 
        /// or the LogPerformanceTrace methods after calling this methods initializes a new trace object.
        /// </summary>
        public static string EndTrace()
        {
            IPerformanceTracer value;
            if (System.Web.HttpContext.Current == null) return string.Empty;
            lock (System.Web.HttpContext.Current)
            {
                value = (IPerformanceTracer)System.Web.HttpContext.Current.Items["PerformanceTracer"];
                System.Web.HttpContext.Current.Items["PerformanceTracer"] = null;
            }
            if (value != null) {
                return value.EndTrace();
            } else {
                return string.Empty;
            }
        }
    }
}

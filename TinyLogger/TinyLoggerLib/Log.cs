/* Copyright 2011 Masson Studio

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

   author: Masson
      see: http://www.imasson.com/
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMasson.Util.TinyLogger.Recorder;

namespace IMasson.Util.TinyLogger
{
    /// <summary>
    /// The enterance class for logging task.
    /// This class is a singletonand class that allows developers to have more customability. 
    /// But I recommand that the only thing we need to do is use the static log method, and no
    /// need to have any configuration. Logger in the basic setting can function well.
    /// </summary>
    public class Log
    {
        /// <summary>The chars to split string into lines</summary>
        public readonly string[] ReturnStrings = { Environment.NewLine };

        private static Log instance;

        /// <summary>
        /// Get the singletonand instance of Log 
        /// </summary>
        /// <returns>the singletonand instance of Log</returns>
        public static Log GetInstance()
        {
            if (instance == null)
            {
                instance = new Log();
            }

            return instance;
        }

        /// <summary>
        /// Set the singletonand instance of Log. 
        /// Please caution when you set your own logger. 
        /// After setting new logger, the old one will be stop and free.
        /// </summary>
        public static void SetInstance(Log newLog)
        {
            if (newLog != null)
            {
                if (instance != null)
                {
                    instance.Shutdown();
                }
                instance = newLog;
            }
        }


        #region Public static methods to log

        /// <summary>
        /// Log a message in Verbose level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        public static void V(string tag, string message)
        {
            GetInstance().AddLog(LogLevel.Verbose, tag, message);
        }

        /// <summary>
        /// Log a message with exception in Verbose level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        /// <param name="ex">An exception to log </param>
        public static void V(string tag, string message, Exception ex)
        {
            GetInstance().AddLog(LogLevel.Verbose, tag, message);
            GetInstance().AddExceptionLog(LogLevel.Verbose, tag, ex);
        }


        /// <summary>
        /// Log a message in Debug level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        public static void D(string tag, string message)
        {
            GetInstance().AddLog(LogLevel.Debug, tag, message);
        }

        /// <summary>
        /// Log a message with exception in Debug level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        /// <param name="ex">An exception to log </param>
        public static void D(string tag, string message, Exception ex)
        {
            GetInstance().AddLog(LogLevel.Debug, tag, message);
            GetInstance().AddExceptionLog(LogLevel.Debug, tag, ex);
        }


        /// <summary>
        /// Log a message in Info level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        public static void I(string tag, string message)
        {
            GetInstance().AddLog(LogLevel.Info, tag, message);
        }

        /// <summary>
        /// Log a message with exception in Info level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        /// <param name="ex">An exception to log </param>
        public static void I(string tag, string message, Exception ex)
        {
            GetInstance().AddLog(LogLevel.Info, tag, message);
            GetInstance().AddExceptionLog(LogLevel.Info, tag, ex);
        }


        /// <summary>
        /// Log a message in Warning level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        public static void W(string tag, string message)
        {
            GetInstance().AddLog(LogLevel.Warning, tag, message);
        }

        /// <summary>
        /// Log a message with exception in Warning level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        /// <param name="ex">An exception to log </param>
        public static void W(string tag, string message, Exception ex)
        {
            GetInstance().AddLog(LogLevel.Warning, tag, message);
            GetInstance().AddExceptionLog(LogLevel.Warning, tag, ex);
        }


        /// <summary>
        /// Log a message in Error level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        public static void E(string tag, string message)
        {
            GetInstance().AddLog(LogLevel.Error, tag, message);
        }

        /// <summary>
        /// Log a message with exception in Error level
        /// </summary>
        /// <param name="tag">the identifer for this kind of message. 
        /// It usually identifies the class or module where the log call occurs.</param>
        /// <param name="message">The message you would like logged. </param>
        /// <param name="ex">An exception to log </param>
        public static void E(string tag, string message, Exception ex)
        {
            GetInstance().AddLog(LogLevel.Error, tag, message);
            GetInstance().AddExceptionLog(LogLevel.Error, tag, ex);
        }

        #endregion


        private LogRecorder[]   _recorders      = null;
        private int             _maxTraceDeep   = 6;
        private int             _minLogLevel    = (int)LogLevel.Verbose;

        private HashSet<LogRecorder> _recorderSet = null;
        
        /// <summary>
        /// The maximal line count when logging the stack trace of an exception. 
        /// The defalut value is 5, and no need to changed in generally.
        /// </summary>
        public int MaxTraceDeep
        {
            get { return _maxTraceDeep; }
            set { _maxTraceDeep = value; }
        }

        /// <summary>
        /// The minimal level to log.
        /// The default value is Verbose, it means log messages in all levels can be log.
        /// So you can use this property as switcher to filter low-improtance 
        /// log messages and improve performance of the app.
        /// </summary>
        public LogLevel MinLogLevel
        {
            get { return (LogLevel)_minLogLevel; }
            set { _minLogLevel = (int)value; }
        }


        /// <summary>
        /// Construct a new Log without any recorders
        /// </summary>
        public Log()
        {
            // initilize recorders base on enum LogLevel
            int levelCount = Enum.GetValues(typeof(LogLevel)).Length;
            if (levelCount < 6) levelCount = 6;  // all the basic levels must be covered
            _recorders = new LogRecorder[levelCount];
            _recorderSet = new HashSet<LogRecorder>();
        }


        #region Public instance methord to handle recorders and log

        /// <summary>
        /// Get the recorder of the special level. 
        /// Return null if there is no recorder on the level.
        /// </summary>
        /// <param name="level">The special log level</param>
        /// <returns>The recorder of special level</returns>
        public LogRecorder GetRecorder(LogLevel level)
        {
            return _recorders[(int)level];
        }

        /// <summary>
        /// Set the recorder of the special level.
        /// </summary>
        /// <param name="level">The special log level</param>
        /// <param name="recoder">The recorder of the special level to set. 
        /// Set null if you want to remove the recorder.</param>
        public void SetRecorder(LogLevel level, LogRecorder recoder)
        {
            _recorders[(int)level] = recoder;
            _recorderSet.Add(recoder);
        }


        /// <summary>
        /// Log a single text message
        /// </summary>
        /// <param name="level">The special log level</param>
        /// <param name="tag">The tag of the log message</param>
        /// <param name="message">The content of the log message</param>
        public void AddLog(LogLevel level, string tag, string message)
        {
            // check the params first
            if (string.IsNullOrEmpty(tag)) return;
            if (string.IsNullOrEmpty(message)) return;

            // check the target level limit
            if ((int)level < _minLogLevel) return;

            // log the message into the recorder of target level (if exist)
            LogRecorder recorder = _recorders[(int)level];
            if (recorder != null)
            {
                recorder.Log(new LogMessage(level, tag, message));
            }
        }

        /// <summary>
        /// Log the message and stack trace of an exception
        /// </summary>
        /// <param name="level">The special log level</param>
        /// <param name="tag">The tag of the log message</param>
        /// <param name="ex">The exception object to log</param>
        public void AddExceptionLog(LogLevel level, string tag, Exception ex)
        {
            // check the params first
            if (string.IsNullOrEmpty(tag)) return;
            if (ex == null) return;

            // check the target level limit
            if ((int)level < _minLogLevel) return;

            // log the message and stack trace of the exception into the recorder 
            // of target level (if exist)
            LogRecorder recorder = _recorders[(int)level];
            if (recorder != null)
            {
                recorder.Log(new LogMessage(level, tag, ex.Message));

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    string[] traceLines = ex.StackTrace.Split(ReturnStrings, _maxTraceDeep,
                        StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in traceLines)
                    {
                        recorder.Log(new LogMessage(level, tag, line));
                    }
                }
            }
        }


        /// <summary>
        /// Stop every recorder in this logger. And every remaining logs in these recorder 
        /// will record to the target before the recorder stop.
        /// Please caution this operation. Normally, we don't need to shutdown manually.
        /// </summary>
        public void Shutdown()
        {
            foreach (LogRecorder recorder in _recorderSet)
            {
                if (recorder != null)
                {
                    if (recorder.Alive) recorder.Stop();
                }
            }
        }

        #endregion
    }
}

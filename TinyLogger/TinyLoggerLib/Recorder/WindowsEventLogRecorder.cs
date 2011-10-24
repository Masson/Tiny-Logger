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
using System.Diagnostics;

namespace IMasson.Util.TinyLogger.Recorder
{
    /// <summary>
    /// A LogRecorder that can output the log message to windows event.
    /// Before use this recorder you must assign a event source name.
    /// Please check out <see cref="System.Diagnostics.EventLog"/> or windows api 
    /// doc for more infomation.
    /// 
    /// Caution: Before use this recorder, you must get the Security Right by adding a
    /// event source name to the registry. And this name must as the same as the property
    /// <c>EventSourceName</c>. SecurityException will throw if you miss this.
    /// </summary>
    public class WindowsEventLogRecorder : LogRecorder
    {
        private const string DefaultLogName = "Application";
        private const string OutputFormat = "[{0}]{1}: {2}";

        private string _eventSourceName = "TinyLogger";

        /// <summary>
        /// The source name of the windows event
        /// </summary>
        public string EventSourceName
        {
            get { return _eventSourceName; }
            set 
            {
                if (value != null && value != string.Empty)
                {
                    _eventSourceName = value;
                }
            }
        }


        public WindowsEventLogRecorder()
        {
            this.Alive = true;
        }


        /// <summary>
        /// Record the log message to Windows Events.
        /// </summary>
        /// <param name="logMessage">The log message object to record</param>
        protected internal override void Log(LogMessage logMessage)
        {
            if (!EventLog.SourceExists(_eventSourceName))
            {
                EventLog.CreateEventSource(_eventSourceName, DefaultLogName);
            }

            char logLevelSign = Enum.GetName(typeof(LogLevel), logMessage.Level)[0];
            String message = string.Format(OutputFormat, logLevelSign, logMessage.Tag, logMessage.Message);
            EventLogEntryType type = GetEventLogEntryTypeFrom(logMessage.Level);

            EventLog.WriteEntry(_eventSourceName, message, type);
        }


        protected EventLogEntryType GetEventLogEntryTypeFrom(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose: return EventLogEntryType.Information;
                case LogLevel.Debug: return EventLogEntryType.Information;
                case LogLevel.Info: return EventLogEntryType.Information;
                case LogLevel.Warning: return EventLogEntryType.Warning;
                case LogLevel.Error: return EventLogEntryType.Error;
                default: return EventLogEntryType.Information;
            }
        }

        /// <summary>
        /// Stop the log recorder in safe way.
        /// </summary>
        public override void Stop()
        {
            // nothing to do
        }

        /// <summary>
        /// Stop the log recorder in forcible way. 
        /// </summary>
        public override void StopImmediately()
        {
            // nothing to do
        }
    }
}

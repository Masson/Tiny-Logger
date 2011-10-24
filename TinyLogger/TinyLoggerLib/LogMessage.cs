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


namespace IMasson.Util.TinyLogger
{
    /// <summary>
    /// Represents a log message item.
    /// </summary>
    public sealed class LogMessage
    {
        private long _time;
        private LogLevel _level;
        private string _tag;
        private string _message;

        /// <summary>
        /// The log time of this message, in binary.
        /// </summary>
        public long Time
        {
            get { return _time; }
            internal set { _time = value; }
        }

        /// <summary>
        /// Log level of this message, see <see cref="LogLevel"/> for more infomation.
        /// </summary>
        public LogLevel Level
        {
            get { return _level; }
            internal set { _level = value; }
        }

        /// <summary>
        /// Log tag of this message. A regular tag is tidy, and no space and break allow.
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            internal set { _tag = value; }
        }

        /// <summary>
        /// Log message content. No break allow in the content.
        /// </summary>
        public string Message
        {
            get { return _message; }
            internal set { _message = value; }
        }


        /// <summary>
        /// Construct the new log message. The property Time will be setted automatically.
        /// </summary>
        /// <param name="level">The level of the new log message</param>
        /// <param name="tag">The tag of the new log message</param>
        /// <param name="message">The content the new log message</param>
        public LogMessage(LogLevel level, string tag, string message)
        {
            _level = level;
            _tag = tag;
            _message = message;
            _time = System.DateTime.Now.ToBinary();
        }
    }
}

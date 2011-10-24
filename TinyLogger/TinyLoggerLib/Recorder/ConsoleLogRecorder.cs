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

namespace IMasson.Util.TinyLogger.Recorder
{
    /// <summary>
    /// A LogRecorder that can output the log message to the system console.
    /// Actually, this is not a real LogRecorder. It's for test and demo purpose.
    /// </summary>
    public class ConsoleLogRecorder : LogRecorder
    {
        private const string OutputFormat = "--{0:yy-MM-dd hh:mm:ss} {1,-8} {2,12}: {3}";

        public ConsoleLogRecorder()
        {
            this.Alive = true;
        }

        /// <summary>
        /// Record the log message to the console.
        /// </summary>
        /// <param name="logMessage">The log message object to record</param>
        protected internal override void Log(LogMessage logMessage)
        {
            if (logMessage == null) return;

            DateTime time = DateTime.FromBinary(logMessage.Time);
            String level = Enum.GetName(typeof(LogLevel), logMessage.Level);
            Console.WriteLine(OutputFormat, time, level, logMessage.Tag, logMessage.Message);
        }

        /// <summary>
        /// Stop the log recorder in safe way.
        /// </summary>
        public override void Stop()
        {
            // nothing to do
            Console.WriteLine("ConsoleLogRecorder stopped");
        }

        /// <summary>
        /// Stop the log recorder in forcible way. 
        /// </summary>
        public override void StopImmediately()
        {
            // nothing to do
            Console.WriteLine("ConsoleLogRecorder stopped immediately");
        }
    }
}

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
    /// The abstract class of all log recorder. A recorder define how to record a 
    /// <see cref="LogMessage"/> object into the source like file or database.
    /// </summary>
    public abstract class LogRecorder
    {
        private bool _alive = false;

        /// <summary>
        /// A boolean value that represent that if this recorder is in work status.
        /// </summary>
        public virtual bool Alive
        {
            get { return _alive; }
            protected set { _alive = value; }
        }


        /// <summary>
        /// Record a log message.
        /// </summary>
        /// <param name="logMessage">The log message object to record</param>
        protected internal abstract void Log(LogMessage logMessage);

        /// <summary>
        /// Stop the log recorder in safe way.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Stop the log recorder in forcible way. The related thread may be 
        /// killed immediately. It is obviously that this methord is not recommended.
        /// </summary>
        public abstract void StopImmediately();
    }
}

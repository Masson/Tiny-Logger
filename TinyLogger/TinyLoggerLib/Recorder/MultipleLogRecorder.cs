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
    /// A special recorder can manager a list of recorders to record a log message
    /// to different source. These recorder may have different thread to handle the
    /// log messages.
    /// </summary>
    public class MultipleLogRecorder : LogRecorder
    {
        private List<LogRecorder> _recorders = new List<LogRecorder>();

        /// <summary>
        /// A boolean value that represent that if all the sub recorder are in work status.
        /// </summary>
        public override bool Alive
        {
            get
            {
                foreach (LogRecorder recorder in _recorders)
                {
                    if (recorder.Alive) return true;
                }
                return false;
            }
            protected set
            {
                base.Alive = value;
            }
        }


        /// <summary>
        /// Get the specific log recorder by index.
        /// </summary>
        /// <param name="index">The index of the target log recorder</param>
        /// <returns>The specific log recorder. 
        /// Null will be returned if index is invalid.</returns>
        public LogRecorder GetRecorder(int index)
        {
            if (0 <= index && index < _recorders.Count)
            {
                return _recorders[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Count of the recorders
        /// </summary>
        public int RecorderCount
        {
            get { return _recorders.Count; }
        }

        /// <summary>
        /// Add and regester a new log recorder into the recorder list.
        /// </summary>
        /// <param name="recorder">The new log recorder</param>
        public void AddRecorder(LogRecorder recorder)
        {
            if (recorder == null) return;

            _recorders.Add(recorder);
        }

        /// <summary>
        /// Remove the exist log recorder in the recorder list.
        /// Return false if the recorder not found.
        /// </summary>
        /// <param name="recorder">The recorder that you want to remove</param>
        /// <returns>A boolean value that represent if the recorder is removed</returns>
        public bool RemoveRecorder(LogRecorder recorder)
        {
            if (recorder == null) return false;

            return _recorders.Remove(recorder);
        }


        /// <summary>
        /// Record a log message to all the registered recorder.
        /// </summary>
        /// <param name="logMessage">The log message object to record</param>
        protected internal override void Log(LogMessage logMessage)
        {
            foreach (LogRecorder recorder in _recorders)
            {
                recorder.Log(logMessage);
            }
        }

        /// <summary>
        ///  Stop all the registered log recorder in safe way.
        /// </summary>
        public override void Stop()
        {
            foreach (LogRecorder recorder in _recorders)
            {
                if (recorder.Alive) recorder.Stop();
            }
        }

        /// <summary>
        /// Stop all the registered log recorder in forcible way. The related thread may 
        /// be killed immediately. It is obviously that this methord is not recommended.
        /// </summary>
        public override void StopImmediately()
        {
            foreach (LogRecorder recorder in _recorders)
            {
                if (recorder.Alive) recorder.StopImmediately();
            }
        }
    }
}

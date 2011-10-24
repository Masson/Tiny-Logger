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
using System.Collections;
using System.Threading;

namespace IMasson.Util.TinyLogger.Recorder
{
    /// <summary>
    /// The abstract recorder that dispatching log messages and record into the target source
    /// in asynchronous thread.
    /// A subclass of AsyncLogRecorder may be probably a text file writer, or a 
    /// database interface, etc.
    /// </summary>
    public abstract class AsyncLogRecorder : LogRecorder
    {
        private Queue   _logQueue   = null;     // The log messages queue
        private bool    _threadAlive      = false;    // switcher of the dispatcher thread
        private Thread  _dispatcher = null;     // the dispatcher to record all the log

        /// <summary>
        /// Construct a LogRecorder.
        /// </summary>
        public AsyncLogRecorder()
        {
            _logQueue = Queue.Synchronized(new Queue(1000));

            Start();
        }


        #region Dispatcher control methods

        /// <summary>
        /// Start the process to handle all the log message in queue.
        /// </summary>
        public void Start()
        {
            // can not start when the dispatcher is already started
            if (_threadAlive) return;

            _threadAlive = true;
            _dispatcher = new Thread(new ThreadStart(dispatchLogQueue));
            _dispatcher.Start();

            // do something that the subclass defined
            onStart();
        }

        /// <summary>
        /// The process methord that the dispatcher thread work on.
        /// </summary>
        protected void dispatchLogQueue()
        {
            while (_threadAlive)
            {
                // record every log message in queue
                while (_logQueue.Count != 0 && _threadAlive)
                {
                    Record((LogMessage)_logQueue.Dequeue());
                }

                // block the thread when no log in the queue
                if (_threadAlive && _logQueue.Count == 0)
                {
                    Monitor.Enter(_logQueue);
                    if (_logQueue.Count == 0)
                    {
                        Monitor.Wait(_logQueue);
                    }
                    Monitor.Exit(_logQueue);
                }
            }

            // make sure that every log message record at last
            while (_logQueue.Count != 0)
            {
                Record((LogMessage)_logQueue.Dequeue());
            }

            // do something that the subclass defined
            onStop();

            _dispatcher = null;
        }

        #endregion


        /// <summary>
        /// Stop the log recorder in safe way.
        /// </summary>
        public override void Stop()
        {
            // no need to stop if the dispatcher is already stopped
            if (!_threadAlive) return;

            _threadAlive = false;

            // notify to release
            Monitor.Enter(_logQueue);
            Monitor.PulseAll(_logQueue);
            Monitor.Exit(_logQueue);

            // do something that the subclass defined
            //onStop();
        }

        /// <summary>
        /// Stop the log recorder in forcible way, and the thread will be aborted immediately.
        /// It is obviously that this methord is not recommended
        /// </summary>
        public override void StopImmediately()
        {
            // no need to stop if the dispatcher is already stopped
            if (!_threadAlive) return;

            _threadAlive = false;

            // force stop the thread
            _dispatcher.Abort();

            // do something that the subclass defined
            onStop();
        }


        /// <summary>
        /// Add a new log message to the queue.
        /// The recorder will not record it immediately, but will be handled by the dispatcher thread.
        /// </summary>
        /// <param name="logMessage">The log message object to record</param>
        protected internal override void Log(LogMessage logMessage)
        {
            // no need to record if dispatcher haven't started
            if (!_threadAlive) return;

            // check the param
            if (logMessage == null) return;

            // enqueue and notify
            _logQueue.Enqueue(logMessage);

            Monitor.Enter(_logQueue);
            Monitor.PulseAll(_logQueue);
            Monitor.Exit(_logQueue);
        }


        #region Abstract methods

        /// <summary>
        /// Record the log message to the target source, such as plain file, database, etc.
        /// This method must be implemented by developers.
        /// </summary>
        /// <param name="logMessage">The log message object to record</param>
        protected abstract void Record(LogMessage logMessage);

        /// <summary>
        /// Call when the log message dispatcher start.
        /// This method must be implemented by developers.
        /// </summary>
        protected abstract void onStart();

        /// <summary>
        /// Call when the log message dispatcher stop.
        /// This method must be implemented by developers.
        /// </summary>
        protected abstract void onStop();

        #endregion
    }
}

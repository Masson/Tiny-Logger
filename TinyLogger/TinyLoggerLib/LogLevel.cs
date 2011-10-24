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

namespace IMasson.Util.TinyLogger
{
    /// <summary>
    /// Enumable that represents different level of importance of a log message.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// This level is reserved for special usage, such as logger initilization and exception.
        /// Developers should't use this level. The other levels can done well with most log message.
        /// </summary>
        Special = 0,

        /// <summary>
        /// Verbose is generally used for low importance or temporal message.
        /// </summary>
        Verbose = 1,

        /// <summary>
        /// Debug is generally used for debug output.
        /// </summary>
        Debug = 2,

        /// <summary>
        /// Info is used for normal message.
        /// </summary>
        Info = 3,

        /// <summary>
        /// Warning is generally used for non-fatal exception message.
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Error is generally used for fatal error or serious problems.
        /// </summary>
        Error = 5
    }
}

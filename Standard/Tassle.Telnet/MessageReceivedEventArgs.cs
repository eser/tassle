// --------------------------------------------------------------------------
// <copyright file="MessageReceivedEventArgs.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

//// This program is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 3 of the License, or
//// (at your option) any later version.
//// 
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace Tassle.Telnet {
    /// <summary>
    /// MessageReceivedEventArgs class.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs {
        // fields

        /// <summary>
        /// The thread id
        /// </summary>
        private readonly int _threadId;

        /// <summary>
        /// The message
        /// </summary>
        private readonly string _message;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="threadId">The thread id</param>
        /// <param name="message">The message</param>
        public MessageReceivedEventArgs(int threadId, string message) {
            this._threadId = threadId;
            this._message = message;
        }

        // properties

        /// <summary>
        /// Gets the thread id
        /// </summary>
        /// <value>
        /// The thread id
        /// </value>
        public int ThreadId {
            get => this._threadId;
        }

        /// <summary>
        /// Gets the message
        /// </summary>
        /// <value>
        /// The message
        /// </value>
        public string Message {
            get => this._message;
        }
    }
}

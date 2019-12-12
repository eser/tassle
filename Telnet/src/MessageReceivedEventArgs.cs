// --------------------------------------------------------------------------
// <copyright file="MessageReceivedEventArgs.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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
        private readonly int threadId;

        /// <summary>
        /// The message
        /// </summary>
        private readonly string message;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="threadId">The thread id</param>
        /// <param name="message">The message</param>
        public MessageReceivedEventArgs(int threadId, string message) {
            this.threadId = threadId;
            this.message = message;
        }

        // properties

        /// <summary>
        /// Gets the thread id
        /// </summary>
        /// <value>
        /// The thread id
        /// </value>
        public int ThreadId {
            get => this.threadId;
        }

        /// <summary>
        /// Gets the message
        /// </summary>
        /// <value>
        /// The message
        /// </value>
        public string Message {
            get => this.message;
        }
    }
}

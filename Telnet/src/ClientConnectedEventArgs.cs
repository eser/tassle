// --------------------------------------------------------------------------
// <copyright file="ClientConnectedEventArgs.cs" company="-">
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
    /// ClientConnectedEventArgs class.
    /// </summary>
    public class ClientConnectedEventArgs : EventArgs {
        // fields

        /// <summary>
        /// The thread id
        /// </summary>
        private readonly int threadId;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnectedEventArgs"/> class.
        /// </summary>
        /// <param name="threadId">The thread id</param>
        public ClientConnectedEventArgs(int threadId) {
            this.threadId = threadId;
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
    }
}

// --------------------------------------------------------------------------
// <copyright file="ITelnetServer.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tassle.Telnet {
    /// <summary>
    /// ITelnetServer interface.
    /// </summary>
    public interface ITelnetServer {
        // events

        /// <summary>
        /// Occurs when [on message received].
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Occurs when [on client connected].
        /// </summary>
        event EventHandler<ClientConnectedEventArgs> ClientConnected;

        /// <summary>
        /// Occurs when [client disconnected].
        /// </summary>
        event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        // properties

        /// <summary>
        /// Gets or sets the listener thread
        /// </summary>
        /// <value>
        /// The listener thread
        /// </value>
        Thread ListenerThread {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the threads
        /// </summary>
        /// <value>
        /// The threads
        /// </value>
        Dictionary<int, TelnetThread> Threads {
            get;
            set;
        }

        /// <summary>
        /// Gets is running
        /// </summary>
        /// <value>
        /// Is running
        /// </value>
        bool IsRunning {
            get;
        }

        /// <summary>
        /// Gets or sets the encoding
        /// </summary>
        /// <value>
        /// The encoding
        /// </value>
        Encoding Encoding {
            get;
            set;
        }

        // methods

        /// <summary>
        /// Starts the server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        void Stop();

        /// <summary>
        /// Broadcasts a message.
        /// </summary>
        /// <param name="message">Message content</param>
        void BroadcastMessage(string message);

        /// <summary>
        /// Sends a message to specific client.
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="message">Message content</param>
        /// <returns>Is message is delivered or not</returns>
        bool SendMessage(int threadId, string message);
    }
}

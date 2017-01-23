// --------------------------------------------------------------------------
// <copyright file="TelnetServerInterface.cs" company="-">
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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tassle.Telnet {
    /// <summary>
    /// TelnetServerInterface interface.
    /// </summary>
    public interface TelnetServerInterface {
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

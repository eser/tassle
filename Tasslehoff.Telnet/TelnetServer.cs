// --------------------------------------------------------------------------
// <copyright file="TelnetServer.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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

using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tasslehoff.Telnet
{
    /// <summary>
    /// A TelnetServer instance.
    /// </summary>
    public class TelnetServer
    {
        // fields

        /// <summary>
        /// The lock object
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// The listener thread
        /// </summary>
        private Thread listenerThread;

        /// <summary>
        /// The listener thread cancelled
        /// </summary>
        private volatile bool listenerThreadCancelled;

        /// <summary>
        /// The IP endpoint
        /// </summary>
        private IPEndPoint bindEndpoint;

        /// <summary>
        /// The threads
        /// </summary>
        private Dictionary<int, TelnetThread> threads;

        /// <summary>
        /// The next thread id
        /// </summary>
        private int nextThreadId;

        /// <summary>
        /// Is running
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// The encoding
        /// </summary>
        private Encoding encoding;

        // events

        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        public event MessageReceivedDelegate MessageReceived;

        /// <summary>
        /// Occurs when [client connected].
        /// </summary>
        public event ClientConnected ClientConnected;

        /// <summary>
        /// Occurs when [client disconnected].
        /// </summary>
        public event ClientDisconnected ClientDisconnected;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetServer"/> class.
        /// </summary>
        public TelnetServer(IPEndPoint bindEndpoint)
        {
            this.bindEndpoint = bindEndpoint;
            this.threads = new Dictionary<int, TelnetThread>();
            this.nextThreadId = 0;
            this.isRunning = false;
            this.encoding = Encoding.ASCII;
        }

        // properties

        /// <summary>
        /// Gets or sets the listener thread
        /// </summary>
        /// <value>
        /// The listener thread
        /// </value>
        public Thread ListenerThread
        {
            get
            {
                return this.listenerThread;
            }
            set
            {
                this.listenerThread = value;
            }
        }

        /// <summary>
        /// Gets or sets the threads
        /// </summary>
        /// <value>
        /// The threads
        /// </value>
        public Dictionary<int, TelnetThread> Threads
        {
            get
            {
                return this.threads;
            }
            set
            {
                this.threads = value;
            }
        }

        /// <summary>
        /// Gets is running
        /// </summary>
        /// <value>
        /// Is running
        /// </value>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        /// <summary>
        /// Gets or sets the encoding
        /// </summary>
        /// <value>
        /// The encoding
        /// </value>
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }

        // methods

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.listenerThread = new Thread(this.ListenerThreadMain);
            this.listenerThread.IsBackground = true;
            this.listenerThread.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            this.listenerThreadCancelled = true;

            foreach (var telnetThread in this.Threads.Values)
            {
                telnetThread.Stop();
            }

            // this.listenerThread.Interrupt();
        }

        /// <summary>
        /// Broadcasts a message.
        /// </summary>
        /// <param name="message">Message content</param>
        public void BroadcastMessage(string message)
        {
            foreach (var telnetThread in this.Threads.Values)
            {
                telnetThread.SendMessageDirect(message);
            }
        }

        /// <summary>
        /// Sends a message to specific client.
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="message">Message content</param>
        /// <returns>Is message is delivered or not</returns>
        public bool SendMessage(int threadId, string message)
        {
            if (!this.Threads.ContainsKey(threadId))
            {
                return false;
            }

            var telnetThread = this.Threads[threadId];

            telnetThread.SendMessageDirect(message);

            return true;
        }

        /// <summary>
        /// OnClientMessageReceived
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="message">Message content</param>
        internal void OnClientMessageReceived(int threadId, string message)
        {
            if (this.MessageReceived != null)
            {
                this.MessageReceived(threadId, message);
            }
        }

        /// <summary>
        /// OnClientConnected
        /// </summary>
        /// <param name="threadId">Thread id</param>
        internal void OnClientConnected(int threadId)
        {
            if (this.ClientConnected != null)
            {
                this.ClientConnected(threadId);
            }
        }

        /// <summary>
        /// OnClientDisconnected
        /// </summary>
        /// <param name="threadId">Thread id</param>
        internal void OnClientDisconnected(int threadId)
        {
            this.Threads.Remove(threadId);

            if (this.ClientDisconnected != null)
            {
                this.ClientDisconnected(threadId);
            }
        }

        /// <summary>
        /// Main loop for listener thread
        /// </summary>
        private void ListenerThreadMain()
        {
            var tcpListener = new TcpListener(bindEndpoint);
            tcpListener.Start();

            this.isRunning = true;

            while (!this.listenerThreadCancelled)
            {
                var tcpClient = tcpListener.AcceptTcpClient();

                TelnetThread clientThread;

                lock (this.lockObject)
                {
                    var threadId = this.nextThreadId++;
                    clientThread = new TelnetThread(this, tcpClient, threadId);

                    this.Threads.Add(threadId, clientThread);
                }

                clientThread.Start();
            }

            tcpListener.Stop();
            this.isRunning = false;
        }
    }
}

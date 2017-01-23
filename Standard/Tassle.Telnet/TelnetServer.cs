// --------------------------------------------------------------------------
// <copyright file="TelnetServer.cs" company="-">
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
    /// A TelnetServer instance.
    /// </summary>
    public class TelnetServer : TelnetServerInterface {
        // fields

        /// <summary>
        /// The lock object
        /// </summary>
        private object _lockObject;

        /// <summary>
        /// The listener thread
        /// </summary>
        private Thread _listenerThread;

        /// <summary>
        /// The listener thread cancelled
        /// </summary>
        private volatile bool _listenerThreadCancelled;

        /// <summary>
        /// The IP endpoint
        /// </summary>
        private IPEndPoint _bindEndpoint;

        /// <summary>
        /// The threads
        /// </summary>
        private Dictionary<int, TelnetThread> _threads;

        /// <summary>
        /// The next thread id
        /// </summary>
        private int _nextThreadId;

        /// <summary>
        /// Is running
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// The encoding
        /// </summary>
        private Encoding _encoding;

        // events

        /// <summary>
        /// Occurs when [on message received].
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Occurs when [on client connected].
        /// </summary>
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;

        /// <summary>
        /// Occurs when [client disconnected].
        /// </summary>
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetServer"/> class.
        /// </summary>
        public TelnetServer(IPEndPoint bindEndpoint) {
            this._lockObject = new object();
            this._bindEndpoint = bindEndpoint;
            this._threads = new Dictionary<int, TelnetThread>();
            this._nextThreadId = 0;
            this._isRunning = false;
            this._encoding = Encoding.ASCII;
        }

        // properties

        /// <summary>
        /// Gets or sets the listener thread
        /// </summary>
        /// <value>
        /// The listener thread
        /// </value>
        public Thread ListenerThread {
            get => this._listenerThread;
            set => this._listenerThread = value;
        }

        /// <summary>
        /// Gets or sets the threads
        /// </summary>
        /// <value>
        /// The threads
        /// </value>
        public Dictionary<int, TelnetThread> Threads {
            get => this._threads;
            set => this._threads = value;
        }

        /// <summary>
        /// Gets is running
        /// </summary>
        /// <value>
        /// Is running
        /// </value>
        public bool IsRunning {
            get => this._isRunning;
        }

        /// <summary>
        /// Gets or sets the encoding
        /// </summary>
        /// <value>
        /// The encoding
        /// </value>
        public Encoding Encoding {
            get => this._encoding;
            set => this._encoding = value;
        }

        // methods

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start() {
            this._listenerThread = new Thread(this.ListenerThreadMain) {
                IsBackground = true
            };

            this._listenerThread.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop() {
            this._listenerThreadCancelled = true;

            foreach (var telnetThread in this._threads.Values) {
                telnetThread.Stop();
            }

            // this._listenerThread.Interrupt();
        }

        /// <summary>
        /// Broadcasts a message.
        /// </summary>
        /// <param name="message">Message content</param>
        public void BroadcastMessage(string message) {
            foreach (var telnetThread in this._threads.Values) {
                telnetThread.SendMessageDirect(message);
            }
        }

        /// <summary>
        /// Sends a message to specific client.
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="message">Message content</param>
        /// <returns>Is message is delivered or not</returns>
        public bool SendMessage(int threadId, string message) {
            if (!this._threads.ContainsKey(threadId)) {
                return false;
            }

            var telnetThread = this._threads[threadId];

            telnetThread.SendMessageDirect(message);

            return true;
        }

        /// <summary>
        /// Gets called when [on message received]
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="message">Message content</param>
        internal void InvokeMessageReceived(int threadId, string message) {
            this.MessageReceived?.Invoke(this, new MessageReceivedEventArgs(threadId, message));
        }

        /// <summary>
        /// Gets called when [on client connected]
        /// </summary>
        /// <param name="threadId">Thread id</param>
        internal void InvokeClientConnected(int threadId) {
            this.ClientConnected?.Invoke(this, new ClientConnectedEventArgs(threadId));
        }

        /// <summary>
        /// Gets called when [on client disconnected]
        /// </summary>
        /// <param name="threadId">Thread id</param>
        internal void InvokeClientDisconnected(int threadId) {
            this._threads.Remove(threadId);

            this.ClientDisconnected?.Invoke(this, new ClientDisconnectedEventArgs(threadId));
        }

        /// <summary>
        /// Main loop for listener thread
        /// </summary>
        private void ListenerThreadMain() {
            var tcpListener = new TcpListener(this._bindEndpoint);
            tcpListener.Start();

            this._isRunning = true;

            while (!this._listenerThreadCancelled) {
                var acceptTcpClientTask = tcpListener.AcceptTcpClientAsync();

                acceptTcpClientTask.ContinueWith(t => this.AcceptTcpClientCallback(t.Result));
            }

            tcpListener.Stop();
            this._isRunning = false;
        }

        private void AcceptTcpClientCallback(TcpClient tcpClient) {
            TelnetThread clientThread;

            lock (this._lockObject) {
                var threadId = this._nextThreadId++;
                clientThread = new TelnetThread(this, tcpClient, threadId);

                this._threads.Add(threadId, clientThread);
            }

            clientThread.Start();
        }
    }
}

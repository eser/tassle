// --------------------------------------------------------------------------
// <copyright file="TelnetServer.cs" company="-">
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tassle.Telnet {
    /// <summary>
    /// A TelnetServer instance.
    /// </summary>
    public class TelnetServer : ITelnetServer {
        // fields

        /// <summary>
        /// The lock object
        /// </summary>
        private object lockObject;

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
            this.lockObject = new object();
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
        public Thread ListenerThread {
            get => this.listenerThread;
            set => this.listenerThread = value;
        }

        /// <summary>
        /// Gets or sets the threads
        /// </summary>
        /// <value>
        /// The threads
        /// </value>
        public Dictionary<int, TelnetThread> Threads {
            get => this.threads;
            set => this.threads = value;
        }

        /// <summary>
        /// Gets is running
        /// </summary>
        /// <value>
        /// Is running
        /// </value>
        public bool IsRunning {
            get => this.isRunning;
        }

        /// <summary>
        /// Gets or sets the encoding
        /// </summary>
        /// <value>
        /// The encoding
        /// </value>
        public Encoding Encoding {
            get => this.encoding;
            set => this.encoding = value;
        }

        // methods

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start() {
            this.listenerThread = new Thread(this.ListenerThreadMain) {
                IsBackground = true
            };

            this.listenerThread.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop() {
            this.listenerThreadCancelled = true;

            foreach (var telnetThread in this.threads.Values) {
                telnetThread.Stop();
            }

            // this.listenerThread.Interrupt();
        }

        /// <summary>
        /// Broadcasts a message.
        /// </summary>
        /// <param name="message">Message content</param>
        public void BroadcastMessage(string message) {
            foreach (var telnetThread in this.threads.Values) {
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
            if (!this.threads.ContainsKey(threadId)) {
                return false;
            }

            var telnetThread = this.threads[threadId];

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
            this.threads.Remove(threadId);

            this.ClientDisconnected?.Invoke(this, new ClientDisconnectedEventArgs(threadId));
        }

        /// <summary>
        /// Main loop for listener thread
        /// </summary>
        private void ListenerThreadMain() {
            var tcpListener = new TcpListener(this.bindEndpoint);
            tcpListener.Start();

            this.isRunning = true;

            while (!this.listenerThreadCancelled) {
                var acceptTcpClientTask = tcpListener.AcceptTcpClientAsync();

                acceptTcpClientTask.ContinueWith(t => this.AcceptTcpClientCallback(t.Result));
            }

            tcpListener.Stop();
            this.isRunning = false;
        }

        private void AcceptTcpClientCallback(TcpClient tcpClient) {
            TelnetThread clientThread;

            lock (this.lockObject) {
                var threadId = this.nextThreadId++;
                clientThread = new TelnetThread(this, tcpClient, threadId);

                this.threads.Add(threadId, clientThread);
            }

            clientThread.Start();
        }
    }
}

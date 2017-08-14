// --------------------------------------------------------------------------
// <copyright file="TelnetThread.cs" company="-">
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
using System.Net.Sockets;
using System.Threading;

namespace Tassle.Telnet {
    /// <summary>
    /// A TelnetThread instance.
    /// </summary>
    public class TelnetThread {
        // constants
        public const int BufferLength = 2048;

        // fields

        /// <summary>
        /// The server
        /// </summary>
        private TelnetServer _server;

        /// <summary>
        /// The thread id
        /// </summary>
        private int _threadId;

        /// <summary>
        /// The client thread
        /// </summary>
        private Thread _clientThread;

        /// <summary>
        /// The client thread cancelled
        /// </summary>
        private volatile bool _clientThreadCancelled;

        /// <summary>
        /// The queued messages
        /// </summary>
        private Queue<string> _queuedMessages;

        /// <summary>
        /// The stream
        /// </summary>
        private NetworkStream _stream;

        /// <summary>
        /// The buffer
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// The string buffer
        /// </summary>
        private string _stringBuffer;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetThread"/> class.
        /// </summary>
        public TelnetThread(TelnetServer telnetServer, TcpClient tcpClient, int threadId) {
            this._server = telnetServer;
            this._queuedMessages = new Queue<string>();
            this._threadId = threadId;

            this._buffer = new byte[TelnetThread.BufferLength];
            this._stringBuffer = string.Empty;

            this._stream = tcpClient.GetStream();
        }

        // properties

        /// <summary>
        /// Gets or sets the thread id
        /// </summary>
        /// <value>
        /// The thread id
        /// </value>
        public int ThreadId {
            get => this._threadId;
            set => this._threadId = value;
        }

        /// <summary>
        /// Gets or sets the client thread
        /// </summary>
        /// <value>
        /// The client thread
        /// </value>
        public Thread ClientThread {
            get => this._clientThread;
            set => this._clientThread = value;
        }

        /// <summary>
        /// Gets or sets the stream
        /// </summary>
        /// <value>
        /// The stream
        /// </value>
        public NetworkStream Stream {
            get => this._stream;
            set => this._stream = value;
        }

        // methods

        public void Start() {
            this._server.InvokeClientConnected(this.ThreadId);

            this._clientThread = new Thread(new ThreadStart(this.ConnectionThread));
            this._clientThread.Start();
        }

        public void Stop() {
            this._clientThreadCancelled = true;

            // this.clientThread.Interrupt();
        }

        public void SendMessageDirect(string message) {
            this._queuedMessages.Enqueue(message + Environment.NewLine);
        }

        private void ConnectionThread() {
            this._stream.WriteByte(0);

            while (!this._clientThreadCancelled) {
                while (this._queuedMessages.Count > 0) {
                    var bytes = this._server.Encoding.GetBytes(this._queuedMessages.Dequeue());
                    this._stream.Write(bytes, 0, bytes.Length);
                }

                var readTask = this._stream.ReadAsync(this._buffer, 0, this._buffer.Length); // TODO: use cancellation token

                readTask.ContinueWith(t => this.ReadCallback(t.Result));
            }

            this._stream.Dispose();
            this._server.InvokeClientDisconnected(this._threadId);
        }

        private void ReadCallback(int read) {
            if (read == 0) {
                this.Stop();
                return;
            }

            this._stringBuffer += this._server.Encoding.GetString(this._buffer, 0, read);

            var newLineIndex = this._stringBuffer.IndexOf('\n');
            if (newLineIndex > 0) {
                var line = this._stringBuffer.Substring(0, newLineIndex - 1);
                this._server.InvokeMessageReceived(this.ThreadId, line);

                this._stringBuffer = this._stringBuffer.Substring(newLineIndex + 1);
            }
        }
    }
}

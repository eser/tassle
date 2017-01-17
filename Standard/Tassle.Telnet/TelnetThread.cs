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
using System.Threading.Tasks;

namespace Tassle.Telnet
{
    /// <summary>
    /// A TelnetThread instance.
    /// </summary>
    public class TelnetThread
    {
        // constants
        public const int BufferLength = 2048;

        // fields

        /// <summary>
        /// The server
        /// </summary>
        private TelnetServer server;

        /// <summary>
        /// The thread id
        /// </summary>
        private int threadId;

        /// <summary>
        /// The client thread
        /// </summary>
        private Thread clientThread;

        /// <summary>
        /// The client thread cancelled
        /// </summary>
        private volatile bool clientThreadCancelled;

        /// <summary>
        /// The queued messages
        /// </summary>
        private Queue<string> queuedMessages;

        /// <summary>
        /// The stream
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// The buffer
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// The string buffer
        /// </summary>
        private string stringBuffer;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetThread"/> class.
        /// </summary>
        public TelnetThread(TelnetServer telnetServer, TcpClient tcpClient, int threadId)
        {
            this.server = telnetServer;
            this.queuedMessages = new Queue<string>();
            this.threadId = threadId;

            this.buffer = new byte[TelnetThread.BufferLength];
            this.stringBuffer = string.Empty;

            this.stream = tcpClient.GetStream();
        }

        // properties

        /// <summary>
        /// Gets or sets the thread id
        /// </summary>
        /// <value>
        /// The thread id
        /// </value>
        public int ThreadId
        {
            get
            {
                return this.threadId;
            }
            set
            {
                this.threadId = value;
            }
        }

        /// <summary>
        /// Gets or sets the client thread
        /// </summary>
        /// <value>
        /// The client thread
        /// </value>
        public Thread ClientThread
        {
            get
            {
                return this.clientThread;
            }
            set
            {
                this.clientThread = value;
            }
        }

        /// <summary>
        /// Gets or sets the stream
        /// </summary>
        /// <value>
        /// The stream
        /// </value>
        public NetworkStream Stream
        {
            get
            {
                return this.stream;
            }
            set
            {
                this.stream = value;
            }
        }

        // methods

        public void Start()
        {
            this.server.OnClientConnected(this.ThreadId);

            this.clientThread = new Thread(new ThreadStart(this.ConnectionThread));
            this.clientThread.Start();
        }

        public void Stop()
        {
            this.clientThreadCancelled = true;

            // this.clientThread.Interrupt();
        }

        public void SendMessageDirect(string message)
        {
            this.queuedMessages.Enqueue(message + Environment.NewLine);
        }

        private void ConnectionThread()
        {
            this.stream.WriteByte(0);

            while (!this.clientThreadCancelled)
            {
                while (this.queuedMessages.Count > 0)
                {
                    var bytes = this.server.Encoding.GetBytes(this.queuedMessages.Dequeue());
                    this.stream.Write(bytes, 0, bytes.Length);
                }

                var readTask = this.stream.ReadAsync(this.buffer, 0, this.buffer.Length); // TODO: use cancellation token

                readTask.ContinueWith(t => this.ReadCallback(t.Result));
            }

            this.stream.Dispose();
            this.server.OnClientDisconnected(this.threadId);
        }

        private void ReadCallback(int read)
        {
            if (read == 0)
            {
                this.Stop();
                return;
            }

            this.stringBuffer += this.server.Encoding.GetString(this.buffer, 0, read);

            var newLineIndex = this.stringBuffer.IndexOf('\n');
            if (newLineIndex > 0)
            {
                var line = this.stringBuffer.Substring(0, newLineIndex - 1);
                this.server.OnClientMessageReceived(this.ThreadId, line);

                this.stringBuffer = this.stringBuffer.Substring(newLineIndex + 1);
            }
        }
    }
}

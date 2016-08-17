// --------------------------------------------------------------------------
// <copyright file="IronMQConnection.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
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

namespace Tasslehoff.Adapters.IronMQ
{
    using System.Collections.Generic;
    using System.Text;
    using Common.Helpers;
    using IronSharp.IronMQ;
    using Services;

    /// <summary>
    /// IronMQConnection class.
    /// </summary>
    public class IronMQConnection : Service, IQueueManager
    {
        // fields

        /// <summary>
        /// The default timeout
        /// </summary>
        public const int DefaultTimeout = 3600;

        /// <summary>
        /// The client
        /// </summary>
        private IronMqRestClient connection = null;

        /// <summary>
        /// The models
        /// </summary>
        private readonly IDictionary<string, QueueClient> models;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IronMQConnection"/> class.
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="token">The token</param>
        public IronMQConnection(string projectId, string token)
        {
            this.connection = Client.New(projectId, token);
            this.models = new Dictionary<string, QueueClient>();
        }

        // attributes

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return "IronMQConnection";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        internal IronMqRestClient Connection
        {
            get
            {
                return this.connection;
            }
        }

        // indexer

        /// <summary>
        /// Gets the <see cref="QueueClient"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="QueueClient"/>.
        /// </value>
        /// <param name="key">The key</param>
        /// <returns>QueueClient instance</returns>
        internal QueueClient this[string key]
        {
            get
            {
                if (!this.models.ContainsKey(key))
                {
                    this.models[key] = this.connection.Queue(key);
                }

                return this.models[key];
            }
        }

        // methods

        /// <summary>
        /// Dequeues a message.
        /// </summary>
        /// <param name="queueKey">The queue key</param>
        /// <param name="timeout">The timeout</param>
        /// <returns>
        /// The message
        /// </returns>
        public byte[] Dequeue(string queueKey, int timeout = IronMQConnection.DefaultTimeout)
        {
            QueueClient client = this[queueKey];

            QueueMessage nextMessage;
            if (!client.Read(out nextMessage, timeout))
            {
                return null;
            }

            return Encoding.Default.GetBytes(nextMessage.Body);
        }

        /// <summary>
        /// Dequeues a message.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="queueKey">The queue key.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// The message
        /// </returns>
        public T DequeueJson<T>(string queueKey, int timeout = IronMQConnection.DefaultTimeout) where T : class
        {
            QueueClient client = this[queueKey];

            QueueMessage nextMessage;
            if (!client.Read(out nextMessage, timeout))
            {
                return null;
            }

            return SerializationHelpers.JsonDeserialize<T>(nextMessage.Body);
        }

        /// <summary>
        /// Enqueues a message.
        /// </summary>
        /// <param name="queueKey">The queue key</param>
        /// <param name="message">The message</param>
        public void Enqueue(string queueKey, byte[] message)
        {
            QueueClient client = this[queueKey];

            client.Post(Encoding.Default.GetString(message));
        }

        /// <summary>
        /// Enqueues the specified queue key.
        /// </summary>
        /// <param name="queueKey">The queue key.</param>
        /// <param name="message">The message.</param>
        public void EnqueueJson(string queueKey, object message)
        {
            QueueClient client = this[queueKey];

            client.Post(SerializationHelpers.JsonSerialize(message));
        }
    }
}

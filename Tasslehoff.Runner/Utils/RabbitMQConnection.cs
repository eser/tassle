// -----------------------------------------------------------------------
// <copyright file="RabbitMQConnection.cs" company="-">
// Copyright (c) 2013 larukedi (eser@sent.com). All rights reserved.
// </copyright>
// <author>larukedi (http://github.com/larukedi/)</author>
// -----------------------------------------------------------------------

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

namespace Tasslehoff.Runner.Utils
{
    using System;
    using System.Collections.Generic;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    /// <summary>
    /// RabbitMQConnection class.
    /// </summary>
    [CLSCompliant(false)]
    public class RabbitMQConnection : IDisposable
    {
        // fields

        /// <summary>
        /// The default timeout
        /// </summary>
        public const int DefaultTimeout = 3600;

        /// <summary>
        /// The host
        /// </summary>
        private static string host = null;

        /// <summary>
        /// The connection factory
        /// </summary>
        private static ConnectionFactory connectionFactory = null;

        /// <summary>
        /// The connection
        /// </summary>
        private readonly IConnection connection;

        /// <summary>
        /// The models
        /// </summary>
        private readonly IDictionary<string, IModel> models;

        /// <summary>
        /// The consumer
        /// </summary>
        private QueueingBasicConsumer consumer = null;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQConnection"/> class.
        /// </summary>
        public RabbitMQConnection()
        {
            if (RabbitMQConnection.host == null)
            {
                throw new ArgumentNullException("host", "Hostname is not specified for RabbitMQConnection.");
            }

            if (RabbitMQConnection.connectionFactory == null)
            {
                RabbitMQConnection.connectionFactory = new ConnectionFactory()
                {
                    HostName = RabbitMQConnection.host
                };
            }

            this.connection = RabbitMQConnection.connectionFactory.CreateConnection();
            this.models = new Dictionary<string, IModel>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RabbitMQConnection"/> class.
        /// </summary>
        ~RabbitMQConnection()
        {
            this.Dispose(false);
        }

        // attributes

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public static string Host
        {
            get
            {
                return RabbitMQConnection.host;
            }

            set
            {
                RabbitMQConnection.host = value;
            }
        }

        /// <summary>
        /// Gets the connection factory.
        /// </summary>
        /// <value>
        /// The connection factory.
        /// </value>
        public static ConnectionFactory ConnectionFactory
        {
            get
            {
                return RabbitMQConnection.connectionFactory;
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Gets the consumer.
        /// </summary>
        /// <value>
        /// The consumer.
        /// </value>
        public QueueingBasicConsumer Consumer
        {
            get
            {
                return this.consumer;
            }
        }

        // indexer

        /// <summary>
        /// Gets the <see cref="IModel"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IModel"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>IModel instance.</returns>
        public IModel this[string key]
        {
            get
            {
                if (!this.models.ContainsKey(key))
                {
                    this.models[key] = this.connection.CreateModel();
                    this.models[key].QueueDeclare(key, true, false, false, null);
                }

                return this.models[key];
            }
        }

        // methods

        /// <summary>
        /// Dequeues a message.
        /// </summary>
        /// <param name="queueKey">The queue key.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// The message.
        /// </returns>
        public byte[] Dequeue(string queueKey, int timeout = RabbitMQConnection.DefaultTimeout)
        {
            IModel channel = this[queueKey];

            if (this.consumer == null)
            {
                channel.BasicQos(0, 1, false);
                this.consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queueKey, false, this.consumer);
            }

            object result;
            if (this.consumer.Queue.Dequeue(timeout, out result))
            {
                BasicDeliverEventArgs eventArgs = (BasicDeliverEventArgs)result;
                channel.BasicAck(eventArgs.DeliveryTag, false);

                return eventArgs.Body;
            }

            return null;
        }

        /// <summary>
        /// Enqueues a message.
        /// </summary>
        /// <param name="queueKey">The queue key.</param>
        /// <param name="message">The message.</param>
        public void Enqueue(string queueKey, byte[] message)
        {
            IModel channel = this[queueKey];

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            channel.BasicPublish(string.Empty, queueKey, properties, message);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (IModel model in this.models.Values)
                {
                    model.Dispose();
                }

                this.connection.Dispose();
            }

            this.disposed = true;
        }
    }
}

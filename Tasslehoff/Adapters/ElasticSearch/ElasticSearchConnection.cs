// --------------------------------------------------------------------------
// <copyright file="ElasticSearchConnection.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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

namespace Tasslehoff.Adapters.Redis
{
    using System;
    using Common.Helpers;
    using Elasticsearch.Net;
    using Elasticsearch.Net.Connection;
    using Services;

    /// <summary>
    /// ElasticSearchConnection class.
    /// </summary>
    public class ElasticSearchConnection : Service, ICacheManager, IDisposable
    {
        // fields

        /// <summary>
        /// The default timeout
        /// </summary>
        public const int DefaultTimeout = 3600;

        /// <summary>
        /// The address
        /// </summary>
        private readonly Uri address;

        /// <summary>
        /// The connection
        /// </summary>
        private ElasticsearchClient connection = null;

        /// <summary>
        /// The database
        /// </summary>
        private ConnectionConfiguration configuration = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearchConnection" /> class.
        /// </summary>
        /// <param name="address">The address</param>
        public ElasticSearchConnection(Uri address)
        {
            this.address = address;

            this.configuration = new ConnectionConfiguration(address);
            this.connection = new ElasticsearchClient(this.configuration);
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
                return "ElasticSearchConnection";
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
        /// Gets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public Uri Address
        {
            get
            {
                return this.address;
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        internal ElasticsearchClient Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        internal ConnectionConfiguration Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        // methods

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The cached object</returns>
        public T Get<T>(string key)
        {
            string[] keys = key.Split(new char[] { '/' }, 3, StringSplitOptions.RemoveEmptyEntries);
            ElasticsearchResponse<T> response = this.connection.GetSource<T>(keys[0], keys[1], keys[2]);

            if (!response.Success) {
                return default(T);
            }

            return response.Response;
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The cached object</returns>
        public T GetJson<T>(string key) where T : class
        {
            string[] keys = key.Split(new char[] { '/' }, 3, StringSplitOptions.RemoveEmptyEntries);
            ElasticsearchResponse<string> response = this.connection.GetSource<string>(keys[0], keys[1], keys[2]);

            if (!response.Success)
            {
                return null;
            }

            return SerializationHelpers.JsonDeserialize<T>(response.Response);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <param name="expiresAt">The expires at</param>
        /// <returns>Is written to cache or not</returns>
        public bool Set(string key, object value, DateTimeOffset? expiresAt = null)
        {
            string[] keys = key.Split(new char[] { '/' }, 3, StringSplitOptions.RemoveEmptyEntries);
            ElasticsearchResponse<DynamicDictionary> response = this.connection.Index(keys[0], keys[1], keys[2], value);

            return response.Success;
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>Is written to cache or not</returns>
        public bool SetJson(string key, object value, DateTimeOffset? expiresAt = null)
        {
            string[] keys = key.Split(new char[] { '/' }, 3, StringSplitOptions.RemoveEmptyEntries);
            string serializedValue = SerializationHelpers.JsonSerialize(value);
            ElasticsearchResponse<DynamicDictionary> response = this.connection.Index(keys[0], keys[1], keys[2], serializedValue);

            return response.Success;
        }
    }
}

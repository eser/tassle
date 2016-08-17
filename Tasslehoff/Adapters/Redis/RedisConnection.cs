// --------------------------------------------------------------------------
// <copyright file="RedisConnection.cs" company="-">
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

namespace Tasslehoff.Adapters.Redis
{
    using System;
    using System.Text;
    using Common.Helpers;
    using Services;
    using StackExchange.Redis;

    /// <summary>
    /// RedisConnection class.
    /// </summary>
    public class RedisConnection : Service, ICacheManager, IDisposable
    {
        // fields

        /// <summary>
        /// The default timeout
        /// </summary>
        public const int DefaultTimeout = 3600;

        /// <summary>
        /// The addresses
        /// </summary>
        private readonly string[] addresses;

        /// <summary>
        /// The connection
        /// </summary>
        private ConnectionMultiplexer connection = null;

        /// <summary>
        /// The database
        /// </summary>
        private IDatabase database = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnection" /> class.
        /// </summary>
        /// <param name="addresses">The addresses</param>
        public RedisConnection(params string[] addresses)
        {
            this.addresses = addresses;

            if (this.addresses.Length > 0)
            {
                this.connection = ConnectionMultiplexer.Connect(string.Join(",", addresses));
                this.database = this.connection.GetDatabase();
            }
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
                return "RedisConnection";
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
        /// Gets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        public string[] Addresses
        {
            get
            {
                return this.addresses;
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        internal ConnectionMultiplexer Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        internal IDatabase Database
        {
            get
            {
                return this.database;
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
            if (this.database == null)
            {
                return default(T);
            }

            RedisValue value = this.database.StringGet(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The cached object</returns>
        public T GetJson<T>(string key) where T : class
        {
            if (this.database == null)
            {
                return null;
            }

            RedisValue value = this.database.StringGet(key);
            string json = Encoding.Default.GetString(value);

            return SerializationHelpers.JsonDeserialize<T>(json);
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
            if (this.database == null)
            {
                return false;
            }

            return this.database.StringSet(key, (RedisValue)value);
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
            string serializedValue = SerializationHelpers.JsonSerialize(value);
            return this.Set(key, serializedValue, expiresAt);
        }
    }
}

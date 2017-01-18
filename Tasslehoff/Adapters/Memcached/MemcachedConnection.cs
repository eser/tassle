// --------------------------------------------------------------------------
// <copyright file="MemcachedConnection.cs" company="-">
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

namespace Tasslehoff.Adapters.Memcached
{
    using System;
    using System.Text;
    using Common.Helpers;
    using Enyim.Caching;
    using Enyim.Caching.Configuration;
    using Enyim.Caching.Memcached;
    using Services;

    /// <summary>
    /// MemcachedConnection class.
    /// </summary>
    public class MemcachedConnection : Service, ICacheManager, IDisposable
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
        private MemcachedClient connection = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedConnection" /> class.
        /// </summary>
        /// <param name="addresses">The addresses</param>
        public MemcachedConnection(params string[] addresses)
        {
            this.addresses = addresses;

            if (this.addresses.Length > 0)
            {
                MemcachedClientConfiguration configuration = new MemcachedClientConfiguration();

                foreach (string address in addresses)
                {
                    configuration.AddServer(address);
                }

                this.connection = new MemcachedClient(configuration);
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
                return "MemcachedConnection";
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
        internal MemcachedClient Connection
        {
            get
            {
                return this.connection;
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
            if (this.connection == null)
            {
                return default(T);
            }

            object value = this.connection.Get(key);

            return (T)value;
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The cached object</returns>
        public T GetJson<T>(string key) where T : class
        {
            if (this.connection == null)
            {
                return null;
            }

            byte[] bytes = this.connection.Get(key) as byte[];
            if (bytes == null)
            {
                return null;
            }

            string json = Encoding.Default.GetString(bytes);
            T value = SerializationHelpers.JsonDeserialize<T>(json);

            return value;
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
            if (this.connection == null)
            {
                return false;
            }

            if (expiresAt.HasValue)
            {
                return this.connection.Store(StoreMode.Set, key, value, expiresAt.Value.Subtract(DateTimeOffset.UtcNow));
            }

            return this.connection.Store(StoreMode.Set, key, value);
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
            byte[] serializedValue = Encoding.Default.GetBytes(SerializationHelpers.JsonSerialize(value));
            return this.Set(key, serializedValue, expiresAt);
        }
    }
}

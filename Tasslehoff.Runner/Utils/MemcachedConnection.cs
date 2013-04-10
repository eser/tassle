// -----------------------------------------------------------------------
// <copyright file="MemcachedConnection.cs" company="-">
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
    using Enyim.Caching;
    using Enyim.Caching.Memcached;

    /// <summary>
    /// MemcachedConnection class.
    /// </summary>
    [CLSCompliant(false)]
    public class MemcachedConnection : IDisposable
    {
        // fields

        /// <summary>
        /// The default timeout
        /// </summary>
        public const int DefaultTimeout = 3600;

        /// <summary>
        /// The host
        /// </summary>
        private readonly string host;

        /// <summary>
        /// The connection
        /// </summary>
        private MemcachedClient connection = null;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedConnection" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public MemcachedConnection(string host)
        {
            this.host = host;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MemcachedConnection"/> class.
        /// </summary>
        ~MemcachedConnection()
        {
            this.Dispose(false);
        }

        // attributes

        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host
        {
            get
            {
                return this.host;
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public MemcachedClient Connection
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
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The cached object.</returns>
        public T Get<T>(string key) where T : class
        {
            return this.connection.Get(key) as T;
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>Is written to cache or not.</returns>
        public bool Set(string key, object value, DateTime? expiresAt = null)
        {
            if (expiresAt.HasValue)
            {
                return this.connection.Store(StoreMode.Set, key, value, expiresAt.Value);
            }

            return this.connection.Store(StoreMode.Set, key, value);
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
                this.connection.Dispose();
            }

            this.disposed = true;
        }
    }
}

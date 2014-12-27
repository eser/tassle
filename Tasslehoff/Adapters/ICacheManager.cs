// -----------------------------------------------------------------------
// <copyright file="ICacheManager.cs" company="-">
// Copyright (c) 2014 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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

namespace Tasslehoff.Adapters
{
    using System;

    public interface ICacheManager
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The cached object</returns>
        T Get<T>(string key);

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The cached object</returns>
        T GetJson<T>(string key) where T : class;

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <param name="expiresAt">The expires at</param>
        /// <returns>Is written to cache or not</returns>
        bool Set(string key, object value, DateTimeOffset? expiresAt = null);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>Is written to cache or not</returns>
        bool SetJson(string key, object value, DateTimeOffset? expiresAt = null);
    }
}

// --------------------------------------------------------------------------
// <copyright file="IQueueManager.cs" company="-">
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

using Tasslehoff.Services;

namespace Tasslehoff.Adapters
{
    public interface IQueueManager : IService
    {
        /// <summary>
        /// Dequeues a message.
        /// </summary>
        /// <param name="queueKey">The queue key</param>
        /// <param name="timeout">The timeout</param>
        /// <returns>
        /// The message
        /// </returns>
        byte[] Dequeue(string queueKey, int timeout);

        /// <summary>
        /// Dequeues a message.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="queueKey">The queue key.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// The message
        /// </returns>
        T DequeueJson<T>(string queueKey, int timeout) where T : class;

        /// <summary>
        /// Enqueues a message.
        /// </summary>
        /// <param name="queueKey">The queue key</param>
        /// <param name="message">The message</param>
        void Enqueue(string queueKey, byte[] message);

        /// <summary>
        /// Enqueues the specified queue key.
        /// </summary>
        /// <param name="queueKey">The queue key.</param>
        /// <param name="message">The message.</param>
        void EnqueueJson(string queueKey, object message);
    }
}

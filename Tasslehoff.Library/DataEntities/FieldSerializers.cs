// -----------------------------------------------------------------------
// <copyright file="FieldSerializers.cs" company="-">
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

namespace Tasslehoff.Library.DataEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// FieldSerializers class.
    /// </summary>
    public static class FieldSerializers
    {
        // fields

        /// <summary>
        /// The serializers
        /// </summary>
        private static IDictionary<string, FieldSerializer> serializers = null;

        // methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serializer">The serializer.</param>
        public static void Add(string key, FieldSerializer serializer)
        {
            if (FieldSerializers.serializers == null)
            {
                FieldSerializers.serializers = new Dictionary<string, FieldSerializer>();
            }

            FieldSerializers.serializers.Add(key, serializer);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The serializer function.</returns>
        public static FieldSerializer Get(string key)
        {
            return FieldSerializers.serializers[key];
        }
    }
}

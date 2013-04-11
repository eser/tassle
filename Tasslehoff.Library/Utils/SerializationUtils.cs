// -----------------------------------------------------------------------
// <copyright file="SerializationUtils.cs" company="-">
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

namespace Tasslehoff.Library.Utils
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// SerializationUtils class.
    /// </summary>
    public static class SerializationUtils
    {
        // methods

        /// <summary>
        /// Serializes an object to byte array.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns>Serialized byte array</returns>
        public static byte[] Serialize(object graph)
        {
            byte[] bytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, graph);

                bytes = memoryStream.ToArray();
            }

            return bytes;
        }

        /// <summary>
        /// Deserializes byte array to an object.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Deserialized object</returns>
        public static T Deserialize<T>(byte[] bytes)
        {
            T graph;

            using (MemoryStream memoryStream = new MemoryStream(bytes, false))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                graph = (T)formatter.Deserialize(memoryStream);
            }

            return graph;
        }
    }
}

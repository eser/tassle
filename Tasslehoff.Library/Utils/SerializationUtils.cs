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
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using ServiceStack.Text;

    /// <summary>
    /// SerializationUtils class.
    /// </summary>
    public static class SerializationUtils
    {
        // methods

        /// <summary>
        /// Binaries the serialize.
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <returns>Serialized data</returns>
        public static byte[] BinarySerialize(object graph)
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
        /// Binaries the deserialize.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="bytes">The bytes</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static T BinaryDeserialize<T>(byte[] bytes)
        {
            T graph;

            using (MemoryStream memoryStream = new MemoryStream(bytes, false))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                graph = (T)formatter.Deserialize(memoryStream);
            }

            return graph;
        }

        /// <summary>
        /// Serializes the object into JSON.
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>
        /// Serialized data
        /// </returns>
        public static byte[] JsonSerialize(object graph, Encoding encoding = null)
        {
            byte[] bytes;

            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(graph.GetType());
            //    serializer.WriteObject(memoryStream, graph);

            //    bytes = memoryStream.ToArray();
            //}
            bytes = (encoding ?? Encoding.Default).GetBytes(JsonSerializer.SerializeToString(graph));

            return bytes;
        }

        /// <summary>
        /// Serializes the object into JSON.
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>
        /// Serialized data
        /// </returns>
        public static string JsonSerialize(object graph)
        {
            string output;

            output = JsonSerializer.SerializeToString(graph);

            return output;
        }

        /// <summary>
        /// Deserializes JSON data into object.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="bytes">The bytes</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static T JsonDeserialize<T>(byte[] bytes, Encoding encoding = null)
        {
            T graph;

            //using (MemoryStream memoryStream = new MemoryStream(bytes, false))
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            //    graph = (T)serializer.ReadObject(memoryStream);
            //}
            graph = JsonSerializer.DeserializeFromString<T>((encoding ?? Encoding.Default).GetString(bytes));

            return graph;
        }

        /// <summary>
        /// Deserializes JSON data into object.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="input">The input</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static T JsonDeserialize<T>(string input)
        {
            T graph;

            graph = JsonSerializer.DeserializeFromString<T>(input);

            return graph;
        }

        /// <summary>
        /// Deserializes JSON data into object.
        /// </summary>
        /// <param name="bytes">The bytes</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static object JsonDeserialize(byte[] bytes, Encoding encoding = null)
        {
            object graph;

            //using (MemoryStream memoryStream = new MemoryStream(bytes, false))
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer();
            //    graph = serializer.ReadObject(memoryStream);
            //}
            graph = JsonSerializer.DeserializeFromString((encoding ?? Encoding.Default).GetString(bytes), typeof(object));

            return graph;
        }

        /// <summary>
        /// Deserializes JSON data into object.
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static object JsonDeserialize(string input)
        {
            object graph;

            graph = JsonSerializer.DeserializeFromString(input, typeof(object));

            return graph;
        }

        /// <summary>
        /// XMLs the serialize.
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <returns>Serialized data</returns>
        public static byte[] XmlSerialize(object graph)
        {
            byte[] bytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(graph.GetType());
                serializer.WriteObject(memoryStream, graph);

                bytes = memoryStream.ToArray();
            }

            return bytes;
        }

        /// <summary>
        /// XMLs the deserialize.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="bytes">The bytes</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static T XmlDeserialize<T>(byte[] bytes)
        {
            T graph;

            using (MemoryStream memoryStream = new MemoryStream(bytes, false))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                graph = (T)serializer.ReadObject(memoryStream);
            }

            return graph;
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="SerializationHelpers.cs" company="-">
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

namespace Tasslehoff.Common.Helpers
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Tasslehoff.Common.Helpers.Internals;

    /// <summary>
    /// SerializationUtils class.
    /// </summary>
    public static class SerializationHelpers
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

        /// <summary>
        /// Gets the serializer instance.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>A data contract serializer</returns>
        public static JsonSerializerSettings GetSerializerSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new OrderedContractResolver()
            };

            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }

        /// <summary>
        /// JSONs the serialize.
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <returns>Serialized data</returns>
        public static string JsonSerialize(object graph)
        {
            return JsonConvert.SerializeObject(graph, SerializationHelpers.GetSerializerSettings());
        }

        /// <summary>
        /// JSONs the deserialize.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="bytes">The bytes</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static T JsonDeserialize<T>(string value)
        {
            JsonSerializerSettings settings = SerializationHelpers.GetSerializerSettings();
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}

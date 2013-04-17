// -----------------------------------------------------------------------
// <copyright file="ConfigSerializer.cs" company="-">
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

namespace Tasslehoff.Library.Config
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;

    /// <summary>
    /// Serializer for configuration classes.
    /// </summary>
    public static class ConfigSerializer
    {
        // methods

        /// <summary>
        /// Gets the serializer instance.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>A data contract serializer</returns>
        public static XmlObjectSerializer GetSerializer(Type type)
        {
            /*
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
                SerializeReadOnlyTypes = false
            };
            */

            return new DataContractJsonSerializer(type);
        }

        /// <summary>
        /// Deserializes a configuration from the stream.
        /// </summary>
        /// <typeparam name="T">An IConfig implementation</typeparam>
        /// <param name="input">The input</param>
        /// <param name="resetFirst">if set to <c>true</c> [reset first]</param>
        /// <returns>Deserialized configuration class instance</returns>
        public static T Load<T>(Stream input, bool resetFirst = true) where T : IConfig
        {
            XmlObjectSerializer serializer = ConfigSerializer.GetSerializer(typeof(T));
            if (resetFirst)
            {
                input.Position = 0;
            }

            return (T)serializer.ReadObject(input);
        }

        /// <summary>
        /// Serializes a configuration class instance to the stream.
        /// </summary>
        /// <param name="output">The output</param>
        /// <param name="configObject">The config object</param>
        /// <param name="flushAfter">if set to <c>true</c> [flush after]</param>
        public static void Save(Stream output, IConfig configObject, bool flushAfter = true)
        {
            Type type = configObject.GetType();
            
            XmlObjectSerializer serializer = ConfigSerializer.GetSerializer(type);
            serializer.WriteObject(output, configObject);

            if (flushAfter)
            {
                output.Flush();
            }
        }

        /// <summary>
        /// Resets the specified config object.
        /// </summary>
        /// <param name="configObject">The config object</param>
        public static void Reset(IConfig configObject)
        {
            Type type = configObject.GetType();
            
            foreach (PropertyInfo property in type.GetProperties())
            {
                object value = null;
                ConfigEntryAttribute[] attributes = (ConfigEntryAttribute[])property.GetCustomAttributes(typeof(ConfigEntryAttribute), true);
                
                if (attributes.Length > 0)
                {
                    if (attributes[0].SkipInReset)
                    {
                        continue;
                    }
                    
                    value = attributes[0].DefaultValue;
                }
                
                property.SetValue(configObject, value, null);
            }
        }
    }
}

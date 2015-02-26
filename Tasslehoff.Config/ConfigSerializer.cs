// --------------------------------------------------------------------------
// <copyright file="ConfigSerializer.cs" company="-">
// Copyright (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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

using System.IO;
using Tasslehoff.Common.Helpers;

namespace Tasslehoff.Config
{
    /// <summary>
    /// Serializer for configuration classes.
    /// </summary>
    public static class ConfigSerializer
    {
        /// <summary>
        /// Deserializes a configuration from a string.
        /// </summary>
        /// <typeparam name="T">A Config implementation</typeparam>
        /// <param name="input">The input</param>
        /// <returns>Deserialized configuration class instance</returns>
        public static T Load<T>(string input) where T : Config
        {
            return SerializationHelpers.JsonDeserialize<T>(input);
        }

        /// <summary>
        /// Deserializes a configuration from a file.
        /// </summary>
        /// <typeparam name="T">A Config implementation</typeparam>
        /// <param name="path">The path</param>
        /// <returns>Deserialized configuration class instance</returns>
        public static T LoadFromFile<T>(string path) where T : Config
        {
            return ConfigSerializer.Load<T>(File.ReadAllText(path));
        }

        /// <summary>
        /// Deserializes a configuration from a file. If file is not found, creates a new one.
        /// </summary>
        /// <typeparam name="T">A Config implementation</typeparam>
        /// <param name="path">The path</param>
        /// <returns>Deserialized configuration class instance</returns>
        public static T InitFromFile<T>(string path) where T : Config, new()
        {
            if (File.Exists(path))
            {
                return ConfigSerializer.LoadFromFile<T>(path);
            }

            T instance = new T();
            instance.SaveToFile(path);
            return instance;
        }
    }
}

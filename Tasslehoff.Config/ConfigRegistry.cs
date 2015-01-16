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

namespace Tasslehoff.Config
{
    /// <summary>
    /// ConfigRegistry class
    /// </summary>
    public class ConfigRegistry
    {
        // fields

        /// <summary>
        /// Source path
        /// </summary>
        private string sourcePath;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigRegistry"/> class.
        /// </summary>
        /// <param name="path">The path of config files</param>
        public ConfigRegistry(string path)
        {
            this.SourcePath = path;
        }

        // properties

        /// <summary>
        /// Gets or sets the source path
        /// </summary>
        public string SourcePath {
            get
            {
                return this.sourcePath;
            }
            set
            {
                this.sourcePath = value;
            }
        }

        // methods

        /// <summary>
        /// Deserializes a configuration from a file.
        /// </summary>
        /// <typeparam name="T">A Config implementation</typeparam>
        /// <param name="relativePath">The relative path</param>
        /// <returns>Deserialized configuration class instance</returns>
        public T Load<T>(string relativePath) where T : Config, new()
        {
            return ConfigSerializer.LoadFromFile<T>(Path.Combine(this.SourcePath, relativePath));
        }

        /// <summary>
        /// Deserializes a configuration from a file. If file is not found, creates a new one.
        /// </summary>
        /// <typeparam name="T">A Config implementation</typeparam>
        /// <param name="relativePath">The relative path</param>
        /// <returns>Deserialized configuration class instance</returns>
        public T Init<T>(string relativePath) where T : Config, new()
        {
            return ConfigSerializer.InitFromFile<T>(Path.Combine(this.SourcePath, relativePath));
        }
    }
}

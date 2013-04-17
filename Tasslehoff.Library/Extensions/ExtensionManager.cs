// -----------------------------------------------------------------------
// <copyright file="ExtensionManager.cs" company="-">
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

namespace Tasslehoff.Library.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Tasslehoff.Library.Logger;
    using Tasslehoff.Library.Services;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// ExtensionManager class.
    /// </summary>
    public class ExtensionManager : Service
    {
        // fields

        /// <summary>
        /// The assemblies
        /// </summary>
        private readonly ICollection<Assembly> assemblies;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionManager"/> class.
        /// </summary>
        public ExtensionManager() : base()
        {
            this.assemblies = new Collection<Assembly>();
        }

        // attributes

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return "ExtensionManager";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        public ICollection<Assembly> Assemblies
        {
            get
            {
                return this.assemblies;
            }
        }

        // methods

        /// <summary>
        /// Searches the files.
        /// </summary>
        /// <param name="path">The path</param>
        public void SearchFiles(string path)
        {
            DirectoryInfo searchDirectory = new DirectoryInfo(Path.GetDirectoryName(path));
            string filePattern = Path.GetFileName(path);

            FileInfo[] files = searchDirectory.GetFiles(filePattern, SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                this.AddFile(file.FullName);
            }
        }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path</param>
        public void AddFile(string path)
        {
            try
            {
                Assembly assembly = Assembly.Load(AssemblyName.GetAssemblyName(path));
                this.Add(assembly);
            }
            catch (FileNotFoundException ex)
            {
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileLoadingAssembly, path), ex);
            }
            catch (FileLoadException ex)
            {
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileLoadingAssembly, path), ex);
            }
            catch (BadImageFormatException ex)
            {
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileLoadingAssembly, path), ex);
            }
        }

        /// <summary>
        /// Adds the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly</param>
        public void Add(Assembly assembly)
        {
            foreach (Assembly item in this.assemblies)
            {
                if (assembly.FullName == item.FullName)
                {
                    this.Log.Write(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, LocalResource.AssemblyHasBeenLoadedPreviously, item.GetName().Name));
                    return;
                }
            }

            this.assemblies.Add(assembly);
            this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.AssemblyHasBeenLoaded, assembly.GetName().Name, assembly.Location));
        }

        /// <summary>
        /// Removes the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly</param>
        public void Remove(Assembly assembly)
        {
            this.assemblies.Remove(assembly);
            this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.AssemblyHasBeenRemoved, assembly.GetName().Name));
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Assembly[] assemblies = ArrayUtils.GetArray<Assembly>(this.assemblies);

            foreach (Assembly item in assemblies)
            {
                this.Remove(item);
            }
        }

        /// <summary>
        /// Searches the interface.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>Matched types</returns>
        public IEnumerable<Type> SearchInterface(Type type)
        {
            return this.SearchInterface(type.FullName);
        }

        /// <summary>
        /// Searches the interface.
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>Matches types</returns>
        public IEnumerable<Type> SearchInterface(string typeName)
        {
            ICollection<Type> types = new Collection<Type>();

            foreach (Assembly item in this.assemblies)
            {
                foreach (Type exportType in item.GetExportedTypes())
                {
                    if (!exportType.IsClass)
                    {
                        continue;
                    }

                    if (exportType.IsAbstract)
                    {
                        continue;
                    }

                    if (exportType.GetInterface(typeName) == null)
                    {
                        continue;
                    }

                    types.Add(exportType);
                }
            }

            return types;
        }

        /// <summary>
        /// Searches the type of the base.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>Matched types</returns>
        public IEnumerable<Type> SearchBaseType(Type type)
        {
            ICollection<Type> types = new Collection<Type>();

            foreach (Assembly item in this.assemblies)
            {
                foreach (Type exportType in item.GetExportedTypes())
                {
                    if (!exportType.IsClass)
                    {
                        continue;
                    }

                    if (exportType.IsAbstract)
                    {
                        continue;
                    }

                    if (exportType.BaseType == null)
                    {
                        continue;
                    }

                    if (!exportType.BaseType.Equals(type))
                    {
                        continue;
                    }

                    types.Add(exportType);
                }
            }

            return types;
        }

        /// <summary>
        /// Searches the type.
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>The exact type</returns>
        public Type SearchType(string typeName)
        {
            foreach (Assembly item in this.assemblies)
            {
                foreach (Type exportType in item.GetExportedTypes())
                {
                    if (!exportType.IsClass)
                    {
                        continue;
                    }

                    if (exportType.IsAbstract)
                    {
                        continue;
                    }

                    if (!exportType.FullName.Equals(typeName, StringComparison.InvariantCulture))
                    {
                        continue;
                    }

                    return exportType;
                }
            }

            return null;
        }
    }
}

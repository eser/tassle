// --------------------------------------------------------------------------
// <copyright file="ExtensionFinder.cs" company="-">
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using Tasslehoff.Common.Helpers;
using Tasslehoff.Logging;
using Tasslehoff.Services;

namespace Tasslehoff.Extensibility
{
    /// <summary>
    /// ExtensionManager class.
    /// </summary>
    public class ExtensionFinder : Service
    {
        // fields

        /// <summary>
        /// The application domain
        /// </summary>
        private AppDomain applicationDomain = null;

        /// <summary>
        /// The assemblies
        /// </summary>
        private readonly IDictionary<string, Assembly> assemblies;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionFinder"/> class.
        /// </summary>
        public ExtensionFinder()
            : base()
        {
            this.assemblies = new Dictionary<string, Assembly>();
        }

        // properties

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
        /// Gets or sets the application domain.
        /// </summary>
        /// <value>
        /// The application domain.
        /// </value>
        public AppDomain ApplicationDomain
        {
            get
            {
                return this.applicationDomain;
            }
            private set
            {
                this.applicationDomain = value;
            }
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        public IDictionary<string, Assembly> Assemblies
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
        public void SearchFiles(params string[] paths)
        {
            foreach (string path in paths)
            {
                string[] searchDirectories = Directory.GetDirectories(path);

                foreach (string searchDirectory in searchDirectories)
                {
                    string name = Path.GetDirectoryName(searchDirectory);
                    string filename = Path.Combine(path, name, name + ".dll");

                    this.AddFile(filename);
                }
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
            if (this.ApplicationDomain == null)
            {
                this.ApplicationDomain = AppDomain.CreateDomain("extensions");
            }

            AssemblyName assemblyName = assembly.GetName();

            if (this.assemblies.ContainsKey(assemblyName.Name))
            {
                this.Log.Write(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, LocalResource.AssemblyHasBeenLoadedPreviously, assemblyName.Name));
                return;
            }

            this.assemblies.Add(assemblyName.Name, assembly);
            this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.AssemblyHasBeenLoaded, assemblyName.Name, assembly.Location));
        }

        /// <summary>
        /// Removes the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly</param>
        public void Remove(string assemblyName)
        {
            this.assemblies.Remove(assemblyName);
            this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.AssemblyHasBeenRemoved, assemblyName));
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            string[] assemblies = ArrayHelpers.GetArray<string>(this.assemblies.Keys);

            foreach (string item in assemblies)
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

            foreach (Assembly item in this.assemblies.Values)
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

            foreach (Assembly item in this.assemblies.Values)
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
            foreach (Assembly item in this.assemblies.Values)
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

        /// <summary>
        /// Unloads the application domain.
        /// </summary>
        public void UnloadDomain()
        {
            AppDomain.Unload(this.ApplicationDomain);
            this.ApplicationDomain = null;
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected override void OnDispose(bool releaseManagedResources)
        {
            base.OnDispose(releaseManagedResources);

            AppDomain.Unload(this.applicationDomain);
        }
    }
}

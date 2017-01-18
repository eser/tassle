// --------------------------------------------------------------------------
// <copyright file="VirtualLibraryRegistry.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using Tasslehoff.DataStructures.Collections;

namespace Tasslehoff.Extensibility.VirtualLibrary
{
    /// <summary>
    /// VirtualLibraryRegistry class.
    /// </summary>
    [ComVisible(false)]
    public class VirtualLibraryRegistry : DictionaryBase<string, Tuple<string, Assembly>>
    {
        // fields

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static VirtualLibraryRegistry instance = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualLibraryRegistry"/> class.
        /// </summary>
        public VirtualLibraryRegistry() : base()
        {
            if (VirtualLibraryRegistry.instance == null)
            {
                VirtualLibraryRegistry.instance = this;
            }
        }

        // properties

        /// <summary>
        /// Gets or Sets singleton instance.
        /// </summary>
        public static VirtualLibraryRegistry Instance
        {
            get
            {
                return VirtualLibraryRegistry.instance;
            }
            set
            {
                VirtualLibraryRegistry.instance = value;
            }
        }

        // methods

        /// <summary>
        /// Registers itself.
        /// </summary>
        public void Register()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new VirtualLibraryPathProvider());
        }

        /// <summary>
        /// Adds the specified virtual path.
        /// </summary>
        /// <param name="virtualPathRoot">The virtual path's root.</param>
        /// <param name="ns">The namespace.</param>
        /// <param name="assembly">The assembly.</param>
        public void Add(string virtualPathRoot, string ns, Assembly assembly)
        {
            this.Add(virtualPathRoot, new Tuple<string, Assembly>(ns, assembly));
        }

        /// <summary>
        /// Checks if matches the specified virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Result</returns>
        public string MatchingKey(string virtualPath)
        {
            string checkPath = VirtualPathUtility.ToAppRelative(virtualPath);

            foreach (string key in this.Keys)
            {
                if (checkPath.StartsWith(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    return key;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the resource stream.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Stream</returns>
        public Stream GetResourceStream(string virtualPath)
        {
            string checkPath = VirtualPathUtility.ToAppRelative(virtualPath);

            foreach (string key in this.Keys)
            {
                if (checkPath.StartsWith(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    Tuple<string, Assembly> values = this[key];
                    return values.Item2.GetManifestResourceStream(values.Item1 + "." + checkPath.Substring(key.Length).Replace('/', '.'));
                }
            }

            return null;
        }
    }
}

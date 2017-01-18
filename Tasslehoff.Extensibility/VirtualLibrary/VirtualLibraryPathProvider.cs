// --------------------------------------------------------------------------
// <copyright file="VirtualLibraryPathProvider.cs" company="-">
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
using System.Collections;
using System.IO;
using System.Web.Caching;
using System.Web.Hosting;

namespace Tasslehoff.Extensibility.VirtualLibrary
{
    /// <summary>
    /// VirtualLibraryPathProvider class.
    /// </summary>
    public class VirtualLibraryPathProvider : VirtualPathProvider
    {
        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool FileExists(string virtualPath)
        {
            Stream stream = VirtualLibraryRegistry.Instance.GetResourceStream(virtualPath);

            if (stream != null)
            {
                return true;
            }

            return Previous.FileExists(virtualPath);
        }

        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile" /> class that represents a file in the virtual file system.
        /// </returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            Stream stream = VirtualLibraryRegistry.Instance.GetResourceStream(virtualPath);

            if (stream != null)
            {
                return new VirtualLibraryFile(virtualPath, stream);
            }

            return Previous.GetFile(virtualPath);
        }

        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Caching.CacheDependency" /> object for the specified virtual resources.
        /// </returns>
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (VirtualLibraryRegistry.Instance.MatchingKey(virtualPath) != null)
            {
                return null;
            }
            
            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        /// <summary>
        /// Returns a hash of the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other virtual resources required by the primary virtual resource.</param>
        /// <returns>
        /// A hash of the specified virtual paths.
        /// </returns>
        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            if (VirtualLibraryRegistry.Instance.MatchingKey(virtualPath) != null)
            {
                // Returns the virtual path value which is made up of the views GUID value that only
                // changes once the view has been updated - essentially working like an updated FileHash
                // but also the ID of the requested view
                return virtualPath;
            }

            return Previous.GetFileHash(virtualPath, virtualPathDependencies);
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="VirtualLibraryFile.cs" company="-">
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
using System.Web.Hosting;

namespace Tasslehoff.Extensibility.VirtualLibrary
{
    /// <summary>
    /// VirtualLibraryFile class.
    /// </summary>
    public class VirtualLibraryFile : VirtualFile
    {
        // fields

        /// <summary>
        /// The source stream
        /// </summary>
        private readonly Stream sourceStream;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualLibraryFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="sourceStream">The source stream.</param>
        public VirtualLibraryFile(string virtualPath, Stream sourceStream)
            : base(virtualPath)
        {
            this.sourceStream = sourceStream;
        }

        // properties

        /// <summary>
        /// Gets SourceStream field
        /// </summary>
        /// <value>
        /// The source stream.
        /// </value>
        public Stream SourceStream
        {
            get
            {
                return this.sourceStream;
            }
        }

        // methods

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open()
        {
            return this.SourceStream;
        }
    }
}

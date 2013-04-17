// -----------------------------------------------------------------------
// <copyright file="WebServiceEndpoint.cs" company="-">
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

namespace Tasslehoff.Library.WebServices
{
    using System;

    /// <summary>
    /// WebServiceEndpoint class.
    /// </summary>
    public class WebServiceEndpoint
    {
        // fields

        /// <summary>
        /// The name
        /// </summary>
        private string name;

        /// <summary>
        /// The type
        /// </summary>
        private Type type;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceEndpoint" /> class.
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="type">The type</param>
        public WebServiceEndpoint(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }

        // properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type
        {
            get
            {
                return this.type;
            }
        }
    }
}

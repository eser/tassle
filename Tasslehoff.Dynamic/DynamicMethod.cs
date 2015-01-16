// --------------------------------------------------------------------------
// <copyright file="DynamicMethod.cs" company="-">
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

using System.Reflection.Emit;

namespace Tasslehoff.Dynamic
{
    /// <summary>
    /// DynamicMethod class.
    /// </summary>
    public class DynamicMethod
    {
        // fields

        /// <summary>
        /// The method builder.
        /// </summary>
        private readonly MethodBuilder methodBuilder;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMethod"/> class.
        /// </summary>
        /// <param name="methodBuilder">The method builder instance</param>
        public DynamicMethod(MethodBuilder methodBuilder)
        {
            this.methodBuilder = methodBuilder;
        }

        // properties

        /// <summary>
        /// Gets or sets the method builder.
        /// </summary>
        /// <value>
        /// The method builder.
        /// </value>
        public MethodBuilder MethodBuilder
        {
            get
            {
                return this.methodBuilder;
            }
        }

        // methods

    }
}

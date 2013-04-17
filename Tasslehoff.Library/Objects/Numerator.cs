// -----------------------------------------------------------------------
// <copyright file="Numerator.cs" company="-">
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

namespace Tasslehoff.Library.Objects
{
    /// <summary>
    /// Numerator class.
    /// </summary>
    public class Numerator
    {
        // fields

        /// <summary>
        /// The sync lock
        /// </summary>
        private readonly object syncLock;

        /// <summary>
        /// The next number
        /// </summary>
        private int nextNumber;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Numerator"/> class.
        /// </summary>
        public Numerator()
        {
            this.syncLock = new object();
            this.nextNumber = int.MinValue;
        }

        // methods

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>The next number</returns>
        public int Get()
        {
            lock (this.syncLock)
            {
                unchecked
                {
                    ////if(this.nextNumber + 1 >= int.MaxValue) {
                    ////    this.nextNumber = int.MinValue;
                    ////}

                    return this.nextNumber++;
                }
            }
        }
    }
}
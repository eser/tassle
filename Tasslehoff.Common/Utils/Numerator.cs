// --------------------------------------------------------------------------
// <copyright file="Numerator.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
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

namespace Tasslehoff.Common.Utils
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Numerator class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Numerator : ICloneable
    {
        // fields

        /// <summary>
        /// The sync lock
        /// </summary>
        [IgnoreDataMember]
        [NonSerialized]
        private readonly object syncLock = new object();

        /// <summary>
        /// The next number
        /// </summary>
        [DataMember(Name = "NextNumber")]
        private int nextNumber;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Numerator"/> class.
        /// </summary>
        /// <param name="startNumber">Starting number</param>
        public Numerator(int startNumber = int.MinValue)
        {
            this.nextNumber = startNumber;
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

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new Numerator(this.nextNumber);
        }
     }
}
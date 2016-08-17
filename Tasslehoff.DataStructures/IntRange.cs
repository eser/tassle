﻿// --------------------------------------------------------------------------
// <copyright file="IntRange.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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
using System.Runtime.Serialization;

namespace Tasslehoff.DataStructures
{
    /// <summary>
    /// IntRange class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class IntRange : ICloneable, IEnumerable
    {
        // fields

        /// <summary>
        /// The start
        /// </summary>
        [DataMember(Name = "Start")]
        private readonly int start;

        /// <summary>
        /// The end
        /// </summary>
        [DataMember(Name = "End")]
        private readonly int end;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntRange" /> class.
        /// </summary>
        /// <param name="start">The start</param>
        /// <param name="end">The end</param>
        public IntRange(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        // properties

        /// <summary>
        /// Gets the start.
        /// </summary>
        [IgnoreDataMember]
        public int Start
        {
            get
            {
                return this.start;
            }
        }

        /// <summary>
        /// Gets the end.
        /// </summary>
        [IgnoreDataMember]
        public int End
        {
            get
            {
                return this.end;
            }
        }

        // methods

        /// <summary>
        /// Gets the range table.
        /// </summary>
        /// <returns>Set of integers</returns>
        public int[] GetRangeTable()
        {
            int[] table = new int[this.end - this.start + 1];

            for (int i = 0; i < table.Length; i++)
            {
                table[i] = this.start + i;
            }

            return table;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            int length = this.end - this.start;

            for (int i = 0; i <= length; i++)
            {
                yield return this.start + i;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
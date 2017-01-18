// --------------------------------------------------------------------------
// <copyright file="SortedList{T}.cs" company="-">
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tasslehoff.DataStructures.Collections
{
    /// <summary>
    /// An sorted list class.
    /// </summary>
    /// <typeparam name="T">Any object type can be stored in a collection</typeparam>
    [Serializable]
    [DataContract]
    public class SortedList<T> : IList<T> where T : IComparable
    {
        // fields

        /// <summary>
        /// The values
        /// </summary>
        [DataMember(Name = "Values")]
        private List<T> values;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedList{T}"/> class.
        /// </summary>
        public SortedList()
        {
            this.values = new List<T>();
        }

        // properties

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        [IgnoreDataMember]
        public int Count
        {
            get
            {
                return this.values.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        [IgnoreDataMember]
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        // indexers

        /// <summary>
        /// Gets or sets the element with the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Object in the dictionary</returns>
        public T this[int index]
        {
            get
            {
                return this.values[index];
            }
            set
            {
                this.values.RemoveAt(index);
                this.Add(value);
            }
        }

        // methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Add(T value)
        {
            int index = ~this.values.BinarySearch(value);
            if (index < 0)
            {
                this.values.Add(value);
            }
            else
            {
                this.values.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts the specified value at specified index.
        /// </summary>
        /// <param name="index">Index of value to be inserted.</param>
        /// <param name="value">Value to be inserted.</param>
        public void Insert(int index, T value)
        {
            throw new NotImplementedException("Cannot insert at index; must preserve order.");
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            return this.values.Contains(item);
        }

        /// <summary>
        /// Returns the index of specified element.
        /// </summary>
        /// <param name="item">The element</param>
        /// <returns>The index</returns>
        public int IndexOf(T item)
        {
            int index = this.values.BinarySearch(item);
            return index < 0 ? -1 : index;
        }

        /// <summary>
        /// Removes the element with the specified element from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="item">The element to be removed.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.
        /// </returns>
        public bool Remove(T item)
        {
            var index = this.values.BinarySearch(item);
            if (index < 0)
            {
                return false;
            }

            this.values.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The index of element to be removed</param>
        public void RemoveAt(int index)
        {
            this.values.RemoveAt(index);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            this.values.Clear();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < this.values.Count; i++)
            {
                array[arrayIndex + i] = this.values[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.values.Count; i++)
            {
                yield return this.values[i];
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

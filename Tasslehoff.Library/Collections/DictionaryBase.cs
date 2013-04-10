// -----------------------------------------------------------------------
// <copyright file="DictionaryBase.cs" company="-">
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

namespace Tasslehoff.Library.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// An essential dictionary base class for making implementations easy.
    /// </summary>
    /// <typeparam name="T">Any object type can be stored in a collection</typeparam>
    public class DictionaryBase<T> : IEnumerable
    {
        // fields

        /// <summary>
        /// The keys
        /// </summary>
        private Collection<string> keys;

        /// <summary>
        /// The values
        /// </summary>
        private Collection<T> values;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryBase{T}"/> class.
        /// </summary>
        public DictionaryBase()
        {
            this.keys = new Collection<string>();
            this.values = new Collection<T>();
        }

        // properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return this.keys.Count;
            }
        }

        /// <summary>
        /// Gets or sets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public Collection<string> Keys
        {
            get
            {
                return this.keys;
            }

            protected set
            {
                this.keys = value;
            }
        }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public Collection<T> Values
        {
            get
            {
                return this.values;
            }

            protected set
            {
                this.values = value;
            }
        }

        // indexer

        /// <summary>
        /// Gets or sets the <see cref="`0"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="`0"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>Object in the dictionary</returns>
        public T this[string key]
        {
            get
            {
                int index = this.keys.IndexOf(key);
                if (index == -1)
                {
                    return default(T);
                }
                
                return this.values[index];
            }

            set
            {
                this.Add(key, value, true);
            }
        }

        // methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        /// <exception cref="System.ArgumentException">If specified key is already exist in the dictionary</exception>
        public void Add(string key, T value, bool replace = false)
        {
            int index = this.keys.IndexOf(key);
            if (index != -1)
            {
                if (replace)
                {
                    this.values[index] = value;
                    return;
                }
                
                throw new ArgumentException();
            }
            
            this.keys.Add(key);
            this.values.Add(value);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Is the key found and removed</returns>
        public bool Remove(string key)
        {
            int index = this.keys.IndexOf(key);
            if (index != -1)
            {
                this.keys.RemoveAt(index);
                this.values.RemoveAt(index);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.keys.Clear();
            this.values.Clear();
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.keys.Contains(key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < this.keys.Count; i++)
            {
                yield return new KeyValuePair<string, T>(this.keys[i], this.values[i]);
            }
        }
    }
}

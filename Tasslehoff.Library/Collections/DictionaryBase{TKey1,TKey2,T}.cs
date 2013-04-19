// -----------------------------------------------------------------------
// <copyright file="DictionaryBase{TKey1,TKey2,T}.cs" company="-">
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
    /// <typeparam name="TKey1">The type of the key1.</typeparam>
    /// <typeparam name="TKey2">The type of the key2.</typeparam>
    /// <typeparam name="T">Any object type can be stored in a collection</typeparam>
    public class DictionaryBase<TKey1, TKey2, T> : IDictionary<TKey1, T>
    {
        // fields

        /// <summary>
        /// The keys 1
        /// </summary>
        private Collection<TKey1> keys1;

        /// <summary>
        /// The keys 2
        /// </summary>
        private Collection<TKey2> keys2;

        /// <summary>
        /// The values
        /// </summary>
        private Collection<T> values;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryBase{TKey1, TKey2, T}"/> class.
        /// </summary>
        public DictionaryBase()
        {
            this.keys1 = new Collection<TKey1>();
            this.keys2 = new Collection<TKey2>();
            this.values = new Collection<T>();
        }

        // properties

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get
            {
                return this.keys1.Count;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<TKey1> Keys
        {
            get
            {
                return this.keys1;
            }
        }

        /// <summary>
        /// Gets the keys2.
        /// </summary>
        /// <value>
        /// The keys2.
        /// </value>
        public ICollection<TKey2> Keys2
        {
            get
            {
                return this.keys2;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<T> Values
        {
            get
            {
                return this.values;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        // indexer

        /// <summary>
        /// Gets the <see cref="`2" /> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="`2" />.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>Object in the dictionary</returns>
        public T this[TKey1 key]
        {
            get
            {
                T value;
                if (this.TryGetValue(key, out value))
                {
                    return value;
                }

                return default(T);
            }

            set
            {
                this.Add(key, value, true);
            }
        }

        /// <summary>
        /// Gets the <see cref="`2" /> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="`2" />.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>Object in the dictionary</returns>
        public T this[TKey2 key]
        {
            get
            {
                T value;
                if (this.TryGetValue(key, out value))
                {
                    return value;
                }

                return default(T);
            }
        }

        /// <summary>
        /// Sets the <see cref="`2" /> with the specified key1.
        /// </summary>
        /// <value>
        /// The <see cref="`2" />.
        /// </value>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns>Object in the dictionary</returns>
        public T this[TKey1 key1, TKey2 key2]
        {
            set
            {
                this.Add(key1, key2, value, true);
            }
        }

        // methods

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey1 key, out T value)
        {
            int index = this.keys1.IndexOf(key);
            if (index == -1)
            {
                value = default(T);
                return false;
            }

            value = this.values[index];
            return true;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey2 key, out T value)
        {
            int index = this.keys2.IndexOf(key);
            if (index == -1)
            {
                value = default(T);
                return false;
            }

            value = this.values[index];
            return true;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <param name="value">The value</param>
        /// <param name="replace">if set to <c>true</c> [replace]</param>
        /// <exception cref="System.ArgumentException">If specified key is already exist in the dictionary</exception>
        public void Add(TKey1 key1, TKey2 key2, T value, bool replace = false)
        {
            int index1 = this.keys1.IndexOf(key1);
            if (index1 != -1)
            {
                if (replace)
                {
                    this.values[index1] = value;
                    this.keys2[index1] = key2;
                    return;
                }

                throw new ArgumentException();
            }

            int index2 = this.keys2.IndexOf(key2);
            if (index2 != -1)
            {
                if (replace)
                {
                    this.values[index2] = value;
                    this.keys1[index2] = key1;
                    return;
                }

                throw new ArgumentException();
            }

            this.keys1.Add(key1);
            this.keys2.Add(key2);
            this.values.Add(value);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        public void Add(TKey1 key, T value, bool replace)
        {
            this.Add(key, default(TKey2), value, replace);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey1 key, T value)
        {
            this.Add(key, default(TKey2), value, false);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<TKey1, T> item)
        {
            this.Add(item.Key, default(TKey2), item.Value, false);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey1 key)
        {
            return this.keys1.Contains(key);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey2 key)
        {
            return this.keys2.Contains(key);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey1, T> item)
        {
            int index = this.keys1.IndexOf(item.Key);
            if (index != -1 && this.values[index].Equals(item.Value))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Is the key found and removed
        /// </returns>
        public bool Remove(TKey1 key)
        {
            int index = this.keys1.IndexOf(key);
            if (index != -1)
            {
                this.keys1.RemoveAt(index);
                this.keys2.RemoveAt(index);
                this.values.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Is the key found and removed
        /// </returns>
        public bool Remove(TKey2 key)
        {
            int index = this.keys2.IndexOf(key);
            if (index != -1)
            {
                this.keys1.RemoveAt(index);
                this.keys2.RemoveAt(index);
                this.values.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<TKey1, T> item)
        {
            int index = this.keys1.IndexOf(item.Key);
            if (index != -1 && this.values[index].Equals(item.Value))
            {
                this.keys1.RemoveAt(index);
                this.keys2.RemoveAt(index);
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
            this.keys1.Clear();
            this.keys2.Clear();
            this.values.Clear();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey1, T>[] array, int arrayIndex)
        {
            for (int i = 0; i < this.keys1.Count; i++)
            {
                array[arrayIndex + i] = new KeyValuePair<TKey1, T>(this.keys1[i], this.values[i]);
            }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey2, T>[] array, int arrayIndex)
        {
            for (int i = 0; i < this.keys2.Count; i++)
            {
                array[arrayIndex + i] = new KeyValuePair<TKey2, T>(this.keys2[i], this.values[i]);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey1, T>> GetEnumerator()
        {
            for (int i = 0; i < this.keys1.Count; i++)
            {
                yield return new KeyValuePair<TKey1, T>(this.keys1[i], this.values[i]);
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
            for (int i = 0; i < this.keys1.Count; i++)
            {
                yield return new KeyValuePair<TKey1, T>(this.keys1[i], this.values[i]);
            }
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="DictionaryBase{TKey,T}.cs" company="-">
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Tasslehoff.DataStructures.Collections
{
    /// <summary>
    /// An essential dictionary base class for making implementations easy.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="T">Any object type can be stored in a collection</typeparam>
    [Serializable]
    [DataContract]
    public class DictionaryBase<TKey, T> : IDictionary<TKey, T>, ICloneable
    {
        // fields

        /// <summary>
        /// The keys
        /// </summary>
        [DataMember(Name = "Keys")]
        private Collection<TKey> keys;

        /// <summary>
        /// The values
        /// </summary>
        [DataMember(Name = "Values")]
        private Collection<T> values;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryBase{TKey, T}"/> class.
        /// </summary>
        public DictionaryBase()
        {
            this.keys = new Collection<TKey>();
            this.values = new Collection<T>();
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
                return this.keys.Count;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        [IgnoreDataMember]
        public ICollection<TKey> Keys
        {
            get
            {
                return this.keys;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        [IgnoreDataMember]
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
        [IgnoreDataMember]
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        // indexer

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Object in the dictionary</returns>
        public T this[TKey key]
        {
            get
            {
                int index = this.keys.IndexOf(key);
                if (index == -1)
                {
                    throw new KeyNotFoundException();
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
        public void Add(TKey key, T value, bool replace)
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
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, T value)
        {
            this.Add(key, value, false);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<TKey, T> item)
        {
            this.Add(item.Key, item.Value, false);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            return this.keys.Contains(key);
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, T> item)
        {
            int index = this.keys.IndexOf(item.Key);
            if (index != -1 && this.values[index].Equals(item.Value))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public bool Remove(TKey key)
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
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<TKey, T> item)
        {
            int index = this.keys.IndexOf(item.Key);
            if (index != -1 && this.values[index].Equals(item.Value))
            {
                this.keys.RemoveAt(index);
                this.values.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            this.keys.Clear();
            this.values.Clear();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey, T>[] array, int arrayIndex)
        {
            for (int i = 0; i < this.keys.Count; i++)
            {
                array[arrayIndex + i] = new KeyValuePair<TKey, T>(this.keys[i], this.values[i]);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey key, out T value)
        {
            int index = this.keys.IndexOf(key);
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
        bool IDictionary<TKey, T>.TryGetValue(TKey key, out T value)
        {
            return this.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, T>> GetEnumerator()
        {
            for (int i = 0; i < this.keys.Count; i++)
            {
                yield return new KeyValuePair<TKey, T>(this.keys[i], this.values[i]);
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

    }
}

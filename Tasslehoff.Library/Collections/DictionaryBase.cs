//
//  DictionaryBase.cs
//
//  Author:
//       larukedi <eser@sent.com>
//
//  Copyright (c) 2013 larukedi
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tasslehoff.Library.Collections
{
    public class DictionaryBase<T> : IEnumerable
    {
        // fields
        protected Collection<string> keys;
        protected Collection<T> values;

        // constructors
        public DictionaryBase()
        {
            this.keys = new Collection<string>();
            this.values = new Collection<T>();
        }

        // properties
        public int Count {
            get {
                return this.keys.Count;
            }
        }

        // indexer
        public T this[string key] {
            get {
                int _index = this.keys.IndexOf(key);
                if(_index == -1) {
                    return default(T);
                }
                
                return this.values[_index];
            }
            set {
                this.Add(key, value, true);
            }
        }

        // methods
        public void Add(string key, T value, bool replace = false) {
            int _index = this.keys.IndexOf(key);
            if(_index != -1) {
                if(replace) {
                    this.values[_index] = value;
                    return;
                }
                
                throw new ArgumentException();
            }
            
            this.keys.Add(key);
            this.values.Add(value);
        }

        public bool Remove(string key) {
            int _index = this.keys.IndexOf(key);
            if(_index != -1) {
                this.keys.RemoveAt(_index);
                this.values.RemoveAt(_index);
                return true;
            }
            
            return false;
        }

        public void Clear() {
            this.keys.Clear();
            this.values.Clear();
        }

        public bool ContainsKey(string key) {
            return this.keys.Contains(key);
        }

        // implementations
        public IEnumerator GetEnumerator() {
            for(int _i = 0;_i < this.keys.Count;_i++) {
                yield return new KeyValuePair<string, T>(this.keys[_i], this.values[_i]);
            }
        }
    }
}


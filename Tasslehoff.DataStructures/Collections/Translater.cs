// --------------------------------------------------------------------------
// <copyright file="Translater.cs" company="-">
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tasslehoff.DataStructures.Collections
{
    /// <summary>
    /// Translater class.
    /// </summary>
    [ComVisible(false)]
    [Serializable]
    public class Translater : DictionaryBase<string, Tuple<Func<string>, string>>
    {
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Translater"/> class.
        /// </summary>
        public Translater()
        {
        }

        // methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, string value)
        {
            this.Add(key, new Tuple<Func<string>, string>(null, value), false);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value dynamically constructed.</param>
        public void Add(string key, Func<string> value)
        {
            this.Add(key, new Tuple<Func<string>, string>(value, null), false);
        }

        /// <summary>
        /// Translates given string
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>Translated string.</returns>
        public string Translate(string source)
        {
            foreach (KeyValuePair<string, Tuple<Func<string>, string>> pair in this)
            {
                if (pair.Value.Item1 != null)
                {
                    source = source.Replace(pair.Key, pair.Value.Item1.Invoke());
                }
                else
                {
                    source = source.Replace(pair.Key, pair.Value.Item2);
                }
            }

            return source;
        }
    }
}

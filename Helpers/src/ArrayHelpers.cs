// --------------------------------------------------------------------------
// <copyright file="ArrayHelpers.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System.Collections.Generic;

namespace Tassle {
    /// <summary>
    /// ArrayUtils class.
    /// </summary>
    public static class ArrayHelpers {
        // methods

        /// <summary>
        /// Gets the array.
        /// </summary>
        /// <typeparam name="T">The type array contains</typeparam>
        /// <param name="collection">The collection</param>
        /// <returns>Array of given type</returns>
        public static T[] GetArray<T>(ICollection<T> collection) {
            var array = new T[collection.Count];
            collection.CopyTo(array, 0);

            return array;
        }

        /// <summary>
        /// Gets the array.
        /// </summary>
        /// <typeparam name="T">The type array contains</typeparam>
        /// <param name="enumerable">The enumerable</param>
        /// <returns>Array of given type</returns>
        public static T[] GetArray<T>(IEnumerable<T> enumerable) {
            var collection = new List<T>(enumerable);

            return collection.ToArray();
        }
    }
}

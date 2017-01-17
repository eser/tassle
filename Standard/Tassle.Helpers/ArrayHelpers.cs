// --------------------------------------------------------------------------
// <copyright file="ArrayHelpers.cs" company="-">
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

using System.Collections.Generic;

namespace Tassle.Helpers
{
    /// <summary>
    /// ArrayUtils class.
    /// </summary>
    public static class ArrayHelpers
    {
        // methods

        /// <summary>
        /// Gets the array.
        /// </summary>
        /// <typeparam name="T">The type array contains</typeparam>
        /// <param name="collection">The collection</param>
        /// <returns>Array of given type</returns>
        public static T[] GetArray<T>(ICollection<T> collection)
        {
            T[] array = new T[collection.Count];
            collection.CopyTo(array, 0);

            return array;
        }

        /// <summary>
        /// Gets the array.
        /// </summary>
        /// <typeparam name="T">The type array contains</typeparam>
        /// <param name="enumerable">The enumerable</param>
        /// <returns>Array of given type</returns>
        public static T[] GetArray<T>(IEnumerable<T> enumerable)
        {
            List<T> collection = new List<T>(enumerable);
            return collection.ToArray();
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="HashHelpers.cs" company="-">
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

namespace Tassle.Helpers {
    /// <summary>
    /// HashUtils class.
    /// </summary>
    public static class HashHelpers {
        // methods

        /// <summary>
        /// This is a simple hashing function from "Robert Sedgwicks Hashing" in C book.
        /// Also, some simple optimizations to the algorithm in order to speed up
        /// its hashing process have been added. from: http://www.partow.net
        /// </summary>
        /// <param name="input">array of objects, parameters combination that you need
        /// to get a unique hash code for them</param>
        /// <returns>Hash code</returns>
        public static int RSHash(params object[] input) {
            const int B = 378551;
            int a = 63689;
            int hash = 0;

            //// I have added the unchecked keyword to make sure
            //// not get an overflow exception.
            //// It can be enhanced later by catching the OverflowException.

            unchecked {
                for (var i = 0; i < input.Length; i++) {
                    if (input[i] != null) {
                        hash = (hash * a) + input[i].GetHashCode();
                        a = a * B;
                    }
                }
            }

            return hash;
        }
    }
}

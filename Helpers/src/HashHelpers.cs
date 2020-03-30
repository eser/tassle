// --------------------------------------------------------------------------
// <copyright file="HashHelpers.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

namespace Tassle {
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

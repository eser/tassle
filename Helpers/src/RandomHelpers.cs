// --------------------------------------------------------------------------
// <copyright file="RandomHelpers.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Text;

namespace Tassle.Helpers {
    /// <summary>
    /// RandomUtils class.
    /// </summary>
    public static class RandomHelpers {
        // fields

        /// <summary>
        /// The random object
        /// </summary>
        private static Random randomObject;

        // constructors

        /// <summary>
        /// Initializes static members of the <see cref="RandomHelpers"/> class.
        /// </summary>
        static RandomHelpers() {
            RandomHelpers.randomObject = new Random();
        }

        // properties

        /// <summary>
        /// Gets or sets the random object.
        /// </summary>
        /// <value>
        /// The random object.
        /// </value>
        public static Random RandomObject {
            get => RandomHelpers.randomObject;
            set => RandomHelpers.randomObject = value;
        }

        // methods

        /// <summary>
        /// Generates a random GUID.
        /// </summary>
        /// <returns>Generated GUID</returns>
        public static Guid RandomGuid() {
            var seed = new byte[16];

            for (var i = 0; i < seed.Length; i++) {
                seed[i] = (byte)RandomHelpers.randomObject.Next(255);
            }

            return new Guid(seed);
        }

        /// <summary>
        /// Generates a random number.
        /// </summary>
        /// <param name="min">The min</param>
        /// <param name="max">The max</param>
        /// <returns>Generated number</returns>
        public static int RandomNumber(int min, int max) {
            return RandomHelpers.RandomNumber(RandomHelpers.randomObject, min, max);
        }

        /// <summary>
        /// Generates a random number.
        /// </summary>
        /// <param name="random">The random</param>
        /// <param name="min">The min</param>
        /// <param name="max">The max</param>
        /// <returns>Generated number</returns>
        public static int RandomNumber(Random random, int min, int max) {
            return random.Next(min, max);
        }

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>Generated string</returns>
        public static string RandomString(int size) {
            return RandomHelpers.RandomString(RandomHelpers.randomObject, size);
        }

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="random">The random</param>
        /// <param name="size">The size</param>
        /// <returns>Generated string</returns>
        public static string RandomString(Random random, int size) {
            var builder = new StringBuilder();

            for (var i = 0; i < size; i++) {
                char currentChar = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65)));

                builder.Append(currentChar);
            }

            return builder.ToString();
        }
    }
}

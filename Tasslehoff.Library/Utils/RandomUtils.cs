// -----------------------------------------------------------------------
// <copyright file="RandomUtils.cs" company="-">
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

namespace Tasslehoff.Library.Utils
{
    using System;
    using System.Text;

    /// <summary>
    /// RandomUtils class.
    /// </summary>
    public static class RandomUtils
    {
        // fields

        /// <summary>
        /// The random object
        /// </summary>
        private static Random randomObject;

        // constructors

        /// <summary>
        /// Initializes static members of the <see cref="RandomUtils"/> class.
        /// </summary>
        static RandomUtils()
        {
            RandomUtils.randomObject = new Random();
        }

        // properties

        /// <summary>
        /// Gets or sets the random object.
        /// </summary>
        /// <value>
        /// The random object.
        /// </value>
        public static Random RandomObject
        {
            get
            {
                return RandomUtils.randomObject;
            }

            set
            {
                RandomUtils.randomObject = value;
            }
        }

        // methods

        /// <summary>
        /// Generates a random GUID.
        /// </summary>
        /// <returns>Generated GUID</returns>
        public static Guid RandomGuid()
        {
            byte[] seed = new byte[16];

            for (int i = 0; i < seed.Length; i++)
            {
                seed[i] = (byte)RandomUtils.randomObject.Next(255);
            }

            return new Guid(seed);
        }

        /// <summary>
        /// Generates a random number.
        /// </summary>
        /// <param name="min">The min</param>
        /// <param name="max">The max</param>
        /// <returns>Generated number</returns>
        public static int RandomNumber(int min, int max)
        {
            return RandomUtils.RandomNumber(RandomUtils.randomObject, min, max);
        }

        /// <summary>
        /// Generates a random number.
        /// </summary>
        /// <param name="random">The random</param>
        /// <param name="min">The min</param>
        /// <param name="max">The max</param>
        /// <returns>Generated number</returns>
        public static int RandomNumber(Random random, int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>Generated string</returns>
        public static string RandomString(int size)
        {
            return RandomUtils.RandomString(RandomUtils.randomObject, size);
        }

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="random">The random</param>
        /// <param name="size">The size</param>
        /// <returns>Generated string</returns>
        public static string RandomString(Random random, int size)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                char currentChar = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65)));
                builder.Append(currentChar);
            }

            return builder.ToString();
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="Charset.cs" company="-">
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

namespace Tasslehoff.Library.Objects
{
    using System;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Charset class.
    /// </summary>
    public class Charset : ICloneable, IEnumerable
    {
        // fields

        /// <summary>
        /// The table
        /// </summary>
        private char[] table;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Charset"/> class.
        /// </summary>
        public Charset()
        {
            this.table = new char[0];
        }

        // methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Charset"/> class.
        /// </summary>
        /// <param name="tables">The tables</param>
        public Charset(params char[][] tables) : this()
        {
            foreach (char[] table in tables)
            {
                this.Insert(table);
            }
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <returns>The table</returns>
        public char[] GetTable()
        {
            return this.table;
        }

        /// <summary>
        /// Inserts the specified char range.
        /// </summary>
        /// <param name="charRange">The char range</param>
        public void Insert(CharRange charRange)
        {
            this.Insert(charRange.GetRangeTable());
        }

        /// <summary>
        /// Inserts the specified chars.
        /// </summary>
        /// <param name="chars">The chars</param>
        public void Insert(params char[] chars)
        {
            int length = this.table.Length;
            Array.Resize<char>(ref this.table, length + chars.Length);

            foreach (char currentChar in chars)
            {
                this.table[length++] = currentChar;
            }
        }

        /// <summary>
        /// Inserts the specified charset.
        /// </summary>
        /// <param name="charset">The charset</param>
        public void Insert(string charset)
        {
            int length = this.table.Length;
            Array.Resize<char>(ref this.table, length + charset.Length);

            foreach (char currentChar in charset)
            {
                this.table[length++] = currentChar;
            }
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

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            foreach (char currentChar in this.table)
            {
                yield return currentChar;
            }
        }
    }
}
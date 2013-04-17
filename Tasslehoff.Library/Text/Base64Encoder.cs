// -----------------------------------------------------------------------
// <copyright file="Base64Encoder.cs" company="-">
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

namespace Tasslehoff.Library.Text
{
    using System.Text;

    /// <summary>
    /// Base64Encoder class.
    /// </summary>
    public sealed class Base64Encoder : Encoder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Encoder"/> class.
        /// </summary>
        public Base64Encoder() : base()
        {
        }

        /// <summary>
        /// When overridden in a derived class, calculates the number of bytes produced by encoding a set of characters from the specified character array. A parameter indicates whether to clear the internal state of the encoder after the calculation.
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to encode</param>
        /// <param name="index">The index of the first character to encode</param>
        /// <param name="count">The number of characters to encode</param>
        /// <param name="flush">true to simulate clearing the internal state of the encoder after the calculation; otherwise, false</param>
        /// <returns>
        /// The number of bytes produced by encoding the specified characters and any characters in the internal buffer.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="chars"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than zero.-or- <paramref name="index"/> and <paramref name="count"/> do not denote a valid range in <paramref name="chars"/>. </exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoder.Fallback"/> is set to <see cref="T:System.Text.EncoderExceptionFallback"/>.</exception>
        public override int GetByteCount(char[] chars, int index, int count, bool flush)
        {
            byte[] input = Encoding.ASCII.GetBytes(chars, index, count);
            string output = System.Convert.ToBase64String(input);
            int length = output.Length;

            return length;
        }

        /// <summary>
        /// When overridden in a derived class, encodes a set of characters from the specified character array and any characters in the internal buffer into the specified byte array. A parameter indicates whether to clear the internal state of the encoder after the conversion.
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to encode</param>
        /// <param name="charIndex">The index of the first character to encode</param>
        /// <param name="charCount">The number of characters to encode</param>
        /// <param name="bytes">The byte array to contain the resulting sequence of bytes</param>
        /// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes</param>
        /// <param name="flush">true to clear the internal state of the encoder after the conversion; otherwise, false</param>
        /// <returns>
        /// The actual number of bytes written into <paramref name="bytes"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="chars"/> is null (Nothing).-or- <paramref name="bytes"/> is null (Nothing). </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="charIndex"/> or <paramref name="charCount"/> or <paramref name="byteIndex"/> is less than zero.-or- <paramref name="charIndex"/> and <paramref name="charCount"/> do not denote a valid range in <paramref name="chars"/>.-or- <paramref name="byteIndex"/> is not a valid index in <paramref name="bytes"/>. </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="bytes"/> does not have enough capacity from <paramref name="byteIndex"/> to the end of the array to accommodate the resulting bytes. </exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoder.Fallback"/> is set to <see cref="T:System.Text.EncoderExceptionFallback"/>.</exception>
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
        {
            byte[] input = Encoding.ASCII.GetBytes(chars, charIndex, charCount);
            string encoded = System.Convert.ToBase64String(input);

            int length = encoded.Length;

            Encoding.ASCII.GetBytes(encoded, 0, length, bytes, byteIndex);

            return length;
        }
    }
}
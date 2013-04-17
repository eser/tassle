// -----------------------------------------------------------------------
// <copyright file="GsmDecoder.cs" company="-">
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
    /// GsmDecoder class.
    /// </summary>
    public class GsmDecoder : Decoder
    {
        /// <summary>
        /// The GSM char table
        /// </summary>
        private readonly string gsmCharTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="GsmDecoder"/> class.
        /// </summary>
        public GsmDecoder() : base()
        {
            this.gsmCharTable = "@£$¥èéùìòÇ`Øø`ÅåΔ_ΦΓΛΩΠΨΣΘΞ`ÆæßÉ !\"#¤%&'()*=,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ`¿abcdefghijklmnopqrstuvwxyzäöñüà````````````````````^```````````````````{}`````\\````````````[~]`|````````````````````````````````````€``````````````````````````";
        }

        /// <summary>
        /// When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode</param>
        /// <param name="index">The index of the first byte to decode</param>
        /// <param name="count">The number of bytes to decode</param>
        /// <returns>
        /// The number of characters produced by decoding the specified sequence of bytes and any bytes in the internal buffer.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bytes"/> is null (Nothing). </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than zero.-or- <paramref name="index"/> and <paramref name="count"/> do not denote a valid range in <paramref name="bytes"/>. </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback"/> is set to <see cref="T:System.Text.DecoderExceptionFallback"/>.</exception>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            int escapeCount = 0;

            for (int i = index; i < (index + count); i++)
            {
                if (bytes[i] == 27)
                {
                    escapeCount++;
                }
            }

            return count - escapeCount;
        }

        /// <summary>
        /// When overridden in a derived class, decodes a sequence of bytes from the specified byte array and any bytes in the internal buffer into the specified character array.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode</param>
        /// <param name="byteIndex">The index of the first byte to decode</param>
        /// <param name="byteCount">The number of bytes to decode</param>
        /// <param name="chars">The character array to contain the resulting set of characters</param>
        /// <param name="charIndex">The index at which to start writing the resulting set of characters</param>
        /// <returns>
        /// The actual number of characters written into <paramref name="chars"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bytes"/> is null (Nothing).-or- <paramref name="chars"/> is null (Nothing). </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="byteIndex"/> or <paramref name="byteCount"/> or <paramref name="charIndex"/> is less than zero.-or- <paramref name="byteindex"/> and <paramref name="byteCount"/> do not denote a valid range in <paramref name="bytes"/>.-or- <paramref name="charIndex"/> is not a valid index in <paramref name="chars"/>. </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="chars"/> does not have enough capacity from <paramref name="charIndex"/> to the end of the array to accommodate the resulting characters. </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback"/> is set to <see cref="T:System.Text.DecoderExceptionFallback"/>.</exception>
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int charCount = 0;
            bool escapeChar = false;

            for (int i = byteIndex; i < (byteIndex + byteCount); i++)
            {
                byte currentByte = bytes[i];

                if (currentByte == 27)
                {
                    escapeChar = true;
                    continue;
                }

                if (escapeChar)
                {
                    currentByte += 128;
                    escapeChar = false;
                }

                chars[charIndex + charCount] = this.gsmCharTable[currentByte];
                charCount++;
            }

            return charCount - 1;
        }
    }
}
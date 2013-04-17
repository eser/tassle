// -----------------------------------------------------------------------
// <copyright file="Base64Encoding.cs" company="-">
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
    /// Base64Encoding class.
    /// </summary>
    public class Base64Encoding : Encoding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Encoding"/> class.
        /// </summary>
        public Base64Encoding() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Encoding" /> class.
        /// </summary>
        /// <param name="codePage">The code page identifier of the preferred encoding.-or- 0, to use the default encoding</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="codePage" /> is less than zero.</exception>
        protected Base64Encoding(int codePage)
            : base(codePage)
        {
        }

        /// <summary>
        /// Gets the base64 encoder.
        /// </summary>
        /// <returns>New instance of Base64Encoder</returns>
        public static Base64Encoder GetBase64Encoder()
        {
            return new Base64Encoder();
        }

        /// <summary>
        /// Gets the base64 decoder.
        /// </summary>
        /// <returns>New instance of Base64Decoder</returns>
        public static Base64Decoder GetBase64Decoder()
        {
            return new Base64Decoder();
        }

        /// <summary>
        /// When overridden in a derived class, calculates the number of bytes produced by encoding a set of characters from the specified character array.
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to encode</param>
        /// <param name="index">The index of the first character to encode</param>
        /// <param name="count">The number of characters to encode</param>
        /// <returns>
        /// The number of bytes produced by encoding the specified characters.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="chars"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than zero.-or- <paramref name="index"/> and <paramref name="count"/> do not denote a valid range in <paramref name="chars"/>. </exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback"/> is set to <see cref="T:System.Text.EncoderExceptionFallback"/>.</exception>
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return Base64Encoding.GetBase64Encoder().GetByteCount(chars, index, count, false);
        }

        /// <summary>
        /// When overridden in a derived class, encodes a set of characters from the specified character array into the specified byte array.
        /// </summary>
        /// <param name="chars">The character array containing the set of characters to encode</param>
        /// <param name="charIndex">The index of the first character to encode</param>
        /// <param name="charCount">The number of characters to encode</param>
        /// <param name="bytes">The byte array to contain the resulting sequence of bytes</param>
        /// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes</param>
        /// <returns>
        /// The actual number of bytes written into <paramref name="bytes"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="chars"/> is null.-or- <paramref name="bytes"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="charIndex"/> or <paramref name="charCount"/> or <paramref name="byteIndex"/> is less than zero.-or- <paramref name="charIndex"/> and <paramref name="charCount"/> do not denote a valid range in <paramref name="chars"/>.-or- <paramref name="byteIndex"/> is not a valid index in <paramref name="bytes"/>. </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="bytes"/> does not have enough capacity from <paramref name="byteIndex"/> to the end of the array to accommodate the resulting bytes. </exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback"/> is set to <see cref="T:System.Text.EncoderExceptionFallback"/>.</exception>
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return Base64Encoding.GetBase64Encoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, false);
        }

        /// <summary>
        /// When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode</param>
        /// <param name="index">The index of the first byte to decode</param>
        /// <param name="count">The number of bytes to decode</param>
        /// <returns>
        /// The number of characters produced by decoding the specified sequence of bytes.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bytes"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than zero.-or- <paramref name="index"/> and <paramref name="count"/> do not denote a valid range in <paramref name="bytes"/>. </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback"/> is set to <see cref="T:System.Text.DecoderExceptionFallback"/>.</exception>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return Base64Encoding.GetBase64Decoder().GetCharCount(bytes, index, count);
        }

        /// <summary>
        /// When overridden in a derived class, decodes a sequence of bytes from the specified byte array into the specified character array.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode</param>
        /// <param name="byteIndex">The index of the first byte to decode</param>
        /// <param name="byteCount">The number of bytes to decode</param>
        /// <param name="chars">The character array to contain the resulting set of characters</param>
        /// <param name="charIndex">The index at which to start writing the resulting set of characters</param>
        /// <returns>
        /// The actual number of characters written into <paramref name="chars"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="bytes"/> is null.-or- <paramref name="chars"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="byteIndex"/> or <paramref name="byteCount"/> or <paramref name="charIndex"/> is less than zero.-or- <paramref name="byteindex"/> and <paramref name="byteCount"/> do not denote a valid range in <paramref name="bytes"/>.-or- <paramref name="charIndex"/> is not a valid index in <paramref name="chars"/>. </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="chars"/> does not have enough capacity from <paramref name="charIndex"/> to the end of the array to accommodate the resulting characters. </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback"/> is set to <see cref="T:System.Text.DecoderExceptionFallback"/>.</exception>
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return Base64Encoding.GetBase64Decoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        /// <summary>
        /// When overridden in a derived class, calculates the maximum number of bytes produced by encoding the specified number of characters.
        /// </summary>
        /// <param name="charCount">The number of characters to encode</param>
        /// <returns>
        /// The maximum number of bytes produced by encoding the specified number of characters.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="charCount"/> is less than zero. </exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback"/> is set to <see cref="T:System.Text.EncoderExceptionFallback"/>.</exception>
        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        /// <summary>
        /// When overridden in a derived class, calculates the maximum number of characters produced by decoding the specified number of bytes.
        /// </summary>
        /// <param name="byteCount">The number of bytes to decode</param>
        /// <returns>
        /// The maximum number of characters produced by decoding the specified number of bytes.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="byteCount"/> is less than zero. </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback"/> is set to <see cref="T:System.Text.DecoderExceptionFallback"/>.</exception>
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
    }
}
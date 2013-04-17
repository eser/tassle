// -----------------------------------------------------------------------
// <copyright file="IOUtils.cs" company="-">
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
    using System.IO;

    /// <summary>
    /// IOUtils class.
    /// </summary>
    public static class IOUtils
    {
        // methods

        /// <summary>
        /// Sanitizes the filename.
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <param name="replaceBackslashes">if set to <c>true</c> [replace backslashes]</param>
        /// <param name="charsToRemove">The chars to remove</param>
        /// <returns>Sanitized filename</returns>
        public static string SanitizeFilename(string filename, bool replaceBackslashes, params char[] charsToRemove)
        {
            char[] newFilename = filename.ToCharArray();
            char[] invalidChars = Path.GetInvalidFileNameChars();

            for (int i = 0; i < newFilename.Length; i++)
            {
                if (Array.IndexOf(invalidChars, newFilename[i]) != -1)
                {
                    newFilename[i] = '_';
                    continue;
                }

                if (Array.IndexOf(charsToRemove, newFilename[i]) != -1)
                {
                    newFilename[i] = '_';
                    continue;
                }

                if (replaceBackslashes && newFilename[i] == '\\')
                {
                    newFilename[i] = '/';
                    continue;
                }
            }

            return new string(newFilename);
        }
    }
}

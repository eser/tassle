// --------------------------------------------------------------------------
// <copyright file="IOHelpers.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.IO;

namespace Tassle.Helpers {
    /// <summary>
    /// IOUtils class.
    /// </summary>
    public static class IOHelpers {
        // methods

        /// <summary>
        /// Sanitizes the filename.
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <param name="replaceBackslashes">if set to <c>true</c> [replace backslashes]</param>
        /// <param name="charsToRemove">The chars to remove</param>
        /// <returns>Sanitized filename</returns>
        public static string SanitizeFilename(string filename, bool replaceBackslashes, params char[] charsToRemove) {
            char[] newFilename = filename.ToCharArray();
            char[] invalidChars = Path.GetInvalidFileNameChars();

            for (var i = 0; i < newFilename.Length; i++) {
                if (Array.IndexOf(invalidChars, newFilename[i]) != -1) {
                    newFilename[i] = '_';
                    continue;
                }

                if (Array.IndexOf(charsToRemove, newFilename[i]) != -1) {
                    newFilename[i] = '_';
                    continue;
                }

                if (replaceBackslashes && newFilename[i] == '\\') {
                    newFilename[i] = '/';
                    continue;
                }
            }

            return new string(newFilename);
        }
    }
}

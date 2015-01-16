// --------------------------------------------------------------------------
// <copyright file="ProcessNames.cs" company="-">
// Copyright (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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

using System.Diagnostics;

namespace Tasslehoff.Profiler
{
    /// <summary>
    /// ProcessNames class.
    /// </summary>
    public static class ProcessNames
    {
        // fields

        /// <summary>
        /// Current Process
        /// </summary>
        private static string me;

        // constructors

        /// <summary>
        /// Initializes static members of the <see cref="ProcessNames"/> class.
        /// </summary>
        static ProcessNames()
        {
            Process process = Process.GetCurrentProcess();
            ProcessNames.me = process.ProcessName;
        }

        // properties

        /// <summary>
        /// Gets the nothing.
        /// </summary>
        /// <value>
        /// The nothing.
        /// </value>
        public static string Nothing
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets me.
        /// </summary>
        /// <value>
        /// The current process.
        /// </value>
        public static string Me
        {
            get
            {
                return ProcessNames.me;
            }
        }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public static string Total
        {
            get
            {
                return "_Total";
            }
        }
    }
}

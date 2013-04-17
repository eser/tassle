// -----------------------------------------------------------------------
// <copyright file="ProcessName.cs" company="-">
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

namespace Tasslehoff.Library.Profiler
{
    using System.Diagnostics;

    /// <summary>
    /// ProcessName class.
    /// </summary>
    public class ProcessName
    {
        // fields

        /// <summary>
        /// The nothing
        /// </summary>
        private static ProcessName nothing;

        /// <summary>
        /// Current Process
        /// </summary>
        private static ProcessName me;

        /// <summary>
        /// The total
        /// </summary>
        private static ProcessName total;

        /// <summary>
        /// The name
        /// </summary>
        private readonly string name;

        // constructors

        /// <summary>
        /// Initializes static members of the <see cref="ProcessName"/> class.
        /// </summary>
        static ProcessName()
        {
            Process process = Process.GetCurrentProcess();

            ProcessName.nothing = new ProcessName(null);
            ProcessName.me = new ProcessName(process.ProcessName);
            ProcessName.total = new ProcessName("_Total");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessName" /> class.
        /// </summary>
        /// <param name="name">The name</param>
        public ProcessName(string name)
        {
            this.name = name;
        }

        // properties

        /// <summary>
        /// Gets the nothing.
        /// </summary>
        /// <value>
        /// The nothing.
        /// </value>
        public static ProcessName Nothing
        {
            get
            {
                return ProcessName.nothing;
            }
        }

        /// <summary>
        /// Gets me.
        /// </summary>
        /// <value>
        /// The current process.
        /// </value>
        public static ProcessName Me
        {
            get
            {
                return ProcessName.me;
            }
        }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public static ProcessName Total
        {
            get
            {
                return ProcessName.total;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}

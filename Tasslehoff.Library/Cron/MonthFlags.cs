// -----------------------------------------------------------------------
// <copyright file="MonthFlags.cs" company="-">
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

namespace Tasslehoff.Library.Cron
{
    using System;

    /// <summary>
    /// MonthFlags enumeration.
    /// </summary>
    [Flags]
    public enum MonthFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        None = 0,

        /// <summary>
        /// Month January.
        /// </summary>
        January = 1,

        /// <summary>
        /// Month February.
        /// </summary>
        February = 2,

        /// <summary>
        /// Month March.
        /// </summary>
        March = 4,

        /// <summary>
        /// Month April.
        /// </summary>
        April = 8,

        /// <summary>
        /// Month May.
        /// </summary>
        May = 16,

        /// <summary>
        /// Month June.
        /// </summary>
        June = 32,

        /// <summary>
        /// Month July.
        /// </summary>
        July = 64,

        /// <summary>
        /// Month August.
        /// </summary>
        August = 128,

        /// <summary>
        /// Month September.
        /// </summary>
        September = 256,

        /// <summary>
        /// Month October.
        /// </summary>
        October = 512,

        /// <summary>
        /// Month November.
        /// </summary>
        November = 1024,

        /// <summary>
        /// Month December.
        /// </summary>
        December = 2048
    }
}

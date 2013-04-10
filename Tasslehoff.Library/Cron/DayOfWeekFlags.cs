// -----------------------------------------------------------------------
// <copyright file="DayOfWeekFlags.cs" company="-">
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
    /// DayOfWeekFlags enumeration.
    /// </summary>
    [Flags]
    public enum DayOfWeekFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        None = 0,

        /// <summary>
        /// Day Sunday.
        /// </summary>
        Sunday = 1,

        /// <summary>
        /// Day Monday.
        /// </summary>
        Monday = 2,

        /// <summary>
        /// Day Tuesday.
        /// </summary>
        Tuesday = 4,

        /// <summary>
        /// Day Wednesday.
        /// </summary>
        Wednesday = 8,

        /// <summary>
        /// Day Thursday.
        /// </summary>
        Thursday = 16,

        /// <summary>
        /// Day Friday.
        /// </summary>
        Friday = 32,

        /// <summary>
        /// Day Saturday.
        /// </summary>
        Saturday = 64
    }
}

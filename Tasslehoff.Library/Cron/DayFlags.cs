// -----------------------------------------------------------------------
// <copyright file="DayFlags.cs" company="-">
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
    /// DayFlags enumeration.
    /// </summary>
    [Flags]
    public enum DayFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        None = 0,

        /// <summary>
        /// Day 01.
        /// </summary>
        D01 = 1,

        /// <summary>
        /// Day 02.
        /// </summary>
        D02 = 2,

        /// <summary>
        /// Day 03.
        /// </summary>
        D03 = 4,

        /// <summary>
        /// Day 04.
        /// </summary>
        D04 = 8,

        /// <summary>
        /// Day 05.
        /// </summary>
        D05 = 16,

        /// <summary>
        /// Day 06.
        /// </summary>
        D06 = 32,

        /// <summary>
        /// Day 07.
        /// </summary>
        D07 = 64,

        /// <summary>
        /// Day 08.
        /// </summary>
        D08 = 128,

        /// <summary>
        /// Day 09.
        /// </summary>
        D09 = 256,

        /// <summary>
        /// Day 10.
        /// </summary>
        D10 = 512,

        /// <summary>
        /// Day 11.
        /// </summary>
        D11 = 1024,

        /// <summary>
        /// Day 12.
        /// </summary>
        D12 = 2048,

        /// <summary>
        /// Day 13.
        /// </summary>
        D13 = 4096,

        /// <summary>
        /// Day 14.
        /// </summary>
        D14 = 8192,

        /// <summary>
        /// Day 15.
        /// </summary>
        D15 = 16384,

        /// <summary>
        /// Day 16.
        /// </summary>
        D16 = 32768,

        /// <summary>
        /// Day 17.
        /// </summary>
        D17 = 65536,

        /// <summary>
        /// Day 18.
        /// </summary>
        D18 = 131072,

        /// <summary>
        /// Day 19.
        /// </summary>
        D19 = 262144,

        /// <summary>
        /// Day 20.
        /// </summary>
        D20 = 524288,

        /// <summary>
        /// Day 21.
        /// </summary>
        D21 = 1048576,

        /// <summary>
        /// Day 22.
        /// </summary>
        D22 = 2097152,

        /// <summary>
        /// Day 23.
        /// </summary>
        D23 = 4194304,

        /// <summary>
        /// Day 24.
        /// </summary>
        D24 = 8388608,

        /// <summary>
        /// Day 25.
        /// </summary>
        D25 = 16777216,

        /// <summary>
        /// Day 26.
        /// </summary>
        D26 = 33554432,

        /// <summary>
        /// Day 27.
        /// </summary>
        D27 = 67108864,

        /// <summary>
        /// Day 28.
        /// </summary>
        D28 = 134217728,

        /// <summary>
        /// Day 29.
        /// </summary>
        D29 = 268435456,

        /// <summary>
        /// Day 30.
        /// </summary>
        D30 = 536870912,

        /// <summary>
        /// Day 31.
        /// </summary>
        D31 = 1073741824
    }
}

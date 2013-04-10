// -----------------------------------------------------------------------
// <copyright file="HourFlags.cs" company="-">
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
    /// HourFlags enumeration.
    /// </summary>
    [Flags]
    public enum HourFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        None = 0,

        /// <summary>
        /// Hour 00.
        /// </summary>
        H00 = 1,

        /// <summary>
        /// Hour 01.
        /// </summary>
        H01 = 2,

        /// <summary>
        /// Hour 02.
        /// </summary>
        H02 = 4,

        /// <summary>
        /// Hour 03.
        /// </summary>
        H03 = 8,

        /// <summary>
        /// Hour 04.
        /// </summary>
        H04 = 16,

        /// <summary>
        /// Hour 05.
        /// </summary>
        H05 = 32,

        /// <summary>
        /// Hour 06.
        /// </summary>
        H06 = 64,

        /// <summary>
        /// Hour 07.
        /// </summary>
        H07 = 128,

        /// <summary>
        /// Hour 08.
        /// </summary>
        H08 = 256,

        /// <summary>
        /// Hour 09.
        /// </summary>
        H09 = 512,

        /// <summary>
        /// Hour 10.
        /// </summary>
        H10 = 1024,

        /// <summary>
        /// Hour 11.
        /// </summary>
        H11 = 2048,

        /// <summary>
        /// Hour 12.
        /// </summary>
        H12 = 4096,

        /// <summary>
        /// Hour 13.
        /// </summary>
        H13 = 8192,

        /// <summary>
        /// Hour 14.
        /// </summary>
        H14 = 16384,

        /// <summary>
        /// Hour 15.
        /// </summary>
        H15 = 32768,

        /// <summary>
        /// Hour 16.
        /// </summary>
        H16 = 65536,

        /// <summary>
        /// Hour 17.
        /// </summary>
        H17 = 131072,

        /// <summary>
        /// Hour 18.
        /// </summary>
        H18 = 262144,

        /// <summary>
        /// Hour 19.
        /// </summary>
        H19 = 524288,

        /// <summary>
        /// Hour 20.
        /// </summary>
        H20 = 1048576,

        /// <summary>
        /// Hour 21.
        /// </summary>
        H21 = 2097152,

        /// <summary>
        /// Hour 22.
        /// </summary>
        H22 = 4194304,

        /// <summary>
        /// Hour 23.
        /// </summary>
        H23 = 8388608
    }
}

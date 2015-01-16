// --------------------------------------------------------------------------
// <copyright file="DayFlags.cs" company="-">
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

using System;
using System.Runtime.Serialization;

namespace Tasslehoff.Tasks
{
    /// <summary>
    /// DayFlags enumeration.
    /// </summary>
    [Serializable]
    [DataContract]
    [Flags]
    public enum DayFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Day 01.
        /// </summary>
        [EnumMember]
        D01 = 1,

        /// <summary>
        /// Day 02.
        /// </summary>
        [EnumMember]
        D02 = 2,

        /// <summary>
        /// Day 03.
        /// </summary>
        [EnumMember]
        D03 = 4,

        /// <summary>
        /// Day 04.
        /// </summary>
        [EnumMember]
        D04 = 8,

        /// <summary>
        /// Day 05.
        /// </summary>
        [EnumMember]
        D05 = 16,

        /// <summary>
        /// Day 06.
        /// </summary>
        [EnumMember]
        D06 = 32,

        /// <summary>
        /// Day 07.
        /// </summary>
        [EnumMember]
        D07 = 64,

        /// <summary>
        /// Day 08.
        /// </summary>
        [EnumMember]
        D08 = 128,

        /// <summary>
        /// Day 09.
        /// </summary>
        [EnumMember]
        D09 = 256,

        /// <summary>
        /// Day 10.
        /// </summary>
        [EnumMember]
        D10 = 512,

        /// <summary>
        /// Day 11.
        /// </summary>
        [EnumMember]
        D11 = 1024,

        /// <summary>
        /// Day 12.
        /// </summary>
        [EnumMember]
        D12 = 2048,

        /// <summary>
        /// Day 13.
        /// </summary>
        [EnumMember]
        D13 = 4096,

        /// <summary>
        /// Day 14.
        /// </summary>
        [EnumMember]
        D14 = 8192,

        /// <summary>
        /// Day 15.
        /// </summary>
        [EnumMember]
        D15 = 16384,

        /// <summary>
        /// Day 16.
        /// </summary>
        [EnumMember]
        D16 = 32768,

        /// <summary>
        /// Day 17.
        /// </summary>
        [EnumMember]
        D17 = 65536,

        /// <summary>
        /// Day 18.
        /// </summary>
        [EnumMember]
        D18 = 131072,

        /// <summary>
        /// Day 19.
        /// </summary>
        [EnumMember]
        D19 = 262144,

        /// <summary>
        /// Day 20.
        /// </summary>
        [EnumMember]
        D20 = 524288,

        /// <summary>
        /// Day 21.
        /// </summary>
        [EnumMember]
        D21 = 1048576,

        /// <summary>
        /// Day 22.
        /// </summary>
        [EnumMember]
        D22 = 2097152,

        /// <summary>
        /// Day 23.
        /// </summary>
        [EnumMember]
        D23 = 4194304,

        /// <summary>
        /// Day 24.
        /// </summary>
        [EnumMember]
        D24 = 8388608,

        /// <summary>
        /// Day 25.
        /// </summary>
        [EnumMember]
        D25 = 16777216,

        /// <summary>
        /// Day 26.
        /// </summary>
        [EnumMember]
        D26 = 33554432,

        /// <summary>
        /// Day 27.
        /// </summary>
        [EnumMember]
        D27 = 67108864,

        /// <summary>
        /// Day 28.
        /// </summary>
        [EnumMember]
        D28 = 134217728,

        /// <summary>
        /// Day 29.
        /// </summary>
        [EnumMember]
        D29 = 268435456,

        /// <summary>
        /// Day 30.
        /// </summary>
        [EnumMember]
        D30 = 536870912,

        /// <summary>
        /// Day 31.
        /// </summary>
        [EnumMember]
        D31 = 1073741824
    }
}

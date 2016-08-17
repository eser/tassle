// --------------------------------------------------------------------------
// <copyright file="HourFlags.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
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
    /// HourFlags enumeration.
    /// </summary>
    [Serializable]
    [DataContract]
    [Flags]
    public enum HourFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Hour 00.
        /// </summary>
        [EnumMember]
        H00 = 1,

        /// <summary>
        /// Hour 01.
        /// </summary>
        [EnumMember]
        H01 = 2,

        /// <summary>
        /// Hour 02.
        /// </summary>
        [EnumMember]
        H02 = 4,

        /// <summary>
        /// Hour 03.
        /// </summary>
        [EnumMember]
        H03 = 8,

        /// <summary>
        /// Hour 04.
        /// </summary>
        [EnumMember]
        H04 = 16,

        /// <summary>
        /// Hour 05.
        /// </summary>
        [EnumMember]
        H05 = 32,

        /// <summary>
        /// Hour 06.
        /// </summary>
        [EnumMember]
        H06 = 64,

        /// <summary>
        /// Hour 07.
        /// </summary>
        [EnumMember]
        H07 = 128,

        /// <summary>
        /// Hour 08.
        /// </summary>
        [EnumMember]
        H08 = 256,

        /// <summary>
        /// Hour 09.
        /// </summary>
        [EnumMember]
        H09 = 512,

        /// <summary>
        /// Hour 10.
        /// </summary>
        [EnumMember]
        H10 = 1024,

        /// <summary>
        /// Hour 11.
        /// </summary>
        [EnumMember]
        H11 = 2048,

        /// <summary>
        /// Hour 12.
        /// </summary>
        [EnumMember]
        H12 = 4096,

        /// <summary>
        /// Hour 13.
        /// </summary>
        [EnumMember]
        H13 = 8192,

        /// <summary>
        /// Hour 14.
        /// </summary>
        [EnumMember]
        H14 = 16384,

        /// <summary>
        /// Hour 15.
        /// </summary>
        [EnumMember]
        H15 = 32768,

        /// <summary>
        /// Hour 16.
        /// </summary>
        [EnumMember]
        H16 = 65536,

        /// <summary>
        /// Hour 17.
        /// </summary>
        [EnumMember]
        H17 = 131072,

        /// <summary>
        /// Hour 18.
        /// </summary>
        [EnumMember]
        H18 = 262144,

        /// <summary>
        /// Hour 19.
        /// </summary>
        [EnumMember]
        H19 = 524288,

        /// <summary>
        /// Hour 20.
        /// </summary>
        [EnumMember]
        H20 = 1048576,

        /// <summary>
        /// Hour 21.
        /// </summary>
        [EnumMember]
        H21 = 2097152,

        /// <summary>
        /// Hour 22.
        /// </summary>
        [EnumMember]
        H22 = 4194304,

        /// <summary>
        /// Hour 23.
        /// </summary>
        [EnumMember]
        H23 = 8388608
    }
}

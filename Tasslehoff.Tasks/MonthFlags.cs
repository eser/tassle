// --------------------------------------------------------------------------
// <copyright file="MonthFlags.cs" company="-">
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
    /// MonthFlags enumeration.
    /// </summary>
    [Serializable]
    [DataContract]
    [Flags]
    public enum MonthFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Month January.
        /// </summary>
        [EnumMember]
        January = 1,

        /// <summary>
        /// Month February.
        /// </summary>
        [EnumMember]
        February = 2,

        /// <summary>
        /// Month March.
        /// </summary>
        [EnumMember]
        March = 4,

        /// <summary>
        /// Month April.
        /// </summary>
        [EnumMember]
        April = 8,

        /// <summary>
        /// Month May.
        /// </summary>
        [EnumMember]
        May = 16,

        /// <summary>
        /// Month June.
        /// </summary>
        [EnumMember]
        June = 32,

        /// <summary>
        /// Month July.
        /// </summary>
        [EnumMember]
        July = 64,

        /// <summary>
        /// Month August.
        /// </summary>
        [EnumMember]
        August = 128,

        /// <summary>
        /// Month September.
        /// </summary>
        [EnumMember]
        September = 256,

        /// <summary>
        /// Month October.
        /// </summary>
        [EnumMember]
        October = 512,

        /// <summary>
        /// Month November.
        /// </summary>
        [EnumMember]
        November = 1024,

        /// <summary>
        /// Month December.
        /// </summary>
        [EnumMember]
        December = 2048
    }
}

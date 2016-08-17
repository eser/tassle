// --------------------------------------------------------------------------
// <copyright file="DayOfWeekFlags.cs" company="-">
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
    /// DayOfWeekFlags enumeration.
    /// </summary>
    [Serializable]
    [DataContract]
    [Flags]
    public enum DayOfWeekFlags
    {
        /// <summary>
        /// None of them.
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Day Sunday.
        /// </summary>
        [EnumMember]
        Sunday = 1,

        /// <summary>
        /// Day Monday.
        /// </summary>
        [EnumMember]
        Monday = 2,

        /// <summary>
        /// Day Tuesday.
        /// </summary>
        [EnumMember]
        Tuesday = 4,

        /// <summary>
        /// Day Wednesday.
        /// </summary>
        [EnumMember]
        Wednesday = 8,

        /// <summary>
        /// Day Thursday.
        /// </summary>
        [EnumMember]
        Thursday = 16,

        /// <summary>
        /// Day Friday.
        /// </summary>
        [EnumMember]
        Friday = 32,

        /// <summary>
        /// Day Saturday.
        /// </summary>
        [EnumMember]
        Saturday = 64
    }
}

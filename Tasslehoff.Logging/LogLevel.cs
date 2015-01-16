// --------------------------------------------------------------------------
// <copyright file="LogLevel.cs" company="-">
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

namespace Tasslehoff.Logging
{
    /// <summary>
    /// Level for logging.
    /// </summary>
    [Serializable]
    [DataContract]
    public enum LogLevel
    {
        /// <summary>
        /// The emergency
        /// </summary>
        [EnumMember]
        Emergency,

        /// <summary>
        /// The alert
        /// </summary>
        [EnumMember]
        Alert,

        /// <summary>
        /// The critical
        /// </summary>
        [EnumMember]
        Critical,

        /// <summary>
        /// The error
        /// </summary>
        [EnumMember]
        Error,

        /// <summary>
        /// The warning
        /// </summary>
        [EnumMember]
        Warning,

        /// <summary>
        /// The notice
        /// </summary>
        [EnumMember]
        Notice,

        /// <summary>
        /// The info
        /// </summary>
        [EnumMember]
        Info,

        /// <summary>
        /// The debug
        /// </summary>
        [EnumMember]
        Debug
    }
}

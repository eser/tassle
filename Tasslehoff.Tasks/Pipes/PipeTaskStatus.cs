// --------------------------------------------------------------------------
// <copyright file="PipeTaskStatus.cs" company="-">
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

namespace Tasslehoff.Tasks.Pipes
{
    /// <summary>
    /// PipeTaskStatus enumeration.
    /// </summary>
    /// <remarks>Converted from byte to int due to CLS compliancy.</remarks>
    [Serializable]
    [DataContract]
    public enum PipeTaskStatus
    {
        /// <summary>
        /// Task is NotStarted
        /// </summary>
        [EnumMember]
        NotStarted = 0,

        /// <summary>
        /// Task is Running
        /// </summary>
        [EnumMember]
        Running = 1,

        /// <summary>
        /// Task is Finished
        /// </summary>
        [EnumMember]
        Finished = 2,

        /// <summary>
        /// Task is Cancelled
        /// </summary>
        [EnumMember]
        Cancelled = 3
    }
}
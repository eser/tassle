// --------------------------------------------------------------------------
// <copyright file="PipelineTaskStatus.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
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

namespace Tassle.Tasks {
    /// <summary>
    /// PipelineTaskStatus enumeration.
    /// </summary>
    /// <remarks>Converted from byte to int due to CLS compliancy.</remarks>
    public enum PipelineTaskStatus {
        /// <summary>
        /// Task is NotStarted
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// Task is Running
        /// </summary>
        Running = 1,

        /// <summary>
        /// Task is Finished
        /// </summary>
        Finished = 2,

        /// <summary>
        /// Task is Cancelled
        /// </summary>
        Cancelled = 3
    }
}

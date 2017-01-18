// --------------------------------------------------------------------------
// <copyright file="PipeTasksDoneEventArgs.cs" company="-">
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

namespace Tasslehoff.Tasks.Pipes
{
    /// <summary>
    /// PipeTasksDoneEventArgs class.
    /// </summary>
    public class PipeTasksDoneEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The is cancelled
        /// </summary>
        private readonly bool isCancelled;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeTasksDoneEventArgs"/> class.
        /// </summary>
        /// <param name="isCancelled">if set to <c>true</c> [is cancelled].</param>
        public PipeTasksDoneEventArgs(bool isCancelled) : base()
        {
            this.isCancelled = isCancelled;
        }

        // properties

        /// <summary>
        /// Gets a value indicating whether this instance is cancelled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is cancelled; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelled
        {
            get
            {
                return this.isCancelled;
            }
        }
    }
}
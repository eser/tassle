// -----------------------------------------------------------------------
// <copyright file="ChannelTasksDoneEventArgs.cs" company="-">
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

namespace Laroux.ScabbiaLibrary.Threading
{
    using System;

    /// <summary>
    /// ChannelTasksDoneEventArgs class.
    /// </summary>
    public class ChannelTasksDoneEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The is cancelled
        /// </summary>
        private readonly bool isCancelled;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelTasksDoneEventArgs"/> class.
        /// </summary>
        /// <param name="isCancelled">if set to <c>true</c> [is cancelled].</param>
        public ChannelTasksDoneEventArgs(bool isCancelled) : base()
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
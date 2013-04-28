// -----------------------------------------------------------------------
// <copyright file="ChannelTaskStatusChangedEventArgs.cs" company="-">
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
    /// ChannelTaskStatusChangedEventArgs class.
    /// </summary>
    public class ChannelTaskStatusChangedEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The status
        /// </summary>
        private readonly ChannelTaskStatus status;

        /// <summary>
        /// The parameter
        /// </summary>
        private readonly object parameter;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelTaskStatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="parameter">The parameter.</param>
        public ChannelTaskStatusChangedEventArgs(ChannelTaskStatus status, object parameter) : base()
        {
            this.status = status;
            this.parameter = parameter;
        }

        // properties

        /// <summary>
        /// Gets the status.
        /// </summary>
        public ChannelTaskStatus Status
        {
            get
            {
                return this.status;
            }
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        public object Parameter
        {
            get
            {
                return this.parameter;
            }
        }
    }
}
// --------------------------------------------------------------------------
// <copyright file="PipeTaskStatusChangedEventArgs.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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

namespace Tasslehoff.Tasks.Pipes
{
    /// <summary>
    /// PipeTaskStatusChangedEventArgs class.
    /// </summary>
    public class PipeTaskStatusChangedEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The status
        /// </summary>
        private readonly PipeTaskStatus status;

        /// <summary>
        /// The parameter
        /// </summary>
        private readonly object parameter;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeTaskStatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="parameter">The parameter.</param>
        public PipeTaskStatusChangedEventArgs(PipeTaskStatus status, object parameter) : base()
        {
            this.status = status;
            this.parameter = parameter;
        }

        // properties

        /// <summary>
        /// Gets the status.
        /// </summary>
        public PipeTaskStatus Status
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
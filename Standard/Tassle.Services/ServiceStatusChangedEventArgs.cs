// --------------------------------------------------------------------------
// <copyright file="ServiceStatusChangedEventArgs.cs" company="-">
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

namespace Tassle.Services {
    /// <summary>
    /// ServiceStatusChangedEventArgs class.
    /// </summary>
    public class ServiceStatusChangedEventArgs : EventArgs {
        // fields

        /// <summary>
        /// The previous state
        /// </summary>
        private readonly ServiceStatus _previousState;

        /// <summary>
        /// The status
        /// </summary>
        private readonly ServiceStatus _status;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="previousState">State of the previous</param>
        /// <param name="status">The status</param>
        public ServiceStatusChangedEventArgs(ServiceStatus previousState, ServiceStatus status) {
            this._previousState = previousState;
            this._status = status;
        }

        // properties

        /// <summary>
        /// Gets the state of the previous.
        /// </summary>
        /// <value>
        /// The state of the previous.
        /// </value>
        public ServiceStatus PreviousState {
            get => this._previousState;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ServiceStatus Status {
            get => this._status;
        }
    }
}

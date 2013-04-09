//
//  ServiceStatusChangedEventArgs.cs
//
//  Author:
//       larukedi <eser@sent.com>
//
//  Copyright (c) 2013 larukedi
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace Tasslehoff.Library.Services
{
    public class ServiceStatusChangedEventArgs : EventArgs
    {
        // fields
        private readonly ServiceStatus previousState;
        private readonly ServiceStatus status;

        // constructors
        public ServiceStatusChangedEventArgs(ServiceStatus previousState, ServiceStatus status)
        {
            this.previousState = previousState;
            this.status = status;
        }

        // properties
        public ServiceStatus PreviousState {
            get {
                return this.previousState;
            }
        }

        public ServiceStatus Status {
            get {
                return this.status;
            }
        }
    }
}


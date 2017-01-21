// --------------------------------------------------------------------------
// <copyright file="ServiceContainerInterface.cs" company="-">
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
using System.Collections.Generic;

namespace Tassle.Services {
    /// <summary>
    /// ServiceContainerInterface interface.
    /// </summary>
    public interface ServiceContainerInterface : ControllableServiceInterface {
        // events

        /// <summary>
        /// Occurs when [started with children].
        /// </summary>
        event EventHandler<ServiceStatusChangedEventArgs> StartedWithChildren;

        // properties

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        ICollection<ServiceInterface> Children { get; }

        // methods

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="services">The services</param>
        void AddChild(params ServiceInterface[] services);

        /// <summary>
        /// Finds an service with path notation.
        /// </summary>
        /// <param name="path">Path of the service</param>
        /// <returns>A service instance</returns>
        ServiceInterface FindByPath(string path);

        /// <summary>
        /// Finds an service with path notation.
        /// </summary>
        /// <typeparam name="T">A type</typeparam>
        /// <param name="path">Path of the service</param>
        /// <returns>A service instance</returns>
        T FindByPath<T>(string path) where T : class;

        /// <summary>
        /// Finds the specified dependency (recursively if needed).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns>Service if found</returns>
        T Find<T>(bool recursive) where T : class, ServiceInterface;
    }
}

// --------------------------------------------------------------------------
// <copyright file="ServiceContainer.cs" company="-">
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
using System.Globalization;
using Microsoft.Extensions.Logging;
using Tassle.Helpers;

namespace Tassle.Services {
    /// <summary>
    /// ServiceContainer class.
    /// </summary>
    public abstract class ServiceContainer : ControllableService, IServiceContainer {
        // fields

        /// <summary>
        /// The children
        /// </summary>
        private readonly ICollection<IService> _children;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer"/> class.
        /// </summary>
        protected ServiceContainer(ILoggerFactory loggerFactory) : base(loggerFactory) {
            this._children = new List<IService>();
        }

        // events

        /// <summary>
        /// Occurs when [on start with children].
        /// </summary>
        public event EventHandler<ServiceStatusChangedEventArgs> StartedWithChildren;

        // properties

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public ICollection<IService> Children {
            get => this._children;
        }

        // methods

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="services">The services</param>
        public void AddChild(params IService[] services) {
            foreach (var service in services) {
                this._children.Add(service);
            }
        }

        /// <summary>
        /// Finds an service with path notation.
        /// </summary>
        /// <param name="path">Path of the service</param>
        /// <returns>A service instance</returns>
        public IService FindByPath(string path) {
            var parts = new Queue<string>(
                path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)
            );

            IService current = this;
            while (parts.Count > 0) {
                string part = parts.Dequeue();

                var currentAsContainer = current as ServiceContainer;
                var found = false;

                if (currentAsContainer != null) {
                    foreach (IService service in currentAsContainer.Children) {
                        if (service.Name == path) {
                            current = service;
                            found = true;
                        }
                    }
                }

                if (!found) {
                    return null;
                }
            }

            return current;
        }

        /// <summary>
        /// Finds an service with path notation.
        /// </summary>
        /// <typeparam name="T">A type</typeparam>
        /// <param name="path">Path of the service</param>
        /// <returns>A service instance</returns>
        public T FindByPath<T>(string path) where T : class {
            return this.FindByPath(path) as T;
        }

        /// <summary>
        /// Finds the specified dependency (recursively if needed).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns>Service if found</returns>
        public T Find<T>(bool recursive) where T : class, IService {
            T result;

            foreach (var service in this.Children) {
                result = service as T;

                if (result != null) {
                    return result;
                }

                if (recursive) {
                    var serviceAsContainer = service as ServiceContainer;

                    return serviceAsContainer?.Find<T>(true);
                }
            }

            return null;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start() {
            try {
                if (this.Status == ServiceStatus.Running) {
                    return;
                }

                ServiceStatus previousState = this.Status;

                base.Start();

                foreach (var child in this._children) {
                    var controllableChild = child as ControllableService;

                    controllableChild?.Start();
                }

                this.StartedWithChildren?.Invoke(this, new ServiceStatusChangedEventArgs(previousState, ServiceStatus.Running));
            }
            catch (Exception ex) {
                this.Logger.LogError(string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStartingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop() {
            try {
                if (this.Status != ServiceStatus.Running) {
                    return;
                }

                // stop children
                var childServices = ArrayHelpers.GetArray<IService>(this._children);
                Array.Reverse(childServices);

                foreach (var child in childServices) {
                    var controllableChild = child as ControllableService;

                    controllableChild?.Stop();
                }

                base.Stop();
            }
            catch (Exception ex) {
                this.Logger.LogError(string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStoppingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected override void OnDispose(bool releaseManagedResources) {
            var childServices = ArrayHelpers.GetArray<IService>(this._children);
            Array.Reverse(childServices);

            foreach (var child in childServices) {
                var disposableChild = child as IDisposable;

                disposableChild?.Dispose();
            }

            base.OnDispose(releaseManagedResources);
        }
    }
}

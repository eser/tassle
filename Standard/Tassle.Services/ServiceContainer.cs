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

namespace Tassle.Services
{
    /// <summary>
    /// ServiceContainer class.
    /// </summary>
    public abstract class ServiceContainer : ServiceControllable
    {
        // fields

        /// <summary>
        /// The children
        /// </summary>
        private readonly ICollection<ServiceInterface> children;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer"/> class.
        /// </summary>
        protected ServiceContainer(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.children = new List<ServiceInterface>();
        }

        // events

        /// <summary>
        /// Occurs when [on start with children].
        /// </summary>
        public event EventHandler OnStartWithChildren;

        // properties

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public ICollection<ServiceInterface> Children
        {
            get
            {
                return this.children;
            }
        }

        // methods

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="services">The services</param>
        public void AddChild(params ServiceInterface[] services)
        {
            foreach (ServiceInterface service in services)
            {
                this.children.Add(service);
            }
        }

        /// <summary>
        /// Finds an service with path notation.
        /// </summary>
        /// <param name="path">Path of the service</param>
        /// <returns>A service instance</returns>
        public ServiceInterface FindByPath(string path)
        {
            Queue<string> parts = new Queue<string>(
                path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)
            );

            ServiceInterface current = this;
            while (parts.Count > 0)
            {
                string part = parts.Dequeue();

                ServiceContainer currentAsContainer = current as ServiceContainer;
                bool found = false;

                if (currentAsContainer != null)
                {
                    foreach (ServiceInterface service in currentAsContainer.Children)
                    {
                        if (service.Name == path)
                        {
                            current = service;
                            found = true;
                        }
                    }
                }

                if (!found)
                {
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
        public T FindByPath<T>(string path) where T : class
        {
            return this.FindByPath(path) as T;
        }

        /// <summary>
        /// Finds the specified dependency (recursively if needed).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns>Service if found</returns>
        public T Find<T>(bool recursive) where T : class, ServiceInterface
        {
            T result;

            foreach (ServiceInterface service in this.Children)
            {
                result = service as T;
                if (result != null)
                {
                    return result;
                }

                if (recursive)
                {
                    ServiceContainer serviceAsContainer = service as ServiceContainer;
                    if (serviceAsContainer != null)
                    {
                        return serviceAsContainer.Find<T>(true);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start()
        {
            try
            {
                if (this.Status == ServiceStatus.Running)
                {
                    return;
                }
                
                base.Start();

                foreach (Service child in this.children)
                {
                    ServiceControllable controllableChild = child as ServiceControllable;

                    if (controllableChild != null)
                    {
                        controllableChild.Start();
                    }
                }

                if (this.OnStartWithChildren != null)
                {
                    this.OnStartWithChildren(this, null);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStartingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            try
            {
                if (this.Status != ServiceStatus.Running)
                {
                    return;
                }

                // stop children
                ServiceInterface[] childServices = ArrayHelpers.GetArray<ServiceInterface>(this.children);
                Array.Reverse(childServices);

                foreach (ServiceInterface child in childServices)
                {
                    ServiceControllable controllableChild = child as ServiceControllable;

                    if (controllableChild != null)
                    {
                        controllableChild.Stop();
                    }
                }

                base.Stop();
            }
            catch (Exception ex)
            {
                this.Logger.LogError(string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStoppingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected override void OnDispose(bool releaseManagedResources)
        {
            ServiceInterface[] childServices = ArrayHelpers.GetArray<ServiceInterface>(this.children);
            Array.Reverse(childServices);

            foreach (ServiceInterface child in childServices)
            {
                IDisposable disposableChild = child as IDisposable;
                if (disposableChild != null)
                {
                    disposableChild.Dispose();
                }
            }

            base.OnDispose(releaseManagedResources);
        }
    }
}

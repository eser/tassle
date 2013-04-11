// -----------------------------------------------------------------------
// <copyright file="ServiceContainer.cs" company="-">
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

namespace Tasslehoff.Library.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Tasslehoff.Library.Logger;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// ServiceContainer class.
    /// </summary>
    public abstract class ServiceContainer : ServiceControllable
    {
        // fields

        /// <summary>
        /// The children
        /// </summary>
        private readonly IDictionary<string, Service> children;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer"/> class.
        /// </summary>
        protected ServiceContainer() : base()
        {
            this.children = new Dictionary<string, Service>();
        }

        // attributes

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public IDictionary<string, Service> Children
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
        /// <param name="service">The service.</param>
        public void AddChild(Service service)
        {
            this.children.Add(service.Name, service);
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

                foreach (Service child in this.children.Values)
                {
                    ServiceControllable controllableChild = child as ServiceControllable;

                    if (controllableChild != null)
                    {
                        controllableChild.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStartingService, this.Name), ex);
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
                Service[] childServices = ArrayUtils.GetArray<Service>(this.children.Values);
                Array.Reverse(childServices);

                foreach (Service child in childServices)
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
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStoppingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.Disposed)
            {
                return;
            }

            if (disposing)
            {
                Service[] childServices = ArrayUtils.GetArray<Service>(this.children.Values);
                Array.Reverse(childServices);

                foreach (Service child in childServices)
                {
                    child.Dispose();
                }

                this.OnDispose();
            }

            this.Disposed = true;
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="ServiceContainer.cs" company="-">
// Copyright (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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
using Tasslehoff.Common.Helpers;
using Tasslehoff.Logging;

namespace Tasslehoff.Services
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
        private readonly IDictionary<string, IService> children;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer"/> class.
        /// </summary>
        protected ServiceContainer() : base()
        {
            this.children = new Dictionary<string, IService>();
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
        public IDictionary<string, IService> Children
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
        /// <param name="service">The service</param>
        public void AddChild(IService service)
        {
            this.children.Add(service.Name, service);
        }

        /// <summary>
        /// Finds an service with path notation.
        /// </summary>
        /// <param name="path">Path of the service</param>
        /// <returns>A service instance</returns>
        public IService Find(string path)
        {
            Queue<string> parts = new Queue<string>(
                path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)
            );

            IService current = this;
            while (parts.Count > 0)
            {
                string part = parts.Dequeue();

                ServiceContainer currentAsContainer = current as ServiceContainer;
                if (!currentAsContainer.Children.ContainsKey(part))
                {
                    return null;
                }

                current = currentAsContainer.Children[part];
            }

            return current;
        }

        /// <summary>
        /// Finds an service with path notation.
        /// </summary>
        /// <typeparam name="T">A type</typeparam>
        /// <param name="path">Path of the service</param>
        /// <returns>A service instance</returns>
        public T Find<T>(string path) where T : class
        {
            return this.Find(path) as T;
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

                if (this.OnStartWithChildren != null)
                {
                    this.OnStartWithChildren(this, null);
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
                IService[] childServices = ArrayHelpers.GetArray<IService>(this.children.Values);
                Array.Reverse(childServices);

                foreach (IService child in childServices)
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
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected override void OnDispose(bool releaseManagedResources)
        {
            IService[] childServices = ArrayHelpers.GetArray<IService>(this.children.Values);
            Array.Reverse(childServices);

            foreach (IService child in childServices)
            {
                child.Dispose();
            }

            base.OnDispose(releaseManagedResources);
        }
    }
}

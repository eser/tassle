// --------------------------------------------------------------------------
// <copyright file="ControllableService.cs" company="-">
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
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Tassle.Services {
    /// <summary>
    /// ControllableService class.
    /// </summary>
    public abstract class ControllableService : Service, ControllableServiceInterface {
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllableService"/> class.
        /// </summary>
        protected ControllableService(ILoggerFactory loggerFactory) : base(loggerFactory) {
        }

        // events

        /// <summary>
        /// Occurs when [started].
        /// </summary>
        public event EventHandler<ServiceStatusChangedEventArgs> Started;

        /// <summary>
        /// Occurs when [stopped].
        /// </summary>
        public event EventHandler<ServiceStatusChangedEventArgs> Stopped;

        // methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public virtual void Start() {
            try {
                if (this.Status == ServiceStatus.Running) {
                    return;
                }

                ServiceStatus previousState = this.Status;

                this.Logger.LogDebug(string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceIsBeingStarted, this.Name));
                this.ServiceStart();

                this.Status = ServiceStatus.Running;
                this.StatusDate = DateTimeOffset.UtcNow;
                this.Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceHasBeenStarted, this.Name));

                this.Started?.Invoke(this, new ServiceStatusChangedEventArgs(previousState, ServiceStatus.Running));
            }
            catch (Exception ex) {
                this.Logger.LogError(string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStartingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public virtual void Stop() {
            try {
                if (this.Status != ServiceStatus.Running) {
                    return;
                }

                ServiceStatus previousState = this.Status;

                this.Logger.LogDebug(string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceIsBeingStopped, this.Name));
                this.ServiceStop();

                this.Status = ServiceStatus.Stopped;
                this.StatusDate = DateTimeOffset.UtcNow;
                this.Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceHasBeenStopped, this.Name));

                this.Stopped?.Invoke(this, new ServiceStatusChangedEventArgs(previousState, ServiceStatus.Stopped));
            }
            catch (Exception ex) {
                this.Logger.LogError(string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStoppingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Restarts this instance.
        /// </summary>
        public virtual void Restart() {
            this.Stop();
            this.Start();
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected override void OnDispose(bool releaseManagedResources) {
            // this.Stop();

            base.OnDispose(releaseManagedResources);
        }

        // abstract methods

        /// <summary>
        /// Invokes events will be occurred during the service start.
        /// </summary>
        protected abstract void ServiceStart();

        /// <summary>
        /// Invokes events will be occurred during the service stop.
        /// </summary>
        protected abstract void ServiceStop();
    }
}

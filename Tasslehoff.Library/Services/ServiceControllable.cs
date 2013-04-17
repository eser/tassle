// -----------------------------------------------------------------------
// <copyright file="ServiceControllable.cs" company="-">
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
    using System.Globalization;
    using Tasslehoff.Library.Logger;

    /// <summary>
    /// ServiceControllable class.
    /// </summary>
    public abstract class ServiceControllable : Service, IServiceControllable
    {
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceControllable"/> class.
        /// </summary>
        protected ServiceControllable() : base()
        {
        }

        // events

        /// <summary>
        /// Occurs when [on start].
        /// </summary>
        public event EventHandler<ServiceStatusChangedEventArgs> OnStart;

        /// <summary>
        /// Occurs when [on stop].
        /// </summary>
        public event EventHandler<ServiceStatusChangedEventArgs> OnStop;

        // methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public virtual void Start()
        {
            try
            {
                if (this.Status == ServiceStatus.Running)
                {
                    return;
                }
                
                ServiceStatus previousState = this.Status;
                
                this.Log.Write(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceIsBeingStarted, this.Name));
                this.ServiceStart();
                
                this.Status = ServiceStatus.Running;
                this.StatusDate = DateTime.UtcNow;
                this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceHasBeenStarted, this.Name));
                
                if (this.OnStart != null)
                {
                    this.OnStart(this, new ServiceStatusChangedEventArgs(previousState, ServiceStatus.Running));
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
        public virtual void Stop()
        {
            try
            {
                if (this.Status != ServiceStatus.Running)
                {
                    return;
                }
                
                ServiceStatus previousState = this.Status;
                
                this.Log.Write(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceIsBeingStopped, this.Name));
                this.ServiceStop();
                
                this.Status = ServiceStatus.Stopped;
                this.StatusDate = DateTime.UtcNow;
                this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceHasBeenStopped, this.Name));
                
                if (this.OnStop != null)
                {
                    this.OnStop(this, new ServiceStatusChangedEventArgs(previousState, ServiceStatus.Stopped));
                }
            }
            catch (Exception ex)
            {
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStoppingService, this.Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Restarts this instance.
        /// </summary>
        public void Restart()
        {
            this.Stop();
            this.Start();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        // abstract methods

        /// <summary>
        /// Services the start.
        /// </summary>
        protected abstract void ServiceStart();

        /// <summary>
        /// Services the stop.
        /// </summary>
        protected abstract void ServiceStop();
    }
}

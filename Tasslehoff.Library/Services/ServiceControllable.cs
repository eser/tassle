//
//  ServiceControllable.cs
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
using System.Globalization;
using Tasslehoff.Library.Logger;

namespace Tasslehoff.Library.Services
{
    public abstract class ServiceControllable : Service
    {
        // constructors
        protected ServiceControllable() : base()
        {
        }

        // events
        public event EventHandler<ServiceStatusChangedEventArgs> OnStart;
        public event EventHandler<ServiceStatusChangedEventArgs> OnStop;

        // attributes
        public override bool IsControllable {
            get {
                return true;
            }
        }

        // abstract methods
        protected abstract void ServiceStart();

        protected abstract void ServiceStop();

        // methods
        public void Start() {
            try {
                if(this.Status == ServiceStatus.Running) {
                    return;
                }
                
                ServiceStatus _previousState = this.Status;
                
                this.Log.Write(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceIsBeingStarted, this.Name));
                this.ServiceStart();
                
                this.Status = ServiceStatus.Running;
                this.StatusDate = DateTime.UtcNow;
                this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceHasBeenStarted, this.Name));
                
                if(this.OnStart != null) {
                    this.OnStart(this, new ServiceStatusChangedEventArgs(_previousState, ServiceStatus.Running));
                }
            }
            catch(Exception _ex) {
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStartingService, this.Name), _ex);
                throw;
            }
        }

        public void Stop() {
            try {
                if(this.Status != ServiceStatus.Running) {
                    return;
                }
                
                ServiceStatus _previousState = this.Status;
                
                this.Log.Write(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceIsBeingStopped, this.Name));
                this.ServiceStop();
                
                this.Status = ServiceStatus.Stopped;
                this.StatusDate = DateTime.UtcNow;
                this.Log.Write(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, LocalResource.ServiceHasBeenStopped, this.Name));
                
                if(this.OnStop != null) {
                    this.OnStop(this, new ServiceStatusChangedEventArgs(_previousState, ServiceStatus.Stopped));
                }
            }
            catch(Exception _ex) {
                this.Log.Write(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, LocalResource.AnErrorOccurredWhileStoppingService, this.Name), _ex);
                throw;
            }
        }

        public void Restart() {
            this.Stop();
            this.Start();
        }
    }
}


//
//  Service.cs
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
using Tasslehoff.Library.Logger;
using Tasslehoff.Library.Utils;

namespace Tasslehoff.Library.Services
{
    public abstract class Service : IDisposable
    {
        // fields
        private ServiceStatus status;
        private DateTime statusDate;
        private LoggerDelegate log;
        private bool disposed;

        // constructors
        protected Service()
        {
            this.status = (this.IsControllable) ? ServiceStatus.Stopped : ServiceStatus.Passive;
            this.statusDate = DateTime.UtcNow;

            this.log = new LoggerDelegate();
        }

        ~Service() {
            this.Dispose(false);
        }

        // abstract properties
        public abstract string Name { get; }

        public virtual string Description {
            get {
                return string.Empty;
            }
        }

        public virtual bool IsControllable {
            get {
                return false;
            }
        }

        // properties
        public ServiceStatus Status {
            get {
                return this.status;
            }
            protected set {
                this.status = value;
            }
        }

        public DateTime StatusDate {
            get {
                return this.statusDate;
            }
            protected set {
                this.statusDate = value;
            }
        }

        public LoggerDelegate Log {
            get {
                return this.log;
            }
            protected set {
                this.log = value;
            }
        }

        public bool Disposed {
            get {
                return this.disposed;
            }
            protected set {
                this.disposed = value;
            }
        }

        protected virtual void OnDispose() {
            VariableUtils.CheckAndDispose(this.log);
        }

        // implementations
        protected void Dispose(bool disposing) {
            if(this.disposed) {
                return;
            }
            
            if(disposing) {
                this.OnDispose();
            }
            
            this.disposed = true;
        }
        
        public void Dispose() {
            this.Dispose(true);
            
            GC.SuppressFinalize(this);
        }
    }
}


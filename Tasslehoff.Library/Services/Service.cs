// -----------------------------------------------------------------------
// <copyright file="Service.cs" company="-">
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
    using System.Diagnostics.CodeAnalysis;
    using Tasslehoff.Library.Logger;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// Service class.
    /// </summary>
    public abstract class Service : IService
    {
        // fields

        /// <summary>
        /// The status
        /// </summary>
        private ServiceStatus status;

        /// <summary>
        /// The status date
        /// </summary>
        private DateTime statusDate;

        /// <summary>
        /// The log
        /// </summary>
        private LoggerDelegate log = null;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>
        protected Service()
        {
            this.status = (this is ServiceControllable) ? ServiceStatus.Stopped : ServiceStatus.Passive;
            this.statusDate = DateTime.UtcNow;

            this.log = new LoggerDelegate();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Service"/> class.
        /// </summary>
        ~Service()
        {
            this.Dispose(false);
        }

        // abstract properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description
        {
            get
            {
                return string.Empty;
            }
        }

        // properties

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ServiceStatus Status
        {
            get
            {
                return this.status;
            }

            protected set
            {
                this.status = value;
            }
        }

        /// <summary>
        /// Gets or sets the status date.
        /// </summary>
        /// <value>
        /// The status date.
        /// </value>
        public DateTime StatusDate
        {
            get
            {
                return this.statusDate;
            }

            protected set
            {
                this.statusDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public LoggerDelegate Log
        {
            get
            {
                return this.log;
            }

            protected set
            {
                this.log = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Service"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get
            {
                return this.disposed;
            }

            protected set
            {
                this.disposed = value;
            }
        }

        // methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "log", Justification = "log is already will be disposed using CheckAndDispose method.")]
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            
            if (disposing)
            {
                VariableUtils.CheckAndDispose(this.log);
                this.log = null;

                this.OnDispose();
            }
            
            this.disposed = true;
        }
    }
}

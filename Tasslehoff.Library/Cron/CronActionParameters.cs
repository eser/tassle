// -----------------------------------------------------------------------
// <copyright file="CronActionParameters.cs" company="-">
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

namespace Tasslehoff.Library.Cron
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// CronActionParameters class.
    /// </summary>
    public class CronActionParameters : IDisposable
    {
        // fields

        /// <summary>
        /// The source
        /// </summary>
        private readonly CronItem source;

        /// <summary>
        /// The action started
        /// </summary>
        private readonly DateTime actionStarted;

        /// <summary>
        /// The lifetime
        /// </summary>
        private readonly TimeSpan lifetime;

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CronActionParameters" /> class.
        /// </summary>
        /// <param name="source">The source</param>
        /// <param name="actionStarted">The action started</param>
        /// <param name="lifetime">The lifetime</param>
        public CronActionParameters(CronItem source, DateTime actionStarted, TimeSpan lifetime)
        {
            this.source = source;
            this.actionStarted = actionStarted;

            if (lifetime != TimeSpan.Zero)
            {
                this.cancellationTokenSource = new CancellationTokenSource(lifetime);
            }
            else
            {
                this.cancellationTokenSource = new CancellationTokenSource();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CronActionParameters"/> class.
        /// </summary>
        ~CronActionParameters()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public CronItem Source
        {
            get
            {
                return this.source;
            }
        }

        /// <summary>
        /// Gets the action started.
        /// </summary>
        /// <value>
        /// The action started.
        /// </value>
        public DateTime ActionStarted
        {
            get
            {
                return this.actionStarted;
            }
        }

        /// <summary>
        /// Gets the cancellation token source.
        /// </summary>
        /// <value>
        /// The cancellation token source.
        /// </value>
        public CancellationTokenSource CancellationTokenSource
        {
            get
            {
                return this.cancellationTokenSource;
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "cancellationTokenSource", Justification = "cancellationTokenSource is already will be disposed using CheckAndDispose method.")]
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                VariableUtils.CheckAndDispose(this.cancellationTokenSource);
                this.cancellationTokenSource = null;
            }

            this.disposed = true;
        }
    }
}

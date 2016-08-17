﻿// --------------------------------------------------------------------------
// <copyright file="TaskActionParameters.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
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
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Tasslehoff.Tasks
{
    /// <summary>
    /// TaskActionParameters class.
    /// </summary>
    public class TaskActionParameters : IDisposable
    {
        // fields

        /// <summary>
        /// The source
        /// </summary>
        private readonly TaskItem source;

        /// <summary>
        /// The action started
        /// </summary>
        private readonly DateTimeOffset actionStarted;

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
        /// Initializes a new instance of the <see cref="TaskActionParameters" /> class.
        /// </summary>
        /// <param name="source">The source</param>
        /// <param name="actionStarted">The action started</param>
        /// <param name="cancellationTokenSource">The cancellation token source</param>
        public TaskActionParameters(TaskItem source, DateTimeOffset actionStarted, CancellationTokenSource cancellationTokenSource)
        {
            this.source = source;
            this.actionStarted = actionStarted;
            this.cancellationTokenSource = cancellationTokenSource;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TaskActionParameters"/> class.
        /// </summary>
        ~TaskActionParameters()
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
        public TaskItem Source
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
        public DateTimeOffset ActionStarted
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
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void OnDispose(bool releaseManagedResources)
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Dispose();
                this.cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "cancellationTokenSource")]
        protected void Dispose(bool releaseManagedResources)
        {
            if (this.Disposed)
            {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this.Disposed = true;
        }
    }
}

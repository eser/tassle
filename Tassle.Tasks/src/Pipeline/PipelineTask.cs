// --------------------------------------------------------------------------
// <copyright file="PipelineTask.cs" company="-">
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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Tassle.Helpers;

namespace Tassle.Tasks.Pipeline {
    /// <summary>
    /// PipelineTask class.
    /// </summary>
    public class PipelineTask : IDisposable {
        // fields

        /// <summary>
        /// The status sync
        /// </summary>
        private readonly object _statusSync;

        /// <summary>
        /// The action
        /// </summary>
        private Action<object> _action;

        /// <summary>
        /// The parameter
        /// </summary>
        private object _parameter;

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// The status
        /// </summary>
        private PipelineTaskStatus _status;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTask"/> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="action">The action.</param>
        public PipelineTask(object parameter, Action<object> action) : this() {
            this._action = action;
            this._parameter = parameter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTask"/> class.
        /// </summary>
        /// <param name="action">The action</param>
        public PipelineTask(Action<object> action) : this() {
            this._action = action;
            this._parameter = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTask"/> class.
        /// </summary>
        protected PipelineTask() {
            this._cancellationTokenSource = new CancellationTokenSource();

            this._statusSync = new object();
            this._status = PipelineTaskStatus.NotStarted;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PipelineTask"/> class.
        /// </summary>
        ~PipelineTask() {
            this.Dispose(false);
        }

        // events

        /// <summary>
        /// Occurs when [status changed].
        /// </summary>
        public event EventHandler<PipelineTaskStatusChangedEventArgs> StatusChanged;

        // properties

        /// <summary>
        /// Gets the status.
        /// </summary>
        public PipelineTaskStatus Status {
            get {
                lock (this._statusSync) {
                    return this._status;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Pipeline"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed {
            get => this._disposed;
            protected set => this._disposed = value;
        }

        // methods

        /// <summary>
        /// Does the task.
        /// </summary>
        /// <returns>The task object</returns>
        public Task DoTask() {
            return Task.Factory.StartNew(this.TaskAction, this._parameter, this._cancellationTokenSource.Token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel() {
            this._cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            this.Dispose(true);

            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Tasks the action.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected void TaskAction(object parameter) {
            if (this._cancellationTokenSource.IsCancellationRequested) {
                lock (this._statusSync) {
                    this._status = PipelineTaskStatus.Cancelled;
                    this.OnStatusChanged();
                }

                return;
            }

            lock (this._statusSync) {
                this._status = PipelineTaskStatus.Running;
                this.OnStatusChanged();
            }

            this._action(parameter);

            lock (this._statusSync) {
                this._status = PipelineTaskStatus.Finished;
                this.OnStatusChanged();
            }
        }

        /// <summary>
        /// Called when [status changed].
        /// </summary>
        protected void OnStatusChanged() {
            this.StatusChanged?.Invoke(this, new PipelineTaskStatusChangedEventArgs(this._status, this._parameter));
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void OnDispose(bool releaseManagedResources) {
            VariableHelpers.CheckAndDispose<CancellationTokenSource>(ref this._cancellationTokenSource);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "cancellationTokenSource")]
        protected void Dispose(bool releaseManagedResources) {
            if (this._disposed) {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this._disposed = true;
        }
    }
}
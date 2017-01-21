// --------------------------------------------------------------------------
// <copyright file="Pipeline.cs" company="-">
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
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Tassle.Helpers;

namespace Tassle.Tasks.Pipeline {
    /// <summary>
    /// Pipeline class.
    /// </summary>
    public class Pipeline : IDisposable {
        // fields

        /// <summary>
        /// The running task count sync
        /// </summary>
        private readonly object _runningTaskCountSync;

        /// <summary>
        /// The maximum tasks
        /// </summary>
        private readonly int _maximumTasks;

        /// <summary>
        /// The parameters
        /// </summary>
        private ConcurrentQueue<object> _parameters;

        /// <summary>
        /// The action
        /// </summary>
        private Action<object> _action;

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// The running task count
        /// </summary>
        private int _runningTaskCount;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        /// <param name="maximumTasks">The maximum tasks.</param>
        /// <param name="action">The action.</param>
        public Pipeline(int maximumTasks, Action<object> action) : this() {
            this._maximumTasks = maximumTasks;
            this._action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        protected Pipeline() {
            this._parameters = new ConcurrentQueue<object>();

            this._cancellationTokenSource = new CancellationTokenSource();

            this._runningTaskCountSync = new object();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Pipeline"/> class.
        /// </summary>
        ~Pipeline() {
            this.Dispose(false);
        }

        // events

        /// <summary>
        /// Occurs when [task status changed].
        /// </summary>
        public event EventHandler<PipelineTaskStatusChangedEventArgs> TaskStatusChanged;

        /// <summary>
        /// Occurs when [tasks done].
        /// </summary>
        public event EventHandler<PipelineTasksDoneEventArgs> TasksDone;

        // properties

        /// <summary>
        /// Gets the running task count.
        /// </summary>
        public int RunningTaskCount {
            get {
                lock (this._runningTaskCountSync) {
                    return this._runningTaskCount;
                }
            }
        }

        /// <summary>
        /// Gets the available task slot count.
        /// </summary>
        public int AvailableTaskSlotCount {
            get {
                lock (this._runningTaskCountSync) {
                    return this._maximumTasks - this._runningTaskCount;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning {
            get {
                lock (this._runningTaskCountSync) {
                    return this._runningTaskCount > 0;
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
        /// Adds the task.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void AddTask(object parameter) {
            this._parameters.Enqueue(parameter);
            this.InvokeTaskStatusChanged(PipelineTaskStatus.NotStarted, parameter);

            this.FillQueue();
        }

        /// <summary>
        /// Cancels the specified empty queues.
        /// </summary>
        /// <param name="emptyQueues">if set to <c>true</c> [empty queues].</param>
        public void Cancel(bool emptyQueues = true) {
            this._cancellationTokenSource.Cancel();

            if (emptyQueues) {
                while (this._parameters.IsEmpty) {
                    object dummy;
                    this._parameters.TryDequeue(out dummy);
                }
            }

            lock (this._runningTaskCountSync) {
                this._runningTaskCount = 0;
            }

            this.InvokeTasksDone(true);
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
        /// Fills the queue.
        /// </summary>
        protected void FillQueue() {
            while (this.DequeueAndDoTask()) {
            }
        }

        /// <summary>
        /// Pops the and do task.
        /// </summary>
        /// <remarks>Lock the running task count before entering the function</remarks>
        /// <returns>Whether the task is done or not</returns>
        protected bool DequeueAndDoTask() {
            object parameter;

            lock (this._runningTaskCountSync) {
                if (this._runningTaskCount >= this._maximumTasks) {
                    return false;
                }

                if (!this._parameters.TryDequeue(out parameter)) {
                    if (this._runningTaskCount == 0) {
                        this.InvokeTasksDone(false);
                    }

                    return false;
                }
            }

            Task.Factory.StartNew(this.TaskAction, parameter, this._cancellationTokenSource.Token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
            return true;
        }

        /// <summary>
        /// Tasks the action.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected void TaskAction(object parameter) {
            if (this._cancellationTokenSource.IsCancellationRequested) {
                this.InvokeTaskStatusChanged(PipelineTaskStatus.Cancelled, parameter);
                return;
            }

            lock (this._runningTaskCountSync) {
                this._runningTaskCount++;
                this.InvokeTaskStatusChanged(PipelineTaskStatus.Running, parameter);
            }

            this._action(parameter);

            lock (this._runningTaskCountSync) {
                this._runningTaskCount--;
                this.InvokeTaskStatusChanged(PipelineTaskStatus.Finished, parameter);

                this.DequeueAndDoTask();
            }
        }

        /// <summary>
        /// Gets called when [tasks done].
        /// </summary>
        /// <param name="isCancelled">if set to <c>true</c> [is cancelled].</param>
        protected void InvokeTasksDone(bool isCancelled) {
            this.TasksDone?.Invoke(this, new PipelineTasksDoneEventArgs(isCancelled));
        }

        /// <summary>
        /// Gets called when [task status changed].
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="parameter">The parameter.</param>
        protected void InvokeTaskStatusChanged(PipelineTaskStatus status, object parameter) {
            this.TaskStatusChanged?.Invoke(this, new PipelineTaskStatusChangedEventArgs(status, parameter));
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
// -----------------------------------------------------------------------
// <copyright file="ChannelTaskQueue.cs" company="-">
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

namespace Laroux.ScabbiaLibrary.Threading
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// ChannelTaskQueue class.
    /// </summary>
    public class ChannelTaskQueue : IDisposable
    {
        // fields

        /// <summary>
        /// The running task count sync
        /// </summary>
        private readonly object runningTaskCountSync;

        /// <summary>
        /// The maximum tasks
        /// </summary>
        private readonly int maximumTasks;

        /// <summary>
        /// The parameters
        /// </summary>
        private ConcurrentQueue<object> parameters;

        /// <summary>
        /// The action
        /// </summary>
        private Action<object> action;

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// The running task count
        /// </summary>
        private int runningTaskCount;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelTaskQueue"/> class.
        /// </summary>
        /// <param name="maximumTasks">The maximum tasks.</param>
        /// <param name="action">The action.</param>
        public ChannelTaskQueue(int maximumTasks, Action<object> action) : this()
        {
            this.maximumTasks = maximumTasks;
            this.action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelTaskQueue"/> class.
        /// </summary>
        protected ChannelTaskQueue()
        {
            this.parameters = new ConcurrentQueue<object>();

            this.cancellationTokenSource = new CancellationTokenSource();

            this.runningTaskCountSync = new object();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ChannelTaskQueue"/> class.
        /// </summary>
        ~ChannelTaskQueue()
        {
            this.Dispose(false);
        }

        // events

        /// <summary>
        /// Occurs when [task status changed].
        /// </summary>
        public event EventHandler<ChannelTaskStatusChangedEventArgs> TaskStatusChanged;

        /// <summary>
        /// Occurs when [tasks done].
        /// </summary>
        public event EventHandler<ChannelTasksDoneEventArgs> TasksDone;

        // properties

        /// <summary>
        /// Gets the running task count.
        /// </summary>
        public int RunningTaskCount
        {
            get
            {
                lock (this.runningTaskCountSync)
                {
                    return this.runningTaskCount;
                }
            }
        }

        /// <summary>
        /// Gets the available task slot count.
        /// </summary>
        public int AvailableTaskSlotCount
        {
            get
            {
                lock (this.runningTaskCountSync)
                {
                    return this.maximumTasks - this.runningTaskCount;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                lock (this.runningTaskCountSync)
                {
                    return this.runningTaskCount > 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ChannelTaskQueue"/> is disposed.
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
        /// Adds the task.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void AddTask(object parameter)
        {
            this.parameters.Enqueue(parameter);
            this.OnTaskStatusChanged(ChannelTaskStatus.NotStarted, parameter);

            this.FillQueue();
        }

        /// <summary>
        /// Cancels the specified empty queues.
        /// </summary>
        /// <param name="emptyQueues">if set to <c>true</c> [empty queues].</param>
        public void Cancel(bool emptyQueues = true)
        {
            this.cancellationTokenSource.Cancel();

            if (emptyQueues)
            {
                while (this.parameters.IsEmpty)
                {
                    object dummy;
                    this.parameters.TryDequeue(out dummy);
                }
            }

            lock (this.runningTaskCountSync)
            {
                this.runningTaskCount = 0;
            }

            this.OnTasksDone(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Fills the queue.
        /// </summary>
        protected void FillQueue()
        {
            while (this.DequeueAndDoTask())
            {
            }
        }

        /// <summary>
        /// Pops the and do task.
        /// </summary>
        /// <remarks>Lock the running task count before entering the function</remarks>
        /// <returns>Whether the task is done or not</returns>
        protected bool DequeueAndDoTask()
        {
            object parameter;

            lock (this.runningTaskCountSync)
            {
                if (this.runningTaskCount >= this.maximumTasks)
                {
                    return false;
                }

                if (!this.parameters.TryDequeue(out parameter))
                {
                    if (this.runningTaskCount == 0)
                    {
                        this.OnTasksDone(false);
                    }

                    return false;
                }
            }

            Task.Factory.StartNew(this.TaskAction, parameter, this.cancellationTokenSource.Token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
            return true;
        }

        /// <summary>
        /// Tasks the action.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected void TaskAction(object parameter)
        {
            if (this.cancellationTokenSource.IsCancellationRequested)
            {
                this.OnTaskStatusChanged(ChannelTaskStatus.Cancelled, parameter);
                return;
            }

            lock (this.runningTaskCountSync)
            {
                this.runningTaskCount++;
                this.OnTaskStatusChanged(ChannelTaskStatus.Running, parameter);
            }

            this.action(parameter);

            lock (this.runningTaskCountSync)
            {
                this.runningTaskCount--;
                this.OnTaskStatusChanged(ChannelTaskStatus.Finished, parameter);

                this.DequeueAndDoTask();
            }
        }

        /// <summary>
        /// Called when [tasks done].
        /// </summary>
        /// <param name="isCancelled">if set to <c>true</c> [is cancelled].</param>
        protected void OnTasksDone(bool isCancelled)
        {
            if (this.TasksDone != null)
            {
                this.TasksDone(this, new ChannelTasksDoneEventArgs(isCancelled));
            }
        }

        /// <summary>
        /// Called when [task status changed].
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="parameter">The parameter.</param>
        protected void OnTaskStatusChanged(ChannelTaskStatus status, object parameter)
        {
            if (this.TaskStatusChanged != null)
            {
                this.TaskStatusChanged(this, new ChannelTaskStatusChangedEventArgs(status, parameter));
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "cancellationTokenSource", Justification = "cancellationTokenSource is already will be disposed using CheckAndDispose method.")]
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (this.disposed)
                {
                    return;
                }

                if (disposing)
                {
                    // Release diposable objects used by this instance here.
                    VariableUtils.CheckAndDispose(this.cancellationTokenSource);
                    this.cancellationTokenSource = null;
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                this.disposed = true;
            }
        }
    }
}
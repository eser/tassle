// --------------------------------------------------------------------------
// <copyright file="TaskItem.cs" company="-">
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Tasks {
    /// <summary>
    /// TaskItem class.
    /// </summary>
    public class TaskItem {
        // fields

        /// <summary>
        /// The recurrence
        /// </summary>
        private Recurrence recurrence;

        /// <summary>
        /// The repeat
        /// </summary>
        private int repeat;

        /// <summary>
        /// The action
        /// </summary>
        private Action<TaskActionParameters> action;

        /// <summary>
        /// The status
        /// </summary>
        private TaskItemStatus status;

        /// <summary>
        /// The last run
        /// </summary>
        private DateTimeOffset lastRun;

        /// <summary>
        /// The lifetime
        /// </summary>
        private TimeSpan lifetime;

        /// <summary>
        /// The active actions
        /// </summary>
        private ICollection<TaskActionParameters> activeActions;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItem" /> class.
        /// </summary>
        /// <param name="recurrence">The recurrence</param>
        /// <param name="action">The action</param>
        /// <param name="lifetime">The lifetime</param>
        public TaskItem(Action<TaskActionParameters> action) {
            this.status = TaskItemStatus.NotStarted;
            this.lastRun = DateTimeOffset.MinValue;

            this.recurrence = Recurrence.Once;
            this.repeat = 1;
            this.action = action;
            this.lifetime = TimeSpan.Zero;

            this.activeActions = new Collection<TaskActionParameters>();
        }

        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        /// <value>
        /// The recurrence.
        /// </value>
        public Recurrence Recurrence {
            get => this.recurrence;
            set => this.recurrence = value;
        }

        /// <summary>
        /// Gets or sets the repeat.
        /// </summary>
        /// <value>
        /// The repeat.
        /// </value>
        public int Repeat {
            get => this.repeat;
            set => this.repeat = value;
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public Action<TaskActionParameters> Action {
            get => this.action;
            set => this.action = value;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TaskItemStatus Status {
            get => this.status;
            protected set => this.status = value;
        }

        /// <summary>
        /// Gets the last run.
        /// </summary>
        /// <value>
        /// The last run.
        /// </value>
        public DateTimeOffset LastRun {
            get => this.lastRun;
            protected set => this.lastRun = value;
        }

        /// <summary>
        /// Gets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public TimeSpan Lifetime {
            get => this.lifetime;
            protected set => this.lifetime = value;
        }

        /// <summary>
        /// Gets the active actions.
        /// </summary>
        /// <value>
        /// The active actions.
        /// </value>
        public ICollection<TaskActionParameters> ActiveActions {
            get => this.activeActions;
            protected set => this.activeActions = value;
        }

        // methods

        /// <summary>
        /// Sets recurrence of task item.
        /// </summary>
        /// <param name="recurrence">Recurrence</param>
        /// <returns>Task Item</returns>
        public TaskItem SetRecurrence(Recurrence recurrence) {
            this.recurrence = recurrence;

            return this;
        }

        /// <summary>
        /// Sets repeat of task item.
        /// </summary>
        /// <param name="repeat">Repeat</param>
        /// <returns>Task Item</returns>
        public TaskItem SetRepeat(int repeat) {
            this.repeat = repeat;

            return this;
        }

        /// <summary>
        /// Sets lifetime of task item.
        /// </summary>
        /// <param name="lifetime">Life time</param>
        /// <returns>Task Item</returns>
        public TaskItem SetLifetime(TimeSpan lifetime) {
            this.lifetime = lifetime;

            return this;
        }

        /// <summary>
        /// Postpones the task schedule.
        /// </summary>
        /// <param name="timespan">The timespan which will be added to start time</param>
        /// <returns>Task Item</returns>
        public TaskItem Postpone(TimeSpan timespan) {
            this.recurrence = new Recurrence(
                DateTimeOffset.UtcNow.Add(timespan),
                this.recurrence.Interval
            );

            return this;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init() {
            this.status = TaskItemStatus.Running;
            this.lastRun = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Runs the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        public void Run(DateTimeOffset? dateTime = null) {
            DateTimeOffset now = dateTime.GetValueOrDefault(DateTimeOffset.UtcNow);

            if (now > this.recurrence.DateEnd) {
                this.status = TaskItemStatus.Stopped;
                return;
            }

            if (!this.recurrence.CheckDate(now)) {
                return;
            }

            if (this.status != TaskItemStatus.Running) {
                return;
            }

            if (now.Subtract(this.lastRun) < this.recurrence.Interval) {
                return;
            }

            this.lastRun = now;

            CancellationTokenSource cancellationTokenSource;
            if (this.lifetime != TimeSpan.Zero) {
                cancellationTokenSource = new CancellationTokenSource(this.lifetime);
            }
            else {
                cancellationTokenSource = new CancellationTokenSource();
            }

            var cronActionParameters = new TaskActionParameters(this, now, cancellationTokenSource);
            this.activeActions.Add(cronActionParameters);

            Action actionToRun = () => {
                for (var i = this.repeat; i > 0; i--) {
                    this.action(cronActionParameters);
                }

                this.activeActions.Remove(cronActionParameters);
            };

            var lastTask = Task.Run(actionToRun, cancellationTokenSource.Token);

            if (this.recurrence.Interval == TimeSpan.Zero) {
                this.status = TaskItemStatus.Stopped;
            }
        }

        /// <summary>
        /// Cancels the active actions.
        /// </summary>
        public void CancelActiveActions() {
            foreach (var activeAction in this.activeActions) {
                activeAction.CancellationTokenSource.Cancel();
            }
        }
    }
}

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
        private Recurrence _recurrence;

        /// <summary>
        /// The repeat
        /// </summary>
        private int _repeat;

        /// <summary>
        /// The action
        /// </summary>
        private Action<TaskActionParameters> _action;

        /// <summary>
        /// The status
        /// </summary>
        private TaskItemStatus _status;

        /// <summary>
        /// The last run
        /// </summary>
        private DateTimeOffset _lastRun;

        /// <summary>
        /// The lifetime
        /// </summary>
        private TimeSpan _lifetime;

        /// <summary>
        /// The active actions
        /// </summary>
        private ICollection<TaskActionParameters> _activeActions;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItem" /> class.
        /// </summary>
        /// <param name="recurrence">The recurrence</param>
        /// <param name="action">The action</param>
        /// <param name="lifetime">The lifetime</param>
        public TaskItem(Action<TaskActionParameters> action) {
            this._status = TaskItemStatus.NotStarted;
            this._lastRun = DateTimeOffset.MinValue;

            this._recurrence = Recurrence.Once;
            this._repeat = 1;
            this._action = action;
            this._lifetime = TimeSpan.Zero;

            this._activeActions = new Collection<TaskActionParameters>();
        }

        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        /// <value>
        /// The recurrence.
        /// </value>
        public Recurrence Recurrence {
            get => this._recurrence;
            set => this._recurrence = value;
        }

        /// <summary>
        /// Gets or sets the repeat.
        /// </summary>
        /// <value>
        /// The repeat.
        /// </value>
        public int Repeat {
            get => this._repeat;
            set => this._repeat = value;
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public Action<TaskActionParameters> Action {
            get => this._action;
            set => this._action = value;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TaskItemStatus Status {
            get => this._status;
            protected set => this._status = value;
        }

        /// <summary>
        /// Gets the last run.
        /// </summary>
        /// <value>
        /// The last run.
        /// </value>
        public DateTimeOffset LastRun {
            get => this._lastRun;
            protected set => this._lastRun = value;
        }

        /// <summary>
        /// Gets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public TimeSpan Lifetime {
            get => this._lifetime;
            protected set => this._lifetime = value;
        }

        /// <summary>
        /// Gets the active actions.
        /// </summary>
        /// <value>
        /// The active actions.
        /// </value>
        public ICollection<TaskActionParameters> ActiveActions {
            get => this._activeActions;
            protected set => this._activeActions = value;
        }

        // methods

        /// <summary>
        /// Sets recurrence of task item.
        /// </summary>
        /// <param name="recurrence">Recurrence</param>
        /// <returns>Task Item</returns>
        public TaskItem SetRecurrence(Recurrence recurrence) {
            this._recurrence = recurrence;

            return this;
        }

        /// <summary>
        /// Sets repeat of task item.
        /// </summary>
        /// <param name="repeat">Repeat</param>
        /// <returns>Task Item</returns>
        public TaskItem SetRepeat(int repeat) {
            this._repeat = repeat;

            return this;
        }

        /// <summary>
        /// Sets lifetime of task item.
        /// </summary>
        /// <param name="lifetime">Life time</param>
        /// <returns>Task Item</returns>
        public TaskItem SetLifetime(TimeSpan lifetime) {
            this._lifetime = lifetime;

            return this;
        }

        /// <summary>
        /// Postpones the task schedule.
        /// </summary>
        /// <param name="timespan">The timespan which will be added to start time</param>
        /// <returns>Task Item</returns>
        public TaskItem Postpone(TimeSpan timespan) {
            this._recurrence = new Recurrence(
                DateTimeOffset.UtcNow.Add(timespan),
                this._recurrence.Interval
            );

            return this;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init() {
            this._status = TaskItemStatus.Running;
            this._lastRun = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Runs the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        public void Run(DateTimeOffset? dateTime = null) {
            DateTimeOffset now = dateTime.GetValueOrDefault(DateTimeOffset.UtcNow);

            if (now > this._recurrence.DateEnd) {
                this._status = TaskItemStatus.Stopped;
                return;
            }

            if (!this._recurrence.CheckDate(now)) {
                return;
            }

            if (this._status != TaskItemStatus.Running) {
                return;
            }

            if (now.Subtract(this._lastRun) < this._recurrence.Interval) {
                return;
            }

            this._lastRun = now;

            CancellationTokenSource cancellationTokenSource;
            if (this._lifetime != TimeSpan.Zero) {
                cancellationTokenSource = new CancellationTokenSource(this._lifetime);
            }
            else {
                cancellationTokenSource = new CancellationTokenSource();
            }

            var cronActionParameters = new TaskActionParameters(this, now, cancellationTokenSource);
            this._activeActions.Add(cronActionParameters);

            Action actionToRun = () => {
                for (var i = this._repeat; i > 0; i--) {
                    this._action(cronActionParameters);
                }

                this._activeActions.Remove(cronActionParameters);
            };

            var lastTask = Task.Run(actionToRun, cancellationTokenSource.Token);

            if (this._recurrence.Interval == TimeSpan.Zero) {
                this._status = TaskItemStatus.Stopped;
            }
        }

        /// <summary>
        /// Cancels the active actions.
        /// </summary>
        public void CancelActiveActions() {
            foreach (var activeAction in this._activeActions) {
                activeAction.CancellationTokenSource.Cancel();
            }
        }
    }
}

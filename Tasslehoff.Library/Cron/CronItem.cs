// -----------------------------------------------------------------------
// <copyright file="CronItem.cs" company="-">
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
    using System.Threading.Tasks;

    /// <summary>
    /// CronItem class.
    /// </summary>
    public class CronItem
    {
        // fields

        /// <summary>
        /// The recurrence
        /// </summary>
        private Recurrence recurrence;

        /// <summary>
        /// The task
        /// </summary>
        private Action task;

        /// <summary>
        /// The status
        /// </summary>
        private CronItemStatus status;

        /// <summary>
        /// The last run
        /// </summary>
        private DateTime lastRun;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CronItem"/> class.
        /// </summary>
        /// <param name="recurrence">The recurrence.</param>
        /// <param name="task">The task.</param>
        public CronItem(Recurrence recurrence, Action task)
        {
            this.status = CronItemStatus.NotStarted;
            this.lastRun = DateTime.MinValue;

            this.recurrence = recurrence;
            this.task = task;
        }

        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        /// <value>
        /// The recurrence.
        /// </value>
        public Recurrence Recurrence
        {
            get
            {
                return this.recurrence;
            }

            set
            {
                this.recurrence = value;
            }
        }

        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        public Action Task
        {
            get
            {
                return this.task;
            }

            set
            {
                this.task = value;
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public CronItemStatus Status
        {
            get
            {
                return this.status;
            }
        }

        /// <summary>
        /// Gets the last run.
        /// </summary>
        /// <value>
        /// The last run.
        /// </value>
        public DateTime LastRun
        {
            get
            {
                return this.lastRun;
            }
        }

        // methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            this.status = CronItemStatus.Running;
            this.lastRun = DateTime.UtcNow;
        }

        /// <summary>
        /// Runs the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        public void Run(DateTime? dateTime = null)
        {
            DateTime now = dateTime.GetValueOrDefault(DateTime.UtcNow);

            if (now > this.recurrence.DateEnd)
            {
                this.status = CronItemStatus.Stopped;
                return;
            }

            if (!this.recurrence.CheckDate(now))
            {
                return;
            }

            if (this.status != CronItemStatus.Running)
            {
                return;
            }
            else if (now.Subtract(this.lastRun) < this.recurrence.Interval)
            {
                return;
            }

            this.task.Invoke();
            this.lastRun = now;
            if (this.recurrence.Interval == TimeSpan.Zero)
            {
                this.status = CronItemStatus.Stopped;
            }
        }
    }
}

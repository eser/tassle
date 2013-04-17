// -----------------------------------------------------------------------
// <copyright file="CronManager.cs" company="-">
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
    using System.Collections.Generic;
    using System.Timers;
    using Tasslehoff.Library.Services;

    /// <summary>
    /// CronManager class.
    /// </summary>
    public class CronManager : ServiceControllable
    {
        // fields

        /// <summary>
        /// The items
        /// </summary>
        private IDictionary<string, CronItem> items;

        /// <summary>
        /// The timer
        /// </summary>
        private Timer timer;

        /// <summary>
        /// The now
        /// </summary>
        private DateTime now;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CronManager"/> class.
        /// </summary>
        public CronManager() : base()
        {
            this.items = new Dictionary<string, CronItem>();

            this.timer = new Timer(1000)
            {
                AutoReset = false,
                Enabled = false
            };
            this.timer.Elapsed += this.Timer_Elapsed;

            this.now = DateTime.UtcNow;
        }

        // properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return "CronManager";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Executes scheduled tasks on time.";
            }
        }

        // methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="item">The item</param>
        public void Add(string key, CronItem item)
        {
            item.Init();

            this.items.Add(key, item);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        public void Remove(string key)
        {
            this.items.Remove(key);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Services the start.
        /// </summary>
        protected override void ServiceStart()
        {
            this.timer.Start();
        }

        /// <summary>
        /// Services the stop.
        /// </summary>
        protected override void ServiceStop()
        {
            foreach (CronItem item in this.items.Values)
            {
                item.CancelActiveActions();
            }

            this.timer.Stop();
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.now = e.SignalTime.ToUniversalTime();

            foreach (KeyValuePair<string, CronItem> pair in this.items)
            {
                pair.Value.Run(this.now);
            }

            this.timer.Start();
        }
    }
}

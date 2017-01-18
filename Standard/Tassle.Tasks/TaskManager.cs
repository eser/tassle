// --------------------------------------------------------------------------
// <copyright file="TaskManager.cs" company="-">
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

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tassle.Services;

namespace Tassle.Tasks
{
    /// <summary>
    /// TaskManager class.
    /// </summary>
    public class TaskManager : ServiceControllable
    {
        // fields

        /// <summary>
        /// The items
        /// </summary>
        private IDictionary<string, TaskItem> items;

        /// <summary>
        /// The timer
        /// </summary>
        private Timer timer;

        /// <summary>
        /// The now
        /// </summary>
        private DateTimeOffset now;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManager"/> class.
        /// </summary>
        public TaskManager(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.items = new Dictionary<string, TaskItem>();

            this.timer = null;

            this.now = DateTimeOffset.UtcNow;
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
                return "TaskManager";
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

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IDictionary<string, TaskItem> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        /// <value>
        /// The timer.
        /// </value>
        protected Timer Timer
        {
            get
            {
                return this.timer;
            }
            set
            {
                this.timer = value;
            }
        }

        /// <summary>
        /// Gets or sets the now.
        /// </summary>
        /// <value>
        /// The now.
        /// </value>
        protected DateTimeOffset Now
        {
            get
            {
                return this.now;
            }
            set
            {
                this.now = value;
            }
        }

        // methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="item">The item</param>
        public void Add(string key, TaskItem item)
        {
            item.Init();

            this.Items.Add(key, item);
            if (this.Status == ServiceStatus.Running)
            {
                item.Run();
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        public void Remove(string key)
        {
            this.Items.Remove(key);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <summary>
        /// Requests a registered object to unregister.
        /// </summary>
        /// <param name="immediate">true to indicate the registered object should unregister from the hosting environment before returning; otherwise, false.</param>
        public void Stop(bool immediate)
        {
            this.Stop();
        }

        /// <summary>
        /// Invokes events will be occurred during the service start.
        /// </summary>
        protected override void ServiceStart()
        {
            this.timer = new Timer(this.TimerCallback, null, Timeout.Infinite, 1000);
        }

        /// <summary>
        /// Invokes events will be occurred during the service stop.
        /// </summary>
        protected override void ServiceStop()
        {
            foreach (TaskItem item in this.Items.Values)
            {
                item.CancelActiveActions();
            }

            this.timer.Dispose();
            this.timer = null;
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="state">Object state</param>
        private void TimerCallback(object state)
        {
            this.Now = DateTimeOffset.UtcNow;

            foreach (KeyValuePair<string, TaskItem> pair in this.Items)
            {
                pair.Value.Run(this.Now);
            }
        }
    }
}

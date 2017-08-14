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

namespace Tassle.Tasks {
    /// <summary>
    /// TaskManager class.
    /// </summary>
    public class TaskManager : ControllableService {
        // fields

        /// <summary>
        /// The items
        /// </summary>
        private IDictionary<string, TaskItem> _items;

        /// <summary>
        /// The timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// The now
        /// </summary>
        private DateTimeOffset _now;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManager"/> class.
        /// </summary>
        public TaskManager(ILoggerFactory loggerFactory) : base(loggerFactory) {
            this._items = new Dictionary<string, TaskItem>();

            this._timer = null;

            this._now = DateTimeOffset.UtcNow;
        }

        // properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name {
            get => "TaskManager";
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description {
            get => "Executes scheduled tasks on time.";
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IDictionary<string, TaskItem> Items {
            get => this._items;
            set => this._items = value;
        }

        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        /// <value>
        /// The timer.
        /// </value>
        protected Timer Timer {
            get => this._timer;
            set => this._timer = value;
        }

        /// <summary>
        /// Gets or sets the now.
        /// </summary>
        /// <value>
        /// The now.
        /// </value>
        protected DateTimeOffset Now {
            get => this._now;
            set => this._now = value;
        }

        // methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="item">The item</param>
        public void Add(string key, TaskItem item) {
            item.Init();

            this._items.Add(key, item);

            if (this.Status == ServiceStatus.Running) {
                item.Run();
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        public void Remove(string key) {
            this._items.Remove(key);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear() {
            this._items.Clear();
        }

        /// <summary>
        /// Requests a registered object to unregister.
        /// </summary>
        /// <param name="immediate">true to indicate the registered object should unregister from the hosting environment before returning; otherwise, false.</param>
        public void Stop(bool immediate) {
            this.Stop();
        }

        /// <summary>
        /// Invokes events will be occurred during the service start.
        /// </summary>
        protected override void ServiceStart() {
            this._timer = new Timer(this.TimerCallback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }

        /// <summary>
        /// Invokes events will be occurred during the service stop.
        /// </summary>
        protected override void ServiceStop() {
            foreach (var item in this._items) {
                item.Value.CancelActiveActions();
            }

            this._timer.Dispose();
            this._timer = null;
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="state">Object state</param>
        private void TimerCallback(object state) {
            this._now = DateTimeOffset.UtcNow;

            foreach (var pair in this._items) {
                pair.Value.Run(this._now);
            }
        }
    }
}

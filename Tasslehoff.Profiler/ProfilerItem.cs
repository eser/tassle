﻿// --------------------------------------------------------------------------
// <copyright file="ProfilerItem.cs" company="-">
// Copyright (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Tasslehoff.Common.Helpers;

namespace Tasslehoff.Profiler
{
    /// <summary>
    /// ProfilerItem class.
    /// </summary>
    public class ProfilerItem : IDisposable
    {
        // fields

        /// <summary>
        /// The category name
        /// </summary>
        private readonly string categoryName;

        /// <summary>
        /// The counter name
        /// </summary>
        private readonly string counterName;

        /// <summary>
        /// The process name
        /// </summary>
        private readonly string processName;

        /// <summary>
        /// The performance counter
        /// </summary>
        private PerformanceCounter performanceCounter;

        /// <summary>
        /// The last value
        /// </summary>
        private double lastValue;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilerItem"/> class.
        /// </summary>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="counterName">Name of the counter</param>
        /// <param name="processName">Name of the process</param>
        public ProfilerItem(string categoryName, string counterName, string processName)
        {
            this.categoryName = categoryName;
            this.counterName = counterName;
            this.processName = processName;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ProfilerItem"/> class.
        /// </summary>
        ~ProfilerItem()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName
        {
            get
            {
                return this.categoryName;
            }
        }

        /// <summary>
        /// Gets the name of the counter.
        /// </summary>
        /// <value>
        /// The name of the counter.
        /// </value>
        public string CounterName
        {
            get
            {
                return this.counterName;
            }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        /// <value>
        /// The name of the process.
        /// </value>
        public string ProcessName
        {
            get
            {
                return this.processName;
            }
        }

        /// <summary>
        /// Gets the last value.
        /// </summary>
        /// <value>
        /// The last value.
        /// </value>
        public double LastValue
        {
            get
            {
                return this.lastValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProfilerItem"/> is disposed.
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
        /// Gets the value.
        /// </summary>
        /// <returns>Performance value</returns>
        public double GetValue()
        {
            this.CreatePerformanceCounter();

            this.lastValue = this.performanceCounter.NextValue();
            return this.lastValue;
        }

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
            VariableHelpers.CheckAndDispose<PerformanceCounter>(ref this.performanceCounter);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "performanceCounter")]
        protected void Dispose(bool releaseManagedResources)
        {
            if (this.disposed)
            {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this.disposed = true;
        }

        /// <summary>
        /// Creates the performance counter.
        /// </summary>
        private void CreatePerformanceCounter()
        {
            if (this.performanceCounter == null)
            {
                if (!string.IsNullOrEmpty(this.processName))
                {
                    this.performanceCounter = new PerformanceCounter(this.categoryName, this.counterName, this.processName, true);
                }
                else
                {
                    this.performanceCounter = new PerformanceCounter(this.categoryName, this.counterName, true);
                }
            }
        }
    }
}

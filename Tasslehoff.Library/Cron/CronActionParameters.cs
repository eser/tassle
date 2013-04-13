// -----------------------------------------------------------------------
// <copyright file="CronActionParameters.cs" company="-">
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
    using System.Threading;

    /// <summary>
    /// CronActionParameters class.
    /// </summary>
    public class CronActionParameters
    {
        // fields

        /// <summary>
        /// The source
        /// </summary>
        private readonly CronItem source;

        /// <summary>
        /// The action started
        /// </summary>
        private readonly DateTime actionStarted;

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CronActionParameters"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="actionStarted">The action started.</param>
        public CronActionParameters(CronItem source, DateTime actionStarted)
        {
            this.source = source;
            this.actionStarted = actionStarted;

            this.cancellationTokenSource = new CancellationTokenSource();
        }

        // properties

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public CronItem Source
        {
            get
            {
                return this.source;
            }
        }

        /// <summary>
        /// Gets the action started.
        /// </summary>
        /// <value>
        /// The action started.
        /// </value>
        public DateTime ActionStarted
        {
            get
            {
                return this.actionStarted;
            }
        }

        /// <summary>
        /// Gets the cancellation token source.
        /// </summary>
        /// <value>
        /// The cancellation token source.
        /// </value>
        public CancellationTokenSource CancellationTokenSource
        {
            get
            {
                return this.cancellationTokenSource;
            }
        }
    }
}

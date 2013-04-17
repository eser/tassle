// -----------------------------------------------------------------------
// <copyright file="CounterPoppedEventArgs.cs" company="-">
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

namespace Tasslehoff.Library.Objects
{
    using System;

    /// <summary>
    /// CounterPoppedEventArgs class.
    /// </summary>
    public class CounterPoppedEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The key
        /// </summary>
        private readonly string key;

        /// <summary>
        /// The period
        /// </summary>
        private TimeSpan period;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterPoppedEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="period">The period</param>
        public CounterPoppedEventArgs(string key, TimeSpan period) : base()
        {
            this.key = key;
            this.period = period;
        }

        // properties

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Gets the period.
        /// </summary>
        public TimeSpan Period
        {
            get
            {
                return this.period;
            }
        }
    }
}
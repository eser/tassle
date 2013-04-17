// -----------------------------------------------------------------------
// <copyright file="Recurrence.cs" company="-">
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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// Recurrence class.
    /// </summary>
    public class Recurrence
    {
        // fields

        /// <summary>
        /// The date start
        /// </summary>
        private readonly DateTime dateStart;

        /// <summary>
        /// The interval
        /// </summary>
        private readonly TimeSpan interval;

        /// <summary>
        /// The date end
        /// </summary>
        private DateTime dateEnd;

        /// <summary>
        /// The excluded hours
        /// </summary>
        private HourFlags excludedHours;

        /// <summary>
        /// The excluded day of weeks
        /// </summary>
        private DayOfWeekFlags excludedDayOfWeeks;

        /// <summary>
        /// The excluded days
        /// </summary>
        private DayFlags excludedDays;

        /// <summary>
        /// The excluded months
        /// </summary>
        private MonthFlags excludedMonths;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Recurrence"/> class.
        /// </summary>
        /// <param name="dateStart">The date start</param>
        /// <param name="interval">The interval</param>
        /// <remarks>Use TimeSpan.Zero as interval for non-recurring events.</remarks>
        public Recurrence(DateTime dateStart, TimeSpan interval)
        {
            this.dateStart = dateStart;
            this.dateEnd = DateTime.MaxValue;
            this.interval = interval;

            this.excludedHours = HourFlags.None;
            this.excludedDayOfWeeks = DayOfWeekFlags.None;
            this.excludedDays = DayFlags.None;
            this.excludedMonths = MonthFlags.None;
        }

        /// <summary>
        /// Gets the date start.
        /// </summary>
        /// <value>
        /// The date start.
        /// </value>
        public DateTime DateStart
        {
            get
            {
                return this.dateStart;
            }

            //// set
            //// {
            ////    this.dateStart = value;
            //// }
        }

        /// <summary>
        /// Gets or sets the date end.
        /// </summary>
        /// <value>
        /// The date end.
        /// </value>
        public DateTime DateEnd
        {
            get
            {
                return this.dateEnd;
            }

            set
            {
                this.dateEnd = value;
            }
        }

        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public TimeSpan Interval
        {
            get
            {
                return this.interval;
            }

            //// set
            //// {
            ////    this.interval = value;
            //// }
        }

        /// <summary>
        /// Gets or sets the excluded hours.
        /// </summary>
        /// <value>
        /// The excluded hours.
        /// </value>
        public HourFlags ExcludedHours
        {
            get
            {
                return this.excludedHours;
            }

            set
            {
                this.excludedHours = value;
            }
        }

        /// <summary>
        /// Gets or sets the excluded day of weeks.
        /// </summary>
        /// <value>
        /// The excluded day of weeks.
        /// </value>
        public DayOfWeekFlags ExcludedDayOfWeeks
        {
            get
            {
                return this.excludedDayOfWeeks;
            }

            set
            {
                this.excludedDayOfWeeks = value;
            }
        }

        /// <summary>
        /// Gets or sets the excluded days.
        /// </summary>
        /// <value>
        /// The excluded days.
        /// </value>
        public DayFlags ExcludedDays
        {
            get
            {
                return this.excludedDays;
            }

            set
            {
                this.excludedDays = value;
            }
        }

        /// <summary>
        /// Gets or sets the excluded months.
        /// </summary>
        /// <value>
        /// The excluded months.
        /// </value>
        public MonthFlags ExcludedMonths
        {
            get
            {
                return this.excludedMonths;
            }

            set
            {
                this.excludedMonths = value;
            }
        }

        // methods

        /// <summary>
        /// Creates recurrence works at once.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        /// <returns>Recurrence instance</returns>
        public static Recurrence OnceAt(DateTime dateTime)
        {
            return new Recurrence(dateTime, TimeSpan.Zero);
        }

        /// <summary>
        /// Creates periodical recurrence instance.
        /// </summary>
        /// <param name="period">The period</param>
        /// <returns>Recurrence instance</returns>
        public static Recurrence Periodically(TimeSpan period)
        {
            return new Recurrence(DateTime.MinValue, period);
        }

        /// <summary>
        /// Checks the date.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        /// <returns>Is date valid or not</returns>
        public bool CheckDate(DateTime dateTime)
        {
            if (this.excludedMonths.HasFlag((MonthFlags)dateTime.Month))
            {
                return false;
            }

            if (this.excludedDayOfWeeks.HasFlag((DayOfWeekFlags)(dateTime.DayOfWeek + 1)))
            {
                return false;
            }

            if (this.excludedDays.HasFlag((DayFlags)dateTime.Day))
            {
                return false;
            }

            if (this.excludedHours.HasFlag((HourFlags)(dateTime.Hour + 1)))
            {
                return false;
            }

            if (this.dateStart != DateTime.MinValue && this.dateStart < dateTime)
            {
                return false;
            }

            if (this.dateEnd != DateTime.MaxValue && this.dateEnd > dateTime)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return HashUtils.RSHash(this.dateStart, this.dateEnd, this.interval, this.excludedHours, this.excludedDayOfWeeks, this.excludedDays, this.excludedMonths);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            Recurrence other = (Recurrence)obj;

            if (this.dateStart != other.dateStart || this.dateEnd != other.DateEnd || this.interval != other.Interval)
            {
                return false;
            }

            if (this.excludedHours != other.ExcludedHours || this.excludedDayOfWeeks != other.ExcludedDayOfWeeks || this.excludedDays != other.ExcludedDays || this.excludedMonths != other.ExcludedMonths)
            {
                return false;
            }

            return true;
        }
    }
}

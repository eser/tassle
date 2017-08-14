// --------------------------------------------------------------------------
// <copyright file="Recurrence.cs" company="-">
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
using Tassle.Helpers;

namespace Tassle.Tasks.Schedule {
    /// <summary>
    /// Recurrence class.
    /// </summary>
    public class Recurrence {
        // fields

        /// <summary>
        /// Once
        /// </summary>
        private static Recurrence s_once = new Recurrence(DateTimeOffset.MinValue, TimeSpan.Zero);

        /// <summary>
        /// The date start
        /// </summary>
        private readonly DateTimeOffset _dateStart;

        /// <summary>
        /// The interval
        /// </summary>
        private readonly TimeSpan _interval;

        /// <summary>
        /// The date end
        /// </summary>
        private DateTimeOffset _dateEnd;

        /// <summary>
        /// The excluded hours
        /// </summary>
        private HourFlags _excludedHours;

        /// <summary>
        /// The excluded day of weeks
        /// </summary>
        private DayOfWeekFlags _excludedDayOfWeeks;

        /// <summary>
        /// The excluded days
        /// </summary>
        private DayFlags _excludedDays;

        /// <summary>
        /// The excluded months
        /// </summary>
        private MonthFlags _excludedMonths;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Recurrence"/> class.
        /// </summary>
        /// <param name="dateStart">The date start</param>
        /// <param name="interval">The interval</param>
        /// <remarks>Use TimeSpan.Zero as interval for non-recurring events.</remarks>
        public Recurrence(DateTimeOffset dateStart, TimeSpan interval) {
            this._dateStart = dateStart;
            this._dateEnd = DateTimeOffset.MaxValue;
            this._interval = interval;

            this._excludedHours = HourFlags.None;
            this._excludedDayOfWeeks = DayOfWeekFlags.None;
            this._excludedDays = DayFlags.None;
            this._excludedMonths = MonthFlags.None;
        }

        // properties

        /// <summary>
        /// Gets the once.
        /// </summary>
        /// <value>
        /// The once.
        /// </value>
        public static Recurrence Once {
            get => Recurrence.s_once;
        }

        /// <summary>
        /// Gets the date start.
        /// </summary>
        /// <value>
        /// The date start.
        /// </value>
        public DateTimeOffset DateStart {
            get => this._dateStart;
        }

        /// <summary>
        /// Gets or sets the date end.
        /// </summary>
        /// <value>
        /// The date end.
        /// </value>
        public DateTimeOffset DateEnd {
            get => this._dateEnd;
            set => this._dateEnd = value;
        }

        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public TimeSpan Interval {
            get => this._interval;
        }

        /// <summary>
        /// Gets or sets the excluded hours.
        /// </summary>
        /// <value>
        /// The excluded hours.
        /// </value>
        public HourFlags ExcludedHours {
            get => this._excludedHours;
            set => this._excludedHours = value;
        }

        /// <summary>
        /// Gets or sets the excluded day of weeks.
        /// </summary>
        /// <value>
        /// The excluded day of weeks.
        /// </value>
        public DayOfWeekFlags ExcludedDayOfWeeks {
            get => this._excludedDayOfWeeks;
            set => this._excludedDayOfWeeks = value;
        }

        /// <summary>
        /// Gets or sets the excluded days.
        /// </summary>
        /// <value>
        /// The excluded days.
        /// </value>
        public DayFlags ExcludedDays {
            get => this._excludedDays;
            set => this._excludedDays = value;
        }

        /// <summary>
        /// Gets or sets the excluded months.
        /// </summary>
        /// <value>
        /// The excluded months.
        /// </value>
        public MonthFlags ExcludedMonths {
            get => this._excludedMonths;
            set => this._excludedMonths = value;
        }

        // methods

        /// <summary>
        /// Creates recurrence works at once on specified time.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        /// <returns>Recurrence instance</returns>
        public static Recurrence OnceAt(DateTimeOffset dateTime) {
            return new Recurrence(dateTime, TimeSpan.Zero);
        }

        /// <summary>
        /// Creates periodical recurrence instance.
        /// </summary>
        /// <param name="period">The period</param>
        /// <returns>Recurrence instance</returns>
        public static Recurrence Periodically(TimeSpan period) {
            return new Recurrence(DateTimeOffset.MinValue, period);
        }

        /// <summary>
        /// Checks the date.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        /// <returns>Is date valid or not</returns>
        public bool CheckDate(DateTimeOffset dateTime) {
            if (this._excludedMonths.HasFlag((MonthFlags)dateTime.Month)) {
                return false;
            }

            if (this._excludedDayOfWeeks.HasFlag((DayOfWeekFlags)(dateTime.DayOfWeek + 1))) {
                return false;
            }

            if (this._excludedDays.HasFlag((DayFlags)dateTime.Day)) {
                return false;
            }

            if (this._excludedHours.HasFlag((HourFlags)(dateTime.Hour + 1))) {
                return false;
            }

            if (this._dateStart != DateTimeOffset.MinValue && this._dateStart > dateTime) {
                return false;
            }

            if (this._dateEnd != DateTimeOffset.MaxValue && this._dateEnd < dateTime) {
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
        public override int GetHashCode() {
            return HashHelpers.RSHash(this._dateStart, this._dateEnd, this._interval, this._excludedHours, this._excludedDayOfWeeks, this._excludedDays, this._excludedMonths);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj) {
            var other = (Recurrence)obj;

            if (this._dateStart != other._dateStart || this._dateEnd != other._dateEnd || this._interval != other._interval) {
                return false;
            }

            if (this._excludedHours != other._excludedHours || this._excludedDayOfWeeks != other._excludedDayOfWeeks || this._excludedDays != other._excludedDays || this._excludedMonths != other._excludedMonths) {
                return false;
            }

            return true;
        }
    }
}

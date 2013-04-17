// -----------------------------------------------------------------------
// <copyright file="DateRange.cs" company="-">
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
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// DateRange class.
    /// </summary>
    public class DateRange : ICloneable
    {
        // fields

        /// <summary>
        /// The start
        /// </summary>
        private readonly DateTime start;

        /// <summary>
        /// The end
        /// </summary>
        private readonly DateTime end;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange" /> class.
        /// </summary>
        /// <param name="start">The start</param>
        /// <param name="end">The end</param>
        public DateRange(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }

        // properties

        /// <summary>
        /// Gets the start.
        /// </summary>
        public DateTime Start
        {
            get
            {
                return this.start;
            }
        }

        /// <summary>
        /// Gets the end.
        /// </summary>
        public DateTime End
        {
            get
            {
                return this.end;
            }
        }

        // methods

        /// <summary>
        /// Creates the week range.
        /// </summary>
        /// <param name="current">The current</param>
        /// <returns>Set of days</returns>
        public static DateRange CreateWeekRange(DateTime current)
        {
            return DateRange.CreateWeekRange(current, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Creates the week range.
        /// </summary>
        /// <param name="current">The current</param>
        /// <param name="cultureInfo">The culture info</param>
        /// <returns>Set of days</returns>
        public static DateRange CreateWeekRange(DateTime current, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int diff = (7 + ((int)current.DayOfWeek - (int)firstDay)) % 7;

            DateTime start = new DateTime(current.Year, current.Month, current.Day - diff, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime end = start.Add(new TimeSpan(7, 0, 0, 0, -1));

            return new DateRange(start, end);
        }

        /// <summary>
        /// Creates the month range.
        /// </summary>
        /// <param name="current">The current</param>
        /// <returns>Set of days</returns>
        public static DateRange CreateMonthRange(DateTime current)
        {
            DateTime start = new DateTime(current.Year, current.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime end = start.AddMonths(1).AddMilliseconds(-1);

            return new DateRange(start, end);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
// --------------------------------------------------------------------------
// <copyright file="DateRange.cs" company="-">
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
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;

namespace Tasslehoff.DataStructures
{
    /// <summary>
    /// DateRange class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DateRange : ICloneable
    {
        // fields

        /// <summary>
        /// The start
        /// </summary>
        [DataMember(Name = "Start")]
        private readonly DateTimeOffset start;

        /// <summary>
        /// The end
        /// </summary>
        [DataMember(Name = "End")]
        private readonly DateTimeOffset end;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange" /> class.
        /// </summary>
        /// <param name="start">The start</param>
        /// <param name="end">The end</param>
        public DateRange(DateTimeOffset start, DateTimeOffset end)
        {
            this.start = start;
            this.end = end;
        }


        // properties

        /// <summary>
        /// Gets the start.
        /// </summary>
        [IgnoreDataMember]
        public DateTimeOffset Start
        {
            get
            {
                return this.start;
            }
        }

        /// <summary>
        /// Gets the end.
        /// </summary>
        [IgnoreDataMember]
        public DateTimeOffset End
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
        public static DateRange CreateWeekRange(DateTimeOffset current)
        {
            return DateRange.CreateWeekRange(current, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Creates the week range.
        /// </summary>
        /// <param name="current">The current</param>
        /// <param name="cultureInfo">The culture info</param>
        /// <returns>Set of days</returns>
        public static DateRange CreateWeekRange(DateTimeOffset current, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int diff = (7 + ((int)current.DayOfWeek - (int)firstDay)) % 7;

            DateTimeOffset start = new DateTimeOffset(current.Year, current.Month, current.Day - diff, 0, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset end = start.Add(new TimeSpan(7, 0, 0, 0, -1));

            return new DateRange(start, end);
        }

        /// <summary>
        /// Creates the month range.
        /// </summary>
        /// <param name="current">The current</param>
        /// <returns>Set of days</returns>
        public static DateRange CreateMonthRange(DateTimeOffset current)
        {
            DateTimeOffset start = new DateTimeOffset(current.Year, current.Month, 1, 0, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset end = start.AddMonths(1).AddMilliseconds(-1);

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
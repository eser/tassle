// -----------------------------------------------------------------------
// <copyright file="DateTimeUtils.cs" company="-">
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

namespace Tasslehoff.Library.Utils
{
    using System;
    using System.Globalization;

    /// <summary>
    /// DateTimeUtils class.
    /// </summary>
    public static class DateTimeUtils
    {
        // methods

        /// <summary>
        /// Converts to Unix Timestamp.
        /// </summary>
        /// <param name="datetime">DateTime to be converted</param>
        /// <returns>Unix Timestamp</returns>
        public static double UnixTimestamp(DateTime? datetime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (datetime.GetValueOrDefault(DateTime.UtcNow) - epoch).TotalSeconds;
        }

        /// <summary>
        /// Converts from Unix Timestamp.
        /// </summary>
        /// <param name="secondsPassed">The seconds passed</param>
        /// <returns>DateTime Object</returns>
        public static DateTime FromUnixtime(double secondsPassed)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return epoch.AddSeconds(secondsPassed);
        }

        /// <summary>
        /// Converts to ISO8601 formatted datetime.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>ISO8601 formatted datetime</returns>
        public static string ISO8601(DateTime datetime)
        {
            return datetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz");
        }

        /// <summary>
        /// Converts from ISO8601 formatted datetime.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>DateTime Object</returns>
        public static DateTime FromISO8601(string datetime)
        {
            return DateTime.ParseExact(
                datetime,
                "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz", // new string[] { "s", "u" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None
            );
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <param name="timeZoneId">The time zone id</param>
        /// <returns>The time zone</returns>
        public static TimeZoneInfo GetTimeZone(string timeZoneId)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        /// <summary>
        /// To the specific time zone.
        /// </summary>
        /// <param name="timeZoneId">The time zone id</param>
        /// <param name="dateTime">The date time</param>
        /// <returns>The converted DateTime object</returns>
        public static DateTime ToSpecificTimeZone(string timeZoneId, DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
            {
                return dateTime;
            }

            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, DateTimeUtils.GetTimeZone(timeZoneId));
        }

        /// <summary>
        /// To the universal.
        /// </summary>
        /// <param name="timeZoneId">The time zone id</param>
        /// <param name="dateTime">The date time</param>
        /// <returns>The converted DateTime object</returns>
        public static DateTime ToUniversal(string timeZoneId, DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
            {
                return dateTime;
            }

            return TimeZoneInfo.ConvertTimeToUtc(dateTime, DateTimeUtils.GetTimeZone(timeZoneId));
        }
    }
}

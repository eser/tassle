// --------------------------------------------------------------------------
// <copyright file="Logger.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
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
using System.Threading;
using System.Threading.Tasks;

namespace Tasslehoff.Logging
{
    /// <summary>
    /// A Logger instance.
    /// </summary>
    [Serializable]
    public class Logger
    {
        // fields

        /// <summary>
        /// The category
        /// </summary>
        private string category;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger(string category)
        {
            this.category = category;
        }

        // properties

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        // methods

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="severity">The severity level</param>
        /// <param name="message">The text</param>
        /// <param name="exception">The exception</param>
        /// <param name="date">The date</param>
        /// <param name="application">The application</param>
        /// <param name="category">The category</param>
        /// <param name="flags">The flags</param>
        /// <returns>Is written or not</returns>
        public bool Write(LogLevel severity, string message, Exception exception = null, LogFlags? flags = LogFlags.None, DateTimeOffset? date = null, string category = null, string threadFrom = null, string application = null)
        {
            var context = LoggerContext.Current;

            return context.Write(new LogEntry()
            {
                Application = application ?? context.Application,
                ThreadFrom = threadFrom ?? Thread.CurrentThread.Name,
                Category = category ?? this.category,
                Date = date ?? DateTimeOffset.UtcNow,
                Exception = exception,
                Flags = flags ?? LogFlags.None,
                Message = message,
                Severity = severity
            });
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="entry">The log entry</param>
        /// <returns>Is written or not</returns>
        public bool Write(LogEntry entry)
        {
            var context = LoggerContext.Current;

            return context.Write(entry);
        }

        /// <summary>
        /// Writes a log entry asyncronously.
        /// </summary>
        /// <param name="severity">The severity level</param>
        /// <param name="message">The text</param>
        /// <param name="exception">The exception</param>
        /// <param name="date">The date</param>
        /// <param name="application">The application</param>
        /// <param name="category">The category</param>
        /// <param name="flags">The flags</param>
        /// <returns>Is written or not</returns>
        public Task<bool> WriteAsync(LogLevel severity, string message, Exception exception = null, LogFlags? flags = LogFlags.None, DateTimeOffset? date = null, string category = null, string threadFrom = null, string application = null)
        {
            var context = LoggerContext.Current;

            return context.WriteAsync(new LogEntry()
            {
                Application = application ?? context.Application,
                ThreadFrom = threadFrom ?? Thread.CurrentThread.Name,
                Category = category ?? this.category,
                Date = date ?? DateTimeOffset.UtcNow,
                Exception = exception,
                Flags = flags ?? LogFlags.None,
                Message = message,
                Severity = severity
            });
        }

        /// <summary>
        /// Writes a log entry asyncronously.
        /// </summary>
        /// <param name="entry">The log entry</param>
        /// <returns>Is written or not</returns>
        public Task<bool> WriteAsync(LogEntry entry)
        {
            var context = LoggerContext.Current;

            return context.WriteAsync(entry);
        }
    }
}

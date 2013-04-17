// -----------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="-">
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

namespace Tasslehoff.Library.Logger
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// A log entry.
    /// </summary>
    public class LogEntry
    {
        // fields

        /// <summary>
        /// The thread from
        /// </summary>
        private readonly string threadFrom;

        /// <summary>
        /// The application name
        /// </summary>
        private string application;

        /// <summary>
        /// The category
        /// </summary>
        private string category;

        /// <summary>
        /// The date
        /// </summary>
        private DateTime date;

        /// <summary>
        /// The message
        /// </summary>
        private string message;

        /// <summary>
        /// The is direct
        /// </summary>
        private bool isDirect;

        /// <summary>
        /// The level
        /// </summary>
        private LogLevel level;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        public LogEntry()
        {
            this.threadFrom = Thread.CurrentThread.Name;
        }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        /// <value>
        /// The application name.
        /// </value>
        public string Application
        {
            get
            {
                return this.application;
            }

            set
            {
                this.application = value;
            }
        }

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

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.date = value;
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is direct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is direct; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirect
        {
            get
            {
                return this.isDirect;
            }

            set
            {
                this.isDirect = value;
            }
        }

        /// <summary>
        /// Gets the thread from.
        /// </summary>
        /// <value>
        /// The thread from.
        /// </value>
        public string ThreadFrom
        {
            get
            {
                return this.threadFrom;
            }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public LogLevel Level
        {
            get
            {
                return this.level;
            }

            set
            {
                this.level = value;
            }
        }

        // methods

        /// <summary>
        /// Applies the format.
        /// </summary>
        /// <param name="input">The format</param>
        /// <returns>Formatted message</returns>
        public string ApplyFormat(string input)
        {
            return Regex.Replace(
                input,
                @"{([^:}]*)(:[^}]*)?}",
                (match) =>
                {
                    string format;
                    if (match.Groups.Count >= 2)
                    {
                        format = match.Groups[2].Value.TrimStart(':');
                    }
                    else
                    {
                        format = null;
                    }
                
                    switch (match.Groups[1].Value)
                    {
                    case "application":
                        return this.application ?? string.Empty;
                    case "category":
                        return this.category ?? string.Empty;
                    case "level":
                        return this.level.ToString();
                    case "date":
                    case "timestamp":
                        if (format != null)
                        {
                            return this.date.ToString(format, CultureInfo.InvariantCulture);
                        }

                        return this.date.ToString(CultureInfo.InvariantCulture);
                    case "message":
                        return this.message;
                    default:
                        return match.Groups[0].Value;
                    }
                });
        }
    }
}

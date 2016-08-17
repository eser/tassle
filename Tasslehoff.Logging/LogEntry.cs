// --------------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="-">
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

namespace Tasslehoff.Logging
{
    /// <summary>
    /// A log entry.
    /// </summary>
    public class LogEntry
    {
        // fields

        /// <summary>
        /// The application name
        /// </summary>
        private string application;

        /// <summary>
        /// The thread from
        /// </summary>
        private string threadFrom;

        /// <summary>
        /// The category
        /// </summary>
        private string category;

        /// <summary>
        /// The date
        /// </summary>
        private DateTimeOffset date;

        /// <summary>
        /// The message
        /// </summary>
        private string message;

        /// <summary>
        /// The exception
        /// </summary>
        private Exception exception;

        /// <summary>
        /// The log flags
        /// </summary>
        private LogFlags flags;

        /// <summary>
        /// The severity
        /// </summary>
        private LogLevel severity;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        public LogEntry()
        {
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
            set
            {
                this.threadFrom = value;
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
        public DateTimeOffset Date
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
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception
        {
            get
            {
                return this.exception;
            }
            set
            {
                this.exception = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating log flags.
        /// </summary>
        /// <value>
        /// The flags.
        /// </value>
        public LogFlags Flags
        {
            get
            {
                return this.flags;
            }
            set
            {
                this.flags = value;
            }
        }

        /// <summary>
        /// Gets or sets the severity level.
        /// </summary>
        /// <value>
        /// The severity level.
        /// </value>
        public LogLevel Severity
        {
            get
            {
                return this.severity;
            }
            set
            {
                this.severity = value;
            }
        }
    }
}

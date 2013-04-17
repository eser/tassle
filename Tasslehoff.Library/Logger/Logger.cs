// -----------------------------------------------------------------------
// <copyright file="Logger.cs" company="-">
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
    using System.Text;

    /// <summary>
    /// Logger class.
    /// </summary>
    public abstract class Logger : IDisposable
    {
        // fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static Logger context = null;

        /// <summary>
        /// The sync
        /// </summary>
        private readonly object sync;

        /// <summary>
        /// The application
        /// </summary>
        private string application;

        /// <summary>
        /// The disabled
        /// </summary>
        private bool disabled;

        /// <summary>
        /// The minimum level
        /// </summary>
        private LogLevel minimumLevel;

        /// <summary>
        /// The custom format
        /// </summary>
        private string customFormat;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        protected Logger()
        {
            // singleton pattern
            if (Logger.context == null)
            {
                Logger.context = this;
            }

            this.sync = new object();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Logger"/> class.
        /// </summary>
        ~Logger()
        {
            this.Dispose(false);
        }

        // attributes

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public static Logger Context
        {
            get
            {
                return Logger.context;
            }
        }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
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
        /// Gets or sets a value indicating whether this <see cref="Logger"/> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        public bool Disabled
        {
            get
            {
                return this.disabled;
            }
            
            set
            {
                this.disabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum level.
        /// </summary>
        /// <value>
        /// The minimum level.
        /// </value>
        public LogLevel MinimumLevel
        {
            get
            {
                return this.minimumLevel;
            }

            set
            {
                this.minimumLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom format.
        /// </summary>
        /// <value>
        /// The custom format.
        /// </value>
        public string CustomFormat
        {
            get
            {
                return this.customFormat;
            }

            set
            {
                this.customFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Logger"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get
            {
                return this.disposed;
            }

            protected set
            {
                this.disposed = value;
            }
        }

        // methods

        /// <summary>
        /// Writes the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry</param>
        /// <returns>Is written or not</returns>
        public bool Write(LogEntry logEntry)
        {
            lock (this.sync)
            {
                if (!this.disabled && this.minimumLevel > logEntry.Level)
                {
                    return false;
                }
                
                return this.WriteLog(logEntry);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="logEntry">The log entry</param>
        /// <returns>Is written or not</returns>
        protected abstract bool WriteLog(LogEntry logEntry);

        /// <summary>
        /// Formats the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry</param>
        /// <returns>Formatted string</returns>
        protected virtual string Format(LogEntry logEntry)
        {
            if (this.customFormat != null)
            {
                return logEntry.ApplyFormat(this.customFormat);
            }

            StringBuilder message = new StringBuilder();

            if (logEntry.Application != null)
            {
                string appName = logEntry.Application;
                if (!string.IsNullOrEmpty(logEntry.ThreadFrom))
                {
                    appName += ":" + logEntry.ThreadFrom;
                }

                message.Append(string.Format("[{0,-12}] ", appName));
            }

            if (!logEntry.IsDirect)
            {
                if (!string.IsNullOrEmpty(logEntry.Category))
                {
                    message.Append(logEntry.Category.PadRight(25));
                }

                message.Append("|");
                message.Append(logEntry.Level.ToString("G").ToUpperInvariant().PadRight(8));
                message.Append("|");
                message.Append(logEntry.Date.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).PadRight(25));
                message.Append("|");
            }

            string newNewLine = Environment.NewLine + "-- ";
            string text = logEntry.Message.Replace(Environment.NewLine, newNewLine);

            message.Append(text);

            return message.ToString();
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            // singleton pattern
            if (Logger.context == this)
            {
                Logger.context = null;
            }

            if (disposing)
            {
                this.OnDispose();
            }

            this.disposed = true;
        }
    }
}

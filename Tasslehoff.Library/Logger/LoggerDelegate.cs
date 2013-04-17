// -----------------------------------------------------------------------
// <copyright file="LoggerDelegate.cs" company="-">
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
    using System.Text;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// A delegate for the Logger instance.
    /// </summary>
    public class LoggerDelegate : IDisposable
    {
        // fields

        /// <summary>
        /// The application
        /// </summary>
        private string application;

        /// <summary>
        /// The category
        /// </summary>
        private string category;

        /// <summary>
        /// The is direct
        /// </summary>
        private bool isDirect;

        /// <summary>
        /// The assigned logger
        /// </summary>
        private Logger assignedLogger = null;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerDelegate"/> class.
        /// </summary>
        public LoggerDelegate()
        {
            this.assignedLogger = Logger.Context;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LoggerDelegate"/> class.
        /// </summary>
        ~LoggerDelegate()
        {
            this.Dispose(false);
        }

        // attributes

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
        /// Gets the assigned logger.
        /// </summary>
        /// <value>
        /// The assigned logger.
        /// </value>
        public Logger AssignedLogger
        {
            get
            {
                return this.assignedLogger;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LoggerDelegate"/> is disposed.
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
        /// Assigns the logger.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="category">The category</param>
        /// <param name="application">The application</param>
        /// <param name="isDirect">if set to <c>true</c> [is direct]</param>
        public void AssignLogger(Logger logger, string category, string application = null, bool isDirect = false)
        {
            this.assignedLogger = logger;
            this.category = category;
            this.application = application;
            this.isDirect = isDirect;
        }

        /// <summary>
        /// Gets the specified level.
        /// </summary>
        /// <param name="level">The level</param>
        /// <param name="text">The text</param>
        /// <param name="exception">The exception</param>
        /// <param name="date">The date</param>
        /// <param name="application">The application</param>
        /// <param name="category">The category</param>
        /// <param name="isDirect">The is direct</param>
        /// <returns>LogEntry instance</returns>
        public LogEntry Get(LogLevel level, string text, Exception exception = null, DateTime? date = null, string application = null, string category = null, bool? isDirect = null)
        {
            if (exception != null)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Exception: ");
                message.AppendLine(exception.GetType().Name);
                
                message.Append("Detail: ");
                message.AppendLine(text);
                
                message.Append("Message: ");
                message.AppendLine(exception.Message);
                
                message.Append("Source: ");
                message.AppendLine(exception.Source);
                
                message.AppendLine("=== Stack Trace ===");
                message.AppendLine(exception.StackTrace);
                
                text = message.ToString();
            }
            
            return new LogEntry()
            {
                Level = level,
                Application = application ?? this.application,
                Category = category ?? this.category,
                Message = text,
                IsDirect = isDirect ?? this.isDirect,
                Date = date.GetValueOrDefault(DateTime.UtcNow)
            };
        }

        /// <summary>
        /// Writes the specified level.
        /// </summary>
        /// <param name="level">The level</param>
        /// <param name="text">The text</param>
        /// <param name="exception">The exception</param>
        /// <param name="date">The date</param>
        /// <param name="application">The application</param>
        /// <param name="category">The category</param>
        /// <param name="isDirect">The is direct</param>
        /// <returns>Is written or not</returns>
        public bool Write(LogLevel level, string text, Exception exception = null, DateTime? date = null, string application = null, string category = null, bool? isDirect = null)
        {
            if (this.assignedLogger == null)
            {
                return false;
            }
            
            LogEntry logEntry = this.Get(level, text, exception, date, application, category, isDirect);
            return this.assignedLogger.Write(logEntry);
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            
            if (disposing)
            {
                this.OnDispose();
            }
            
            this.disposed = true;
        }

        // abstract methods

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        protected virtual void OnDispose()
        {
            VariableUtils.CheckAndDispose(this.assignedLogger);
            this.assignedLogger = null;
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="LoggerContext.cs" company="-">
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace Tasslehoff.Logging
{
    /// <summary>
    /// LoggerContext class.
    /// </summary>
    public class LoggerContext : IDisposable
    {
        // fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static LoggerContext current = null;

        /// <summary>
        /// The context lock
        /// </summary>
        private static readonly object contextLock = new object();

        /// <summary>
        /// The formatter
        /// </summary>
        private LogFormatter formatter;

        /// <summary>
        /// The application
        /// </summary>
        private string application;

        /// <summary>
        /// The disabled
        /// </summary>
        private bool disabled;

        /// <summary>
        /// The minimum severity level
        /// </summary>
        private LogLevel minimumSeverity;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // events
        public event EventHandler<LogEntry> LogEntryPopped;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerContext"/> class.
        /// </summary>
        internal LoggerContext()
        {
            // var assemblyName = Assembly.GetEntryAssembly().GetName();
            var stackTrace = new StackTrace();
            
            var stackTraceFrames = stackTrace.GetFrames();

            var lastStackTraceFrame = stackTraceFrames[stackTraceFrames.Length - 1];

            var assembly = lastStackTraceFrame.GetMethod().Module.Assembly;

            this.application = assembly.GetName().Name;

            this.formatter = new LogFormatter();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LoggerContext"/> class.
        /// </summary>
        ~LoggerContext()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public static LoggerContext Current
        {
            get
            {
                if (LoggerContext.current == null)
                {
                    lock (LoggerContext.contextLock)
                    {
                        if (LoggerContext.current == null)
                        {
                            LoggerContext.current = new LoggerContext();
                        }
                    }
                }

                return LoggerContext.current;
            }
        }

        /// <summary>
        /// Gets or sets the formatter.
        /// </summary>
        /// <value>
        /// The formatter.
        /// </value>
        public LogFormatter Formatter
        {
            get
            {
                return this.formatter;
            }
            set
            {
                this.formatter = value;
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
        /// Gets or sets a value indicating whether this <see cref="LoggerContext"/> is disabled.
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
        /// Gets or sets the minimum severity level.
        /// </summary>
        /// <value>
        /// The minimum severity level.
        /// </value>
        public LogLevel MinimumSeverity
        {
            get
            {
                return this.minimumSeverity;
            }
            set
            {
                this.minimumSeverity = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LoggerContext"/> is disposed.
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
        /// <param name="entry">The log entry</param>
        /// <returns>Is written or not</returns>
        public bool Write(LogEntry entry)
        {
            if (!this.disabled && this.LogEntryPopped != null)
            {
                if (entry.Severity >= this.minimumSeverity)
                {
                    this.LogEntryPopped(this, entry);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Writes the specified log entry.
        /// </summary>
        /// <param name="entry">The log entry</param>
        /// <returns>Is written or not</returns>
        public Task<bool> WriteAsync(LogEntry entry)
        {
            return Task.Factory.StartNew<bool>(
                () => this.Write(entry));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }


        // methods

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void OnDispose(bool releaseManagedResources)
        {
            // singleton pattern
            if (LoggerContext.current == this)
            {
                LoggerContext.current = null;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        protected void Dispose(bool releaseManagedResources)
        {
            if (this.disposed)
            {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this.disposed = true;
        }
    }
}

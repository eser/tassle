//
//  LoggerDelegate.cs
//
//  Author:
//       larukedi <eser@sent.com>
//
//  Copyright (c) 2013 larukedi
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Text;
using Tasslehoff.Library.Utils;

namespace Tasslehoff.Library.Logger
{
    public class LoggerDelegate : IDisposable
    {
        // fields
        private string application;
        private string category;
        private bool isDirect;
        private Logger assignedLogger;
        private bool disposed;

        // constructors
        public LoggerDelegate()
        {
            this.assignedLogger = Logger.Context;
        }

        ~LoggerDelegate() {
            this.Dispose(false);
        }

        // attributes
        public string Application {
            get {
                return this.application;
            }
            set {
                this.application = value;
            }
        }

        public string Category {
            get {
                return this.category;
            }
            set {
                this.category = value;
            }
        }

        public bool IsDirect {
            get {
                return this.isDirect;
            }
            set {
                this.isDirect = value;
            }
        }

        public Logger AssignedLogger {
            get {
                return this.assignedLogger;
            }
        }

        public bool Disposed {
            get {
                return this.disposed;
            }
            protected set {
                this.disposed = value;
            }
        }

        // abstract methods
        protected virtual void OnDispose() {
            VariableUtils.CheckAndDispose(this.assignedLogger);
        }

        // methods
        public void AssignLogger(Logger logger, string category, string application = null, bool isDirect = false) {
            this.assignedLogger = logger;
            this.category = category;
            this.application = application;
            this.isDirect = isDirect;
        }

        public LogEntry Get(LogLevel level, string text, Exception exception = null, DateTime? date = null, string application = null, string category = null, bool? isDirect = null) {
            if(exception != null) {
                StringBuilder _string = new StringBuilder();
                _string.Append("Exception: ");
                _string.AppendLine(exception.GetType().Name);
                
                _string.Append("Detail: ");
                _string.AppendLine(text);
                
                _string.Append("Message: ");
                _string.AppendLine(exception.Message);
                
                _string.Append("Source: ");
                _string.AppendLine(exception.Source);
                
                _string.AppendLine("=== Stack Trace ===");
                _string.AppendLine(exception.StackTrace);
                
                text = _string.ToString();
            }
            
            return new LogEntry() {
                Level = level,
                Application = application ?? this.application,
                Category = category ?? this.category,
                Message = text,
                IsDirect = isDirect ?? this.isDirect,
                Date = date.GetValueOrDefault(DateTime.UtcNow)
            };
        }

        public bool Write(LogLevel level, string text, Exception exception = null, DateTime? date = null, string application = null, string category = null, bool? isDirect = null) {
            if(this.assignedLogger == null) {
                return false;
            }
            
            LogEntry _logEntry = this.Get(level, text, exception, date, application, category, isDirect);
            return this.assignedLogger.Write(_logEntry);
        }

        // implementations
        protected void Dispose(bool disposing) {
            if(this.disposed) {
                return;
            }
            
            if(disposing) {
                this.OnDispose();
            }
            
            this.disposed = true;
        }
        
        public void Dispose() {
            this.Dispose(true);
            
            GC.SuppressFinalize(this);
        }
    }
}


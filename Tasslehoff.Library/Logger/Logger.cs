//
//  Logger.cs
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
using System.Globalization;
using System.Text;

namespace Tasslehoff.Library.Logger
{
    public abstract class Logger : IDisposable
    {
        // singleton pattern
        private static Logger context = null;

        public static Logger Context {
            get {
                return Logger.context;
            }
        }

        // fields
        protected string application;
        protected bool disabled;
        protected LogLevel minimumLevel;
        protected string customFormat;
        protected readonly object sync;
        private bool disposed;

        // constructors
        protected Logger ()
        {
            // singleton pattern
            if(Logger.context == null) {
                Logger.context = this;
            }

            this.sync = new object();
        }

        ~Logger() {
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

        public bool Disabled {
            get {
                return this.disabled;
            }
            set {
                this.disabled = value;
            }
        }

        public LogLevel MinimumLevel {
            get {
                return this.minimumLevel;
            }
            set {
                this.minimumLevel = value;
            }
        }

        public string CustomFormat {
            get {
                return this.customFormat;
            }
            set {
                this.customFormat = value;
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
        protected abstract bool WriteLog(LogEntry logEntry);

        protected virtual string Format(LogEntry logEntry) {
            if(this.customFormat != null) {
                return logEntry.ApplyFormat(this.customFormat);
            }

            StringBuilder _message = new StringBuilder();
            
            if(logEntry.Application != null) {
                string _appName = logEntry.Application;
                if(!string.IsNullOrEmpty(logEntry.ThreadFrom)) {
                    _appName += ":" + logEntry.ThreadFrom;
                }
                
                _message.Append(string.Format("[{0,-12}] ", _appName));
            }
            
            if(!logEntry.IsDirect) {
                if(!string.IsNullOrEmpty(logEntry.Category)) {
                    _message.Append(logEntry.Category.PadRight(25));
                }
                
                _message.Append("|");
                _message.Append(logEntry.Level.ToString("G").ToUpperInvariant().PadRight(8));
                _message.Append("|");
                _message.Append(logEntry.Date.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).PadRight(25));
                _message.Append("|");
            }
            
            string _newNewLine = Environment.NewLine + "-- ";
            string _text = logEntry.Message.Replace(Environment.NewLine, _newNewLine);
            
            _message.Append(_text);
            
            return _message.ToString();
        }

        protected virtual void OnDispose() {

        }

        // methods
        public bool Write(LogEntry logEntry) {
            lock(this.sync) {
                if(!this.disabled && this.minimumLevel > logEntry.Level) {
                    return false;
                }
                
                return this.WriteLog(logEntry);
            }
        }

        // implementations
        protected void Dispose(bool disposing) {
            if(this.disposed) {
                return;
            }

            // singleton pattern
            if(Logger.context == this) {
                Logger.context = null;
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


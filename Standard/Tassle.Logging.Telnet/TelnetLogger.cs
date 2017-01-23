// --------------------------------------------------------------------------
// <copyright file="TelnetLogger.cs" company="-">
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

using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using Tassle.Telnet;

namespace Tassle.Logging.Telnet {
    public class TelnetLogger : ILogger {
        // fields

        // Writing to console is not an atomic operation in the current implementation and since multiple logger
        // instances are created with a different name. Also since Console is global, using a static lock is fine.
        private static readonly object s_lock = new object();
        private static readonly string s_loglevelPadding = ": ";
        private static readonly string s_messagePadding;
        private static readonly string s_newLineWithMessagePadding;

        [ThreadStatic]
        private static StringBuilder s_logBuilder;

        private Func<string, LogLevel, bool> _filter;

        private bool _includeScopes;

        private string _name;

        private TelnetServerInterface _telnetServer;

        // constructors

        static TelnetLogger() {
            var logLevelString = TelnetLogger.GetLogLevelString(LogLevel.Information);

            TelnetLogger.s_messagePadding = new string(' ', logLevelString.Length + TelnetLogger.s_loglevelPadding.Length);
            TelnetLogger.s_newLineWithMessagePadding = Environment.NewLine + TelnetLogger.s_messagePadding;
        }

        public TelnetLogger(string name, TelnetServerInterface telnetServer, Func<string, LogLevel, bool> filter, bool includeScopes) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            this._name = name;
            this._filter = filter ?? ((category, logLevel) => true);
            this._includeScopes = includeScopes;
            this._telnetServer = telnetServer;
        }

        // properties

        public Func<string, LogLevel, bool> Filter {
            get => this._filter;
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                this._filter = value;
            }
        }

        public bool IncludeScopes {
            get => this._includeScopes;
            set => this._includeScopes = value;
        }

        public string Name {
            get => this._name;
        }

        // methods

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (!this.IsEnabled(logLevel)) {
                return;
            }

            if (formatter == null) {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null) {
                this.WriteMessage(logLevel, this._name, eventId.Id, message, exception);
            }
        }

        public virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception) {
            var logBuilder = TelnetLogger.s_logBuilder;
            TelnetLogger.s_logBuilder = null;

            if (logBuilder == null) {
                logBuilder = new StringBuilder();
            }

            var logLevelString = string.Empty;

            // Example:
            // INFO: ConsoleApp.Program[10]
            //       Request received
            if (!string.IsNullOrEmpty(message)) {
                logLevelString = TelnetLogger.GetLogLevelString(logLevel);
                // category and event id
                logBuilder.Append(TelnetLogger.s_loglevelPadding);
                logBuilder.Append(logName);
                logBuilder.Append("[");
                logBuilder.Append(eventId);
                logBuilder.AppendLine("]");
                // scope information
                if (this._includeScopes) {
                    this.GetScopeInformation(logBuilder);
                }
                // message
                logBuilder.Append(TelnetLogger.s_messagePadding);
                var len = logBuilder.Length;
                logBuilder.AppendLine(message);
                logBuilder.Replace(Environment.NewLine, TelnetLogger.s_newLineWithMessagePadding, len, message.Length);
            }

            // Example:
            // System.InvalidOperationException
            //    at Namespace.Class.Function() in File:line X
            if (exception != null) {
                // exception message
                logBuilder.AppendLine(exception.ToString());
            }

            if (logBuilder.Length > 0) {
                var logMessage = logBuilder.ToString();
                lock (TelnetLogger.s_lock) {
                    if (!string.IsNullOrEmpty(logLevelString)) {
                        // log level string
                        this._telnetServer.BroadcastMessage(logLevelString);
                    }

                    // use default colors from here on
                    this._telnetServer.BroadcastMessage(logMessage);

                    // In case of AnsiLogConsole, the messages are not yet written to the console,
                    // this would flush them instead.
                    // Console.Flush();
                }
            }

            logBuilder.Clear();
            if (logBuilder.Capacity > 1024) {
                logBuilder.Capacity = 1024;
            }

            TelnetLogger.s_logBuilder = logBuilder;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return this._filter(this._name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state) {
            if (state == null) {
                throw new ArgumentNullException(nameof(state));
            }

            return TelnetLogScope.Push(this._name, state);
        }

        private static string GetLogLevelString(LogLevel logLevel) {
            switch (logLevel) {
                case LogLevel.Trace:
                    return "trce";
                case LogLevel.Debug:
                    return "dbug";
                case LogLevel.Information:
                    return "info";
                case LogLevel.Warning:
                    return "warn";
                case LogLevel.Error:
                    return "fail";
                case LogLevel.Critical:
                    return "crit";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        private void GetScopeInformation(StringBuilder builder) {
            var current = TelnetLogScope.Current;
            var scopeLog = string.Empty;
            var length = builder.Length;

            while (current != null) {
                if (length == builder.Length) {
                    scopeLog = $"=> {current}";
                }
                else {
                    scopeLog = $"=> {current} ";
                }

                builder.Insert(length, scopeLog);
                current = current.Parent;
            }

            if (builder.Length > length) {
                builder.Insert(length, TelnetLogger.s_messagePadding);
                builder.AppendLine();
            }
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="TelnetLogger.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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
        private static readonly object lockObject = new object();
        private static readonly string loglevelPadding = ": ";
        private static readonly string messagePadding;
        private static readonly string newLineWithMessagePadding;

        [ThreadStatic]
        private static StringBuilder logBuilder;

        private Func<string, LogLevel, bool> filter;

        private bool includeScopes;

        private string name;

        private ITelnetServer telnetServer;

        // constructors

        static TelnetLogger() {
            var logLevelString = TelnetLogger.GetLogLevelString(LogLevel.Information);

            TelnetLogger.messagePadding = new string(' ', logLevelString.Length + TelnetLogger.loglevelPadding.Length);
            TelnetLogger.newLineWithMessagePadding = Environment.NewLine + TelnetLogger.messagePadding;
        }

        public TelnetLogger(string name, ITelnetServer telnetServer, Func<string, LogLevel, bool> filter, bool includeScopes) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            this.filter = filter ?? ((category, logLevel) => true);
            this.includeScopes = includeScopes;
            this.telnetServer = telnetServer;
        }

        // properties

        public Func<string, LogLevel, bool> Filter {
            get => this.filter;
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                this.filter = value;
            }
        }

        public bool IncludeScopes {
            get => this.includeScopes;
            set => this.includeScopes = value;
        }

        public string Name {
            get => this.name;
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
                this.WriteMessage(logLevel, this.name, eventId.Id, message, exception);
            }
        }

        public virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception) {
            var logBuilder = TelnetLogger.logBuilder;
            TelnetLogger.logBuilder = null;

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
                logBuilder.Append(TelnetLogger.loglevelPadding);
                logBuilder.Append(logName);
                logBuilder.Append("[");
                logBuilder.Append(eventId);
                logBuilder.AppendLine("]");
                // scope information
                if (this.includeScopes) {
                    this.GetScopeInformation(logBuilder);
                }
                // message
                logBuilder.Append(TelnetLogger.messagePadding);
                var len = logBuilder.Length;
                logBuilder.AppendLine(message);
                logBuilder.Replace(Environment.NewLine, TelnetLogger.newLineWithMessagePadding, len, message.Length);
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
                lock (TelnetLogger.lockObject) {
                    if (!string.IsNullOrEmpty(logLevelString)) {
                        // log level string
                        this.telnetServer.BroadcastMessage(logLevelString);
                    }

                    // use default colors from here on
                    this.telnetServer.BroadcastMessage(logMessage);

                    // In case of AnsiLogConsole, the messages are not yet written to the console,
                    // this would flush them instead.
                    // Console.Flush();
                }
            }

            logBuilder.Clear();
            if (logBuilder.Capacity > 1024) {
                logBuilder.Capacity = 1024;
            }

            TelnetLogger.logBuilder = logBuilder;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return this.filter(this.name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state) {
            if (state == null) {
                throw new ArgumentNullException(nameof(state));
            }

            return TelnetLogScope.Push(this.name, state);
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
                builder.Insert(length, TelnetLogger.messagePadding);
                builder.AppendLine();
            }
        }
    }
}

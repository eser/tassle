// --------------------------------------------------------------------------
// <copyright file="TelnetLoggerProvider.cs" company="-">
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Tassle.Telnet;

namespace Tassle.Logging.Telnet {
    public class TelnetLoggerProvider : ILoggerProvider {
        // fields
        private readonly ConcurrentDictionary<string, TelnetLogger> loggers = new ConcurrentDictionary<string, TelnetLogger>();

        private readonly Func<string, LogLevel, bool> filter;
        private ITelnetLoggerSettings settings;

        private IServiceProvider serviceProvider;

        // constructors

        public TelnetLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, bool includeScopes) {
            if (filter == null) {
                throw new ArgumentNullException(nameof(filter));
            }

            this.filter = filter;
            this.settings = new TelnetLoggerSettings() {
                IncludeScopes = includeScopes,
            };

            this.serviceProvider = serviceProvider;
        }

        public TelnetLoggerProvider(IServiceProvider serviceProvider, ITelnetLoggerSettings settings) {
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            this.settings = settings;
            this.serviceProvider = serviceProvider;

            if (this.settings.ChangeToken != null) {
                this.settings.ChangeToken.RegisterChangeCallback(this.OnConfigurationReload, null);
            }
        }

        // methods

        private void OnConfigurationReload(object state) {
            // The settings object needs to change here, because the old one is probably holding on
            // to an old change token.
            this.settings = this.settings.Reload();

            foreach (var logger in this.loggers.Values) {
                logger.Filter = GetFilter(logger.Name, this.settings);
                logger.IncludeScopes = this.settings.IncludeScopes;
            }

            // The token will change each time it reloads, so we need to register again.
            if (this.settings?.ChangeToken != null) {
                this.settings.ChangeToken.RegisterChangeCallback(this.OnConfigurationReload, null);
            }
        }

        public ILogger CreateLogger(string name) {
            return this.loggers.GetOrAdd(name, this.CreateTelnetImplementation);
        }

        private TelnetLogger CreateTelnetImplementation(string name) {
            var telnetServer = this.serviceProvider.GetService(typeof(ITelnetServer)) as ITelnetServer;

            return new TelnetLogger(name, telnetServer, this.GetFilter(name, this.settings), this.settings.IncludeScopes);
        }

        private Func<string, LogLevel, bool> GetFilter(string name, ITelnetLoggerSettings settings) {
            if (this.filter != null) {
                return this.filter;
            }

            if (settings != null) {
                foreach (var prefix in GetKeyPrefixes(name)) {
                    LogLevel level;

                    if (settings.TryGetSwitch(prefix, out level)) {
                        return (n, l) => l >= level;
                    }
                }
            }

            return (n, l) => false;
        }

        private IEnumerable<string> GetKeyPrefixes(string name) {
            while (!string.IsNullOrEmpty(name)) {
                yield return name;

                var lastIndexOfDot = name.LastIndexOf('.');
                if (lastIndexOfDot == -1) {
                    yield return "Default";
                    break;
                }

                name = name.Substring(0, lastIndexOfDot);
            }
        }

        public void Dispose() {
        }
    }
}

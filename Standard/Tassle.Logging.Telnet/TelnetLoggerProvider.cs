// --------------------------------------------------------------------------
// <copyright file="TelnetLoggerProvider.cs" company="-">
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Tassle.Logging.Telnet
{
    public class TelnetLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, TelnetLogger> _loggers = new ConcurrentDictionary<string, TelnetLogger>();

        private readonly Func<string, LogLevel, bool> _filter;
        private TelnetLoggerSettingsInterface _settings;

        public TelnetLoggerProvider(Func<string, LogLevel, bool> filter, bool includeScopes) {
            if (filter == null) {
                throw new ArgumentNullException(nameof(filter));
            }

            this._filter = filter;
            this._settings = new TelnetLoggerSettings() {
                IncludeScopes = includeScopes,
            };
        }

        public TelnetLoggerProvider(TelnetLoggerSettingsInterface settings) {
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            this._settings = settings;

            if (this._settings.ChangeToken != null) {
                this._settings.ChangeToken.RegisterChangeCallback(this.OnConfigurationReload, null);
            }
        }

        private void OnConfigurationReload(object state) {
            // The settings object needs to change here, because the old one is probably holding on
            // to an old change token.
            this._settings = this._settings.Reload();

            foreach (var logger in this._loggers.Values) {
                logger.Filter = GetFilter(logger.Name, this._settings);
                logger.IncludeScopes = this._settings.IncludeScopes;
            }

            // The token will change each time it reloads, so we need to register again.
            if (this._settings?.ChangeToken != null) {
                this._settings.ChangeToken.RegisterChangeCallback(this.OnConfigurationReload, null);
            }
        }

        public ILogger CreateLogger(string name) {
            return this._loggers.GetOrAdd(name, this.CreateTelnetImplementation);
        }

        private TelnetLogger CreateTelnetImplementation(string name) {
            return new TelnetLogger(name, GetFilter(name, this._settings), _settings.IncludeScopes);
        }

        private Func<string, LogLevel, bool> GetFilter(string name, TelnetLoggerSettingsInterface settings) {
            if (this._filter != null) {
                return this._filter;
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

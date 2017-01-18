// --------------------------------------------------------------------------
// <copyright file="ConfigurationTelnetLoggerSettings.cs" company="-">
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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;

namespace Tassle.Logging.Telnet
{
    public class ConfigurationTelnetLoggerSettings : TelnetLoggerSettingsInterface
    {
        private readonly IConfiguration _configuration;

        public ConfigurationTelnetLoggerSettings(IConfiguration configuration) {
            this._configuration = configuration;

            this.ChangeToken = configuration.GetReloadToken();
        }

        public IChangeToken ChangeToken { get; private set; }

        public bool IncludeScopes
        {
            get {
                bool includeScopes;
                var value = this._configuration["IncludeScopes"];

                if (string.IsNullOrEmpty(value)) {
                    return false;
                }

                if (bool.TryParse(value, out includeScopes)) {
                    return includeScopes;
                }

                var message = $"Configuration value '{value}' for setting '{nameof(IncludeScopes)}' is not supported.";
                throw new InvalidOperationException(message);
            }
        }

        public TelnetLoggerSettingsInterface Reload() {
            this.ChangeToken = null;

            return new ConfigurationTelnetLoggerSettings(this._configuration);
        }

        public bool TryGetSwitch(string name, out LogLevel level) {
            var switches = this._configuration.GetSection("LogLevel");
            if (switches == null) {
                level = LogLevel.None;

                return false;
            }

            var value = switches[name];
            if (string.IsNullOrEmpty(value)) {
                level = LogLevel.None;

                return false;
            }

            if (Enum.TryParse<LogLevel>(value, out level)) {
                return true;
            }

            var message = $"Configuration value '{value}' for category '{name}' is not supported.";
            throw new InvalidOperationException(message);
        }
    }
}

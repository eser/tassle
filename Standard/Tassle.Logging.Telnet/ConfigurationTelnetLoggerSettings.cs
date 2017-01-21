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
using System.Net;

namespace Tassle.Logging.Telnet {
    public class ConfigurationTelnetLoggerSettings : TelnetLoggerSettingsInterface {
        // fields

        private readonly IConfiguration _configuration;
        private IChangeToken _changeToken;

        // constructors

        public ConfigurationTelnetLoggerSettings(IConfiguration configuration) {
            this._configuration = configuration;

            this._changeToken = configuration.GetReloadToken();
        }

        // properties

        public bool IncludeScopes {
            get {
                var value = this._configuration["IncludeScopes"];

                if (string.IsNullOrEmpty(value)) {
                    return false;
                }

                bool includeScopes;

                if (bool.TryParse(value, out includeScopes)) {
                    return includeScopes;
                }

                var message = $"Configuration value '{value}' for setting '{nameof(IncludeScopes)}' is not supported.";
                throw new InvalidOperationException(message);
            }
        }

        public IPEndPoint BindEndpoint {
            get {
                var value = this._configuration["BindEndpoint"];

                if (string.IsNullOrEmpty(value)) {
                    return null;
                }

                IPEndPoint bindEndpoint;

                try {
                    var bindEndpointParts = value.Split(new char[] { ':' }, 2, StringSplitOptions.None);

                    var bindEndpointHost = IPAddress.Parse(bindEndpointParts[0]);
                    var bindEndpointPort = int.Parse(bindEndpointParts[1]);

                    bindEndpoint = new IPEndPoint(bindEndpointHost, bindEndpointPort);
                }
                catch (Exception ex) {
                    var message = $"Configuration value '{value}' for setting '{nameof(IncludeScopes)}' is not supported.";
                    throw new InvalidOperationException(message, ex);
                }

                return bindEndpoint;
            }
        }

        public IChangeToken ChangeToken {
            get => this._changeToken;
        }

        // methods

        public TelnetLoggerSettingsInterface Reload() {
            this._changeToken = null;

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

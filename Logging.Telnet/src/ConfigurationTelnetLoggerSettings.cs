// --------------------------------------------------------------------------
// <copyright file="ConfigurationTelnetLoggerSettings.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Net;

namespace Tassle.Logging.Telnet {
    public class ConfigurationTelnetLoggerSettings : ITelnetLoggerSettings {
        // fields

        private readonly IConfiguration configuration;
        private IChangeToken changeToken;

        // constructors

        public ConfigurationTelnetLoggerSettings(IConfiguration configuration) {
            this.configuration = configuration;

            this.changeToken = configuration.GetReloadToken();
        }

        // properties

        public bool IncludeScopes {
            get {
                var value = this.configuration["IncludeScopes"];

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

        //public IPEndPoint BindEndpoint {
        //    get {
        //        var value = this.configuration["BindEndpoint"];

        //        if (string.IsNullOrEmpty(value)) {
        //            return null;
        //        }

        //        IPEndPoint bindEndpoint;

        //        try {
        //            var bindEndpointParts = value.Split(new char[] { ':' }, 2, StringSplitOptions.None);

        //            var bindEndpointHost = IPAddress.Parse(bindEndpointParts[0]);
        //            var bindEndpointPort = int.Parse(bindEndpointParts[1]);

        //            bindEndpoint = new IPEndPoint(bindEndpointHost, bindEndpointPort);
        //        }
        //        catch (Exception ex) {
        //            var message = $"Configuration value '{value}' for setting '{nameof(IncludeScopes)}' is not supported.";
        //            throw new InvalidOperationException(message, ex);
        //        }

        //        return bindEndpoint;
        //    }
        //}

        public IChangeToken ChangeToken {
            get => this.changeToken;
        }

        // methods

        public ITelnetLoggerSettings Reload() {
            this.changeToken = null;

            return new ConfigurationTelnetLoggerSettings(this.configuration);
        }

        public bool TryGetSwitch(string name, out LogLevel level) {
            var switches = this.configuration.GetSection("LogLevel");
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

// --------------------------------------------------------------------------
// <copyright file="TelnetLoggerSettings.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Net;

namespace Tassle.Logging.Telnet {
    public class TelnetLoggerSettings : ITelnetLoggerSettings {
        // fields
        private bool includeScopes;

        //private IPEndPoint bindEndpoint;

        private IDictionary<string, LogLevel> switches;

        private IChangeToken changeToken;

        // constructors

        public TelnetLoggerSettings() {
            this.switches = new Dictionary<string, LogLevel>();
        }

        // properties
        public bool IncludeScopes {
            get => this.includeScopes;
            set => this.includeScopes = value;
        }

        //public IPEndPoint BindEndpoint {
        //    get => this.bindEndpoint;
        //    set => this.bindEndpoint = value;
        //}

        public IDictionary<string, LogLevel> Switches {
            get => this.switches;
            set => this.switches = value;
        }

        public IChangeToken ChangeToken {
            get => this.changeToken;
            set => this.changeToken = value;
        }

        // methods

        public ITelnetLoggerSettings Reload() {
            return this;
        }

        public bool TryGetSwitch(string name, out LogLevel level) {
            return this.switches.TryGetValue(name, out level);
        }
    }
}

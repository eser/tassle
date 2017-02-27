// --------------------------------------------------------------------------
// <copyright file="TelnetLoggerSettings.cs" company="-">
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
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Net;

namespace Tassle.Logging.Telnet {
    public class TelnetLoggerSettings : ITelnetLoggerSettings {
        // fields
        private bool _includeScopes;

        //private IPEndPoint _bindEndpoint;

        private IDictionary<string, LogLevel> _switches;

        private IChangeToken _changeToken;

        // constructors

        public TelnetLoggerSettings() {
            this._switches = new Dictionary<string, LogLevel>();
        }

        // properties
        public bool IncludeScopes {
            get => this._includeScopes;
            set => this._includeScopes = value;
        }

        //public IPEndPoint BindEndpoint {
        //    get => this._bindEndpoint;
        //    set => this._bindEndpoint = value;
        //}

        public IDictionary<string, LogLevel> Switches {
            get => this._switches;
            set => this._switches = value;
        }

        public IChangeToken ChangeToken {
            get => this._changeToken;
            set => this._changeToken = value;
        }

        // methods

        public ITelnetLoggerSettings Reload() {
            return this;
        }

        public bool TryGetSwitch(string name, out LogLevel level) {
            return this._switches.TryGetValue(name, out level);
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="ITelnetLoggerSettings.cs" company="-">
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
using System.Net;

namespace Tassle.Logging.Telnet {
    public interface ITelnetLoggerSettings {
        // properties

        bool IncludeScopes { get; }

        //IPEndPoint BindEndpoint { get; }

        IChangeToken ChangeToken { get; }

        // methods

        bool TryGetSwitch(string name, out LogLevel level);

        ITelnetLoggerSettings Reload();
    }
}

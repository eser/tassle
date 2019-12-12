﻿// --------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tassle.Telnet;

namespace Tassle.TestConsole {
    public class Bootstrapper {
        public IServiceProvider GetServiceProvider() {
            var services = new ServiceCollection();

            services.AddLogging();
            // services.AddSingleton<ITelnetServer>(this.CreateTelnetServer());

            return services.BuildServiceProvider();
        }

        public ITelnetServer CreateTelnetServer() {
            var telnetServer = new TelnetServer(new IPEndPoint(IPAddress.Any, 8084));

            return telnetServer;
        }
    }
}

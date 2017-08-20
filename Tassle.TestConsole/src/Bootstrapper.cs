// --------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="-">
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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tassle.Tasks;
using Tassle.Telnet;

namespace Tassle.TestConsole {
    public class Bootstrapper {
        public IServiceProvider GetServiceProvider() {
            var services = new ServiceCollection();

            services.AddLogging();
            // services.AddSingleton<ITelnetServer>(this.CreateTelnetServer());
            // services.AddSingleton<TaskManager>();

            return services.BuildServiceProvider();
        }

        public ITelnetServer CreateTelnetServer() {
            var telnetServer = new TelnetServer(new IPEndPoint(IPAddress.Any, 8084));

            return telnetServer;
        }
    }
}

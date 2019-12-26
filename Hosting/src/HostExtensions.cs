// --------------------------------------------------------------------------
// <copyright file="HostExtensions.cs" company="-">
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
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting {
    public static class HostExtensions {
        public const string StartMessage = "Starting...";
        public const string TerminationMessage = "Application terminated unexpectedly";

        public static async Task<int> LogAndRunAsync<T>(this IHost host)
            where T : notnull { // , IHost
            // Use the W3C Trace Context format to propagate distributed trace identifiers.
            // See https://devblogs.microsoft.com/aspnet/improvements-in-net-core-3-0-for-troubleshooting-and-monitoring-distributed-apps/
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            var logger = host.Services.GetRequiredService<ILogger<T>>();

            try {
                logger.LogInformation(HostExtensions.StartMessage);

                await host.RunAsync().ConfigureAwait(false);

                return 0;
            }
            catch (Exception exception) {
                logger.LogCritical(exception, HostExtensions.TerminationMessage);

                return 1;
            }
        }
    }
}

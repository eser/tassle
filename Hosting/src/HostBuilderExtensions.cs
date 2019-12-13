// --------------------------------------------------------------------------
// <copyright file="HostBuilderExtensions.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting {
    public static class HostBuilderExtensions {
        public static IHostBuilder ConfigureTassleDefaults(this IHostBuilder hostBuilder, string[] args) {
            var basepath = Directory.GetCurrentDirectory();

            return hostBuilder
                .ConfigureHostConfiguration(configHost => configHost
                    .SetBasePath(basepath)
                    .AddJsonFile("hostsettings.json", optional: true)
                    .AddEnvironmentVariables(prefix: "DOTNET_")
                    .AddCommandLine(args))
                .ConfigureAppConfiguration((hostingContext, config) => config
                    .SetBasePath(basepath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args))
                .ConfigureServices((context, services) => {
                    services.AddLogging(logging => {
                        // logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    });

                    // // Light up the GenericWebHostBuilder implementation
                    // if (hostBuilder is ISupportsUseDefaultServiceProvider supportsDefaultServiceProvider)
                    // {
                    //     return supportsDefaultServiceProvider.UseDefaultServiceProvider(configure);
                    // }

                    var isDevelopment = context.HostingEnvironment.IsDevelopment();

                    var options = new ServiceProviderOptions() {
                        ValidateScopes = isDevelopment,
                        ValidateOnBuild = isDevelopment,
                    };

                    var provider = new DefaultServiceProviderFactory(options);

                    services.Replace(
                        ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(
                            provider));
                })
                .UseConsoleLifetime();
        }
    }
}

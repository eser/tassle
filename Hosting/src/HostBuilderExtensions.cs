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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Microsoft.Extensions.Hosting {
    public static class HostBuilderExtensions {
        public static IHostBuilder UseTassleCliStartup(this IHostBuilder hostBuilder, string[] args) {
            return HostBuilderExtensions.UseTassleStartup<DefaultCliStartup>(hostBuilder, args);
        }

        public static IHostBuilder UseTassleWebApiStartup(this IHostBuilder hostBuilder, string[] args) {
            return HostBuilderExtensions.UseTassleStartup<DefaultWebApiStartup>(hostBuilder, args);
        }

        public static IHostBuilder UseTassleStartup<T>(this IHostBuilder hostBuilder, string[] args)
            where T : class, ITassleStartup, new() { // + notnull constraint
            var startup = new T();
            var basepath = Directory.GetCurrentDirectory();

            return hostBuilder
                .UseConsoleLifetime()
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
                // .ConfigureContainer<ContainerBuilder>((context, builder) => {
                // })
                .ConfigureLogging((context, logging) => {
                    // logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .ConfigureServices((context, services) => {
                    var isDevelopment = context.HostingEnvironment.IsDevelopment();

                    var options = new ServiceProviderOptions() {
                        ValidateScopes = isDevelopment,
                        ValidateOnBuild = isDevelopment,
                    };

                    var provider = new DefaultServiceProviderFactory(options);

                    services.Replace(
                        ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(
                            provider));

                    services.Replace(
                        ServiceDescriptor.Singleton<ITassleStartup>(startup));

                    startup.ConfigureServiceProvider(context, services);
                });
        }

        public static IHostBuilder UseTassleWebDefaults(this IHostBuilder hostBuilder) {
            return hostBuilder.ConfigureWebHost(
                builder => {
                    builder.UseKestrel(
                        (builderContext, serverOptions) => {
                            serverOptions.AddServerHeader = false;
                            serverOptions.AllowSynchronousIO = true;
                        })
                    .UseStartup<WebHostStartup>();
                });
        }
    }
}

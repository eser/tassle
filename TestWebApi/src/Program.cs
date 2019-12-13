// --------------------------------------------------------------------------
// <copyright file="Program.cs" company="-">
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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tassle.TestWebApi {
    public class Program {
        public static Task<int> Main(string[] args) {
            var host = Program.CreateHostBuilder(args).Build();

            return Program.LogAndRunAsync(host);
        }

        public static async Task<int> LogAndRunAsync(IHost host) {
            // Use the W3C Trace Context format to propagate distributed trace identifiers.
            // See https://devblogs.microsoft.com/aspnet/improvements-in-net-core-3-0-for-troubleshooting-and-monitoring-distributed-apps/
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try {
                logger.LogInformation("Starting...");

                await host.RunAsync().ConfigureAwait(false);

                return 0;
            }
            catch (Exception exception) {
                logger.LogCritical(exception, "Application terminated unexpectedly");

                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return new HostBuilder()
                .ConfigureLogging(logging => {
                    logging.AddConsole();
                })
                .ConfigureHostConfiguration(
                    configurationBuilder => configurationBuilder
                        .AddEnvironmentVariables(prefix: "DOTNET_")
                        .AddCommandLine(args)
                    )
                .ConfigureAppConfiguration((hostingContext, config) => config
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                )
                .UseDefaultServiceProvider(
                    (context, options) => {
                        var isDevelopment = context.HostingEnvironment.IsDevelopment();
                        options.ValidateScopes = isDevelopment;
                        options.ValidateOnBuild = isDevelopment;
                    })
                .ConfigureWebHost(ConfigureWebHostBuilder)
                .UseConsoleLifetime();
        }

        private static void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder) {
            webHostBuilder
                .UseKestrel(
                    (builderContext, serverOptions) => {
                        serverOptions.AddServerHeader = false;
                        serverOptions.AllowSynchronousIO = true;
                    })
                .UseStartup<Startup>();
        }
    }
}

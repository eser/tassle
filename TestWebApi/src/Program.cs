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
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Tassle.TestWebApi {
    public class Program {
        public static Task<int> Main(string[] args) {
            var host = new HostBuilder()
                .ConfigureTassleDefaults(args)
                .ConfigureWebHost(ConfigureWebHostBuilder)
                .Build();

            return host.LogAndRunAsync<Program>();
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

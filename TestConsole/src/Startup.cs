// --------------------------------------------------------------------------
// <copyright file="Startup.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Tassle.TestConsole {
    public class Startup : DefaultCliStartup {
        // methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServiceProvider(HostBuilderContext hostingContext, IServiceCollection services) {
            base.ConfigureServiceProvider(hostingContext, services);

            services.Configure<AppSettings>(hostingContext.Configuration.GetSection("AppSettings"));
        }

        public void Configure(ILogger<Startup> logger) {
            logger.LogWarning("cli test");
        }
    }
}

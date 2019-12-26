// --------------------------------------------------------------------------
// <copyright file="WebHostStartup.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting {
    public class WebHostStartup {
        public WebHostStartup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services) {
        }

        public void Configure(IApplicationBuilder app, ITassleStartup tassleStartup) {
            // call Configure method with IoC parameters
            ServiceProviderUtils.CallMethodWithServiceProvider(
                tassleStartup,
                "Configure",
                app.ApplicationServices,
                new Dictionary<Type, object>() {
                    { typeof(IApplicationBuilder), app },
                }
            );
        }
    }
}

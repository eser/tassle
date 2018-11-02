// --------------------------------------------------------------------------
// <copyright file="Startup.cs" company="-">
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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Tassle.TestWebApi {
    public class Startup {
        // fields

        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;

        // constructors

        public Startup(IConfiguration configuration, IHostingEnvironment environment) {
            this.configuration = configuration;
            this.environment = environment;
        }

        // properties

        public IConfiguration Configuration {
            get => this.configuration;
        }

        public IHostingEnvironment Environment {
            get => this.environment;
        }

        // methods

        public void ConfigureServices(IServiceCollection services) {
            services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));

            services.AddLogging();

            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.DateFormatHandling   = DateFormatHandling.IsoDateFormat;
                        options.SerializerSettings.NullValueHandling    = NullValueHandling.Include;
                        options.SerializerSettings.Formatting           = Formatting.Indented;
                        options.SerializerSettings.TypeNameHandling     = TypeNameHandling.Auto;
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });

            // register external services
            switch (this.environment.EnvironmentName) {
                case "Development":
                    services.AddTransient<IDummyExternalService, FakeDummyExternalService>();
                    break;
                case "Testing":
                case "Staging":
                case "Production":
                    services.AddTransient<IDummyExternalService, LiveDummyExternalService>();
                    break;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                loggerFactory.AddDebug();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            app.UseMvc();
        }
    }
}

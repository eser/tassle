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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tassle.TestWebApi {
    public class Startup {
        // fields

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        // constructors

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            this.configuration = configuration;
            this.environment = environment;
        }

        // properties

        public IConfiguration Configuration {
            get => this.configuration;
        }

        public IWebHostEnvironment Environment {
            get => this.environment;
        }

        // methods

        public void ConfigureServices(IServiceCollection services) {
            services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));

            services.AddLogging();

            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        // typename handling?
                        options.JsonSerializerOptions.AllowTrailingCommas         = true;
                        options.JsonSerializerOptions.DictionaryKeyPolicy         = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.IgnoreNullValues            = false;
                        options.JsonSerializerOptions.IgnoreReadOnlyProperties    = false;
                        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                        options.JsonSerializerOptions.PropertyNamingPolicy        = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.ReadCommentHandling         = JsonCommentHandling.Skip;
                        options.JsonSerializerOptions.WriteIndented               = true;

                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggingBuilder loggingBuilder) {
            if (env.EnvironmentName == "Development") {
                app.UseDeveloperExceptionPage();

                loggingBuilder.AddDebug();
            }

            loggingBuilder.AddConsole();

            app.UseMvc();
        }
    }
}

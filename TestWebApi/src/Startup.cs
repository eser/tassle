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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));

            services.AddHealthChecks();

            services.AddControllers()
                    .AddJsonOptions(options => {
                        // typename handling?
                        options.JsonSerializerOptions.AllowTrailingCommas = true;
                        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.IgnoreNullValues = false;
                        options.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
                        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                        options.JsonSerializerOptions.WriteIndented = true;

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                // app.UseBrowserLink();
            }
            else {
                // app.UseExceptionHandler("/Home/Error");
            }

            // app.UseHttpsRedirection();
            app.UseHttpMethodOverride();
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseCors(); // "All"

            // app.UseAuthentication();
            // app.UseAuthorization();

            // FIXME is there any need for output caching?
            // app.UseStaticFiles();

            app.UseEndpoints(endpoints => {
                // endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHealthChecks("/health", new HealthCheckOptions() { });
                // endpoints.RequireAuthorization(new AuthorizeAttribute() { Roles = "admin", });
                endpoints.MapControllers();
            });
        }
    }
}

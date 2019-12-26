// --------------------------------------------------------------------------
// <copyright file="DefaultWebApiStartup.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting {
    public class DefaultWebApiStartup : ITassleStartup {
        public virtual void ConfigureServiceProvider(HostBuilderContext hostingContext, IServiceCollection services) {
            var entryAssembly = Assembly.GetEntryAssembly();

            services.AddHealthChecks();

            services.AddControllers()
                .AddApplicationPart(entryAssembly)
                .AddControllersAsServices()
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

        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                // app.UseBrowserLink();
            }
            else {
                // app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            // app.UseHttpsRedirection();
            app.UseHttpMethodOverride();
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseCors(builder => {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });

            // app.UseAuthentication();
            // app.UseAuthorization();

            // FIXME is there any need for output caching?
            // app.UseStaticFiles();

            app.UseEndpoints(endpoints => {
                // endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHealthChecks("/health", new HealthCheckOptions() { });
                // endpoints.RequireAuthorization(new AuthorizeAttribute() { Roles = "admin", });
                endpoints.MapControllers();
                // endpoints.MapRazorPages();
            });
        }
    }
}

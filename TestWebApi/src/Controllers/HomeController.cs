// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tassle.TestWebApi.Controllers {
    //[Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // fields
        private readonly AppSettings appSettings;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment environment;
        private readonly IDummyExternalService dummyExternalService;

        // constructors

        public HomeController(IOptions<AppSettings> appSettingsOptions,
            ILogger<HomeController> logger,
            IWebHostEnvironment environment,
            IDummyExternalService dummyExternalService)
        {
            this.appSettings = appSettingsOptions.Value;
            this.logger = logger;
            this.environment = environment;
            this.dummyExternalService = dummyExternalService;
        }

        [HttpGet("~/")]
        public async Task<HomeIndexResult> Index()
        {
            var appVersion = Assembly.GetEntryAssembly()?
                                     .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                     .InformationalVersion;

            var serviceResult = this.dummyExternalService.Calculate();

            var result = new HomeIndexResult() {
                Placeholder           = this.appSettings.Placeholder,
                Version               = appVersion,
                EnvironmentName       = this.environment.EnvironmentName,
                ExternalServiceResult = serviceResult,
            };

            return result;
        }
    }
}

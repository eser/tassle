// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="-">
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
        private readonly IHostingEnvironment environment;
        private readonly IDummyExternalService dummyExternalService;

        // constructors

        public HomeController(IOptions<AppSettings> appSettingsOptions,
            ILogger<HomeController> logger,
            IHostingEnvironment environment,
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

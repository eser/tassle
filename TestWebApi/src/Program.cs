// --------------------------------------------------------------------------
// <copyright file="Program.cs" company="-">
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

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tassle.TestWebApi {
    public class Program {
        public static void Main(string[] args) {
            Program.CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // .ConfigureAppConfiguration((hostingContext, config) => {
                //     config.SetBasePath(Directory.GetCurrentDirectory());
                //     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                //     config.AddJsonFile("appsettings.{Environment}.json", optional: true, reloadOnChange: false);
                //     config.AddEnvironmentVariables();
                //     config.AddCommandLine(args);
                // })
                .UseStartup<Startup>();
    }
}

// --------------------------------------------------------------------------
// <copyright file="FunctionManager.cs" company="-">
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

using System;
using System.Threading.Tasks;

namespace Tassle.Functions {
    /// <summary>
    /// FunctionManager class.
    /// </summary>
    public class FunctionManager : IFunctionManager {
        // fields

        private readonly IServiceProvider serviceProvider;

        // constructors

        public FunctionManager(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        // properties

        public IServiceProvider ServiceProvider {
            get => this.serviceProvider;
        }

        // methods

        public async Task<Response> RunAsync(IFunction target, Request request) {
            var methodNames = new string[] { "RunAsync", "Run" };

            var response = DependencyInjectionUtils.CallMethodWithServiceProvider(target, methodNames, this.serviceProvider, request);

            if (response.methodName == "RunAsync") {
                return await (response.returnValue as Task<Response>);
            }

            if (response.methodName == "Run") {
                return (response.returnValue as Response);
            }

            return null;
        }
    }
}

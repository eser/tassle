// --------------------------------------------------------------------------
// <copyright file="FunctionManager.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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

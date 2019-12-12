// --------------------------------------------------------------------------
// <copyright file="TestFunction.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using Tassle.Functions;

namespace Tassle.TestConsole {
    /// <summary>
    /// TestFunction class.
    /// </summary>
    public class TestFunction : IFunction {
        public Response Run(Request<int> request) {
            Console.WriteLine(request.Value + 3);

            return this.Ok();
        }
    }
}

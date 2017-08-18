// --------------------------------------------------------------------------
// <copyright file="FunctionManager.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
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
        public Task Run(Func<Request, Response> target) {
        }
    }
}
